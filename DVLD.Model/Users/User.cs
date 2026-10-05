using DVLD.Model.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model.Users
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PersonId { get; set; }
        public bool IsActive { get; set; }
        public enMode UserMode { get; set; }

        public User()
        {
            UserId = -1;
            UserName = "";
            PersonId = -1;
            IsActive = false;
            UserMode = enMode.AddNew;
        }

        public User(User user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            PersonId = user.PersonId;
            IsActive = user.IsActive;
            UserMode = user.UserMode;
        }

    }
}
