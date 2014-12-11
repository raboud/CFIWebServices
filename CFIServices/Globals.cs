using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFI;
using CFI.Utilities;

namespace CFIServices
{
    public static class Globals
    {
        private static FileTransferManager fileTransferManager;
        public static FileTransferManager FileTransferManager
        {
            get { return Globals.fileTransferManager; }
        }

        static Globals()
        {
            fileTransferManager = new FileTransferManager();
            LogAPI.WebServiceLog.Debug("FileTransferManager created");

            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
                LogAPI.WebServiceLog.Debug("AppDomain unloading.  Glabal data objects going bye bye.");
            }
            catch
            {
            }
        }

    }
}