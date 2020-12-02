using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string Area { get; set; }


        public string Phone { get; set; }


        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string  Password { get; set; }
    }
}