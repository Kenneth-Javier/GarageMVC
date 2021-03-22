using System;
using System.ComponentModel.DataAnnotations;

namespace GarageMVC.Models.DataAnnotations
{
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || value.Equals(VehicleType.None))
                return false;

            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }
    }
}
