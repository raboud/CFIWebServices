using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CFI.Utilities
{
    internal static class ResourceAPI
    {
        public static string GetStringResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader sr = new StreamReader(resourceStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static List<string> ReadAllLines(string nameSpace, string resource)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            StreamReader reader = getResourceStream(nameSpace, resource, assembly);
            return extractValidlines(reader);
        }

        public static string ReadAllText(string nameSpace, string resource)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            StreamReader reader = getResourceStream(nameSpace, resource, assembly);
            return reader.ReadToEnd();
        }

        private static StreamReader getResourceStream(string nameSpace, string resource, Assembly assembly)
        {
            resource = resource.Replace('\\', '.').Replace('/', '.');
            return new StreamReader(assembly.GetManifestResourceStream(nameSpace + '.' + resource));
        }

        private static List<string> extractValidlines(StreamReader reader)
        {
            List<string> ret = new List<string>();
            string s = null;
            while ((s = reader.ReadLine()) != null)
            {
                //if its not a comment or empty line
                if (s.Trim().Length > 0 && s[0] != '#')
                {
                    ret.Add(s.Trim());
                }
            }
            reader.Close();
            return ret;
        }

    }
}
