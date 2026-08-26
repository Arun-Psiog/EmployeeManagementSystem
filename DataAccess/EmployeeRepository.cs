using Dapper;
using EmployeeManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace EmployeeManagementSystem.DataAccess
{
    public class EmployeeRepository
    {

        /// <summary>
        /// Retrieves all active departments from the database ordered by department name.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{Department}"/> containing active departments.
        /// </returns>
        /// <remarks>
        /// This method opens a database connection via <c>DBHelper.GetConnection()</c> and executes
        /// a SQL query using Dapper's <c>Query{T}</c> extension method.
        /// </remarks>
        /// <exception cref="System.Data.Common.DbException">
        /// Thrown when an error occurs while communicating with the database.
        /// </exception>
        public IEnumerable<Department> GetDepartments()
        {
            using (var conn = DBHelper.GetConnection())
            {
                return conn.Query<Department>("SELECT DepartmentId, DepartmentName FROM Departments Where IsActive = 1 ORDER By DepartmentName");
            }
        }

        /// <summary>
        /// Inserts a new <see cref="Employee"/> record into the Employees table and returns the generated identity value.
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> instance containing values to insert. Expected properties include EmployeeCode, FirstName, LastName, Email, Phone, DepartmentId, jobId and CreatedBy.</param>
        /// <returns>The database-generated identity (LAST_INSERT_ID) for the newly inserted employee.</returns>
        /// <exception cref="System.Data.Common.DbException">Thrown when a database-related error occurs during insertion.</exception>
        /// <remarks>
        /// This method obtains a connection via <c>DBHelper.GetConnection()</c>, uses Dapper's <c>ExecuteScalar&lt;int&gt;</c> to execute the INSERT,
        /// sets IsDeleted to 0, CreatedAt to NOW() in the SQL, and returns the last inserted id.
        /// </remarks>
        public int InsertEmployee(Employee employee)
        {
            using (var conn = DBHelper.GetConnection())
            {
                var sql = @"INSERT INTO Employees (EmployeeCode, FirstName, LastName, Email, Phone, DepartmentId, jobId, IsDeleted, CreatedBy, CreatedAt) 
                            VALUES (@EmployeeCode, @FirstName, @LastName, @Email, @Phone, @DepartmentId, @jobId, 0, @CreatedBy, GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return conn.ExecuteScalar<int>(sql, employee);
            }
        }
    }
}