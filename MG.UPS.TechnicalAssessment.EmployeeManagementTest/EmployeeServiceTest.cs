using MG.UPS.TechnicalAssessment.EmployeeManagement.Model;
using MG.UPS.TechnicalAssessment.EmployeeManagement.Services;
using MG.UPS.TechnicalAssessment.ServiceClientRest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeManagementTest
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private Mock<IRestClient> restClient;
        private IEmployeeManagementService employeeService;

        [TestInitialize]
        public void Initialize()
        {
            restClient = new Mock<IRestClient>();
            employeeService = new EmployeeManagementService(restClient.Object);
        }

        [TestMethod]
        public async Task GetEmployee()
        {
            var stubbedResult = Stubs.GetStubbedResultSet();

            restClient.Setup(r => r.GetAsync<ResultSet>(It.IsAny<string>()))
                .ReturnsAsync(stubbedResult);

            var result = await employeeService.GetEmployees();

            Assert.AreEqual(result.Data, stubbedResult.Data);

        }
        [TestMethod]
        public async Task SaveEmployee_Should_Save()
        {
            var stubbedResult = Stubs.GetCreatedStubbedResult();
            var stubbedEmployee = Stubs.GetStubbedEmployee();

            restClient.Setup(r => r.PostAsync<Result>(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(stubbedResult);

            var result = await employeeService.SaveEmployee(stubbedEmployee);

            Assert.AreEqual(result.Code, stubbedResult.Code);
        }

        [TestMethod]
        public async Task SaveEmployee_Should_Save_Throw_IfEmployee_NotExist()
        {
            var stubbedResult = Stubs.GetCreatedStubbedResult();

            restClient.Setup(r => r.PostAsync<Result>(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(stubbedResult);
            await Assert.ThrowsExceptionAsync<Exception>(async () => { await employeeService.SaveEmployee(null); });
        }

        [TestMethod]
        public async Task UpdateEmployee()
        {
            var stubbedResult = Stubs.GetStubbedResult();
            var stubbedEmployee = Stubs.GetStubbedEmployee();

            restClient.Setup(r => r.PutAsync<Result>(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(stubbedResult);

            var result = await employeeService.UpdateEmployee(stubbedEmployee.Id, stubbedEmployee);

            Assert.AreEqual(result.Code, stubbedResult.Code);

        }

        [TestMethod]
        public async Task UpdateEmployee_Should_Save_Throw_IfEmployee_NotExist()
        {
            var stubbedResult = Stubs.GetStubbedResult();

            restClient.Setup(r => r.PostAsync<Result>(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(stubbedResult);
            await Assert.ThrowsExceptionAsync<Exception>(async () => { await employeeService.UpdateEmployee(1, null); });
        }

        [TestMethod]
        public async Task UpdateEmployee_Should_Save_Throw_IfId_NotExist()
        {
            var stubbedResult = Stubs.GetStubbedResult();

            restClient.Setup(r => r.PostAsync<Result>(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(stubbedResult);
            await Assert.ThrowsExceptionAsync<Exception>(async () => { await employeeService.UpdateEmployee(-1, null); });
        }

        [TestMethod]
        public async Task DeleteEmployee()
        {
            var stubbedResult = Stubs.GetDeletedStubbedResult();
            var id = 1;

            restClient.Setup(r => r.DeleteAsync<Result>(It.IsAny<string>()))
                .ReturnsAsync(stubbedResult);

            var result = await employeeService.DeleteEmployee(1);

            Assert.AreEqual(result.Code, stubbedResult.Code);
        }
        [TestMethod]
        public async Task DeleteEmployee_Should_Fail_IfNoIdPassed()
        {
            var stubbedResult = Stubs.GetDeletedStubbedResult();
            var id = 1;

            restClient.Setup(r => r.DeleteAsync<Result>(It.IsAny<string>()))
                .ReturnsAsync(stubbedResult);

            await Assert.ThrowsExceptionAsync<Exception>(async () => { await employeeService.DeleteEmployee(It.IsAny<int>()); });
        }
    }
}
