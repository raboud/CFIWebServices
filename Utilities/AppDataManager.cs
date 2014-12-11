using System;
using System.IO;
using CFI.Utilities;

namespace CFI.Utilities
{
    public static class AppDataManager
    {
        private static string companyName = "Custom Flooring";
        public static string CompanyName
        {
            get
            {
                return companyName;
            }
        }

        private static string applicationName = "Job Inspector";
        public static string ApplicationName
        {
            get
            {
                return applicationName;
            }
        }

        public static string CreateDirectoryIfNeeded(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == true)
            {
                return directoryPath;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                    if (Directory.Exists(directoryPath) == false)
                    {
                        return null;
                    }
                    return directoryPath;
                }
                catch
                {
                    return null;
                }
            }
        }


        private static string BasePath
        {
            get
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
                return CreateDirectoryIfNeeded(Path.Combine(path, companyName));
            }
        }

        public static string ApplicationDataPath
        {
            get
            {
                return CreateDirectoryIfNeeded(Path.Combine(BasePath, applicationName));
            }
        }

        public static string ServerDataPath
        {
            get
            {
                return CreateDirectoryIfNeeded(Path.Combine(ApplicationDataPath, "Server"));
            }
        }

        public static string ClientDataPath
        {
            get
            {
                return CreateDirectoryIfNeeded(Path.Combine(ApplicationDataPath, "Client"));
            }
        }

        public static string WebServiceLogDirectory
        {
            get
            {
                return CreateDirectoryIfNeeded(Path.Combine(ServerDataPath, "Logs"));
            }
        }

        public static string DataCacheDirectory
        {
            get
            {
                return CreateDirectoryIfNeeded(Path.Combine(ClientDataPath, "DataCache"));
            }
        }

    }
}
