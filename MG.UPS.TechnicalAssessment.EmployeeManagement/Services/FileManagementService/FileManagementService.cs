using System.Collections.Generic;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement.Services
{
    public interface IFileManagementService
    {
        void ExportFile<T>(string filename, IEnumerable<T> data, FileType fileType);
    }

    public enum FileType
    {
        Csv = 0,
        Tab
    }

    public class FileManagementService : IFileManagementService
    {
        ICsvManagementService csvFileManagement;

        public FileManagementService(ICsvManagementService csvFileManagement)
        {
            this.csvFileManagement = csvFileManagement;
        }
        public void ExportFile<T>(string filename, IEnumerable<T> data, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Csv: csvFileManagement.ExportFile<T>(filename, data); break;
                case FileType.Tab: break; //implementation for tab
                default: csvFileManagement.ExportFile<T>(filename, data); break;// let default as Csv
            }
        }
    }
}
