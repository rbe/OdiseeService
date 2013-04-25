using Microsoft.Win32;
using OdiseeService;
using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace OdiseeService
{

    class OdiseeService : ServiceBase
    {

        /// <summary>
        /// Public Constructor for WindowsService.
        /// </summary>
        public OdiseeService()
        {
            this.ServiceName = "Odisee Server Worker Instances";
            this.EventLog.Log = "Application";
            // These Flags set whether or not to handle that specific type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        /// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new OdiseeService());
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether
        ///    or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnStart(): Put startup code here, start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            // Get path of application soffice.exe
            string sofficePath = OdiseeSystemHelper.GetApplicationPath("soffice.exe");
            if (null != sofficePath)
            {
                string sofficeExe = String.Format("{0}program\\soffice.exe", sofficePath);
                // Start instance on tcp://localhost:2001
                string host = "127.0.0.1";
                int port = 2001;
                // Get path of application, change backslashes to slashes, URL-encode spaces
                string odiseePath = OdiseeSystemHelper.GetMyApplicationPath().Replace('\\', '/').Replace(" ", "%20");
                string odiseeProfile = String.Format("{0}/var/profile/odisee_port{1}", odiseePath, port);
                string sofficeArgs = String.Format("-env:UserInstallation=\"{0}\" --accept=\"socket,host={1},port={2};urp;StarOffice.ServiceManager\" --invisible", odiseeProfile, host, port);
                Console.WriteLine("odiseeInst=" + sofficePath);
                Console.WriteLine("odiseeProfile=" + odiseeProfile);
                Console.WriteLine("sofficeExe=" + sofficeExe);
                Console.WriteLine("sofficeArgs=" + sofficeArgs);
                Process.Start(sofficeExe, sofficeArgs);
            }
        }

        /// <summary>
        /// OnStop(): Put your stop code here, stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// OnPause: Put your pause code here, pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue(): Put your continue code here, un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// Put code here when you need special handling of code
        /// that deals with a system shutdown, such as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// If you need to send a command to your service without
        /// the need for remoting or sockets, use this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary integer between 128 and 256</param>
        protected override void OnCustomCommand(int command)
        {
            // A custom command can be sent to a service by using this method:
            //int command = 128; // Some arbitrary number between 128 and 256
            //ServiceController sc = new ServiceController("NameOfService");
            //sc.ExecuteCommand(command);
            base.OnCustomCommand(command);
        }

        /// <summary>
        /// Useful for detecting power status changes, such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The power broadcast status (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// To handle a change event from a Terminal Server session.
        /// Useful if you need to determine when a user logs in remotely or logs off,
        /// or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription">The session change event that occured.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }

    }

}
