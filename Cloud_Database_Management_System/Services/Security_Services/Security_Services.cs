﻿using Cloud_Database_Management_System.Services.Security_Services.Hashing_Services;
using Cloud_Database_Management_System.Services.Security_Services.Security_Table.Data_Models;
using Cloud_Database_Management_System.Services.Security_Services.Security_Table;
using Cloud_Database_Management_System.Repositories.Repository_Group_1.Table_Interface;
using System.Text.RegularExpressions;
using Cloud_Database_Management_System.Services.Security_Services.AES_Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Server_Side.Database_Services.Output_Schema.Log_Database_Schema;
using Cloud_Database_Management_System.Models.Group_Data_Models;

namespace Cloud_Database_Management_System.Services.Security_Services
{
    public static class Security_Database_Services_Centre
    {
        private static string? connection_string { get; set; }
        private static string? Security_Schema {  get; set; }
        public static List<Security_UserId_Record>? Security_UserId_Record_List;
        public static List<Security_Password_Record>? Security_Password_Record_List;
        private static readonly string user_id_table_name = "security_userid";
        private static readonly string password_table_name = "security_password";
        public async static Task<bool> SignInProcess(string username,string email, string password)
        {
            bool isValid = false;
            if (Checking_Input(username, email, password))
            {
                isValid = true;
                return isValid;
            }
            return isValid;
        }
        public async static Task<bool> SignUpProcess(string username,string email, string password)
        {
            bool isValid = false;

            // Check for the input first no special character
            if (Checking_Input(username, email, password))
            {
                List<Security_Data_Model_Abtraction>? userID_List = await Security_Table_DB_Control.ReadAllAsyncTablename(user_id_table_name);
                Security_UserId_Record_List = userID_List?.Cast<Security_UserId_Record>().ToList();

                List<Security_Data_Model_Abtraction>? password_List = await Security_Table_DB_Control.ReadAllAsyncTablename(password_table_name);
                Security_Password_Record_List = password_List?.Cast<Security_Password_Record>().ToList();

                // Retrieve the counter
                int index_temp = Security_UserId_Record_List.Count();
                // hasing rawkey and take first 16 character for the input for AES 128bit
                string hasing_value = Hasing_Services.HashString(username);
                // Check if hasing_value == any UserID value
                Security_UserId_Record? account_record = Security_UserId_Record_List.FirstOrDefault(info => info.User_ID == hasing_value);
                if (account_record == null)
                {
                    // Create the key for the string
                    string key = hasing_value.Substring(0, 16 - CountDigits(index_temp)) + index_temp.ToString();
                    // Encrypt the password with the key
                    string encrypted_password = AES_Services_Control.Encrypt(password, key);

                    // Store the information of new account to the database into 2 tables
                    Security_UserId_Record security_UserId_Record = new Security_UserId_Record
                    {
                        Index_UserID = index_temp,
                        User_ID = hasing_value,
                        Email_Address = email,
                    };
                    Security_Password_Record Security_password_Record = new Security_Password_Record
                    {
                        Index_pass = index_temp,
                        Password = encrypted_password,
                    };

                    // Check the input for database
                    bool Check_Username_Input_Valid = await ValidateDataAnnotations(security_UserId_Record, user_id_table_name);
                    bool Check_Password_Input_Valid = await ValidateDataAnnotations(Security_password_Record, password_table_name);
                    if (Check_Username_Input_Valid && Check_Password_Input_Valid)
                    {
                        try
                        {
                            // Write the data to the database
                            bool create_accound_ID = await Security_Table_DB_Control.CreateAsyncTablename(security_UserId_Record, user_id_table_name);
                            bool create_password_ID = await Security_Table_DB_Control.CreateAsyncTablename(Security_password_Record, password_table_name);

                            if (create_accound_ID && create_password_ID)
                            {
                                isValid = true;
                                return isValid;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            isValid = false;
                            return isValid;   // Write Error
                        }
                    }else
                    {
                        isValid = false;
                        return isValid; // In valid input for length
                    }
                }
                else   // else account has been created before -> return false
                {
                    isValid = false;
                    return isValid;
                }
            }
            return isValid;
        }
        private async static Task<bool> ValidateDataAnnotations(object? obj, string tablename)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, results, validateAllProperties: true);

            if (!isValid)
            {
                StringBuilder errorMessageBuilder = new StringBuilder();
                foreach (var validationResult in results)
                {
                    errorMessageBuilder.AppendLine($"Validation error: {validationResult.ErrorMessage}");
                }
                string combinedErrorMessage = errorMessageBuilder.ToString();

                string dataString = JsonSerializer.Serialize(obj);
                string requestType = "Validate DataAnnotations Error";
                string issues = "Not pass the DataAnnotations check for the data model input: " + combinedErrorMessage;
                string requestStatus = "Failed";

                await LogError(requestType, tablename, dataString, requestStatus, issues);

                Console.WriteLine("Not pass the DataAnnotations check for the data model input");
                return isValid;
            }

            return isValid;
        }

        static int CountDigits(int number)
        {
            string numberString = Math.Abs(number).ToString();
            return numberString.Length;
        }

        public static bool Checking_Input(string username, string email, string password)
        {
            bool isValid = true;

            if (!IsInvalidInput(username, "Username"))
            {
                isValid = false;
            }

            if (!IsInvalidEmail(email))
            {
                isValid = false;
            }

            if (!IsInvalidInput(password, "Password"))
            {
                isValid = false;
            }

            return isValid;
        }

        private static bool IsInvalidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email is null or empty.");
                return false;
            }

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, pattern))
            {
                Console.WriteLine("Invalid email format.");
                return false;
            }

            return true;
        }

        private static bool IsInvalidInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"{fieldName} is null or empty.");
                return false;
            }

            string pattern = @"[^a-zA-Z0-9]";

            if (Regex.IsMatch(input, pattern))
            {
                Console.WriteLine($"{fieldName} contains special characters.");
                return false;
            }

            return true;
        }
        private async static Task LogError(string requestType, string tableNumber, string dataString, string requestStatus, string issues)
        {
            bool logStatus = await Analysis_and_reporting_log_data_table.WriteLogData_ProcessAsync(
                requestType,
                DateTime.Now,
                tableNumber,
                dataString,
                requestStatus,
                issues
            );

            if (!logStatus)
            {
                Console.WriteLine("Error: Unable to log error.");
            }
        }
    }
}
