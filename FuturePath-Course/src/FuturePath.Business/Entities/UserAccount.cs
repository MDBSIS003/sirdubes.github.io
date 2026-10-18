using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class UserAccount
    {
        private int id;
        private string username = "";
        private string passwordHash = "";
        private UserRole role;
        private bool active;
        private int failedAttempts;
        private DateTime? lockedUntilUtc;
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public string Username
        {
            get { return username; }

            set { username = value; }
        }

        public string PasswordHash
        {
            get { return passwordHash; }

            set { passwordHash = value; }
        }

        public UserRole Role
        {
            get { return role; }

            set { role = value; }
        }

        public bool Active
        {
            get { return active; }

            set { active = value; }
        }

        public int FailedAttempts
        {
            get { return failedAttempts; }

            set { failedAttempts = value; }
        }

        public DateTime? LockedUntilUtc
        {
            get { return lockedUntilUtc; }

            set { lockedUntilUtc = value; }
        }
    }
}
