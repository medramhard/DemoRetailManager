using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidRate = decimal.TryParse(rateText, out decimal output);

            if (isValidRate == false)
            {
                throw new ConfigurationErrorsException($"Invalid input of tax rate ({rateText})");
            }

            return output;
        }
    }
}
