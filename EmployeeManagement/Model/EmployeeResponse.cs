using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Model
{
    public class EmployeeResponse
    {
        public int code { get; set; }
        public MetaData meta { get; set; }
        public List<Employee> data { get; set; }
    }
    public class PostResponse
    {
        public int code { get; set; }
        public MetaData meta { get; set; }
        public object data { get; set; }
    }
    public class PostMessage
    {
        public string Message { get; set; }
    }
    public class FieldMessage
    {
        public string field { get; set; }
        public string message { get; set; }
    }
    public class SingleEmployeeResponse
    {
        public int code { get; set; }
        public MetaData meta { get; set; }
        public Employee data { get; set; }
    }
    public class MetaData
    {
        public Pagination pagination { get; set; }
    }
    public class Pagination
    {
        public int total { get; set; }
        public int pages { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
    }
}
