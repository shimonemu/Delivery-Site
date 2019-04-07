using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliverySite.Models
{
    public class Product
    {
        [Required]
        public string PrdName { get; set; }

        [Required]
        [Range(21,10000000, ErrorMessage = "Please enter valid price, above 20")]
        public int price { get; set; }

        [Required]
        public string CompCode { get; set; }

        [Required(ErrorMessage = "ID is Required")]
        [Key]
        //[StringLength(4, ErrorMessage = "Product ID length have to be 4 numbers.")]
        [RegularExpression("([0-9]{4})", ErrorMessage = "Product ID must contain 4 numbers.")]
        public string PrdId { get; set; }

        
    }
}