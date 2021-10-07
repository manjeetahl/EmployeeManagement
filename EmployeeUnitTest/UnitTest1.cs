using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeManagement.EmployeService;
using EmployeeManagement.Model;
using System.Threading.Tasks;

namespace EmployeeUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetUsers()
        {
            EmployeeResponse response = new EmployeeService(true).GetEmployeeList().Result;
            Assert.AreEqual(response.code ,200);
        }

        [TestMethod]
        public void TestGetUsersByName()
        {
            EmployeeResponse response = new EmployeeService(true).SearchEmployeeByName("Sinha").Result;
            Assert.AreEqual(response.code, 200);
        }

        [TestMethod]
        public void TestGetUserById()
        {
            SingleEmployeeResponse response = new EmployeeService(true).SearchEmployeeById(37).Result;
            Assert.AreEqual(response.code, 200);
        }

        [TestMethod]
        public void TestDeleteUserById()
        {
            string response = new EmployeeService(true).DeleteUser(37).Result;
            Assert.IsNotNull(response);
        }
        [TestMethod]
        public void TestCreateUser()
        {
            Employee emp = new Employee
            {
                name="manjeet_user_5",
                email= "manjeet_user_5@test.com",
                gender = "male",
                status="active"
            };
            string response = new EmployeeService(true).CreateNewUser(emp).Result;
            Assert.IsNotNull(response);
        }
        [TestMethod]
        public void TestUpdateUser()
        {
            Employee emp = new Employee
            {
                id = 1404,
                name = "manjeet_user_2",
                email = "manjeet_user_2@test.com",
                gender = "male",
                status = "active"
            };
            string response = new EmployeeService(true).UpdateUser(emp).Result;
            Assert.IsNotNull(response);
        }
    }
}
