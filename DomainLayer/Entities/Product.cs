using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name character maximum length is 50!")]
        public string Name { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "Description character maximum length is 100!")]
        public string Description { get; set; }

        [Required]
        public string Image1 { get; set; }

        public string Image2 { get; set; }




        //REFERENCES 
        public virtual Category Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
