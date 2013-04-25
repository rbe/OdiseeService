using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OdiseeService
{
    class OdiseeSystemHelper
    {

        /*
        /// <summary>
        /// Find installation.
        /// </summary>
        public static void findOffice(string product)
        {
            Type type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Installer msi = (Installer)Activator.CreateInstance(type);
            foreach (string productcode in msi.Products)
            {
                string productname = msi.get_ProductInfo(productcode, "InstalledProductName");
                if (productname.Contains(product))
                {
                    string installdir = msi.get_ProductInfo(productcode, "InstallLocation");
                    Console.WriteLine("{0}: {1} @({2})", productcode, productname, installdir);
                }
            }
        }
        */

        /// <summary>
        /// Find files in a directory.
        /// </summary>
        /// <param name="name">Name to find</param>
        /// <returns>string[]</returns>
        public static string[] FindFiles(String name)
        {
            string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string[] files = Directory.GetFiles(programFilesFolder, name, SearchOption.AllDirectories);
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetApplicationPathForExtension(string extension)
        {
            var appName = (string)Registry.ClassesRoot.OpenSubKey(extension).GetValue(null);
            var openWith = (string)Registry.ClassesRoot.OpenSubKey(appName + @"\shell\open\command").GetValue(null);
            var appPath = Regex.Match(openWith, "[a-zA-Z0-9:,\\\\\\. ]+").Value.Trim();
            return new FileInfo(appPath).Directory.FullName;
        }

        /// <summary>
        /// Get path of a certain application.
        /// </summary>
        /// <param name="appname">somename.exe</param>
        /// <returns>string</returns>
        public static string GetApplicationPath(string appname)
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, ""))
            {
                using (Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + appname))
                {
                    if (subkey == null)
                        return "";

                    object path = subkey.GetValue("Path");

                    if (path != null)
                        return (string)path;
                }

            }
            return "";
        }

        /// <summary>
        /// Get this application's path.
        /// </summary>
        /// <returns>string</returns>
        public static string GetMyApplicationPath()
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            return fi.DirectoryName;
        }

        /// <summary>
        /// Check if a process is running
        /// </summary>
        /// <param name="processname">the processname</param>
        /// <returns>true if the process runns otherwise false</returns>
        private bool CheckIfAProcessIsRunning(string processname)
        {
            return Process.GetProcessesByName(processname).Length > 0;
        }

        /*
        /// <summary>
        /// Get path of a process.
        /// </summary>
        /// <returns>string</returns>
        public static string GetProcessPath()
        {
            var procList = Process.GetProcesses().Where(process => process.ProcessName.Contains("notepad"));
            foreach (var process in procList)
            {
                Console.WriteLine("Path to {0}: {1}", process.ProcessName, Path.GetDirectoryName(process.MainModule.FileName));
            }
        }
        */

        /// <summary>
        /// Get number of physical processors in the system.
        /// 2 dual-core hyper-threading-enabled processors: there are 2 physical processors, 4 cores, and 8 logical processors.
        /// </summary>
        /// <returns>Number of physical processors, 2 in this example.</returns>
        public static int GetNumberOfProcessors()
        {
            int count = 0;
            foreach (var item in new ManagementObjectSearcher("select * from Win32_ComputerSystem").Get())
            {
                count += Convert.ToInt32((UInt32)item["NumberOfProcessors"]);
            }
            return count;
        }

        /// <summary>
        /// Get number of physical processors in the system.
        /// 2 dual-core hyper-threading-enabled processors: there are 2 physical processors, 4 cores, and 8 logical processors.
        /// </summary>
        /// <returns>Number of physical processors, 2 in this example.</returns>
        public static int GetNumberOfProcessorsFromEnvironment()
        {
            int processors = 1;
            string envProcessors = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
            if (null != envProcessors)
            {
                processors = int.Parse(envProcessors);
            }
            return processors;
        }

        /// <summary>
        /// Get number of CPU cores in the system.
        /// 2 dual-core hyper-threading-enabled processors: there are 2 physical processors, 4 cores, and 8 logical processors.
        /// </summary>
        /// <returns>Number of cores, 4 in this example.</returns>
        public static int GetNumberOfCores()
        {
            int count = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("select * from Win32_Processor").Get())
            {
                count += int.Parse(item["NumberOfCores"].ToString());
            }
            return count;
        }

        /// <summary>
        /// Get logical processor count.
        /// 2 dual-core hyper-threading-enabled processors: there are 2 physical processors, 4 cores, and 8 logical processors.
        /// See http://msdn.microsoft.com/en-us/library/system.environment.processorcount.aspx.
        /// </summary>
        /// <returns>Number of logical processors, 8 in this example.</returns>
        public static int getLogicalProcessorCount()
        {
            return Environment.ProcessorCount;
        }

    }

}
