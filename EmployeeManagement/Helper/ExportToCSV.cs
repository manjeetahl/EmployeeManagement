using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace EmployeeManagement.Helper
{
    
    public static class ExportToCSV<T>
    {
        static StringBuilder strBuilder;
       public static ObservableCollection<T> objList;
        public static string ExportData()
        {
            strBuilder = new StringBuilder();
            foreach (PropertyInfo propInfo in objList[0].GetType().GetProperties())
            {
                strBuilder.Append (propInfo.Name.ToString() + ",");
            }
            foreach(T obj in objList)
            {
                strBuilder.AppendLine();
                foreach (PropertyInfo propInfo in objList[0].GetType().GetProperties())
                {
                    strBuilder.Append(propInfo.GetValue(obj).ToString() + ",");
                }
                
            }
            string filename = EmployeeConstants.FileSavepath + "export_" + System.DateTime.Now.ToString("ddmmyyyyhhss") + ".csv";
            File.WriteAllText(filename, strBuilder.ToString());
            return filename;
        }
    }
  }
