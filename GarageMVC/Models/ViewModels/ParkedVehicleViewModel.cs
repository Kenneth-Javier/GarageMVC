using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageMVC.Models.ViewModels
{
    public class ParkedVehicleViewModel
    {

        public int Id { get; set; }

        public VehicleType VehicleType { get; set; }
        public string RegNumber { get; set; }

        public string Color { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int NumberOfWheels { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        public TimeSpan ParkedTime { get; set; }
    }
}
