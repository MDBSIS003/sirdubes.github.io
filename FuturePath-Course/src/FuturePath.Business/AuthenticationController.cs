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

        public void ChangePassword(string currentPassword, string newPassword)
        {
            access.RequireStaff();
            UserAccount user = store.GetUser(access.UserId!.Value) ?? throw new AccessDeniedException();
            if (!hasher.Verify(currentPassword, user.PasswordHash))
            {
                throw new ValidationException("Current password is incorrect.");
            }

            store.SavePassword(user.Id, hasher.Hash(newPassword));
            store.Audit(user.Id, "Password changed", "Self service", clock.UtcNow);
        }

        public void SaveUser(int id, string username, UserRole role, bool active, string? password)
        {
            access.RequireAdmin();
            username = username.Trim();
            if (username.Length < 3 || username.Length > 60 || !ValidUsername(username))
            {
                throw new ValidationException("Username must be 3–60 letters, numbers, dots, underscores or hyphens.");
            }

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
