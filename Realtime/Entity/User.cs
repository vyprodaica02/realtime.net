﻿using System.Text.Json.Serialization;

namespace Realtime.Entity
{
    public class User : BaseEnity
    {
        public string TenUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
