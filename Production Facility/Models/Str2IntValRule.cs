using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Production_Facility.Models
{
    public class Str2IntValRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Int32.TryParse(value.ToString(), out int i))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Not integer from 1 to million.");
        }
    }
}
