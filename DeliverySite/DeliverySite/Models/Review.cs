using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliverySite.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int ReviewNum { get; set; }
        [Required]
        public int OrderNum { get; set; }
        //[Required]
        //public bool Good_Bad { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserFirstName { get; set; }
        [Required]
        public string UserLastName { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}