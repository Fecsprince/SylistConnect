using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }

        [Required]
        public int UserID { get; set; }

        [Display(Name = "Product")]
        public int ProductID { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        [StringLength(100, ErrorMessage = "Shipping Address character maximum length is 100!")]
        public string ShippingAddress { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}