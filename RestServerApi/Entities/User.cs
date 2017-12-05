﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RestServerApi.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}