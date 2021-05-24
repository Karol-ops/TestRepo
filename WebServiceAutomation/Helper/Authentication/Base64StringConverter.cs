using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Helper.Authentication
{
    public class Base64StringConverter
    {
        public static string GetBase64String(string username, string password)
        {
            string auth = username + ":" + password;
            byte[] inArray = Encoding.UTF8.GetBytes(auth);

            return Convert.ToBase64String(inArray);
        }
    }
}
