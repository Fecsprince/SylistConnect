using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Order
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public int UserID { get; set; } 

        [ForeignKey("Product")]
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


        //REFERENCES
        public virtual Product Product { get; set; }
    }
}
