using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int AgeCalc(this DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;

            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}