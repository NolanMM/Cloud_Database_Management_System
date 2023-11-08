﻿namespace Cloud_Database_Management_System.Models.Group_Data_Models.Group_1_Data_Model_Tables
{
    public class UserView : Group_1_Record_Interface
    {
        public string User_Id { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public DateTime Timestamp { get; set; }

        public UserView() {
            User_Id = string.Empty;
            Timestamp = DateTime.MinValue;
            End_Date = new DateOnly();
            Start_Date = new DateOnly();
        }
    }
}