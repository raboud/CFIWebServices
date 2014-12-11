using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFI.Utilities;

namespace CFIServices
{
    public static class HostInitializer
    {
        private static bool isInitialized = false;
        private static object dataLock = new object();

        public static void Initialize()
        {
            lock (dataLock)
            {
                if (isInitialized == true)
                {
                    return;
                }
                else
                {
                    isInitialized = true;
                }
            }

            LogAPI.InitializeLogging(AppDataManager.WebServiceLogDirectory);
            LogAPI.LogLevel = LogLevel.DEBUG;
            LogAPI.WebServiceLog.Info("Logging initialized");

            // subscribe to event notifications for unhandled thread exceptions
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogAPI.WebServiceLog.Error("Unhandled thread exception caught at the application level.", e.ExceptionObject as Exception);
        }

    }
}