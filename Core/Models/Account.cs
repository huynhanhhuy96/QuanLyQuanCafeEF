using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models
{
    public partial class Account
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Type { get; set; }
    }
}
