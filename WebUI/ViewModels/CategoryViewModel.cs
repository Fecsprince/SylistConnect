using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [MaxLength(10, ErrorMessage ="More than 10 characters")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}