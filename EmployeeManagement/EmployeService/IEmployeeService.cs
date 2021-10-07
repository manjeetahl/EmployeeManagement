using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Model;

namespace EmployeeManagement.EmployeService
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse> GetEmployeeList();
        Task<EmployeeResponse> SearchEmployeeByName(string name);
        Task<SingleEmployeeResponse> SearchEmployeeById(int empId);
        Task<string> UpdateUser(Employee emp);
        Task<string> CreateNewUser(Employee emp);
        Task<string> DeleteUser(int empId);
        Task<EmployeeResponse> GetEmployeePage(int pageNumber);



    }
}
