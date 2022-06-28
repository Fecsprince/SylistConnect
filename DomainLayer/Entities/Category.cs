using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name character maximum length is 50!")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Description character maximum length is 100!")]
        public string Description { get; set; } 


        //REFERENCES
        public virtual ICollection<Product> Products { get; set; }
    }
}
