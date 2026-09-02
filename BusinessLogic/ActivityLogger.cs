using Dapper;
using EmployeeManagementSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagementSystem.BusinessLogic
{
    public class ActivityLogger
    {
        public static void Log(int? userId, string action, string entity, int? entityId, string details)
        {
             string  ip = HttpContext.Current?.Request?.UserHostAddress ?? "127.0.0.1";
            using (var conn = DBHelper.GetConnection())
            {
                 string sql = "INSERT INTO ActivityLog (UserId, Action, EntityName, EntityId, Details, IPAddress, Timestamp) VALUES (@UserId, @Action, @EntityName, @EntityId, @Details, @IPAddress, Now())";
                conn.Execute(sql, new { UserId = userId, Action = action, EntityName = entity, EntityId = entityId, Details = details, IPAddress = ip });
            }
        }
    }
}