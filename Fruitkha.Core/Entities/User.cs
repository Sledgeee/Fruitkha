using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Fruitkha.Core.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DefaultValue(true)]
        public bool IsEmailingEnabled { get; set; }

        [Required]
        public DateTimeOffset CreateDate { get; set; }
    }
}