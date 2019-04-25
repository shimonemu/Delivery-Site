﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliverySite.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int OrderNum { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserFirstName { get; set; }
        [Required]
        public string UserLastName { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string CompanyCode { get; set; }
    }
}