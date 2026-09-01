using EmployeeManagementSystem.DataAccess;
using System;
using Dapper;
using System.Net.Mail;

namespace EmployeeManagementSystem.BusinessLogic
{
    public class EmailService
    {
        public static void SendWelcomeEmail(int employeeId, string toEmail, string firstName)
        {
            try
            {
                using (var mail = new MailMessage("no-reply@employeemanagement.com", toEmail))
                {
                    mail.Subject = "Welcome to the Company!";
                    mail.Body = "Hi {Firstname}, \n\n Your employee account has been created successfully. Your Employee ID is {EmployeeId}. \n\nBest regards,\nHR Team"
                        .Replace("{Firstname}", firstName)
                        .Replace("{EmployeeId}", employeeId.ToString());
                    //client.SendMail(mail); // Uncomment this line to actually send the email
                }
                LogEmail(employeeId, toEmail, "Welcome Email", null);

            } 
            catch (Exception ex)
            {
                LogEmail(employeeId, toEmail, "Failed to Send Welcome Email", ex.Message);
            }
        }

        public static void LogEmail(int employeeId, string email, string status, string errorMessage)
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = "INSERT INTO EmailLog (EmployeeId, EmailType, RecipientEmail, Status, ErrorMessage, SentData) VALUES (@EmployeeId, 'Welcome_Email', @Email, @Status, @Error, NOW())";
                conn.Execute(sql, new { EmployeeId = employeeId, Email = email, Status = status, ErrorMessage = errorMessage});
            }
        }
    }
}