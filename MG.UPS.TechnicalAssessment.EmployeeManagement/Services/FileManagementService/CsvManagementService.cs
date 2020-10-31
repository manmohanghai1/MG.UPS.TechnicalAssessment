
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement.Services
{
    public interface ICsvManagementService
    {
        void ExportFile<T>(string filename, IEnumerable<T> data);
    }
    public class CsvManagementService : ICsvManagementService
    {
        public void ExportFile<T>(string filename, IEnumerable<T> data)
        {
            string output = string.Empty;
            if (data != null && data.Count() > 0)
            {
                //Header Row
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    output += propertyInfo.Name + ",";
                }
                output = output.Substring(0, output.Length - 1);
                output += "\r\n";

                //Data Rows
                foreach (var row in data)
                {
                    foreach (var propertyInfo in row.GetType().GetProperties())
                    {
                        output += propertyInfo.GetValue(row).ToString() + ",";
                    }
                    output = output.Substring(0, output.Length - 1);
                    output += "\r\n";
                }
            }
            File.WriteAllText(filename, output, Encoding.UTF8);
        }
    }
}
