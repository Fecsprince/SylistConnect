using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name character maximum length is 50!")]
        public string Name { get; set; }


        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "Description character maximum length is 100!")]
        public string Description { get; set; }

        [Required]
        public HttpPostedFileBase Image1 { get; set; }

        public HttpPostedFileBase Image2 { get; set; }

        public string _Image1 { get; set; }

        public string _Image2 { get; set; }

        public string CategoryName { get; set; }

    }
}