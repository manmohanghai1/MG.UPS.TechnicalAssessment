using MG.UPS.TechnicalAssessment.EmployeeManagement.Model;
using MG.UPS.TechnicalAssessment.EmployeeManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmployeeManagementTest
{
    [TestClass]
    public class FileManagementServiceTest
    {
        private Mock<ICsvManagementService> csvManagementService;
        private IFileManagementService fileManagementService;

        [TestInitialize]
        public void Initialize()
        {
            csvManagementService = new Mock<ICsvManagementService>();
            fileManagementService = new FileManagementService(csvManagementService.Object);
        }

        [TestMethod]
        public void Should_able_Call_ExportCSV()
        {
            var stubbedEmployees = Stubs.GetStubbedEmployeeList();
            csvManagementService.Setup(c => c.ExportFile<Employee>(It.IsAny<string>(), stubbedEmployees));                

            fileManagementService.ExportFile<Employee>(It.IsAny<string>(), stubbedEmployees, FileType.Csv);

            csvManagementService.Verify(c=> c.ExportFile<Employee>(It.IsAny<string>(), stubbedEmployees));
        }
    }
}
