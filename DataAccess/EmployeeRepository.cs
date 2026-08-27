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
                            VALUES (@EmployeeCode, @FirstName, @LastName, @Email, @Phone, @DepartmentId, @jobId, 0, @CreatedBy, NOW());
                            SELECT LAST_INSERT_ID();";
                return conn.ExecuteScalar<int>(sql, employee);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of employees matching optional search and department filters.
        /// </summary>
        /// <param name="search">Optional search term applied to first name, last name, or employee code. If null or whitespace, no search filter is applied.</param>
        /// <param name="deptId">Optional department id filter. When null or less than or equal to 0, no department filter is applied.</param>
        /// <param name="pageIndex">1-based page index to retrieve. Used to calculate the OFFSET for pagination.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <returns>
        /// A tuple containing:
        /// - Records: an enumerable of <see cref="EmployeeGridDto"/> for the requested page.
        /// - TotalCount: the total number of matching records across all pages.
        /// </returns>
        /// <remarks>
        /// This method uses DBHelper.GetConnection() and Dapper to execute the queries. Database-related exceptions from the connection or Dapper are propagated to the caller.
        /// Callers should validate <paramref name="pageIndex"/> and <paramref name="pageSize"/> are positive to avoid unexpected results.
        /// </remarks>
        public (IEnumerable<EmployeeGridDto> Records, int TotalCount) GetPagedEmployees(string search,int? deptId,int pageIndex,int pageSize)
        {
            using (var conn = DBHelper.GetConnection())
            {
                var parameters = new DynamicParameters();

                string where = " WHERE e.IsDeleted = 0 ";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    where += @" AND (
                            e.FirstName LIKE @Search
                            OR e.LastName LIKE @Search
                            OR e.EmployeeCode LIKE @Search
                        )";

                    parameters.Add("Search", "%" + search + "%");
                }

                if (deptId.HasValue && deptId.Value > 0)
                {
                    where += " AND e.DepartmentId = @DeptId ";
                    parameters.Add("DeptId", deptId.Value);
                }

                int total = conn.ExecuteScalar<int>(
                    $"SELECT COUNT(1) FROM Employees e {where}",
                    parameters);

                parameters.Add("Offset", (pageIndex - 1) * pageSize);
                parameters.Add("PageSize", pageSize);

                string sql = $@"
            SELECT
                e.EmployeeId,
                e.EmployeeCode,
                CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
                e.Email,
                e.Phone,
                d.DepartmentName,
                e.CreatedAt
            FROM Employees e
            INNER JOIN Departments d
                ON e.DepartmentId = d.DepartmentId
            {where}
            ORDER BY e.CreatedAt DESC
            LIMIT @Offset, @PageSize";

                var list = conn.Query<EmployeeGridDto>(sql, parameters);

                return (list, total);
            }
        }

        public bool SoftDeleteEmployee(int employeeId, int updatedBy)
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = "UPDATE Employees SET IsDeleted = 1, UpdatedBy = @UpdatedBy, UpdatedAt = NOW() WHERE EmployeeId = @EmployeeId";
                return conn.Execute(sql, new { EmployeeId = employeeId, UpdatedBy = updatedBy }) > 0;
            }
        }
    }
}