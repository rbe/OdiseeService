using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace OdiseeService
{

    [RunInstaller(true)]
    public class OdiseeServiceInstaller : Installer
    {

        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public OdiseeServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            // Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;
            // Service Information
            serviceInstaller.DisplayName = "Odisee Server";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            // This must be identical to the WindowsService.ServiceBase name set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "Odisee Server Instance 1";
            // Add installers
            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }

    }

}
