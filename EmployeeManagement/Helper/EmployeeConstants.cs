using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeManagement.Helper
{
    public class EmployeeConstants
    {
        public const string SucessStatus = "Sucess";
        public const string FailureStatus = "Failed";
        public static string FileSavepath = ConfigurationManager.AppSettings["FileSavePath"].ToString();


    }
}
