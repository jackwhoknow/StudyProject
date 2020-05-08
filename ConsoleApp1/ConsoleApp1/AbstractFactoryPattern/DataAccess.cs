using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace DesignPattern
{
    public class DataAccess
    {
        private static readonly string AssemblyName = "DesignPattern";
        private static readonly string db = GetDbValue();

        private static string GetDbValue()
        {
            string path = System.Environment.CurrentDirectory;
            string pre1Path= Directory.GetParent(path).FullName;
            string pre2Path = Directory.GetParent(pre1Path).FullName;
            Directory.SetCurrentDirectory(pre2Path);
            string parentPath = Directory.GetCurrentDirectory();
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = parentPath + "\\App1.config";// 这里对应你app1文件的路径
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            string key = config.AppSettings.Settings["DB"].Value;
            return key;
        }
       public static IUser CreateUser()
        {
            string className = AssemblyName + "." + db + "User";
            return (IUser)Assembly.Load(AssemblyName).CreateInstance(className);
        }
        public static IDepartment CreateDepartment()
        {
            string className = AssemblyName + "." + db + "Department";
            return (IDepartment)Assembly.Load(AssemblyName).CreateInstance(className);
        }
    }
}
