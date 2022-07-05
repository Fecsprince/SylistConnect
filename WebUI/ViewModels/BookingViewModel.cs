using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Service")]
        public int ServiceID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Display(Name = "Appointment Date")]
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Appointment Time")]
        [Required]
        public string AppointmentTime { get; set; }

        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Address character maximum length is 100!")]
        public string Address { get; set; }
    }
}