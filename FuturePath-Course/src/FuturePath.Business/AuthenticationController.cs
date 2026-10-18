using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace FuturePath.Business
{
    public sealed class AuthenticationController
    {
        private readonly IFuturePathStore store;
        private readonly AccessSession access;
        private readonly IClock clock;
        public AuthenticationController(IFuturePathStore store, AccessSession access, IClock clock)
        {
            this.store = store;
            this.access = access;
            this.clock = clock;
        }

        private readonly PasswordHasher hasher = new PasswordHasher();
        public void Login(string username, string password)
        {
            UserAccount? user = store.FindUser(username.Trim());
            if (user is null || !user.Active || user.LockedUntilUtc > clock.UtcNow)
            {
                throw new ValidationException("Sign-in failed. Check your details, account status, or wait 5 minutes if locked.");
            }

            bool valid = hasher.Verify(password, user.PasswordHash);
            store.RecordLogin(user.Id, valid, clock.UtcNow);
            store.Audit(user.Id, valid ? "Login" : "Failed login", user.Username, clock.UtcNow);
            if (!valid)
            {
                throw new ValidationException("Sign-in failed. Check your details, account status, or wait 5 minutes if locked.");
            }

            access.SignIn(user);
        }

        private string pendingEmail = "";
        private string pendingUsername = "";
        private string pendingHash = "";
        private UserRole pendingRole;
        private DateTime? pendingUntil;

        public void RequestTesterRegistration(string email, string username, string password, UserRole role)
        {
            CancelTesterRegistration();
            email = email.Trim().ToLowerInvariant();
            username = username.Trim();
            if (email.Length > 150 || !System.Net.Mail.MailAddress.TryCreate(email, out var address) || address.Address != email)
            {
                throw new ValidationException("Enter a valid email address.");
            }
            ValidateUsername(username);
            if (role is not (UserRole.Advisor or UserRole.Administrator))
            {
                throw new ValidationException("Choose Advisor or Administrator for the tester account.");
            }
            string hash = hasher.Hash(password);
            if (store.FindUser(username) != null)
            {
                throw new ValidationException("That username already exists. Sign in or choose another username.");
            }
            pendingEmail = email;
            pendingUsername = username;
            pendingHash = hash;
            pendingRole = role;
            pendingUntil = clock.UtcNow.AddMinutes(5);
        }

        public void CompleteTesterRegistration()
        {
            if (pendingUntil == null || clock.UtcNow >= pendingUntil.Value)
            {
                CancelTesterRegistration();
                throw new ValidationException("Request a new test code before continuing.");
            }
            UserAccount user = store.CreateTesterAccount(pendingEmail, pendingUsername, pendingHash, pendingRole, clock.UtcNow);
            CancelTesterRegistration();
            access.SignIn(user);
        }

        public void CancelTesterRegistration()
        {
            pendingEmail = pendingUsername = pendingHash = "";
            pendingUntil = null;
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            access.RequireStaff();
            UserAccount user = store.GetUser(access.UserId!.Value) ?? throw new AccessDeniedException();
            ChangeSecurity(user.Username, currentPassword, newPassword);
        }

        public void ChangeSecurity(string username, string currentPassword, string newPassword)
        {
            access.RequireStaff();
            UserAccount user = store.GetUser(access.UserId!.Value) ?? throw new AccessDeniedException();
            if (!user.Active || user.Role is not (UserRole.Advisor or UserRole.Administrator))
            {
                throw new AccessDeniedException();
            }
            if (!hasher.Verify(currentPassword, user.PasswordHash))
            {
                throw new ValidationException("Current password is incorrect.");
            }

            username = username.Trim();
            ValidateUsername(username);
            UserAccount? other = store.FindUser(username);
            if (other != null && other.Id != user.Id)
            {
                throw new ValidationException("That username is already in use. Choose another username.");
            }

            string? hash = newPassword.Length == 0 ? null : hasher.Hash(newPassword);
            store.SaveSecurity(user.Id, username, hash, clock.UtcNow);
            user.Username = username;
            access.RefreshStaff(user);
        }

        private void ValidateUsername(string username)
        {
            if (username.Length < 3 || username.Length > 60 || !ValidUsername(username))
            {
                throw new ValidationException("Username must be 3–60 letters, numbers, dots, underscores or hyphens.");
            }
        }

        public void SaveUser(int id, string username, UserRole role, bool active, string? password)
        {
            access.RequireAdmin();
            username = username.Trim();
            ValidateUsername(username);

            if (role == UserRole.Learner)
            {
                throw new ValidationException("Learners use private workspaces; only staff accounts are maintained here.");
            }

            if (id == access.UserId && (!active || role != UserRole.Administrator))
            {
                throw new ValidationException("You cannot deactivate or demote your signed-in administrator account.");
            }

            if (id == 0 && string.IsNullOrEmpty(password))
            {
                throw new ValidationException("A password is required for a new account.");
            }

            store.SaveUser(id, username, role, active, string.IsNullOrEmpty(password) ? null : hasher.Hash(password));
            store.Audit(access.UserId, "Account saved", username, clock.UtcNow);
            if (id == access.UserId)
            {
                UserAccount user = store.GetUser(id) ?? throw new AccessDeniedException();
                access.RefreshStaff(user);
            }
        }

        private bool ValidUsername(string username)
        {
            foreach (char character in username)
            {
                if (!char.IsLetterOrDigit(character) && character != '.' && character != '_' && character != '-')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
