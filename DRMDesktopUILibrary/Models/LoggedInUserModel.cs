﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        public void LogOut()
        {
            Token = String.Empty;
            Id = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            EmailAddress = String.Empty;
            CreatedDate = DateTime.MinValue;
        }
    }
}
