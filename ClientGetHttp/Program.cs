﻿using ClientGetHttp.DatabaseServices.Services.Network_Database_Services;

namespace ClientGetHttp.DatabaseServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Fetch data for a specific table");
            Console.WriteLine("2. Test ProcessDataForGetTableCorrespondingUserID function");
            Console.WriteLine("3. Test Product_Group_Database_Services.GetDataServiceAsync function");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option == 1)
                {
                    Console.WriteLine("Enter the table number (0=UserView, 1=PageView, 2=SaleTransaction, 3=Feedback): ");

                    if (int.TryParse(Console.ReadLine(), out int tableNumber))
                    {
                        var data = await Database_Centre.GetDataForDatabaseServiceID(tableNumber);

                        if (data != null)
                        {
                            Console.WriteLine($"Data received and processed successfully for table {tableNumber}:");

                            foreach (var item in data)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid table number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
                else if (option == 2)
                {
                    Console.WriteLine("Enter the UserID for testing ProcessDataForGetTableCorrespondingUserID function: ");
                    string userID = Console.ReadLine();

                    Dictionary<string, (string, string, string, string)>? result = await Database_Centre.ProcessDataForGetTableCorrespondingUserID_Database(userID);

                    if (result != null)
                    {
                        Console.WriteLine("Results of ProcessDataForGetTableCorrespondingUserID function:");

                        int productIdMaxLength = result.Keys.Max(k => k.Length);
                        int userViewNumberMaxLength = result.Values.Max(v => v.Item1.Length);
                        int pageViewNumberMaxLength = result.Values.Max(v => v.Item2.Length);
                        int totalQuantityMaxLength = result.Values.Max(v => v.Item3.Length);
                        int dateMaxLength = result.Values.Max(v => v.Item4.Length);

                        string header = $"| {"ProductId".PadRight(productIdMaxLength)} | {"UserViewNumber".PadRight(userViewNumberMaxLength)} | {"PageViewNumber".PadRight(pageViewNumberMaxLength)} | {"TotalQuantity".PadRight(totalQuantityMaxLength)} | {"Date".PadRight(dateMaxLength)} |";
                        Console.WriteLine(header);

                        foreach (var entry in result)
                        {
                            string productIdColumn = entry.Key.PadRight(productIdMaxLength);
                            string userViewNumberColumn = entry.Value.Item1.PadRight(userViewNumberMaxLength + 13);
                            string pageViewNumberColumn = entry.Value.Item2.PadRight(pageViewNumberMaxLength + 13);
                            string totalQuantityColumn = entry.Value.Item3.PadRight(totalQuantityMaxLength + 12);
                            string dateColumn = entry.Value.Item4.PadRight(dateMaxLength);

                            string formattedRow = $"| {productIdColumn} | {userViewNumberColumn} | {pageViewNumberColumn} | {totalQuantityColumn} | {dateColumn} |";
                            Console.WriteLine(formattedRow);
                        }
                    }
                    
                }
                else if (option == 3)
                {
                    List<ResponseData>? return_response = await Product_Group_Database_Services.GetDataServiceAsync();

                    if (return_response != null)
                    {
                        foreach (var responseData in return_response)
                        {
                            Console.WriteLine($"ProductId: {responseData.pid}, SupplierId: {responseData.sid}, " +
                                              $"Name: {responseData.Name}, Description: {responseData.Description}, " +
                                              $"Image: {responseData.Image}, Category: {responseData.Category}, " +
                                              $"Price: {responseData.Price}, Stock: {responseData.Stock}, " +
                                              $"Sales: {responseData.Sales}, Rating: {responseData.Rating}, Clicks: {responseData.Clicks}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data returned from the service.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}
