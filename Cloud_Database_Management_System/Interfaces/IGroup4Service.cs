﻿using Cloud_Database_Management_System.Controllers;
using Cloud_Database_Management_System.Models.Group_Data_Models;

namespace Cloud_Database_Management_System.Interfaces
{
    public interface IGroup4Service : IGroupService
    {
        bool TryProcessData(int groupId, object data, out Group4_Data_Model result);

    }
}
