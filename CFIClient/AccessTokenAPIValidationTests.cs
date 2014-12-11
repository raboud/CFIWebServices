using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

#if WINDOWS
using CFI.Client.JobInspectionService;
using System.Reflection;
#endif

namespace CFI.Client
{
    public static class AccessTokenAPIValidationTests
    {
#if WINDOWS
        public static bool TestAccessTokenEnforcement(string serviceURL, string cachePath, string validUserName, string invalidUserName, bool isValidUser, out string errorMessage)
        {
            errorMessage = null;
            CFIClient client = new CFIClient();
            client.Initialize( cachePath );
            // we use the valid user name only for the channel connection
            bool userNameWasInvalid;
            if (client.Connect(serviceURL, validUserName, false, out errorMessage, out userNameWasInvalid) == false)
            {
                return false;
            }
            client.DebugSetUserName(invalidUserName);



            MethodInfo[] methods = typeof(JobInspectionClient).GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach ( MethodInfo method in methods )
            {
                if (method.DeclaringType.Namespace == "CFI.Client.JobInspectionService")
                {
                    try
                    {
                        List<object> arguments = new List<object>();
                        int i = 0;
                        foreach (ParameterInfo parameter in method.GetParameters())
                        {
                            if (i == 0)
                            {
                                if (isValidUser == true)
                                {
                                    // the wrapper API would generate a token this way
                                    arguments.Add( SecurityUtils.CreateAccessToken(validUserName) );
                                }
                                else
                                {
                                    // a rogue user or someone using the app without knowing a proper username would generate an invalid token
                                    // (that's the idea anyway...). So we just tokenize a bogus 'guessed' username for this test
                                    arguments.Add(SecurityUtils.CreateAccessToken(invalidUserName));
                                }
                            }
                            else
                            {
                                if (parameter.ParameterType.IsClass)
                                {
                                    arguments.Add(null);
                                }
                                else if ( isNumericType( parameter.ParameterType ) == true )
                                {
                                    arguments.Add(0);
                                }
                                else if (parameter.ParameterType.Name == "Boolean")
                                {
                                    arguments.Add(false);
                                }
                                else
                                {
                                    errorMessage = "Unhandled parameter type in unit test code.  Need to add this type to the handler.";
                                    return false;
                                }
                            }
                            i++;
                        }
                        method.Invoke(client.WebServiceAPI.ServiceClient, arguments.ToArray());
                    }
                    catch (Exception ex)
                    {
                        InvalidUserException iuex = WebServiceAPIWrapper.TranslateException(ex) as InvalidUserException;
                        if (iuex != null)
                        {
                            // we expect this exception sometimes so check to see if this is one of those times
                            if (isValidUser == true)
                            {
                                // we said it was an valid user so this is NOT expected
                                errorMessage = string.Format("unexpected access token exception in test method '{0}.'\r\n{1}", method.Name, iuex.Message);
                                return false;
                            }
                            else
                            {
                                // we said it was an invalid user so this IS expected
                                return true;
                            }
                        }
                        else
                        {
                            errorMessage = string.Format("unexpected exception in test method '{0}'\r\n{1}\r\n{2}", method.Name, ex.Message, ex.StackTrace);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool isNumericType(Type type)
        {
            string name = type.Name;
            if ( name.StartsWith("System.") )
            {
                name = name.Substring(7);
            }
            switch ( name )
            {
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Decimal":
                case "Double":
                    return true;
                default: 
                    return false;
            }
        }

#else
        // MONOTOUCH
        public static bool TestAccessTokenEnforcement(string serviceURL, string cachePath, string validUserName, string invalidUserName, bool isValidUser, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }
#endif
    }




}
