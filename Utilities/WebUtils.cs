using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace CFI.Utilities
{
    public static class WebUtils
    {
        public static string GetUrlContents( string url )
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    string result = sr.ReadToEnd();
                    return result;
                }
            }
        }       

    }
}
