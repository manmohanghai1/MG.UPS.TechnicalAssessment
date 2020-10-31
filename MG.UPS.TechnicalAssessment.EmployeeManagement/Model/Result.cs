using System.Collections.Generic;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement.Model
{
    public class Pagination
    {
        public int Total { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
        public int limit { get; set; }
    }    
    public class Meta
    {
        public Pagination Pagination { get; set; }
    }
    public class ResultSet
    {
        public string Code { get; set; }
        public Meta Meta { get; set; }
        public IEnumerable<Employee> Data { get; set; }
    }
    public class Result
    {
        public string Code { get; set; }
        public Meta Meta { get; set; }
        public Employee Data { get; set; }
    }
}
