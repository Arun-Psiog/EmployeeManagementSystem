using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EmployeeManagementSystem.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EmployeeManagementSystem.BusinessLogic
{

    /// <summary>
    /// Provides helper methods to export employee grid data into common report formats.
    /// </summary>
    public static class ReportExporter
    {
        /// <summary>
        /// Exports the provided employee grid data to a CSV byte array encoded as UTF-8.
        /// </summary>
        /// <param name="data">A sequence of <see cref="EmployeeGridDto"/> representing rows to include in the CSV. Must not be null.</param>
        /// <returns>A UTF-8 encoded byte array containing the CSV representation of <paramref name="data"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
        public static byte[] ExportToCsv(IEnumerable<EmployeeGridDto> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var sb = new StringBuilder();
            sb.AppendLine("EmployeeCode,FullName,Email,Phone,DepartmentName,CreatedAt");

            foreach (var item in data)
            {
                sb.AppendLine($"{item.EmployeeCode},{item.FullName},{item.Email},{item.Phone},{item.DepartmentName},{item.CreatedAt:yyyy-MM-dd}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Exports the provided employee grid data to a PDF document and returns the PDF as a byte array.
        /// </summary>
        /// <param name="data">A sequence of <see cref="EmployeeGridDto"/> representing rows to include in the PDF. Must not be null.</param>
        /// <returns>A byte array containing the generated PDF document.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
        /// <remarks>
        /// Uses iTextSharp to generate a simple A4 PDF with a header and a table containing the employee data.
        /// </remarks>
        public static byte[] ExportToPdf(IEnumerable<EmployeeGridDto> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Add(new Paragraph("Employee Management System - Report Summary", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                doc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(5) { WidthPercentage = 100 };
                table.AddCell("Code"); table.AddCell("Name"); table.AddCell("Email"); table.AddCell("Department"); table.AddCell("Created");

                foreach (var item in data)
                {
                    table.AddCell(item.EmployeeCode);
                    table.AddCell(item.FullName);
                    table.AddCell(item.Email);
                    table.AddCell(item.DepartmentName);
                    table.AddCell(item.CreatedAt.ToString("yyyy-MM-dd"));
                }

                doc.Add(table);
                doc.Close();
                return ms.ToArray();

            }

       }
    }
}
