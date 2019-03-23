using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliverySite.Models
{
    public class Company
    {
        [Required]
        public string CompName { get; set; }


        [Required]
        [Key]
        [StringLength(6, ErrorMessage = "Company Code length have to be 6 characters.")]
        [RegularExpression("([0-9a-zA-Z]{6})", ErrorMessage = "Company Code must contain 6 characters (letters or numbers only).")]
        public string CompCode { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]{8,16}$", ErrorMessage = "Password must contain between 8 to 16 chars or numbers")]
        public string Password { get; set; }
    }
}