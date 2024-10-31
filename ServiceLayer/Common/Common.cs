using System.Text.RegularExpressions;
using ServiceLayer.Helper;

namespace ServiceLayer.Common
{
    public class Common
    {

        public static Request_Model IsValidEmail(string emailaddress)
        {
            try
            {
                if (string.IsNullOrEmpty(emailaddress))
                    return Request_Response.Get_Response("Failed", Response_Messages.EmailRequired, 400, null);

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailaddress);
                if (match.Success)
                {
                    return new Request_Model { Status = "Success" };
                }
                else
                {
                    return Request_Response.Get_Response("Failed", Response_Messages.EnterValidEmail, 400, null);
                }
            }
            catch (FormatException ex)
            {
                return Request_Response.Get_Response("Exception", Response_Messages.SomethingWentWrong, 404, null);
            }
        }
        public static Request_Model IsPasswordValid(string Password, string ConfirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                    return Request_Response.Get_Response("Failed", "Password is required.", 400, null);

                if (!Password.Equals(ConfirmPassword))
                    return Request_Response.Get_Response("Failed", "Password and confirm password do not match.", 400, null);

                return new Request_Model { Status = "Success" };
            }
            catch (Exception ex)
            {
                return Request_Response.Get_Response("Exception", Response_Messages.SomethingWentWrong, 400, null);
            }
        }
        public static Request_Model CheckPasswordValid(string password, string confirmPassword)
        {
            try
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };
                if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                {
                    return Request_Response.Get_Response("Failed", "Please fill Password.", 400, null);
                }
                else if (string.IsNullOrEmpty(confirmPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return Request_Response.Get_Response("Failed", "Please fill Confirm Password.", 400, null);
                }
                else if (!password.Contains(confirmPassword))
                {
                    return Request_Response.Get_Response("Failed", "Password and Confirm password do not match.", 400, null);
                }
                else if (password.Length < 9)
                {
                    return Request_Response.Get_Response("Failed", "Password should be at least 8 characters long and should include numbers, letters and special characters", 400, null);
                }
                else if (password.IndexOfAny(special) == -1)
                {
                    return Request_Response.Get_Response("Failed", "Password should be at least 8 characters long and should include numbers, letters and special characters", 400, null);
                }
                else
                {
                    return new Request_Model { Status = "Success", Code = 200 };
                }
            }
            catch (Exception ex)
            {
                return Request_Response.Get_Response("Exception", Response_Messages.SomethingWentWrong, 400, null);
            }
        }
        public static bool IsStringValue(string input)
        {
            // Try to parse the input as an integer
            if (int.TryParse(input, out _))
            {
                return false; // It's a number
            }

            // Try to parse the input as a double (to cover decimals)
            if (double.TryParse(input, out _))
            {
                return false; // It's a number
            }
            if (string.IsNullOrEmpty(input))
                return false;

            return true; // It's not a number, so it's a string
        }
    }
}
