using System;
using System.ServiceModel;

#if WINDOWS 
using CFI.Client.JobInspectionService;
#endif

namespace CFI.Client
{
    public class WebServiceUtils
    {
        public static JobInspectionClient CreateServiceClient( string url )
        {
            return new JobInspectionClient
            (
                CreateBasicHttpBinding(),
                new EndpointAddress(url)
            );
        }

        public static BasicHttpBinding CreateBasicHttpBinding()
        {
            
            // create binding and endpoint info
            BasicHttpBinding basicBinding = new BasicHttpBinding();
            basicBinding.SendTimeout = TimeSpan.FromMinutes(5);
            basicBinding.OpenTimeout = TimeSpan.FromSeconds(20);

            basicBinding.MaxReceivedMessageSize = 2147483647;
            basicBinding.MaxBufferSize = 2147483647;
            basicBinding.MaxBufferPoolSize = 2147483647;

            basicBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            basicBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            basicBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            basicBinding.ReaderQuotas.MaxDepth = 2147483647;
            basicBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            basicBinding.ReaderQuotas.MaxStringContentLength = 2147483647;

            return basicBinding;
        }
    }

}