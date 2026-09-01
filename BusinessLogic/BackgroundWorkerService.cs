using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.IO;
using Dapper;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
using System.Web.Hosting;

namespace EmployeeManagementSystem.BusinessLogic
{
    public class BackgroundWorkerService
    {
        private static Timer _workerTimer;
        public static void Start()
        {
            _workerTimer = new Timer(ExecuteQueue, null, 1000, 5000);
        }
        private static void ExecuteQueue(object state)
        {
            try
            {
                string stagingDir = HostingEnvironment.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(stagingDir))
                {
                    Directory.CreateDirectory(stagingDir);
                }

                using (var conn = DBHelper.GetConnection())
                {
                    var queuedJobs = conn.Query<BulkUploadJob>("SELECT * FROM BulkUploadJobs WHERE Status = 'Queued' ORDER By JobId ASC");
                    foreach (var job in queuedJobs)
                    {
                        // Process the job
                        // For example, you can call a method to process the job based on its type
                        ProcessJob(job, stagingDir);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }
        private static void ProcessJob(BulkUploadJob job, string stagingDir)
        {
            string filePath = Path.Combine(stagingDir, job.FileName);
            if (!File.Exists(filePath))
            {
                return;
            }
            // Process the file
            // For example, read the file and insert data into the database
            // After processing, update the job status to Completed
            using (var conn = DBHelper.GetConnection())
            {
                conn.Execute("UPDATE BulkUploadJobs SET Status = 'Processing' WHERE JobId = @JobId", new { job.JobId });
            }
            var lines = File.ReadAllLines(filePath).Skip(1).ToList();
            int count = 0;
            foreach (var line in lines)
            {
                count++;
                var cols = line.Split(',');
                if (cols.Length < 6) continue;
                using (var conn = DBHelper.GetConnection())
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var emp = new Employee
                        {
                            EmployeeCode = cols[0].Trim(),
                            FirstName = cols[1].Trim(),
                            LastName = cols[2].Trim(),
                            Email = cols[3].Trim(),
                            Phone = cols[4].Trim(),
                            DepartmentId = int.Parse(cols[5].Trim()),
                            jobId = job.JobId,
                            CreatedBy = job.CreatedBy,
                        };
                        string sql = @"INSERT INTO Employees (EmployeeCode,FirstName, LastName, Email, Phone, DepartmentId, JobId, CreatedBy, CreatedAt) 
                                           VALUES (@EmployeeCode, @FirstName, @LastName, @Email, @Phone, @DepartmentId, @jobId, @CreatedBy, NOW());
                                           SELECT LAST_INSERT_ID()";
                        int empId = conn.ExecuteScalar<int>(sql, emp, tran);
                        tran.Commit();
                        EmailService.SendWelcomeEmail(empId, emp.Email, emp.FirstName);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }

                }
            }
            using (var conn = DBHelper.GetConnection())
            {
                conn.Execute("UPDATE BulkUploadJobs SET Status = 'Completed', ProcessedRows = @Count, CompletedDate = NOW() WHERE JobId = @JobId", new { Count = count, job.JobId });
            }
        }

        /// <summary>
        /// Performs a transactional rollback of a bulk upload job by soft-deleting associated employees
        /// and marking the job as rolled back for auditing purposes.
        /// </summary>
        /// <param name="jobId">The identifier of the bulk upload job to roll back.</param>
        /// <param name="adminUserId">The identifier of the admin user performing the rollback (used for audit logging).</param>
        /// <returns>True if the rollback completed and the transaction committed; otherwise false.</returns>
        /// <remarks>
        /// The method executes two database updates inside a single transaction:
        /// - Marks employees related to the specified job as deleted (IsDeleted = 1).
        /// - Updates the BulkUploadJobs record to set Status = 'RolledBack', RolledBackBy and RolledBackDate.
        /// An activity log entry is written when the rollback succeeds. Exceptions are caught, the transaction is rolled back,
        /// and the method returns false on failure.
        /// </remarks>
        public static bool RollbackBatch(int jobId, int adminUserId)
        {
            using (var conn = DBHelper.GetConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    // Mark employees as deleted associated with the job
                    conn.Execute("UPDATE Employees SET IsDeleted = 1 WHERE JobId = @JobId", new { JobId = jobId }, tran);
                    // Update the job status to 'RolledBack'
                    conn.Execute("UPDATE BulkUploadJobs SET Status = 'RolledBack', RolledBackBy = @AdminId, RolledBackDate = NOW() WHERE JobId = @JobId", new { JobId = jobId, AdminId = adminUserId }, tran);
                    tran.Commit();
                    ActivityLogger.Log(adminUserId, "BATCH_ROLLBACK", "BulkUploadJobs", jobId, $"Jobs {jobId} rolled back via soft delete.");
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return false;
                }
            }

        }
    }
}