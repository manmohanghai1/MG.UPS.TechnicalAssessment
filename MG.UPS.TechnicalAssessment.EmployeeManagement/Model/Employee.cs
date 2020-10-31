using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
    public class SearchEmployee : Employee
    {
        public int Page { get; set; }
    }
    public static class ResponseCode
    {
        public const string SUCCESS_CODE = "200";
        public const string RESOURCE_CREATED = "201";
        public const string RESOURCE_DELETED = "204";
    }
}
