using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Helper
{
    public sealed class Response_Messages
    {
        public const string EmailExist = "Email address already exist";
        public const string EmailDoesNotExist = "Email address does'nt exist";
        public const string RegistrationFailed = "User Registration Failed. Try Again";
        public const string UpdateWalletFailed = "Wallet Update Failed. Try Again";
        public const string RegistrationSuccess = "User Registered Successfully.";
        public const string GoogleRegistrationSuccess = "Google Registered Successfully.";
        public const string FacebookRegistrationSuccess = "Facebook Registered Successfully.";
        public const string UserUpdateFailed = "User Update Failed. Try Again";
        public const string EnterValidEmailOrPass = "Please enter valid Email or Password";
        public const string ChangePasswordMail = "Change password mail has been sent to the user.";
        public const string UserAuthenticationFailed = "User is not authenticated";
        public const string Logout = "User successfully logout";
        public const string EmailRequired = "Email is required";
        public const string EnterValidEmail = "Please enter a valid email address";
        public const string SomethingWentWrong = "Something went wrong";
        public const string ModelIsNotValid = "Model state is not valid";
        public const string UserDoesNotExist = "User address does'nt exist";
        public const string CoordinatesDoesNotExist = "Coordinates does'nt exist";
        public const string CoordinatesDeleted = "Coordinates deleted";
        public const string CoordinatesUpdated = "Coordinates updated";
        public const string CoordinatesAdded = "Coordinates added successfully";
        public const string WalletAdded = "Wallet added successfully";
        public const string EmergencyContactFailed = "Contact more than 5";
        public const string EmergencyContactSuccess = "Emergency Contact added successfully";
    }
}
