using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagementSystem.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public bool IsActive { get; set; }
    }

}