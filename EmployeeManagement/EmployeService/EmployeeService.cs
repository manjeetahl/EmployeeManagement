using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmployeeManagement.Model;
using EmployeeManagement.Helper;
using System.Configuration;
using System.Net.Http.Headers;
using System.Collections.Specialized;

namespace EmployeeManagement.EmployeService
{
    public class EmployeeService : IEmployeeService
    {
        public HttpClient ServiceClient { get; set; }
        private const string ACCESS_TOKEN = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public EmployeeService()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseUrl"].ToString());
            ServiceClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }
        public EmployeeService(bool unitTest)
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri("https://gorest.co.in/public-api/");
            ServiceClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
        }
       
        public async Task<string> CreateNewUser(Employee emp)
        {
            string jsonString = JsonConvert.SerializeObject(emp);
            string result = string.Empty;
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await this.ServiceClient.PostAsync("users", content);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                PostResponse obj = JsonConvert.DeserializeObject<PostResponse>(result);
                if (obj == null)
                    result = EmployeeConstants.SucessStatus;
                else
                {
                    result = obj.code == 201 ? EmployeeConstants.SucessStatus
                        :obj.code == 422 ? ("Fields Missing " + obj.data.ToString()): string.Empty;
                    emp.id = JsonConvert.DeserializeObject<Employee>(obj.data.ToString()).id;
                }
                    
            }
            return result.ToString();
        }

        public async Task<string> DeleteUser(int empId)
        {
            string result = string.Empty;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.ServiceClient.BaseAddress.ToString() + "users/" + empId.ToString());
            request.Method = HttpMethod.Delete;
            var response = await this.ServiceClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                PostResponse obj = JsonConvert.DeserializeObject<PostResponse>(result);
                if (obj.data == null)
                    result = EmployeeConstants.SucessStatus;
                else
                    result = ((PostMessage)(obj.data)).Message;
            }
            return result;
        }

      public  async Task<EmployeeResponse> GetEmployeeList()
        {
            var response = (await this.ServiceClient.GetAsync("users")).Content.ReadAsStringAsync();
            EmployeeResponse empresponse = JsonConvert.DeserializeObject<EmployeeResponse>(response.Result.ToString());
            return empresponse;
        }
        public async Task<EmployeeResponse> GetEmployeePage(int pageNumber)
        {
            var response = (await this.ServiceClient.GetAsync("users?page=" + pageNumber.ToString())).Content.ReadAsStringAsync();
            EmployeeResponse empresponse = JsonConvert.DeserializeObject<EmployeeResponse>(response.Result.ToString());
            return empresponse;
        }

        public async Task<EmployeeResponse> SearchEmployeeByName(string name)
        {
            var response = (await this.ServiceClient.GetAsync("users?name=" + name)).Content.ReadAsStringAsync();
            EmployeeResponse empresponse = JsonConvert.DeserializeObject<EmployeeResponse>(response.Result.ToString());
            return empresponse;
        }

        public async Task<SingleEmployeeResponse> SearchEmployeeById(int empId)
        {
            var response = (await this.ServiceClient.GetAsync("users/" + empId.ToString())).Content.ReadAsStringAsync();
            SingleEmployeeResponse empresponse = JsonConvert.DeserializeObject<SingleEmployeeResponse>(response.Result.ToString());
            return empresponse;
        }

        public async Task<string> UpdateUser(Employee emp)
        {
            string result = string.Empty;
            string jsonString = JsonConvert.SerializeObject(emp);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await this.ServiceClient.PutAsync("users/" + emp.id.ToString(), content);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                PostResponse obj = JsonConvert.DeserializeObject<PostResponse>(result);
                if (obj == null)
                    result = "Created sucessfully";
                else
                {
                    result = obj.code == 201 ? "Employee Updated With Employee Id" + ((Employee)obj.data).id.ToString()
                        : obj.code == 422 ? ("Fields Missing " + obj.data.ToString()) 
                        : obj.code == 404 ? ((PostMessage)obj.data).Message.ToString(): string.Empty;
                }
            }
            return result.ToString();
        }
    }
}
