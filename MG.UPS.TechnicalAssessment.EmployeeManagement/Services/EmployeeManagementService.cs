using MG.UPS.TechnicalAssessment.EmployeeManagement.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MG.UPS.TechnicalAssessment.ServiceClientRest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement.Services
{
    public interface IEmployeeManagementService
    {
        Task<ResultSet> GetEmployees();
        Task<ResultSet> SearchEmployee(SearchEmployee searchEmployee);
        Task<Result> SaveEmployee(Employee employee);
        Task<Result> UpdateEmployee(int id, Employee employee);
        Task<Result> DeleteEmployee(int id);
    }

    public class EmployeeManagementService : IEmployeeManagementService
    {
        private string api = "/users";
        IRestClient restClient;

        public EmployeeManagementService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        /// <summary>
        /// GetEmployees
        /// </summary>
        /// <returns></returns>
        public async Task<ResultSet> GetEmployees()
        {
            var response = await restClient.GetAsync<ResultSet>(api);
            return response;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="searchEmployee"></param>
        /// <returns></returns>
        public async Task<ResultSet> SearchEmployee(SearchEmployee searchEmployee)
        {
            if (searchEmployee == null)
                throw new Exception("Invalid Search Object.");

            try
            {
                var endpoint = api + "?" + ComposeParameters(searchEmployee);
                var response = await restClient.GetAsync<ResultSet>(endpoint);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private string ComposeParameters(SearchEmployee searchEmployee)
        {
            string param = string.Empty;
            foreach (var propertyInfo in searchEmployee.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(searchEmployee) != null
                        && !string.IsNullOrEmpty(propertyInfo.GetValue(searchEmployee).ToString()))
                        param = param + propertyInfo.Name.ToLower() + "=" + propertyInfo.GetValue(searchEmployee).ToString() + "&";
                }
                if (propertyInfo.PropertyType == typeof(int))
                {
                    int.TryParse(propertyInfo.GetValue(searchEmployee).ToString(), out var propIntVal);
                    if (propIntVal > 0)
                        param = param + propertyInfo.Name.ToLower() + "=" + propIntVal.ToString() + "&";
                }
            }
            if (!string.IsNullOrEmpty(param))
                param = param.Substring(0, param.Length - 1);
            return param;
        }

        /// <summary>
        /// Save Employee (Post Request)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Result> SaveEmployee(Employee employee)
        {
            if (employee == null)
                throw new Exception("Invalid Employee Data.");

            var httpContent = GetHttpContent(employee);
            var response = await restClient.PostAsync<Result>(api, httpContent);
            return response;
        }

        private StringContent GetHttpContent(Employee employee)
        {
            var serialized = JsonConvert.SerializeObject(employee, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Update (Put Request)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Result> UpdateEmployee(int id, Employee employee)
        {
            if (!(id > 0) || employee == null)
                throw new Exception("Invalid Employee Data.");

            var httpContent = GetHttpContent(employee);
            var endpoint = api + "/" + id.ToString();
            var response = await restClient.PutAsync<Result>(endpoint, httpContent);
            return response;
        }

        /// <summary>
        /// Delete (Delete Request)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Result> DeleteEmployee(int id)
        {
            if (!(id > 0))
                throw new Exception("Invalid Employee Data.");

            var endpoint = api + "/" + id.ToString();
            var response = await restClient.DeleteAsync<Result>(endpoint);
            return response;
        }        
    }
}
