using MG.UPS.TechnicalAssessment.EmployeeManagement.Model;
using MG.UPS.TechnicalAssessment.EmployeeManagement.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement
{
    public partial class frmEmployeeManagement : Form
    {
        IEmployeeManagementService employeeService;
        IFileManagementService fileManagementService;

        Employee employee;
        SearchEmployee searchEmployee;
        Pagination Pagination { get; set; }

        public frmEmployeeManagement(IEmployeeManagementService employeeService,
                                    IFileManagementService fileManagementService)
        {
            this.employeeService = employeeService;
            this.fileManagementService = fileManagementService;
            InitializeComponent();
        }

        private void frmEmployeeManagement_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            try
            {
                var result = await employeeService.GetEmployees();

                if (result.Code != ResponseCode.SUCCESS_CODE || result.Data == null)
                {
                     MessageBox.Show("Service respond error with Code: " + result.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Pagination = result.Meta.Pagination;

                SetPager();

                dgvEmployees.Refresh();
                dgvEmployees.DataSource = result.Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void Search()
        {
            try
            {
                CreateSearchObject();
                var result = await employeeService.SearchEmployee(searchEmployee);

                if (result.Code != ResponseCode.SUCCESS_CODE || result.Data == null)
                {
                    MessageBox.Show("Service respond error with Code: " + result.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Pagination = result.Meta.Pagination;

                SetPager();

                dgvEmployees.Refresh();
                dgvEmployees.DataSource = result.Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void SetPager()
        {
            if (Pagination.Total == 0)
            {
                lblPageNumber.Text = "0";
                lblTotalPages.Text = "0";
                lblTotalRecords.Text = "0";
            }
            else
            {
                lblPageNumber.Text = Pagination.Page.ToString();
                lblTotalPages.Text = Pagination.Pages.ToString();
                lblTotalRecords.Text = Pagination.Total.ToString();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void CreateSearchObject()
        {
            searchEmployee = new SearchEmployee
            {
                Name = search_name.Text,
                Email = search_email.Text,
                Gender = search_gender.Text,
                Status = search_status.Text,
                Page = Pagination.Page < 1 ? 1 : Pagination.Page
            };
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearSearch();
        }

        private void ClearSearch()
        {
            search_name.Text = "";
            search_email.Text = "";
            search_gender.Text = "";
            search_status.Text = "";

            Pagination = new Pagination();

            Search();
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SetForm(employee = new Employee
            {
                Id = (int)dgvEmployees.Rows[e.RowIndex].Cells["Id"].Value,
                Name = (string)dgvEmployees.Rows[e.RowIndex].Cells["EmployeeName"].Value,
                Email = (string)dgvEmployees.Rows[e.RowIndex].Cells["Email"].Value,
                Gender = (string)dgvEmployees.Rows[e.RowIndex].Cells["Gender"].Value,
                Status = (string)dgvEmployees.Rows[e.RowIndex].Cells["Status"].Value,
                Created_At = (DateTime)dgvEmployees.Rows[e.RowIndex].Cells["Created_At"].Value,
                Updated_At = (DateTime)dgvEmployees.Rows[e.RowIndex].Cells["Updated_At"].Value,
            });
        }

        private void SetForm(Employee employee)
        {
            txtId.Text = employee.Id.ToString();
            txtName.Text = employee.Name;
            txtEmail.Text = employee.Email;
            cmbGender.Text = employee.Gender;
            cmbStatus.Text = employee.Status;
            lblCreatedAt.Text = employee.Created_At == DateTime.MinValue ? "" : employee.Created_At.ToString("dd.MM.yyyy");
            lblUpdatedAt.Text = employee.Updated_At == DateTime.MinValue ? "" : employee.Updated_At.ToString("dd.MM.yyyy");
        }

        private void GetFormData()
        {
            int.TryParse(txtId.Text, out var id);
            employee = new Employee
            {
                Id = id,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Gender = cmbGender.Text,
                Status = cmbStatus.Text,
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now,
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            GetFormData();

            if (employee.Id == 0)
                SaveEmployee();
            else
                UpdateEmployee();
        }

        private async void SaveEmployee()
        {
            try
            {
                if (await IsEmployeeExists())
                {
                    MessageBox.Show("Record already Exists", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = await employeeService.SaveEmployee(employee);

                if (!(result.Code == ResponseCode.SUCCESS_CODE || result.Code == ResponseCode.RESOURCE_CREATED) || result.Data == null)
                {
                    MessageBox.Show("Service respond error with Code: " + result.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Employee Saved!");
                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async Task<bool> IsEmployeeExists()
        {
            var searchResult = await employeeService.SearchEmployee(new SearchEmployee { Name = txtName.Text });

            if (searchResult.Code != ResponseCode.SUCCESS_CODE || searchResult.Data == null)
            {
                throw new Exception("Service respond error!");
            }

            return searchResult.Data.Count() > 0 ? true : false;
        }

        private async void UpdateEmployee()
        {
            try
            {
                var result = await employeeService.UpdateEmployee(employee.Id, employee);

                if (result.Code != ResponseCode.SUCCESS_CODE || result.Data == null)
                {
                    MessageBox.Show("Service respond error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Employee Updated!");
                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool ValidateForm()
        {
            string errorMessage = string.Empty;

            if (txtName.Text == "")
                errorMessage = errorMessage + "Name is required." + "\n";

            if (txtEmail.Text == "")
                errorMessage = errorMessage + "Email is required." + "\n";

            if (cmbGender.Text == "")
                errorMessage = errorMessage + "Gender is required." + "\n";

            if (cmbStatus.Text == "")
                errorMessage = errorMessage + "Status is required." + "\n";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetForm(employee = new Employee());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            PreviousPage();
        }

        private void PreviousPage()
        {
            if (Pagination != null && Pagination.Page > 1)
            {
                int.TryParse(txtId.Text, out var id);
                if (id > 0)
                    SetForm(employee = new Employee());

                Pagination.Page--;
                Search();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            NextPage();
        }
        private void NextPage()
        {
            if (Pagination != null && Pagination.Page < Pagination.Pages)
            {
                int.TryParse(txtId.Text, out var id);
                if (id > 0)
                    SetForm(employee = new Employee());

                Pagination.Page++;
                Search();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private async void ExportToCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (*.csv)|*.csv";
            saveFileDialog.FileName = "Export_" + DateTime.Now.ToString("ddMMyyyyss") + ".csv";
            saveFileDialog.Title = "Export data";
            saveFileDialog.ShowDialog();

            CreateSearchObject();

            // The below call can be replaced by loop through datagridview.
            // This will download the current page visible in datagridview.
            // User service only provides paged data. 
            // To download whole data we need to call page by page
            var result = await employeeService.SearchEmployee(searchEmployee);

            if (result.Code != ResponseCode.SUCCESS_CODE || result.Data == null)
            {
                MessageBox.Show("Service respond error with Code: " + result.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fileManagementService.ExportFile<Employee>(saveFileDialog.FileName, result.Data, FileType.Csv);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }
        private async void Delete()
        {
            try
            {
                GetFormData();
                var result = await employeeService.DeleteEmployee(employee.Id);

                if (!(result.Code == ResponseCode.SUCCESS_CODE || result.Code == ResponseCode.RESOURCE_DELETED) || result.Data != null)
                {
                    MessageBox.Show(@"Service respond error with Code: " + result.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Employee Deleted!");
                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnCLearForm_Click(object sender, EventArgs e)
        {
            clearForm();
        }
        private void clearForm()
        {
            SetForm(employee = new Employee());
            Search();
        }
    }
}
