using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CFI.Client
{
    public class CFIClientAutoUnitTest
    {
        private string serviceUrl;
        private string cachePath;
        private bool extendedTest = false;

        private CFIClient cfiClient;
        private TestInvoker[] tests;
        private Random random = new Random();
        private int numNoteTypes = 0;
        private OrderInfo randomOrder = null;
        private OrderInfo randomOrder2 = null;
        private OrderInfo randomOrderWithDiagram = null;
        private TestResults results = new TestResults();

        private delegate bool TestInvoker();

        private const string testUsername = "dbartram";

        private CFIClientAutoUnitTest() {}

        #region events
        public class TestErrorEventArgs : EventArgs
        {
            public bool IsException = false;
            public string Message;
        }
        public event EventHandler<TestErrorEventArgs> TestError;
        private void onTestError( bool isException, string message )
        {
            if (TestError != null)
            {
                TestErrorEventArgs args = new TestErrorEventArgs();
                args.IsException = isException;
                args.Message = message;
                TestError( this, args );
            }
        }

        public class TestMessageEventArgs : EventArgs
        {
            public string Message;
        }
        public event EventHandler<TestMessageEventArgs> TestMessage;
        private void onTestMessage(string message)
        {
            if (TestMessage != null)
            {
                TestMessageEventArgs args = new TestMessageEventArgs();
                args.Message = message;
                TestMessage(this, args);
            }
        }
        #endregion

        public CFIClientAutoUnitTest( string serviceUrl, string cachePath, bool extendedTest )
        {
            this.serviceUrl = serviceUrl;
            this.cachePath = cachePath;
            this.extendedTest = extendedTest;
        }

        public class TestResults
        {
            public int ExceptionCount;
            public int ErrorCount;
            public int PassCount;
            public int FailCount;

            public List<string> ErrorMessages = new List<string>();
            public List<string> PassedTests = new List<string>();
            public List<string> FailedTests = new List<string>();

            public string SummaryText
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("PASSED:    {0}\r\n", PassCount);
                    sb.AppendFormat("FAILED:    {0}\r\n", FailCount);
                    sb.AppendFormat("Exceptions:{0}\r\n", ExceptionCount);
                    sb.AppendFormat("Errors:    {0}\r\n", ErrorCount);
                    foreach ( string passedTest in PassedTests )
                    {
                        sb.AppendFormat("{0}    PASSED\r\n", passedTest);
                    }
                    foreach (string failedTest in FailedTests)
                    {
                        sb.AppendFormat("{0}    FAILED\r\n", failedTest);
                    }
                    return sb.ToString();
                }
            }

            public void RecordError( string errorMessage )
            {
                ErrorCount++;
                ErrorMessages.Add(errorMessage);
            }

            public void RecordException(Exception ex)
            {
                ExceptionCount++;
                RecordError(BuildExceptionText(ex));
            }

            public void RecordTestPassed( string testName )
            {
                PassCount++;
                PassedTests.Add( testName );
            }

            public void RecordTestFailed(string testName)
            {
                FailCount++;
                FailedTests.Add(testName);
            }

            public static string BuildExceptionText(Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}\r\n\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    sb.AppendFormat("[INNER EXCEPTION] {0}\r\n\r\n{1}\r\n\r\n", innerException.Message, innerException.StackTrace);
                    innerException = innerException.InnerException;
                }
                return sb.ToString();
            }
        }


        private TestInvoker[] buildTestArray(TestSuite suite)
        {
            if (suite == TestSuite.WebService)
            {
                tests = new TestInvoker[]
                {
                    new TestInvoker( echoTest ),
                    new TestInvoker( getDiagram ),
                    new TestInvoker( testDebugAPIs ),
                    new TestInvoker( updateCacheMetaData ),
                    new TestInvoker( getNoteTypesTest ),
                    new TestInvoker( getOrderByCustomerName ),
                    new TestInvoker( getOrdersByPONumber ),
                    new TestInvoker( getOrdersByMultipleCriteria ),
                    new TestInvoker( getNotesByOrderID ),
                    new TestInvoker( searchByDateRange ),
                    new TestInvoker( getOrdersOneAtATime ),
                    new TestInvoker( exerciseNoteAPIs ),
                    new TestInvoker( exercisePhotoAPIs ),
                    new TestInvoker( testSecurityUtils ),
                    new TestInvoker( testAcessTokenEnforcement ),
                };
            }
            else if (suite == TestSuite.LocalCache)
            {
                tests = new TestInvoker[]
                {
                    new TestInvoker( exerciseCacheDownloadAndManipulation ),
                    new TestInvoker( exerciseCacheUpload ),
                    new TestInvoker( exerciseCacheSynchronize ),
                    new TestInvoker( checkCacheMetaData ),
                    new TestInvoker( testSecurityUtils ),
                };
            }
            else if (suite == TestSuite.All)
            {
                tests = new TestInvoker[]
                {
                    new TestInvoker( echoTest ),
                    new TestInvoker( testDebugAPIs ),
                    new TestInvoker( checkCacheMetaData ),
                    new TestInvoker( updateCacheMetaData ),
                    new TestInvoker( getNoteTypesTest ),
                    new TestInvoker( getOrderByCustomerName ),
                    new TestInvoker( getOrdersByPONumber ),
                    new TestInvoker( getOrdersByMultipleCriteria ),
                    new TestInvoker( getNotesByOrderID ),
                    new TestInvoker( searchByDateRange ),
                    new TestInvoker( getOrdersOneAtATime ),
                    new TestInvoker( exerciseNoteAPIs ),
                    new TestInvoker( exercisePhotoAPIs ),
                    new TestInvoker( getDiagram ),
                    new TestInvoker( exerciseCacheDownloadAndManipulation ),
                    new TestInvoker( exerciseCacheUpload ),
                    new TestInvoker( exerciseCacheSynchronize ),
                    new TestInvoker( testSecurityUtils ),
                    new TestInvoker( testAcessTokenEnforcement ),
                };
            }

            return tests;
        }

        private void initTestData()
        {
            results = new TestResults();
            numNoteTypes = 0;
            randomOrder = null;
        }

        public void RecordTestError( string message )
        {
            string methodName = new StackFrame(1).GetMethod().Name;
            message = string.Format("[TEST ERROR in '{0}'] - {1}", methodName, message);
            onTestError(false, message);
            results.RecordError(message);
        }

        public void RecordTestException( Exception ex )
        {
            onTestError(true, TestResults.BuildExceptionText(ex));
            results.RecordException(ex);
        }

        public void RecordTestComplete( string name, bool passed )
        {
            onTestMessage( string.Format("{0} {1}", name, passed ? "PASSED" : "FAILED") );
            if (passed == true)
            {
                results.RecordTestPassed(name);
            }
            else
            {
                results.RecordTestFailed( name );
            }
        }

        public void RecordTestMessage( string message )
        {
            onTestMessage(message);
        }

        public enum TestSuite
        {
            WebService,
            LocalCache,
            All
        }

        public TestResults RunTests( TestSuite suite )
        {
            tests = buildTestArray(suite);

            RecordTestMessage("Initializing test data");
            initTestData();

            // connect
            RecordTestMessage("Creating Web Service client");
            cfiClient = new CFIClient();
            cfiClient.Initialize( this.cachePath );
            string errorMessage;
            bool invalidUserName;
            RecordTestMessage("Connecting to web service");
            if (cfiClient.Connect(this.serviceUrl, testUsername, false, out errorMessage, out invalidUserName) == false)
            {
                RecordTestError(string.Format("Failed to connect to web service at {0}.\r\nError Message: {1}", serviceUrl, errorMessage));
            }
            else
            {
                RecordTestMessage("Connected to web service at " + serviceUrl);
            }

            // get random, active, scheduled order (this will be needed by many of the tests)
            randomOrder = getRandomOrderFromLastWeek( cfiClient );
            if ( randomOrder == null )
            {
                RecordTestError("Could not find active, scheduled order for testing");
                return results;
            }

            // record an order with a diagram for use in another test
            randomOrderWithDiagram = getRandomOrderWithDiagram( cfiClient ); 
            if (randomOrderWithDiagram == null)
            {
                RecordTestError("Could not find order with diagram");
                return results;
            }

            // loop over tests
            foreach (TestInvoker invoker in tests)
            {
                string testName = invoker.Method.Name;
                RecordTestMessage(string.Format("Starting test [{0}]", testName));
                startTimer();
                try
                {
                    bool passed = invoker.Invoke();
                    RecordTestComplete(testName, passed);
                }
                catch (Exception ex)
                {
                    RecordTestMessage(string.Format("Exception occurred during test"));
                    RecordTestException(ex);
                    RecordTestComplete(testName, false);
                }
                stopTimer(string.Format("Completed test [{0}]", testName));
            }

            // dicsonnect
            if (cfiClient != null)
            {
                RecordTestMessage("Disconnection from web service");
                cfiClient.Disconnect();
                cfiClient = null;
            }

            return results;
        }

        private OrderInfo getRandomOrderFromLastWeek(CFIClient cfiClient)
        {
            DateRange range = new DateRange();
            range.Start = DateTime.Now.Subtract( TimeSpan.FromDays(7) );
            range.End = DateTime.Now;
            int[] ids = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 100);
            if (ids.Length == 0)
            {
                return null;
            }
            OrderInfo orderInfo = null;
            while ( orderInfo == null )
            {
                int index = random.Next(0, ids.Length - 1);
                int id = ids[index];
                orderInfo = cfiClient.WebServiceAPI.GetOrderByID(id);
                if ( orderInfo != null )
                {
                    if ( orderInfo.PONumber.Trim().Length != 8 )
                    {
                        orderInfo = null;
                    }
                }
            }

            return orderInfo;
        }

        private OrderInfo getRandomOrderWithDiagram(CFIClient cfiClient)
        {
            DateRange range = new DateRange();
            range.Start = DateTime.Now.Subtract( TimeSpan.FromDays(30) );
            range.End = DateTime.Now;
            int[] ids = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 1000);
            if (ids.Length == 0)
            {
                return null;
            }
            OrderInfo orderInfo = null;
            for (int i = 0; i < ids.Length; i++ )
            {
                int id = ids[i];
                orderInfo = cfiClient.WebServiceAPI.GetOrderByID(id);
                if (orderInfo != null)
                {
                    if (orderInfo.HasDiagram == true)
                    {
                        return orderInfo;
                    }
                }
            }
            return null;
        }

        private bool echoTest()
        {
            string ping = Guid.NewGuid().ToString();
            if (cfiClient.WebServiceAPI.Echo(ping) != ping)
            {
                RecordTestError("Echo Failed");
                return false;
            }
            return true;
        }

        private bool getNoteTypesTest()
        {
            NoteTypeInfo[] noteTypes = cfiClient.WebServiceAPI.GetAllNoteTypes();
            numNoteTypes = noteTypes.Length;
            if (numNoteTypes < 1)
            {
                RecordTestError("failed to get note types array");
                return false;
            }
            StringBuilder sb = new StringBuilder();
            foreach (NoteTypeInfo noteType in noteTypes)
            {
                sb.AppendFormat("[{0}] {1}\r\n", noteType.TypeID, noteType.Description);
            }
            RecordTestMessage("Retrieved Note Types:\r\n" + sb.ToString());

            return true;
        }

        private bool testDebugAPIs()
        {
            // get all directories and validate format
            string[] logDirectories = cfiClient.WebServiceAPI.GetLogDirectoryNames();
            if (logDirectories.Length == 0)
            {
                RecordTestError("Failed to get all log directory names");
                return false;
            }
            foreach ( string directory in logDirectories )
            {
                // match this format 7-28-2011_8-13-11_PM
                if (Regex.IsMatch(directory, @"[0-9\-_APM]+") == false)
                {
                    RecordTestError("unrecognized log directory name format.  Either there are additional folders in the log root folder that were not anticipated or the directory naming format has changed.  This unit test code may need to be updated to reflect those changes.");
                    return false;
                }
            }

            // get the current log folder and make sure it is in the complete list
            string currentLogDirectory = cfiClient.WebServiceAPI.GetCurrentLogDirectoryName();
            if ( currentLogDirectory == null )
            {
                RecordTestError("Failed to get current log directory");
                return false;
            }
            if ( logDirectories.Contains( currentLogDirectory) == false )
            {
                RecordTestError("Current log directory not in list of all log directories");
                return false;
            }

            // get log files
            string[] logFiles = cfiClient.WebServiceAPI.GetLogFileNames(currentLogDirectory);
            if ( logFiles.Length == 0 )
            {
                RecordTestError("Failed to get log files from specified folder");
                return false;
            }

            // download log file and confirm contents
            string logFile = logFiles[0];
            logFile = logFile.Replace("\\", "/");
            logFile = logFile.Substring(logFile.LastIndexOf("/") + 1);

            byte[] bytes = cfiClient.WebServiceAPI.DownloadLogFile(currentLogDirectory, logFile);
            if (( bytes == null ) || (bytes.Length == 0) )
            {
                RecordTestError("Failed to download log file from current log directory");
                return false;
            }
            string tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, bytes);
            string text = File.ReadAllText(tempFile);
            if ( text.Length == 0 )
            {
                RecordTestError("Failed to load downloaded log file from temporary text file");
                return false;
            }

            if ( (text.Contains("Logging initialized") == false) || (text.Contains("Client Connected:") == false ) )
            {
                RecordTestError("unexpected contents in downloaded of file");
                return false;
            }

            return true;
        }

        private bool getOrderByCustomerName()
        {
            string lastName = "Holloway";
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByCustomerLastName(lastName, true, false, 60000);

            StringBuilder sb = new StringBuilder();
            foreach (int orderID in orderIDs)
            {
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if ( order == null )
                {
                    continue;
                }
                if (order.IsScheduled == true)
                {
                    sb.AppendFormat("{0}\r\n", order.DebugText);
                }
            }
            RecordTestMessage( string.Format("Retrieved scheduled orders for {0}:\r\n{1}", lastName, sb.ToString()));

            foreach (int orderID in orderIDs)
            {
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if ( order.IsScheduled == true)
                {
                    if ( string.Compare(lastName, order.Customer.LastName, true ) != 0)
                    {
                        RecordTestError("Name did not match");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool getOrdersByMultipleCriteria()
        {
            // test each of the 8 ways that the three args can combine n this API
            // Name - PO Number - Date Range

            // 1. nothing valid
            int[] ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, null, null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where all args were invalid"));
                return false;
            }

            // 2. date range
            DateRange range = new DateRange();
            range.Start = DateTime.Parse("1/1/2008");
            range.End = DateTime.Now;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, null, null, range, true, true, 60000);
            if (ids.Length == 0)
            {
                RecordTestError(string.Format("Failed case where only valid date range was specified with expected results"));
                return false;
            }
            range.Start = DateTime.Now;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, null, null, range, true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where only valid date range was specified with expected null set result"));
                return false;
            }

            // 3. po number
            string poNumber = randomOrder.PONumber;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, poNumber, null, new DateRange(), true, true, 60000);
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where only po number was specified with expected single item result set"));
                return false;
            }
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, "99999999", null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where only po number was specified with expected empty result set"));
                return false;
            }

            // 4. po number and date range
            range.Start = DateTime.Parse("1/1/1990");
            range.End = DateTime.Now;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, randomOrder.PONumber, null, range, true, true, 60000);
            // the expected intersection of the 195 orders in the date range and a single po number is 1 order
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where po number and date range was specified with expected single item result set"));
                return false;
            }
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(null, "99999999", null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where po number and date range was specified with expected empty result set"));
                return false;
            }

            // 5. name
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria( randomOrder.Customer.LastName , null, null, new DateRange(), true, true, 60000);
            if (ids.Length == 0)
            {
                RecordTestError(string.Format("Failed case where only name was specified with expected result set"));
                return false;
            }
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria("qwertxyz", null, null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where only name was specified with expected empty result set"));
                return false;
            }

            // 6. name and date range
            range.Start = randomOrder.ScheduledDate;
            range.End = randomOrder.ScheduledDate;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(randomOrder.Customer.LastName, null, null, range, true, true, 60000);
            if (ids.Length == 0)
            {
                RecordTestError(string.Format("Failed case where name and date were specified with expected result set"));
                return false;
            }

            // 7. name and po number
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(randomOrder.Customer.LastName, randomOrder.PONumber, null, new DateRange(), true, true, 60000);
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where name and po number were specified with expected result set"));
                return false;
            }
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria("Holloway", "59444076", null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where name and po number were specified with expected empty result set"));
                return false;
            }

            // 8. name, po number and date range
            range.Start = randomOrder.ScheduledDate;
            range.Start = randomOrder.ScheduledDate;
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(randomOrder.Customer.LastName, randomOrder.PONumber, null, range, true, true, 60000);
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where name and po number and date range were specified with expected result set"));
                return false;
            }

            range.Start = DateTime.Parse("5/14/2011");
            range.End = DateTime.Parse("5/15/2011");
            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria("Holloway", "59444076", null, new DateRange(), true, true, 60000);
            if (ids.Length != 0)
            {
                RecordTestError(string.Format("Failed case where name and po number and date range were specified with expected empty result set"));
                return false;
            }

            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(randomOrder.Customer.LastName, randomOrder.PONumber, null, null, true, true, 60000);
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where name and po number and date range were specified with expected single item result set (NULL DateRange object)"));
                return false;
            }

            ids = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(randomOrder.Customer.LastName, randomOrder.PONumber, null, new DateRange(), true, true, 60000);
            if (ids.Length != 1)
            {
                RecordTestError(string.Format("Failed case where name and po number and date range were specified with expected single item result set (empty DateRange object)"));
                return false;
            }

            return true;
        }

        private bool getOrdersByPONumber()
        {
            // fetch by the po number of the previously recorded random order and make sure the same order comes back
            int[] ids = cfiClient.WebServiceAPI.GetOrdersByPONumber( randomOrder.PONumber, true, true, 100 );
            return ids.Contains(randomOrder.ID);
        }

        private bool getNotesByOrderID()
        {
            int orderID = randomOrder.ID;
            NoteInfo[] notes = cfiClient.WebServiceAPI.GetNotes( orderID );
            if ( notes.Length > 0 )
            {
                StringBuilder sb = new StringBuilder();
                foreach (NoteInfo note in notes)
                {
                    sb.AppendFormat("[{0}] {1}\r\n", note.NoteTypeDescription, note.Text);
                }
                RecordTestMessage(string.Format("Retrieved notes for orderID {0}:\r\n{1}", orderID, sb.ToString() ));
            }
            return true;
        }

        private bool searchByDateRange()
        {
            // create a dummy date range +/- 1 week around the random order that was previously stored
            List<DateRange> ranges = new List<DateRange>()
            {
                new DateRange()
                {
                    Start = randomOrder.ScheduledDate.Subtract(TimeSpan.FromDays(7)),
                    End = randomOrder.ScheduledDate.Add(TimeSpan.FromDays(7))
                },

                new DateRange()
                {
                    Start = randomOrder.ScheduledDate,
                    End = randomOrder.ScheduledDate.Add(TimeSpan.FromDays(7)),
                },

                new DateRange()
                {
                    Start = randomOrder.ScheduledDate.Subtract(TimeSpan.FromDays(7)),
                    End = randomOrder.ScheduledDate
                },

                new DateRange()
                {
                    Start = randomOrder.ScheduledDate,
                    End = randomOrder.ScheduledDate
                },

                new DateRange()
                {
                    End = randomOrder.ScheduledDate.Subtract(TimeSpan.FromDays(7)),
                    Start = randomOrder.ScheduledDate.Add(TimeSpan.FromDays(7))
                },

                new DateRange()
                {
                    End = randomOrder.ScheduledDate,
                    Start = randomOrder.ScheduledDate.Add(TimeSpan.FromDays(7)),
                },

                new DateRange()
                {
                    End = randomOrder.ScheduledDate.Subtract(TimeSpan.FromDays(7)),
                    Start = randomOrder.ScheduledDate
                },

                new DateRange()
                {
                    End = randomOrder.ScheduledDate,
                    Start = randomOrder.ScheduledDate
                },



            };

            foreach (DateRange range in ranges)
            {
                int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 60000);
                if (orderIDs.Length < 1)
                {
                    RecordTestError(string.Format("Failed to find orders in specified date range [ {0} - {1} ]", range.Start.ToLongDateString(), range.End.ToLongDateString()));
                }

                // confirm that the stored random order is in the retrieved set
                bool found = false;
                foreach (int orderID in orderIDs)
                {
                    if (randomOrder.ID == orderID)
                    {
                        RecordTestMessage(string.Format("FOUND order ID {0} (scheduled for {1}) in the specified date range [ {2} - {3} ]",
                                                randomOrder.ID,
                                                randomOrder.ScheduledDate.ToLongDateString(),
                                                range.Start.ToLongDateString(),
                                                range.End.ToLongDateString()));
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    RecordTestError(string.Format("Failed to find order ID {0} (scheduled for {1}) in the specified date range [ {2} - {3} ]",
                                            randomOrder.ID,
                                            randomOrder.ScheduledDate.ToLongDateString(),
                                            range.Start.ToLongDateString(),
                                            range.End.ToLongDateString()));
                    return false;
                }
            }

            DateRange futureRange = new DateRange()
            {
                Start = DateTime.Parse("1/1/2050"),
                End = DateTime.Parse("1/1/2055"),
            };
            int[] futureOrderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(futureRange, true, true, 60000);
            if (futureOrderIDs.Length != 0)
            {
                RecordTestError(string.Format("INCORRECTLY found orders in the range range [ {0} - {1} ]", futureRange.Start.ToLongDateString(), futureRange.End.ToLongDateString()));
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool getOrdersOneAtATime()
        {
            DateRange range = new DateRange()
            {
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 60000);
            List<int> randomOrderIDs = new List<int>();
            int count = this.extendedTest ? 100 : 5;
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, orderIDs.Length - 1);
                randomOrderIDs.Add(orderIDs[index]);
            }
            orderIDs = randomOrderIDs.ToArray();

            if (orderIDs.Length < count)
            {
                RecordTestError("Failed to get all orders in large date range");
                return false;
            }

            foreach (int orderID in orderIDs)
            {
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if (order.ID != orderID)
                {
                    RecordTestError(string.Format("Returned Order ID did not match when fetching order by ID {0}", orderID));
                    return false;
                }
                if ( order.IsScheduled == false)
                {
                    RecordTestError(string.Format("INCORRECTLY fetched unscheduled order {0}", orderID));
                    return false;
                }

                RecordTestMessage(string.Format("PASSED Order retrieval test for OrderID {0}", orderID));
            }

            return true;
        }

        private bool exerciseNoteAPIs()
        {
            DateRange range = new DateRange()
            {
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };

            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 60000);
            List<int> randomOrderIDs = new List<int>();
            int count = this.extendedTest ? 100 : 5;

            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, orderIDs.Length - 1);
                randomOrderIDs.Add(orderIDs[index]);
            }
            orderIDs = randomOrderIDs.ToArray();

            foreach (int orderID in orderIDs)
            {
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                int oldNoteCount = order.Notes.Length;

                NoteInfo note = createNote( Guid.NewGuid().ToString(), 6 );

                // add note and confirm
                cfiClient.WebServiceAPI.AddNote(order.ID, note);
                order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                int newNoteCount = order.Notes.Length;
                if (oldNoteCount != (newNoteCount - 1))
                {
                    RecordTestError(string.Format("Failed to add note to orderID {0}", orderID));
                    return false;
                }
                NoteInfo lastNote = order.Notes[order.Notes.Length - 1];
                if ((note.Text != lastNote.Text) || (note.TypeID != lastNote.TypeID))
                {
                    RecordTestError(string.Format("Failed to add note to end of notes for orderID {0}", orderID));
                    return false;
                }

                // update note and confirm
                string newText = Guid.NewGuid().ToString();
                cfiClient.WebServiceAPI.UpdateNoteText(lastNote.ID, newText);
                order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                lastNote = order.Notes[order.Notes.Length - 1];
                if (lastNote.Text != newText)
                {
                    RecordTestError(string.Format("Failed to EDIT note at end of notes for orderID {0}", orderID));
                    return false;
                }

                // delete note and confirm
                int deletedNoteID = lastNote.ID;
                cfiClient.WebServiceAPI.DeleteNote(lastNote.ID);
                order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if (order.Notes.Length > oldNoteCount)
                {
                    RecordTestError(string.Format("Failed to delete note at end of notes for orderID {0}", orderID));
                    return false;
                }
                NoteInfo deletedNote = cfiClient.WebServiceAPI.GetNote(deletedNoteID);
                if (deletedNote != null)
                {
                    RecordTestError(string.Format("Failed to delete note at end of notes for orderID {0}", orderID));
                    return false;
                }

                RecordTestMessage(string.Format("PASSED NoteAPI tests for OrderID {0}", orderID));
            }
            return true;
        }

        private bool exercisePhotoAPIs()
        {
            DateRange range = new DateRange()
            {
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 60000);

            int count = this.extendedTest ? 100 : 5;
            List<int> randomOrderIDs = new List<int>();
            for (int i = 0; i < count; i++ )
            {
                int index = random.Next(0, orderIDs.Length - 1);
                randomOrderIDs.Add( orderIDs[index] );
            }
            orderIDs = randomOrderIDs.ToArray();

            foreach (int orderID in orderIDs)
            {
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                int oldPhotoCount = order.Photos.Length;

                // create a photo and an info object to wrap it and then send it up to the web service
                byte[] photoBytes = simulatePhotoSnapped();

                PhotoInfo photo = new PhotoInfo();
                photo.ID = -1;
                photo.Title = "Pic for PO Number " + order.PONumber;
                photo.EnteredByUserID = 6;
                photo.FilePath = "IRRELEVANT - THIS WILL BE IGNORED ON THE SERVER IF ALL GOES WELL";
                photo.DateTimeEntered = DateTime.Now;

                // add photo and confirm
                if (cfiClient.WebServiceAPI.AddPhoto(order.ID, photo, photoBytes, "jpg") == false)
                {
                    RecordTestError(string.Format("Failed to add photo to orderID {0}", orderID));
                    return false;
                }
                order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                int newPhotoCount = order.Photos.Length;
                if (oldPhotoCount != (newPhotoCount - 1))
                {
                    RecordTestError(string.Format("Failed to add photo to orderID {0}", orderID));
                    return false;
                }
                PhotoInfo lastPhoto = order.Photos[order.Photos.Length - 1];
                if (lastPhoto.FilePath.Contains("photo_") == false)
                {
                    RecordTestError(string.Format("Failed to add photo to end of photos for orderID {0}", orderID));
                    return false;
                }

                // download the just-created/uploaded photo by its ID
                byte[] downloadedPhotoBytes = cfiClient.WebServiceAPI.DownloadPhoto(lastPhoto.ID); 
                if ( photoBytes.Length != downloadedPhotoBytes.Length )
                {
                    RecordTestError(string.Format("Failed to download photo"));
                    return false;
                }
                for (int i = 0; i < photoBytes.Length; i++ )
                {
                    if ( photoBytes[i] != downloadedPhotoBytes[i] )
                    {
                        RecordTestError(string.Format("Failed to download photo correctly"));
                        return false;
                    }
                }

                // delete photo and confirm
                int deletedPhotoID = lastPhoto.ID;
                cfiClient.WebServiceAPI.DeletePhoto(lastPhoto.ID);
                order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if (order.Photos.Length > oldPhotoCount)
                {
                    RecordTestError(string.Format("Failed to delete photo at end of photos for orderID {0}", orderID));
                    return false;
                }
                PhotoInfo deletedPhoto = cfiClient.WebServiceAPI.GetPhoto(deletedPhotoID);
                if (deletedPhoto != null)
                {
                    RecordTestError(string.Format("Failed to delete photo at end of photos for orderID {0}", orderID));
                    return false;
                }

                RecordTestMessage(string.Format("PASSED PhotoAPI tests for OrderID {0}", orderID));
            }
            return true;
        }

        private bool getDiagram()
        {
            if ( ( randomOrderWithDiagram == null ) || ( randomOrderWithDiagram.HasDiagram == false ) )
            {
                RecordTestError("Failed to find order with diagram");
                return false;
            }

            byte[] diagram = cfiClient.WebServiceAPI.DownloadDiagram( randomOrderWithDiagram.DiagramNumber );
            if ( ( diagram == null ) || ( diagram.Length == 0  ) )
            {
                RecordTestError("Failed to retrieve diagram");
                return false;
            }

            string fileName = Path.GetTempFileName();
            File.WriteAllBytes( fileName, diagram );
            
            return true;
        }

        private bool exerciseCacheDownloadAndManipulation()
        {
            int count = this.extendedTest ? 100 : 5;
            for (int i = 0; i < count; i++ )
            {
                // get the orders we'll be working with
                randomOrder = getRandomOrder();
                randomOrder2 = null;
                while (randomOrder2 == null)
                {
                    randomOrder2 = getRandomOrder();
                    if (randomOrder.ID == randomOrder2.ID)
                    {
                        // unlikely, but just in case of refetch of same order we set it to null to force another random fetch
                        randomOrder2 = null;
                    }
                }

                // empty the cache and confirm
                if (testCacheClearing() == false) { return false; }

                // download two random orders
                if (testDownloadIntoCleanCache() == false) { return false; }

                // confirm retrieval of all orders
                if (testGetAllOrdersFromCache() == false) { return false; }

                // confirm retrieval of single order
                if (testGetOrderFromCache() == false) { return false; }

                // delete the second order
                if (testDeleteOrderFromCache() == false) { return false; }

                // make sure the first one is still there
                if (testGetOrderFromCache() == false) { return false; }

                // add 3 notes to the first order
                if (testAddNotesToCachedOrder() == false) { return false; }

                // update the middle note
                if (testUpdateNoteInCachedOrder() == false) { return false; }

                // delete the middle note
                if (testDeleteNoteInCachedOrder() == false) { return false; }

                // add 3 new photos to cached order
                if (testAddNewPhotosToCachedOrder() == false) { return false; }

                // delete photo from cache and confirm
                if (testDeleteNewPhotosFromCachedOrder() == false) { return false; }

                // test that download overwrites existng order with same ID
                if (testDownloadOverwriteOfCachedOrder() == false) { return false; }
            }

            return true;
        }

        private bool exerciseCacheUpload()
        {
            // get the orders we'll be working with
            randomOrder = getRandomOrder();
            randomOrder2 = null;
            while (randomOrder2 == null)
            {
                randomOrder2 = getRandomOrder();
                if (randomOrder.ID == randomOrder2.ID)
                {
                    // unlikely, but just in case of refetch of same order we set it to null to force another random fetch
                    randomOrder2 = null;
                }
            }

            // empty the cache and confirm
            if (testCacheClearing() == false) { return false; }

            // download two random orders
            if (testDownloadIntoCleanCache() == false) { return false; }

            // add 3 notes to the first order
            if (testAddNotesToCachedOrder() == false) { return false; }

            // add 3 new photos to cached order
            if (testAddNewPhotosToCachedOrder() == false) { return false; }

            // upload all dirty orders
            if (testUploadDirtyCachedOrdersAndRemoveFromCache() == false) { return false; }

            return true;
        }

        private bool exerciseCacheSynchronize()
        {
            // get the orders we'll be working with
            randomOrder = getRandomOrder();
            randomOrder2 = null;
            while (randomOrder2 == null)
            {
                randomOrder2 = getRandomOrder();
                if (randomOrder.ID == randomOrder2.ID)
                {
                    // unlikely, but just in case of refetch of same order we set it to null to force another random fetch
                    randomOrder2 = null;
                }
            }

            // empty the cache and confirm
            if (testCacheClearing() == false) { return false; }

            // download two random orders
            if (testDownloadIntoCleanCache() == false) { return false; }

            // add 3 notes to the first order
            if (testAddNotesToCachedOrder() == false) { return false; }

            // add 3 new photos to cached order
            if (testAddNewPhotosToCachedOrder() == false) { return false; }

            // synch all dirty orders
            if (testSynchronizeDirtyCachedOrders() == false) { return false; }

            return true;
        }

        private bool checkCacheMetaData()
        {
            // empty the cache and confirm
            if (testCacheClearing() == false) { return false; }

            if (testAutoGenerateMetaData() == false) { return false; }

            return true;
        }

        private bool updateCacheMetaData()
        {
            // empty the cache and confirm
            if (testCacheClearing() == false) { return false; }

            if (testAutoGenerateMetaData() == false) { return false; }

            if (testRefreshMetaDataFromServer() == false) { return false; }

            return true;
        }

        private bool testUploadDirtyCachedOrdersAndRemoveFromCache()
        {
            // get the dirty orders
            CacheOrder[] dirtyOrders = cfiClient.Cache.GetAllDirtyOrders();
            if ( dirtyOrders.Length != 1 )
            {
                RecordTestError("wrong number of dirty orders retrieved from cache");
                return false;
            }
            
            foreach ( CacheOrder dirtyOrder in dirtyOrders )
            {
                int numExistingPhotos = dirtyOrder.Order.Photos.Length;
                int numNewPhotos = dirtyOrder.NewNotes.Length;
                int numExistingNotes = dirtyOrder.Order.Notes.Length;
                int numNewNotes = dirtyOrder.NewNotes.Length;

                string errorMessage;
                WebServiceAPIResult result = cfiClient.UploadCachedOrder( dirtyOrder, out errorMessage);
                if (result == WebServiceAPIResult.ConnectivityFail)
                {
                    RecordTestError("Connectivity Fail during dirty order upload from cache");
                    return false;
                }
                else if (result == WebServiceAPIResult.Fail)
                {
                    RecordTestError("Failure during dirty order upload from cache.\r\n" + errorMessage);
                    return false;
                }
                else // ( result == WebServiceAPIResult.Success)
                {
                    RecordTestMessage(string.Format("Uploaded dirty order ID={0} PO_Number={1}", dirtyOrder.Order.ID, dirtyOrder.Order.PONumber));
                }

                // remove the order from the cache
                cfiClient.Cache.DeleteOrder(dirtyOrder.Order.ID);

                // download the just-uploaded order and confirm that the notes and photos added are now part of the existing items in the collections
                if (cfiClient.DownLoadOrderToCache(dirtyOrder.Order.ID, out errorMessage) != WebServiceAPIResult.Success)
                {
                    RecordTestError("Failure during dirty order confirmation download.\r\n" + errorMessage);
                    return false;
                }
                CacheOrder roundTripOrder = cfiClient.Cache.GetOrder(dirtyOrder.Order.ID);
                if (roundTripOrder.Order.Notes.Length != (numExistingNotes + numNewNotes))
                {
                    RecordTestError("Existing notes collection not properly updated after upload");
                    return false;
                }
                if (roundTripOrder.NewNotes.Length != 0)
                {
                    RecordTestError("new notes collection not properly updated after upload/download");
                    return false;
                }
                if (roundTripOrder.Order.Photos.Length != (numExistingPhotos + numNewPhotos))
                {
                    RecordTestError("Existing photos collection not properly updated after upload");
                    return false;
                }
                if (roundTripOrder.NewPhotos.Length != 0)
                {
                    RecordTestError("new photos collection not properly updated after upload/download");
                    return false;
                }
            }

            // confirm that there are no dirty orders remaining
            dirtyOrders = cfiClient.Cache.GetAllDirtyOrders();
            if (dirtyOrders.Length != 0)
            {
                RecordTestError("wrong number of dirty orders retrieved from cache after uploads");
                return false;
            }

            return true;
        }

        private bool testSynchronizeDirtyCachedOrders()
        {
            // get the dirty orders
            CacheOrder[] dirtyOrders = cfiClient.Cache.GetAllDirtyOrders();
            if (dirtyOrders.Length != 1)
            {
                RecordTestError("wrong number of dirty orders retrieved from cache");
                return false;
            }

            foreach (CacheOrder dirtyOrder in dirtyOrders)
            {
                int numExistingPhotos = dirtyOrder.Order.Photos.Length;
                int numNewPhotos = dirtyOrder.NewNotes.Length + 1;
                int numExistingNotes = dirtyOrder.Order.Notes.Length;
                int numNewNotes = dirtyOrder.NewNotes.Length + 1;

                // add a randomly generated note (one not in the cached order)to simulate out of band updates
                NoteInfo newNote = createNote( "Simulated note added by another client " + Guid.NewGuid().ToString(), 9999 );
                if (cfiClient.WebServiceAPI.AddNote(dirtyOrder.Order.ID, newNote) == false)
                {
                    RecordTestError("Failed to add simulated non-client-added note");
                    return false;
                }

                // add a randomly generated photo (one not in the cache) to simulate out of band updates
                PhotoInfo photo = new PhotoInfo();
                photo.ID = -1;
                photo.Title = "Non-client generated pic for PO Number " + dirtyOrder.Order.PONumber;
                photo.EnteredByUserID = 6;
                photo.FilePath = "IRRELEVANT - THIS WILL BE IGNORED ON THE SERVER IF ALL GOES WELL";
                photo.DateTimeEntered = DateTime.Now;
                if ( cfiClient.WebServiceAPI.AddPhoto( dirtyOrder.Order.ID, photo, simulatePhotoSnapped(), "jpg") == false )
                {
                    RecordTestError("Failed to add simulated non-client-added photo");
                    return false;
                }

                string errorMessage;
                
                // mark the cached order as pending and save it
                dirtyOrder.Status = CacheStatus.SynchPending;
                cfiClient.Cache.SaveOrder(dirtyOrder);

                // refetch it just as an added test
                CacheOrder order = cfiClient.Cache.GetOrder(dirtyOrder.Order.ID);

                WebServiceAPIResult result = cfiClient.SynchronizeCachedOrder(order, out errorMessage);

                if (result == WebServiceAPIResult.ConnectivityFail)
                {
                    RecordTestError("Connectivity Fail during dirty order synch");
                    return false;
                }
                else if (result == WebServiceAPIResult.Fail)
                {
                    RecordTestError("Failure during dirty order synch.\r\n" + errorMessage);
                    return false;
                }
                else // ( result == WebServiceAPIResult.Success)
                {
                    RecordTestMessage(string.Format("synched dirty order {0}", order.Order.ID));
                }

                // download the just-uploaded order and confirm that the notes and photos added are now part of the existing items in the collections
                if (cfiClient.DownLoadOrderToCache(order.Order.ID, out errorMessage) != WebServiceAPIResult.Success)
                {
                    RecordTestError("Failure during dirty order confirmation download.\r\n" + errorMessage);
                    return false;
                }
                CacheOrder roundTripOrder = cfiClient.Cache.GetOrder(order.Order.ID);
                if (roundTripOrder.Order.Notes.Length != (numExistingNotes + numNewNotes))
                {
                    RecordTestError("Existing notes collection not properly updated after upload");
                    return false;
                }
                if (roundTripOrder.NewNotes.Length != 0)
                {
                    RecordTestError("new notes collection not properly updated after upload/download");
                    return false;
                }
                
                // confirm that there are the right number of photos and in the right places
                if (roundTripOrder.Order.Photos.Length != (numExistingPhotos + numNewPhotos))
                {
                    RecordTestError("Existing photos collection not properly updated after upload");
                    return false;
                }
                string newDir = cfiClient.Cache.GetNewPhotosFolderName(roundTripOrder.Order.ID);
                if ( Directory.Exists( newDir ) && Directory.GetFiles(newDir).Length != 0 )
                {
                    RecordTestError("New photos folder should have been absent or empty");
                    return false;
                }
                string downloadDir = cfiClient.Cache.GetDownloadedPhotosFolderName(roundTripOrder.Order.ID);
                int numDownloadedPhotos = Directory.GetFiles(downloadDir).Length;
                int expectNumberOfDownloadedPhotos = numExistingPhotos + numNewPhotos;
                if ( numDownloadedPhotos != expectNumberOfDownloadedPhotos )
                {
                    RecordTestError(string.Format("download photos folder has wrong number of files.  should have been {0} but was {1}", expectNumberOfDownloadedPhotos, numDownloadedPhotos ));
                    return false;
                }
                if (roundTripOrder.NewPhotos.Length != 0)
                {
                    RecordTestError("new photos collection not properly updated after upload/download");
                    return false;
                }
            }

            // confirm that there are no dirty orders remaining
            dirtyOrders = cfiClient.Cache.GetAllDirtyOrders();
            if (dirtyOrders.Length != 0)
            {
                RecordTestError("wrong number of dirty orders retrieved from cache after synchronization");
                return false;
            }

            return true;
        }

        private NoteInfo createNote(string text, int userID)
        {
            NoteInfo note = new NoteInfo();
            note.Text = text;
            note.ID = -1;
            note.TypeID = 1; // INTERNAL
            note.EnteredByUserID = userID;
            note.DateTimeEntered = DateTime.Now;
            return note;
        }


        private bool testDeleteNewPhotosFromCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("cached order should have been in dirty state");
                return false;
            }

            // confirm the photo IDs are correct
            if ((cachedOrder.NewPhotos[0].ID != 0) ||
                 (cachedOrder.NewPhotos[1].ID != 1) ||
                 (cachedOrder.NewPhotos[2].ID != 2))
            {
                RecordTestError("new photos have wrong IDs");
                return false;
            }

            // delete the middle photo, save, reload, and confirm change
            cachedOrder.DeleteNewPhoto(1);
            cfiClient.Cache.SaveOrder(cachedOrder);
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.NewPhotos.Length != 2)
            {
                RecordTestError("photo not correctly deleted");
                return false;
            }

            // confirm the photo IDs are still correct (they should have been corrected on write)
            if ((cachedOrder.NewPhotos[0].ID != 0) ||
                 (cachedOrder.NewPhotos[1].ID != 1))
            {
                RecordTestError("new photos have wrong IDs after delete, save, and reload");
                return false;
            }

            return true;
        }

        private bool testAddNewPhotosToCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("cached order should have been in dirty state");
                return false;
            }

            // add photo to the order and save it back to the cache
            string photoPath1 = cfiClient.Cache.SaveNewPhoto(cachedOrder.Order.ID, simulatePhotoSnapped(), "jpg");
            string photoPath2 = cfiClient.Cache.SaveNewPhoto(cachedOrder.Order.ID, simulatePhotoSnapped(), "jpg");
            string photoPath3 = cfiClient.Cache.SaveNewPhoto(cachedOrder.Order.ID, simulatePhotoSnapped(), "jpg");
            cachedOrder.AddNewPhoto("First Photo",photoPath1, 6, "" );
            cachedOrder.AddNewPhoto("Second Photo", photoPath2, 6, "");
            cachedOrder.AddNewPhoto("Third Photo", photoPath3, 6, "");
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("Modified cache order not marked as dirty before save");
                return false;
            }

            cfiClient.Cache.SaveOrder(cachedOrder);

            // confirm presence of files
            if ((File.Exists(photoPath1) == false) || 
                (File.Exists(photoPath2) == false) || 
                (File.Exists(photoPath3) == false))
            {
                RecordTestError("photo files not saved to cache");
                return false;
            }

            // retrieve the cached order
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("Modified cache order not marked as dirty after retrieval");
                return false;
            }

            // confirm the presence of the new photo and the status
            if ((cachedOrder.NewPhotos.Length != 3) ||
                 (cachedOrder.NewPhotos[0].Title != "First Photo") ||
                 (cachedOrder.NewPhotos[1].Title != "Second Photo") ||
                 (cachedOrder.NewPhotos[2].Title != "Third Photo"))
            {
                RecordTestError("new photos not added to order");
                return false;
            }

            // confirm the photo IDs are correct
            if ((cachedOrder.NewPhotos[0].ID != 0) ||
                 (cachedOrder.NewPhotos[1].ID != 1) ||
                 (cachedOrder.NewPhotos[2].ID != 2))
            {
                RecordTestError("new photos have wrong IDs");
                return false;
            }

            return true;
        }

        private byte[] simulatePhotoSnapped()
        {
            // this is a really cool photo of Calvin Johnson.  It has nothing to do with flooring but I think you'll like it.
            string text = "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";
            return Convert.FromBase64String(text);
        }

        private bool testDownloadOverwriteOfCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("cached order should have been in dirty state");
                return false;
            }

            // confirm the note IDs are still correct
            if ((cachedOrder.NewNotes[0].ID != 0) ||
                 (cachedOrder.NewNotes[1].ID != 1))
            {
                RecordTestError("new notes have wrong IDs");
                return false;
            }

            if (testDownloadByPONumber(randomOrder.PONumber) == false)
            {
                RecordTestError("Download into dirty cache failed");
                return false;
            }
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Synched)
            {
                RecordTestError("cached order should have been in downloaded state");
                return false;
            }

            if ( cachedOrder.NewNotes.Length != 0 )
            {
                RecordTestError("overwrite failed");
                return false;
            }

            return true;
        }

        private bool testDeleteNoteInCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("cached order should have been in dirty state");
                return false;
            }

            // confirm the note IDs are correct
            if ((cachedOrder.NewNotes[0].ID != 0) ||
                 (cachedOrder.NewNotes[1].ID != 1) ||
                 (cachedOrder.NewNotes[2].ID != 2))
            {
                RecordTestError("new notes have wrong IDs");
                return false;
            }

            // delete the middle note, save, reload, and confirm change
            cachedOrder.DeleteNewNote(1);
            cfiClient.Cache.SaveOrder(cachedOrder);
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.NewNotes.Length != 2)
            {
                RecordTestError("note not correctly deleted");
                return false;
            }

            // confirm the note IDs are still correct (they should have been corrected on write)
            if ((cachedOrder.NewNotes[0].ID != 0) ||
                 (cachedOrder.NewNotes[1].ID != 1))
            {
                RecordTestError("new notes have wrong IDs after delete, save, and reload");
                return false;
            }

            return true;
        }

        private bool testUpdateNoteInCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("cached order should have been in dirty state");
                return false;
            }

            // update text of middle note, save, reload, and confirm change
            cachedOrder.UpdateNewNote(1, "hi mom");
            cfiClient.Cache.SaveOrder(cachedOrder);
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if ( cachedOrder.NewNotes[1].Text != "hi mom" )
            {
                RecordTestError("text not correctly modified in note");
                return false;
            }

            // confirm the note IDs are correct
            if ((cachedOrder.NewNotes[0].ID != 0) ||
                 (cachedOrder.NewNotes[1].ID != 1) ||
                 (cachedOrder.NewNotes[2].ID != 2))
            {
                RecordTestError("new notes have wrong IDs after note update, save, and reload");
                return false;
            }

            return true;
        }

        private bool testAddNotesToCachedOrder()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Synched)
            {
                RecordTestError("cached order should have been in downloaded state");
                return false;
            }

            // generate random notes
            NoteInfo note1 = createNote(Guid.NewGuid().ToString(), 6);
            NoteInfo note2 = createNote(Guid.NewGuid().ToString(), 6);
            NoteInfo note3 = createNote(Guid.NewGuid().ToString(), 6);

            // add notes to the order and save it back to the cache
            cachedOrder.AddNewNote(note1);
            cachedOrder.AddNewNote(note2);
            cachedOrder.AddNewNote(note3);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("Modified cache order not marked as dirty before save");
                return false;
            }

            cfiClient.Cache.SaveOrder(cachedOrder);

            // retrieve the cached order
            cachedOrder = cfiClient.Cache.GetOrder(randomOrder.ID);
            if (cachedOrder.Status != CacheStatus.Dirty)
            {
                RecordTestError("Modified cache order not marked as dirty after retrieval");
                return false;
            }

            // confirm the presence of the new notes and the status
            if ( ( cachedOrder.NewNotes.Length != 3 ) || 
                 ( cachedOrder.NewNotes[0].Text != note1.Text ) ||
                 ( cachedOrder.NewNotes[1].Text != note2.Text)  ||
                 ( cachedOrder.NewNotes[2].Text != note3.Text) )
            {
                RecordTestError("new notes not added to order");
                return false;
            }

            // confirm the note IDs are correct
            if ( (cachedOrder.NewNotes[0].ID != 0) ||
                 (cachedOrder.NewNotes[1].ID != 1) ||
                 (cachedOrder.NewNotes[2].ID != 2))
            {
                RecordTestError("new notes have wrong IDs");
                return false;
            }

            return true;
        }

        private bool testDeleteOrderFromCache()
        {
            cfiClient.Cache.DeleteOrder(randomOrder2.ID);
            if (cfiClient.Cache.GetOrder(randomOrder2.ID) != null)
            {
                RecordTestError("Failed to delete cached order");
                return false;
            }

            return true;
        }

        private bool testGetAllOrdersFromCache()
        {
            CacheOrder[] cachedOrders = cfiClient.Cache.GetAllOrders();
            if ( cachedOrders.Length < 2 )
            {
                RecordTestError("Failed to correctly get all cached orders");
                return false;
            }

            bool foundFirst = false;
            bool foundSecond = false;
            foreach ( CacheOrder cachedOrder in cachedOrders )
            {
                if ( cachedOrder.Order.Equals(randomOrder) == true )
                {
                    foundFirst = true;
                }
                else if (cachedOrder.Order.Equals(randomOrder2) == true)
                {
                    foundSecond = true;
                }
            }

            bool result = ( foundFirst && foundSecond );

            return result;
        }

        private bool testGetOrderFromCache()
        {
            CacheOrder cachedOrder = cfiClient.Cache.GetOrder( randomOrder.ID );
            if ( randomOrder.Equals( cachedOrder.Order ) == false )
            {
                RecordTestError("Failed to get correct cached order");
                return false;
            }

            return true;
        }

        private bool testDownloadIntoCleanCache()
        {
            return testDownloadByPONumber(randomOrder.PONumber) && testDownloadByPONumber( randomOrder2.PONumber );
        }

        private bool testDownloadByPONumber( string poNumber )
        {
            string errorMessage;
            int[] downloadedIDs;
            WebServiceAPIResult result = cfiClient.DownLoadOrdersToCacheByPONumber(poNumber, true, true, 100, out errorMessage, out downloadedIDs);
            if (result == WebServiceAPIResult.ConnectivityFail)
            {
                RecordTestError("Connectivity Fail during order download to cache");
                return false;
            }
            else if (result == WebServiceAPIResult.Fail)
            {
                RecordTestError("Failure during order download to cache.\r\n" + errorMessage);
                return false;
            }
            else // ( result == WebServiceAPIResult.Success)
            {
                if (downloadedIDs == null)
                {
                    RecordTestError("failed to download to cache by poNumber. Empty set returned with error message:\r\n" + errorMessage);
                    return false;
                }
                else
                {
                    foreach (int id in downloadedIDs)
                    {
                        string file = cfiClient.Cache.GetOrderFilePath(id);
                        if (File.Exists(file) == false)
                        {
                            RecordTestError(string.Format("Allegedly downloaded file '{0}' not in cache.", file));
                            return false;
                        }
                        else
                        {
                            RecordTestMessage(string.Format("Downloaded PO Number {0} to local cache", randomOrder.PONumber));
                        }
                    }
                    return true;
                }
            }
        }

        private bool testCacheClearing()
        {
            cfiClient.Cache.Clear();
            if (Directory.GetFiles(cfiClient.Cache.OrdersDirectory).Length > 0)
            {
                RecordTestError("Failed to clear cache folder " + this.cachePath);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool testAutoGenerateMetaData()
        {
            // check file does not exist
            if (Directory.GetFiles(cfiClient.Cache.MetaDataDirectory).Length > 0)
            {
                RecordTestError("meta Data folder was not empty as expected");
                return false;
            }

            // access the metadata property and confirm correct default contents
            CacheMetaData metaData = cfiClient.Cache.MetaData;
            if ( metaData == null )
            {
                RecordTestError("default meta data object not created");
                return false;
            }
            
            // confirm file now exists
            if ( File.Exists( cfiClient.Cache.MetaDataFile ) == false )
            {
                RecordTestError("default meta data file not created");
                return false;
            }

            if ( metaData.GetUserNames().Length != 31 )
            {
                RecordTestError("default meta data has wrong number of users");
                return false;
            }

            if (metaData.GetNoteTypeDescriptions().Length != 16)
            {
                RecordTestError("default meta data has wrong number of note types");
                return false;
            }

            return true;
        }

        private bool testRefreshMetaDataFromServer()
        {
            // this test modifies the default meta data XML and then refreshes it by connecting to the 
            // server and confirming that the deleted data items have been restored.

            // load the XML into a document and delete some elements
            string originalXml = File.ReadAllText( cfiClient.Cache.MetaDataFile );
            string metaDataTag = "CacheMetaData";
            string usersTag = "Users";
            string noteTypesTag = "NoteTypes";
            XmlDocument document = new XmlDocument();
            document.LoadXml(originalXml);
            XmlElement rootElement = document.SelectSingleNode(metaDataTag) as XmlElement;
            XmlElement usersElement = rootElement.SelectSingleNode(usersTag) as XmlElement;
            usersElement.RemoveAll();
            XmlElement noteTypesElement = rootElement.SelectSingleNode(noteTypesTag) as XmlElement;
            noteTypesElement.RemoveAll();

            // save the file
            document.Save(cfiClient.Cache.MetaDataFile);
            string modifiedXml = File.ReadAllText( cfiClient.Cache.MetaDataFile );
            if ( modifiedXml == originalXml )
            {
                RecordTestError("XML not modified in file as expected");
                return false;
            }

            // connect to the server
            var client = new CFIClient();
            client.Initialize( this.cachePath );
            string errorMessage;
            bool invalidUserName;
            if (client.Connect(this.serviceUrl, testUsername, true, out errorMessage, out invalidUserName) == false)
            {
                RecordTestError("Failed client connection to web service");
                return false;
            }

            string refreshedXml = File.ReadAllText(cfiClient.Cache.MetaDataFile);
            if (originalXml != refreshedXml)
            {
                CacheMetaData md2 = CacheMetaData.ParseXml(refreshedXml);
                if ( ( md2.NumNoteTypes == 0 ) || ( md2.NumUsers == 0 ) )
                {
                    RecordTestError("Failed meta data reconstruction");
                    return false;
                }
            }
            else
            {
                // this just tests the equivalence API
                CacheMetaData x = CacheMetaData.ParseXml(originalXml);
                CacheMetaData y = CacheMetaData.ParseXml(refreshedXml);
                if (CacheMetaData.AreEquivalent(x, y) == false)
                {
                    RecordTestError("Meta Data XML parsing Failed");
                    return false;
                }
            }

            // for our next trick we corrupt the XML file and make sure it gets recreated as default
            string garbageXml = originalXml.Replace("ID", "YoMama");
            File.WriteAllText( cfiClient.Cache.MetaDataFile, garbageXml);
            var anotherClient = new CFIClient();
            anotherClient.Initialize(this.cachePath);
            CacheMetaData metaData = anotherClient.Cache.MetaData;
            if (metaData == null)
            {
                RecordTestError("Corrupt XML was not repaired to default state. Null object created.");
                return false;
            }

            string repairedXml = File.ReadAllText(cfiClient.Cache.MetaDataFile);
            if (originalXml != repairedXml)
            {
                RecordTestError("Corrupt XML was not repaired to default state. XML does not match.");
                return false;
            }

            return true;
        }

        private bool testSecurityUtils()
        {
            string plainText = "abcdefghijklmnopqrstuvwxyz123456789";
            string cipherText = SecurityUtils.Encrypt(plainText);
            string decryptedText = SecurityUtils.Decrypt(cipherText);
            if ( plainText != decryptedText )
            {
                RecordTestError("encrypt/decrypt cycle failed to reproduce original text");
                return false;
            }

            string userName = "bobthebuilder";
            string accessToken = SecurityUtils.CreateAccessToken(userName);
            string decodedUsername = SecurityUtils.UserNameFromAccessToken(accessToken);
            if (userName != decodedUsername)
            {
                RecordTestError("access token codec cycle failed");
                return false;
            }
            return true;
        }

        private bool testAcessTokenEnforcement()
        {
            // validate that invalid user cannot even connect
            string errorMessage;
            bool invalidUserName;
            CFIClient client = new CFIClient();
            client.Initialize(this.cachePath);
            if (client.Connect(this.serviceUrl, "barneythedinosaur", false, out errorMessage, out invalidUserName) == true)
            {
                RecordTestError("invalid user should not have been allowed to connect");
                return false;
            }
            if ( invalidUserName == false )
            {
                RecordTestError("Failed to detect bogus user name and set flag");
                return false;
            }
            
            bool passed = AccessTokenAPIValidationTests.TestAccessTokenEnforcement( this.serviceUrl, this.cachePath, testUsername, testUsername, true, out errorMessage);
            if (passed == false)
            {
                RecordTestError("access token test failed for valid user name.  " + errorMessage);
                return false;
            }

            passed = AccessTokenAPIValidationTests.TestAccessTokenEnforcement(this.serviceUrl, this.cachePath, testUsername, "yomama", false, out errorMessage);
            if (passed == false)
            {
                RecordTestError("access token test failed for INVALID user name.  " + errorMessage);
                return false;
            }

            return true;
        } 

        private delegate void webServiceAPIInvoker( WebServiceAPIWrapper wsAPI );


        private OrderInfo getRandomOrder()
        {
            int[] ids = cfiClient.WebServiceAPI.GetOrderIDs( true, true, 60000 );
            int index = random.Next(0, ids.Length - 1);
            int id = ids[index];
            return cfiClient.WebServiceAPI.GetOrderByID( id );
        }

        private DateTime start;
        private void startTimer()
        {
            start = DateTime.Now;
        }

        private void stopTimer(string message)
        {
            TimeSpan span = DateTime.Now.Subtract(start);
            RecordTestMessage(string.Format("[elapsed time: {0} milliSeconds] - {1}", span.TotalMilliseconds, message));
        }

    }
}
