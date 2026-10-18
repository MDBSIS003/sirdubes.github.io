using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class LearnerProfile
    {
        private int id;
        private string name = "";
        private int grade = 12;
        private string school = "";
        private string province = "";
        private string email = "";
        private string phone = "";
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public int Grade
        {
            get { return grade; }

            set { grade = value; }
        }

        public string School
        {
            get { return school; }

            set { school = value; }
        }

        public string Province
        {
            get { return province; }

            set { province = value; }
        }

        public string Email
        {
            get { return email; }

            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }

            set { phone = value; }
        }
    }
}
