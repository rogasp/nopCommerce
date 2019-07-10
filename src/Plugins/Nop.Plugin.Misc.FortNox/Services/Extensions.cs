using System;
using System.Collections.Generic;
using System.Linq;
using FortnoxAPILibrary;

namespace Nop.Plugin.Misc.FortNox.Services
{
    public static class Extensions
    {

        public static bool HasEmail(this CustomerSubset customer)
        {
            if (string.IsNullOrEmpty(customer.Email))
                return false;
            else
                return true;
        }

        public static (string, string) GetLastNameAndFirstName(this Customer customer)
        {
            List<string> names = customer.Name.Split(' ').ToList();
            string firstName = names.First();
            names.RemoveAt(0);

            return (String.Join(" ", names.ToArray()), firstName);
        }

    }
}
