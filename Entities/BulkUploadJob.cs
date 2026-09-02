using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagementSystem.Entities
{
    public class BulkUploadJob
    {
        public int JobId { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}