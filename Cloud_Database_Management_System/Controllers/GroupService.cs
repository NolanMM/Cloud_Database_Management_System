﻿using Cloud_Database_Management_System.Models.Group_Data_Models;
using System.Text.Json;
using Cloud_Database_Management_System.Interfaces.Database_Services_Interfaces;

namespace Cloud_Database_Management_System.Controllers
{
    public abstract class GroupService : IGroupService
    {
        private readonly IGroupService groupRepository;
        private DateTime _created;
        
        public bool TryProcessData(int groupId, object data, out object result)
        {
            DateTime create = DateTime.Now;
            this._created = create;
            if (groupId == 0)
            {
                result = null;
                return false;
            }
            else
            {
                switch (groupId)
                {
                    case 1:
                        result = ProcessDataForGroup1(data);
                        if(result == null)
                        {
                            return false;
                        }else
                        {
                            return true;
                        }
                    case 2:
                        result = ProcessDataForGroup2(data);
                        if (result == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    case 3:
                        result = ProcessDataForGroup3(data);
                        if (result == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    case 4:
                        result = ProcessDataForGroup4(data);
                        if (result == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    case 5:
                        result = ProcessDataForGroup5(data);
                        if (result == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    case 6:
                        result = ProcessDataForGroup6(data);
                        if (result == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    default:
                        result = null;
                        return false;
                }
            }
        }

        private Group1_Data_Model? ProcessDataForGroup1(object data)
        {
            if(data == null) { return null; }
            try
            {
                return JsonSerializer.Deserialize<Group1_Data_Model>(data.ToString());
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Group2_Data_Model? ProcessDataForGroup2(object data)
        {
            if (data == null) { return null; }
            try
            {
                return JsonSerializer.Deserialize<Group2_Data_Model>(data.ToString());
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Group3_Data_Model? ProcessDataForGroup3(object data)
        {
            if (data == null) { return null; }
            try
            {
                Group3_Data_Model data_return = JsonSerializer.Deserialize<Group3_Data_Model>(data.ToString());
                return data_return;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Group4_Data_Model? ProcessDataForGroup4(object data)
        {
            if (data == null) { return null; }
            try
            {
                return JsonSerializer.Deserialize<Group4_Data_Model>(data.ToString());
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Group5_Data_Model? ProcessDataForGroup5(object data)
        {
            if (data == null) { return null; }
            try
            {
                return JsonSerializer.Deserialize<Group5_Data_Model>(data.ToString());
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Group6_Data_Model? ProcessDataForGroup6(object data)
        {
            if (data == null) { return null; }
            try
            {
                return JsonSerializer.Deserialize<Group6_Data_Model>(data.ToString());
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
