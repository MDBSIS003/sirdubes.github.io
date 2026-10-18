using System.Security.Cryptography;

namespace FuturePath.Business;
public sealed class PasswordHasher
{
    private const int Iterations = 210_000;
    public string Hash(string password)
    {
        Validate(password);
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string encoded)
    {
        try
        {
            var parts = encoded.Split(':');
            if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations) || iterations < 100_000 || iterations > 1_000_000)
                return false;
            var expected = Convert.FromBase64String(parts[2]);
            var salt = Convert.FromBase64String(parts[1]);
            if (expected.Length != 32 || salt.Length != 16)
                return false;
            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
            return CryptographicOperations.FixedTimeEquals(expected, actual);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public void Validate(string password)
    {
        if (password.Length < 12 || password.Length > 128 || !password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            throw new ValidationException("Use a password of 12–128 characters with a letter and a number.");
    }
}

/// <summary>Identity is only changed by successful authentication, never by a caller-supplied role.</summary>
public sealed class AccessSession
{
    private readonly HashSet<int> ownedSessions = [];
    public int? UserId { get; private set; }
    public string DisplayName { get; private set; } = "Learner workspace";
    public UserRole Role { get; private set; } = UserRole.Learner;
    public bool IsStaff => Role is UserRole.Advisor or UserRole.Administrator;

    public void RequireStaff()
    {
        if (!IsStaff)
            throw new AccessDeniedException();
    }

    public void RequireAdmin()
    {
        if (Role != UserRole.Administrator)
            throw new AccessDeniedException();
    }

    public void RequireSession(int id)
    {
        if (!IsStaff && !ownedSessions.Contains(id))
            throw new AccessDeniedException();
    }

    internal void OwnSession(int id) => ownedSessions.Add(id);
    internal void SignIn(UserAccount user)
    {
        SignOut();
        UserId = user.Id;
        DisplayName = user.Username;
        Role = user.Role;
    }

    public void SignOut()
    {
        UserId = null;
        DisplayName = "Learner workspace";
        Role = UserRole.Learner;
        ownedSessions.Clear();
    }
}

public sealed class AuthenticationService
{
    private readonly IFuturePathStore store;
    private readonly AccessSession access;
    private readonly IClock clock;
    public AuthenticationService(IFuturePathStore store, AccessSession access, IClock clock)
    {
        this.store = store;
        this.access = access;
        this.clock = clock;
    }

    private readonly PasswordHasher hasher = new();
    public void Login(string username, string password)
    {
        var user = store.FindUser(username.Trim());
        if (user is null || !user.Active || user.LockedUntilUtc > clock.UtcNow)
            throw new ValidationException("Sign-in failed. Check your details, account status, or wait 5 minutes if locked.");
        var valid = hasher.Verify(password, user.PasswordHash);
        store.RecordLogin(user.Id, valid, clock.UtcNow);
        store.Audit(user.Id, valid ? "Login" : "Failed login", user.Username, clock.UtcNow);
        if (!valid)
            throw new ValidationException("Sign-in failed. Check your details, account status, or wait 5 minutes if locked.");
        access.SignIn(user);
    }

    public void ChangePassword(string currentPassword, string newPassword)
    {
        access.RequireStaff();
        var user = store.GetUser(access.UserId!.Value) ?? throw new AccessDeniedException();
        if (!hasher.Verify(currentPassword, user.PasswordHash))
            throw new ValidationException("Current password is incorrect.");
        store.SavePassword(user.Id, hasher.Hash(newPassword));
        store.Audit(user.Id, "Password changed", "Self service", clock.UtcNow);
    }

    public void SaveUser(int id, string username, UserRole role, bool active, string? password)
    {
        access.RequireAdmin();
        username = username.Trim();
        if (username.Length < 3 || username.Length > 60 || username.Any(c => !char.IsLetterOrDigit(c) && c != '.' && c != '_' && c != '-'))
            throw new ValidationException("Username must be 3–60 letters, numbers, dots, underscores or hyphens.");
        if (role == UserRole.Learner)
            throw new ValidationException("Learners use private workspaces; only staff accounts are maintained here.");
        if (id == access.UserId && (!active || role != UserRole.Administrator))
            throw new ValidationException("You cannot deactivate or demote your signed-in administrator account.");
        if (id == 0 && string.IsNullOrEmpty(password))
            throw new ValidationException("A password is required for a new account.");
        store.SaveUser(id, username, role, active, string.IsNullOrEmpty(password) ? null : hasher.Hash(password));
        store.Audit(access.UserId, "Account saved", username, clock.UtcNow);
    }
}
