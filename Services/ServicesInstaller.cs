using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Services
{
    [RunInstaller(true)]
    public partial class ServicesInstaller : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        public ServicesInstaller()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "RozetkaTracker";
            serviceInstaller.Description = "Track items price on Rozetka and save to database";
            serviceInstaller.DisplayName = "RozetkaTracker";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
