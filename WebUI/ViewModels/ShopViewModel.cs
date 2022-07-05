using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.ViewModels
{
    public class ShopViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name character maximum length is 50!")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Permanant Address")]
        [StringLength(100, ErrorMessage = "Address character maximum length is 100!")]
        public string PermanantAddress { get; set; }

        [Required]
        [MaxLength(11, ErrorMessage = "Contact1 character length should be 11!")]
        [MinLength(11, ErrorMessage = "Contact1 character length should be 11!")]
        [DataType(DataType.PhoneNumber)]
        public string Contact1 { get; set; }

        [Required]
        [MaxLength(11, ErrorMessage = "Contact2 character length should be 11!")]
        [MinLength(11, ErrorMessage = "Contact2 character length should be 11!")]
        [DataType(DataType.PhoneNumber)]
        public string Contact2 { get; set; }

        [Required]
        public HttpPostedFileBase Image1 { get; set; }

        public HttpPostedFileBase Image2 { get; set; }

        public string _Image1 { get; set; }

        public string _Image2 { get; set; }

        public string UserID { get; set; }

        [Required]
        [Display(Name = "Booking Days")]
        public string BookingDays { get; set; }

        [Required]
        [Display(Name = "Opening Hour")]
        public string OpeningHour { get; set; }

        [Required]
        [Display(Name = "Closing Hour")]
        public string ClosingHour { get; set; }
    }
}