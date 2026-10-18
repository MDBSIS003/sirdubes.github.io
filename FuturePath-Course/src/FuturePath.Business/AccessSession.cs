using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace FuturePath.Business
{
         public sealed class AccessSession
    {
        private int? userId;
        private string displayName = "Learner workspace";
        private UserRole role = UserRole.Learner;
        private readonly HashSet<int> ownedSessions = new HashSet<int>();
        public int? UserId
        {
            get { return userId; }

            private set { userId = value; }
        }

        public string DisplayName
        {
            get { return displayName; }

            private set { displayName = value; }
        }

        public UserRole Role
        {
            get { return role; }

            private set { role = value; }
        }

        public bool IsStaff
        {
            get
            {
                return Role is UserRole.Advisor or UserRole.Administrator;
            }
        }

        public void RequireStaff()
        {
            if (!IsStaff)
            {
                throw new AccessDeniedException();
            }
        }

        public void RequireAdmin()
        {
            if (Role != UserRole.Administrator)
            {
                throw new AccessDeniedException();
            }
        }

        public void RequireSession(int id)
        {
            if (!IsStaff && !ownedSessions.Contains(id))
            {
                throw new AccessDeniedException();
            }
        }

        internal void OwnSession(int id)
        {
            ownedSessions.Add(id);
        }

        internal void SignIn(UserAccount user)
        {
            SignOut();
            UserId = user.Id;
            DisplayName = user.Username;
            Role = user.Role;
        }

        internal void RefreshStaff(UserAccount user)
        {
            UserId = user.Id;
            DisplayName = user.Username;
            Role = user.Role;
        }

        public void ClearLearnerWorkspace()
        {
            ownedSessions.Clear();
        }

        public void SignOut()
        {
            UserId = null;
            DisplayName = "Learner workspace";
            Role = UserRole.Learner;
            ownedSessions.Clear();
        }
    }
}
