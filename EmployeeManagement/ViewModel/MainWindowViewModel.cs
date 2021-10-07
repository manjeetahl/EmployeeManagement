using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManagement.EmployeService;
using EmployeeManagement.Model;
using EmployeeManagement.Helper;
using System.Windows;

namespace EmployeeManagement.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region Initialiaze
        public void Initialiaze()
        {
            CmdGetAllEmployee = new DelegateCommand(GetAllEmployee, CanGetAllEmployee);
            CmdGetEmployeeById = new DelegateCommand(GetEmployeeById, CanGetEmployeeById);
            CmdGetEmployeeByName = new DelegateCommand(GetEmployeeByName, CanGetEmployeeByName);
            CmdCreateEmployee = new DelegateCommand(CreateEmployee, CanCreateEmployee);
            CmdDeleteEmployee = new DelegateCommand(DeleteEmployee, CanDeleteEmployee);
            CmdUpdateEmployee = new DelegateCommand(UpdateEmployee, CanUpdateEmployee);
            CmdExportData = new DelegateCommand(ExportData, CanExportData);
            CmdGeTEmployeePage = new DelegateCommand(GetEmployeeForPage, CanGetEmployeeForPage);
            CmdResetEmployee = new DelegateCommand(ResetEmployee, CanResetEmployee);
            ErrorMsg = string.Empty;
            EmpService = new EmployeeService();
        }
        #endregion
        #region Properties
        private ObservableCollection<Employee> _oEmployeeList;
        public ObservableCollection<Employee> EmployeeList
        {
            get
            {
                if (_oEmployeeList == null)
                {
                    _oEmployeeList = new ObservableCollection<Employee>();
                }
                return _oEmployeeList;
            }
            set
            {
                _oEmployeeList = value;
                NotifyPropertyChanged("EmployeeList");
            }
        }
        public EmployeeService EmpService { get; set; }
     

        private EmployeeResponse _oEmpResponse;
        public EmployeeResponse EmpResponse
        {
            get { return _oEmpResponse; }
            set
            {
                _oEmpResponse = value;
                if(_oEmployeeList != null)
                {
                    this.TotalRecords = _oEmpResponse.meta.pagination.total;
                    this.CurrentPageNumber = _oEmpResponse.meta.pagination.page;
                    this.PageLimit = _oEmpResponse.meta.pagination.limit;
                    this.TotalPageNumber = _oEmpResponse.meta.pagination.pages;
                }
                NotifyPropertyChanged("EmpResponse");
            }
        }
        private int _iTotalRecords;
        private int _TotalPageNumber;
        private int _iPageNumber;
        private int _iCurrentPageNumber;
        private int _iPageLimit;
        private int _iID;
        private string _sName;
        private string _sEmail;
        private string _enGender;
        private string _enStatus;
        private Employee _sSelectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _sSelectedEmployee; }
            set
            {
                _sSelectedEmployee = value;
                NotifyPropertyChanged("SelectedEmployee");
                if (value != null)
                {
                    this.ID = _sSelectedEmployee.id;
                    this.Name = _sSelectedEmployee.name;
                    this.Email = _sSelectedEmployee.email;
                    this.Gender = _sSelectedEmployee.gender;
                    this.Status = _sSelectedEmployee.status;
                }
            }
        }
        private string _sSearchText;
        public string SearchText
        {
            get { return _sSearchText; }
            set
            {
                _sSearchText = value;
                NotifyPropertyChanged("SearchText");
            }
        }
        public int ID
        {
            get { return _iID; }
            set
            {
                _iID = value;
                NotifyPropertyChanged("ID");
            }
        }
        public string Name
        {
            get { return _sName; }
            set
            {
                _sName = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string Email
        {
            get { return _sEmail; }
            set
            {
                _sEmail = value;
                NotifyPropertyChanged("Email");
            }
        }
        public string Gender
        {
            get { return _enGender; }
            set
            {
                _enGender = value;
                NotifyPropertyChanged("Gender");
            }
        }
        public string Status
        {
            get { return _enStatus; }
            set
            {
                _enStatus = value;
                NotifyPropertyChanged("Status");
            }
        }
        public int TotalRecords
        {
            get { return _iTotalRecords; }
            set
            {
                _iTotalRecords = value;
                NotifyPropertyChanged("TotalRecords");
            }
        }
        public int PageNumber
        {
            get { return _iPageNumber; }
            set
            {
                _iPageNumber = value;
                NotifyPropertyChanged("PageNumber");
            }
        }
        public int TotalPageNumber
        {
            get { return _TotalPageNumber; }
            set
            {
                _TotalPageNumber = value;
                NotifyPropertyChanged("TotalPageNumber");
            }
        }
        public int CurrentPageNumber
        {
            get { return _iCurrentPageNumber; }
            set
            {
                _iCurrentPageNumber = value;
                NotifyPropertyChanged("CurrentPageNumber");
            }
        }
        public int PageLimit
        {
            get { return _iPageLimit; }
            set
            {
                _iPageLimit = value;
                NotifyPropertyChanged("PageLimit");
            }
        }
        private string _sErrorMsg; 
            public string ErrorMsg
        {
            get
            {
                return _sErrorMsg;
            }
            set
            {
                _sErrorMsg = value;
                NotifyPropertyChanged("ErrorMsg");
            }
        }

        #endregion

        #region Commands
        public ICommand CmdGetAllEmployee { get; set; }
        public ICommand CmdGetEmployeeById { get; set; }
        public ICommand CmdGetEmployeeByName { get; set; }
        public ICommand CmdUpdateEmployee { get; set; }
        public ICommand CmdCreateEmployee { get; set; }
        public ICommand CmdDeleteEmployee { get; set; }
        public ICommand CmdExportData { get; set; }
        public ICommand CmdGeTEmployeePage { get; set; }
        public ICommand CmdResetEmployee { get; set; }

        #endregion

        #region ExecuteMethods
        private void ResetEmployee(object obj)
        {
            this.ID = 0;
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Gender = string.Empty;
            this.Status = string.Empty;
        }
        private async void GetAllEmployee(object obj)
        {
            try
            {
                EmpResponse = await this.EmpService.GetEmployeeList();
                if (EmpResponse.code == 200)
                {
                    this.EmployeeList = new ObservableCollection<Employee>(EmpResponse.data);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
          
        }
        private async void GetEmployeeForPage(object obj)
        {
            try
            {
                EmpResponse = await this.EmpService.GetEmployeePage(this.PageNumber);
                if (EmpResponse.code == 200)
                {
                    this.EmployeeList = new ObservableCollection<Employee>(EmpResponse.data);
                }
            }
           
             catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }
        private async void GetEmployeeById(object obj)
        {
            try {
                int result;
                if (!Int32.TryParse(SearchText, out result))
                {
                    MessageBox.Show("Please Enter A Valid Number");
                    return;
                }

                SingleEmployeeResponse empResponse = await this.EmpService.SearchEmployeeById(Int32.Parse(SearchText));
                if (empResponse.code == 200)
                {
                    this.EmployeeList = new ObservableCollection<Employee>();
                    this.EmployeeList.Add(empResponse.data);
                }
            }
            
              catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }
        private async void GetEmployeeByName(object obj)
        {
            try
            {
                EmpResponse = await this.EmpService.SearchEmployeeByName(SearchText);
                if (EmpResponse.code == 200)
                {
                    this.EmployeeList = new ObservableCollection<Employee>(EmpResponse.data);
                }
            }
           
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }
        private async void UpdateEmployee(object obj)
        {
            try
            {
                string responseStatus = string.Empty;
                Employee emp = new Employee
                {
                    name = this.Name,
                    email = this.Email,
                    gender = this.Gender,
                    status = this.Status
                };
                if (this.ID > 0)
                    responseStatus = await this.EmpService.UpdateUser(emp);
                else
                    responseStatus = await this.EmpService.CreateNewUser(emp);

                if (responseStatus == EmployeeConstants.SucessStatus)
                    this.ID = emp.id;
                MessageBox.Show(responseStatus);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }
        private async void CreateEmployee(object obj)
        {
            try
            {
                string responseStatus = string.Empty;
                Employee emp = new Employee
                {
                    id = this.ID,
                    name = this.Name,
                    email = this.Email,
                    gender = this.Gender,
                    status = this.Status
                };
                if (this.ID > 0)
                    responseStatus = await this.EmpService.UpdateUser(emp);
                else
                    responseStatus = await this.EmpService.CreateNewUser(emp);
                if (responseStatus == EmployeeConstants.SucessStatus)
                    this.ID = emp.id;
                MessageBox.Show(responseStatus);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }
        private async void DeleteEmployee(object obj)
        {
            try
            {
                string responseStatus = string.Empty;
                if (SelectedEmployee == null)
                {
                    MessageBox.Show("No Employee Selected");
                    return;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Are you Sure to Delete Employee Id" + SelectedEmployee.name, "Delete Confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                        return;
                    responseStatus = await this.EmpService.DeleteUser(SelectedEmployee.id);
                    if (responseStatus == EmployeeConstants.SucessStatus)
                        GetAllEmployee(null);
                    MessageBox.Show(responseStatus);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }
        private void ExportData(object obj)
        {
            try
            {
                if (this.EmployeeList.Count > 0)
                {
                    //MyExcel myexcel = new MyExcel(this.EmployeeList);
                    ExportToCSV<Employee>.objList = this.EmployeeList;
                    string filename = ExportToCSV<Employee>.ExportData();
                    MessageBox.Show("Data Exported SucessFully " + filename);
                }
            }
          catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region  CanExecuteMethods

        private bool CanResetEmployee(object arg)
        {
            return true;
        }
        private bool CanGetAllEmployee(object arg)
        {
            return true;
        }

        private bool CanGetEmployeeForPage(object arg)
        {
            return true;
        }
        private bool CanGetEmployeeById(object arg)
        {
            return true;
        }
        private bool CanGetEmployeeByName(object arg)
        {
            return true;
        }
        private bool CanUpdateEmployee(object arg)
        {
            return true;
        }
        private bool CanCreateEmployee(object arg)
        {
            return true;
        }

        private bool CanDeleteEmployee(object arg)
        {
            return true;
        }
        private bool CanExportData(object arg)
        {
            return true;
        }

        #endregion


    }
}
