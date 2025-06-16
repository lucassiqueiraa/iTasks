using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.helpers
{
    public static class Validator
    {
        public static bool IsUsernameValid(string username)
        {
            string regEx = @"^[a-zA-Z0-9_]{3,64}$";
            return System.Text.RegularExpressions.Regex.IsMatch(username, regEx);
        }

        public static bool IsPasswordValid(string password)
        {   //TODO : MELHORAR A VALIDAÇÃO DA PASSWORD
            // Example validation: password must be at least 8 characters long
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
        }
    }
}
