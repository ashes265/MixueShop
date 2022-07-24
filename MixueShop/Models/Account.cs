using System;
using System.Collections.Generic;

#nullable disable

namespace MixueShop.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string UPassword { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AAddress { get; set; }

        public virtual RoleAccount Role { get; set; }
    }
}
