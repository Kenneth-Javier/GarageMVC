using GarageMVC.Models.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace GarageMVC.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }
        [RequiredEnumAttribute]
        public VehicleType VehicleType { get; set; }

        [Required(AllowEmptyStrings = false)]
        //[StringLength(maximumLength: 6)]
        [StringLength(maximumLength: 6, ErrorMessage = "Registration length must contain 6 digits", MinimumLength = 6)]
        public string RegNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(30)]
        public string Color { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(30)]
        public string Brand { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(30)]
        public string Model { get; set; }

        [Range(0, int.MaxValue)]
        public int NumberOfWheels { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
