using MG.UPS.TechnicalAssessment.EmployeeManagement.Model;
using System;
using System.Collections.Generic;

namespace EmployeeManagementTest
{
    internal static class Stubs
    {
        public static ResultSet GetStubbedResultSet()
        {
            return new ResultSet
            {
                Code = ResponseCode.SUCCESS_CODE,
                Meta = null,
                Data = GetStubbedEmployeeList()
            };
        }

        public static Result GetStubbedResult()
        {
            return new Result
            {
                Code = ResponseCode.SUCCESS_CODE,
                Meta = null,
                Data = GetStubbedEmployee()
            };
        }

        public static Result GetCreatedStubbedResult()
        {
            return new Result
            {
                Code = ResponseCode.RESOURCE_CREATED,
                Meta = null,
                Data = GetStubbedEmployee()
            };
        }

        public static Result GetDeletedStubbedResult()
        {
            return new Result
            {
                Code = ResponseCode.RESOURCE_DELETED,
                Meta = null,
                Data = null
            };
        }

        public static Employee GetStubbedEmployee()
        {
            return new Employee
            {
                Id = 1405,
                Name = "test EmployeeName",
                Email = "tempEmployee",
                Gender = "Male",
                Status = "Active",
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };
        }

        public static List<Employee> GetStubbedEmployeeList()
        {
            return new List<Employee>() {
                new Employee
                {
                    Id = 1405,
                    Name = "test EmployeeName",
                    Email = "tempEmployee",
                    Gender = "Male",
                    Status = "Active",
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now
                },
                new Employee
                {
                    Id = 1406,
                    Name = "test EmployeeName1",
                    Email = "tempEmployee1",
                    Gender = "Male",
                    Status = "Active",
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now
                }
            };
        }
    }
}
