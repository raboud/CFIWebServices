using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CFIDataAccess.Models;
using CFI.Client;
using CFI.Utilities;

namespace CFI.ServicesTest
{
    public partial class MainForm : Form
    {
        private CFIClient cfiClient;
        private bool hasBeenConnected = false;

        private const string testUserName = "dbartram";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if ( this.DesignMode == false )
            {
                initializeClient();

                // URls to choose from
                string urlProductionServer = "http://vpn.custom-installs.com:8000/CFIJobInspection/JobInspection.svc";
                string urlSteveDevelopmentServer = string.Format("http://{0}:8000/CFIJobInspection/JobInspection.svc", System.Net.Dns.GetHostName());
                this.textBoxServiceUrl.Text = urlSteveDevelopmentServer;

                this.textBoxLastName.Text = "";
                this.textBoxStartDate.Text = DateTime.Now.Subtract( TimeSpan.FromDays(7) ).ToShortDateString();
                this.textBoxEndDate.Text = DateTime.Now.ToShortDateString();
                this.statusLabel.Text = "";
                this.checkBoxActiveOnly.Checked = true;
                this.checkBoxScheduledOnly.Checked = true;

                int width = Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Width * .7 );
                int height = Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Height * .7 );
                int x = Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Width * .15 );
                int y = Convert.ToInt32( Screen.PrimaryScreen.WorkingArea.Height * .15 );
                this.Size = new System.Drawing.Size(width, height);
                this.Location = new Point(x, y);

                updateUI();
            }
        }

        private void initializeClient()
        {
            cfiClient = new CFIClient();
            cfiClient.Initialize(AppDataManager.DataCacheDirectory);
            cfiClient.ChannelStateChanged += new EventHandler<CFIClient.WebServiceChannelStateChangedEventArgs>(cfiClient_ChannelStateChanged);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cfiClient != null)
                {
                    cfiClient.Disconnect();
                }
            }
            catch
            {
            }
        }


        private void buttonEcho_Click(object sender, EventArgs e)
        {
            string text = Guid.NewGuid().ToString();
            string echoText = cfiClient.WebServiceAPI.Echo(text);
            echoText = cfiClient.WebServiceAPI.Echo(text);
            echoText = cfiClient.WebServiceAPI.Echo(text);
            if (text != echoText)
            {
                appendMessage("Echo Failed");
            }
            else
            {
                appendMessage("Echo Succeeded - Woo Hoo!");
            }
        }

        private void buttonNoteTypes_Click(object sender, EventArgs e)
        {
            startTimer();
            NoteTypeInfo[] noteTypes = cfiClient.WebServiceAPI.GetAllNoteTypes();
            checkTimer("retrieve all note types via WS call");
            StringBuilder sb = new StringBuilder();
            foreach (NoteTypeInfo noteType in noteTypes)
            {
                sb.AppendLine( string.Format("NoteType[{0}]: {1}", noteType.TypeID, noteType.Description) );
            }
            appendMessage(sb.ToString());
        }

        private void buttonOrdersByCustomerName_Click(object sender, EventArgs e)
        {
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByCustomerLastName(textBoxLastName.Text, true, true, 60000);
            downloadAndDiaplyOrders(orderIDs);
        }

        private void buttonGetOrderByStoreNumber_Click(object sender, EventArgs e)
        {
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByStoreNumber(this.textBoxStoreNumber.Text, true, true, 60000);
            downloadAndDiaplyOrders(orderIDs);
        }

        private void buttongetOrderByPONumber_Click(object sender, EventArgs e)
        {
            int[] ids = cfiClient.WebServiceAPI.GetOrdersByPONumber(textBoxPONumber.Text, this.checkBoxScheduledOnly.Checked, this.checkBoxActiveOnly.Checked, 100);
            downloadAndDiaplyOrders(ids);
        }

        private void buttonGetOrdersByDate_Click(object sender, EventArgs e)
        {
            DateRange range = new DateRange();
            try
            {
                range.Start = DateTime.Parse(textBoxStartDate.Text);
            }
            catch
            {
                range.Start = DateTime.MinValue;
                textBoxStartDate.Text = range.Start.ToShortDateString();
            }

            try
            {
                range.End = DateTime.Parse(textBoxEndDate.Text);
            }
            catch
            {
                range.End = DateTime.MaxValue;
                textBoxEndDate.Text = range.End.ToShortDateString();
            }

            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByDateRange(range, true, true, 60000);
            downloadAndDiaplyOrders(orderIDs);
        }

        private void downloadAndDiaplyOrders(int[] orderIDs)
        {
            appendMessage(string.Format("{0} matching orders will be downloaded", orderIDs.Length));
            List<OrderInfo> orders = new List<OrderInfo>();
            int numDownloaded = 0;
            foreach (int orderID in orderIDs)
            {
                startTimer();
                OrderInfo order = cfiClient.WebServiceAPI.GetOrderByID(orderID);
                if (order == null)
                {
                    appendMessage(string.Format("Order ID {0} retrieval caused NULL OrderInfo object.", orderID));
                }
                else
                {
                    orders.Add(order);
                    numDownloaded++;
                    checkTimer(string.Format("Downloaded order ID {0} - ({1}/{2})", orderID, numDownloaded, orderIDs.Length));
                }
            }

            // add to list
            foreach (OrderInfo order in orders)
            {
                appendOrderText(order);
            }

            appendMessage(string.Format("{0} orders downloaded", orderIDs.Length));
        }

        private void appendMessage(string message)
        {
            textBox.AppendText( string.Format("{0}\r\n\r\n", message.Trim()) );
            textBox.SelectionStart = textBox.Text.Length - 1;
            textBox.ScrollToCaret();
        }

        private DateTime start;
        private void startTimer()
        {
            start = DateTime.Now;
        }

        private void checkTimer( string message )
        {
            TimeSpan span = DateTime.Now.Subtract(start);
            appendMessage(string.Format("[elapsed time: {0} milliSeconds] - {1}", span.TotalMilliseconds, message));
        }

        private void appendOrderText( OrderInfo order )
        {
            appendMessage( order.DebugText );
        }

        private void buttonUnittest_Click(object sender, EventArgs e)
        {
            runTests(CFIClientAutoUnitTest.TestSuite.WebService);
        }

        private void buttonRunCacheUnitTests_Click(object sender, EventArgs e)
        {
            runTests(CFIClientAutoUnitTest.TestSuite.LocalCache);
        }
        
        private void runTests( CFIClientAutoUnitTest.TestSuite suite )
        {
            CFIClientAutoUnitTest unitTester = new CFIClientAutoUnitTest(this.textBoxServiceUrl.Text, AppDataManager.DataCacheDirectory, this.checkBoxExtendedTests.Checked);
            unitTester.TestError += (sender, e) =>
            {
                string errorMessage = string.Format("{0}{1}", (e.IsException ? "[EXCEPTION] " : ""), e.Message);
                errorMessage = errorMessage.TrimEnd() + "\r\n";
                appendMessage( errorMessage );
                this.textBoxUnitTestResults.Text += errorMessage;
            };

            unitTester.TestMessage += (sender,e) =>
            {
                appendMessage(e.Message);
            };

            CFIClientAutoUnitTest.TestResults results = unitTester.RunTests( suite );
            this.textBoxUnitTestResults.Text += "\r\n" + results.SummaryText;

            textBoxUnitTestResults.SelectionStart = textBoxUnitTestResults.Text.Length - 1;
            textBoxUnitTestResults.ScrollToCaret();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string serviceUrl = textBoxServiceUrl.Text;
            string errorMessage;
            bool invalidUserName;
            if (cfiClient.Connect(serviceUrl, testUserName, false, out errorMessage, out invalidUserName) == false)
            {
                appendMessage(string.Format("Failed to connect to web service at {0}.\r\nError Message: {1}", serviceUrl, errorMessage));
            }
            else
            {
                appendMessage( string.Format("Connected to web service at {0}. Data will be cached in '{1}'", serviceUrl, AppDataManager.DataCacheDirectory) );
                hasBeenConnected = true;
            }
            updateUI();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            cfiClient.Disconnect();
            updateUI();
        }

        private string lastStateText = "";
        private void cfiClient_ChannelStateChanged(object sender, CFIClient.WebServiceChannelStateChangedEventArgs e)
        {
            this.statusLabel.Text = "Channel " + e.State.ToString();
            if ( lastStateText == "" )
            {
                this.appendMessage(string.Format("[{0}] Web Service State changed to {1}", DateTime.Now.ToLongTimeString(), e.State.ToString()));
            }
            else
            {
                this.appendMessage(string.Format("[{0}] Web Service State changed from {1} to {2}", DateTime.Now.ToLongTimeString(), lastStateText, e.State.ToString()));
            }
            updateUI();
        }

        private void textBoxServiceUrl_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void buttonGetWSDL_Click(object sender, EventArgs e)
        {
            try
            {
                string wsdlUrl = textBoxServiceUrl.Text.ToLower();
                if ( wsdlUrl.EndsWith("?wsdl") == false )
                {
                    wsdlUrl += "?wsdl";
                }
                string wsdl = CFI.Utilities.WebUtils.GetUrlContents(wsdlUrl);

                // format the WSDL text
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(wsdl);
                    string tempFile = System.IO.Path.GetTempFileName();
                    document.Save(tempFile);
                    wsdl = System.IO.File.ReadAllText(tempFile);
                }
                catch
                {
                }
                appendMessage(wsdl);
            }
            catch
            {
                appendMessage("Failed to get WSDL");
            }
            updateUI();
        }

        private void updateUI()
        {
            this.groupBoxWebServiceTests.Enabled = ( cfiClient.IsConnected() == true );
            bool urlPresent = ( string.IsNullOrEmpty( this.textBoxServiceUrl.Text ) == false );
            this.buttonConnect.Enabled = ((urlPresent == true) && ( cfiClient.IsConnected() == false) );
            this.buttonReconnect.Enabled = hasBeenConnected;
            this.buttonDisconnect.Enabled = ( cfiClient.IsConnected() == true );
            this.buttonGetWSDL.Enabled = (urlPresent == true);
            this.buttongetOrderByPONumber.Enabled = isPONumberValid();
            this.buttonOrdersByCustomerName.Enabled = isNameValid();
            this.buttonGetOrderByStoreNumber.Enabled = isStoreNumberValid();

            this.buttonGetOrdersByDate.Enabled = areDatesValid();

            this.groupBoxCacheTests.Enabled = true;
            this.buttonGetOrdersByMultipleCriteria.Enabled = areDatesValid() || isPONumberValid() || isNameValid();
            this.buttonDownLoadPoNumber.Enabled = (string.IsNullOrEmpty( this.textBoxDownLoadPONumber.Text ) == false ) && cfiClient.IsConnected();
            
            updateChannelStatus();
        }

        private bool isNameValid()
        {
            return (string.IsNullOrEmpty(this.textBoxLastName.Text) == false);
        }

        private bool isPONumberValid()
        {
            return (this.textBoxPONumber.Text != null) && ( (this.textBoxPONumber.Text.Length == 8) || (this.textBoxPONumber.Text.Length == 4) );
        }

        private bool isStoreNumberValid()
        {
            return (this.textBoxStoreNumber.Text != null) && (this.textBoxStoreNumber.Text.Length == 4);
        }

        private bool areDatesValid()
        {
            DateTime notUsed;
            bool startDateOk = DateTime.TryParse(this.textBoxStartDate.Text, out notUsed);
            bool endDateOk = DateTime.TryParse(this.textBoxEndDate.Text, out notUsed);
            return (startDateOk && endDateOk);
        }

        private void updateChannelStatus()
        {
            if (cfiClient != null)
            {
                if ((cfiClient.WebServiceAPI != null) && (cfiClient.WebServiceAPI.ChannelState != null))
                {
                    this.statusLabel.Text = "Channel " + cfiClient.WebServiceAPI.ChannelState.Value.ToString();
                }
                else
                {
                    this.statusLabel.Text = "No Channel";
                }
            }
            else
            {
                this.statusLabel.Text = "No Channel";
            }
        }

        private void textBoxLastName_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void textBoxStartDate_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void textBoxEndDate_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void textBoxPONumber_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void textBoxStoreNumber_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void buttonIsConnected_Click(object sender, EventArgs e)
        {
            appendMessage( cfiClient.IsConnected() ? "CFIClient CONNECTED" : "CFIClient DISCONNECTED" );
            updateUI();
        }

        private void textBoxDownLoadPONumber_TextChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void buttonDownLoadPoNumber_Click(object sender, EventArgs e)
        {
            string errorMessage;
            int[] downloadedIDs;
            WebServiceAPIResult result = cfiClient.DownLoadOrdersToCacheByPONumber(this.textBoxDownLoadPONumber.Text.Trim(), true, true, 100, out errorMessage, out downloadedIDs);
            if ( result == WebServiceAPIResult.Success )
            {
                appendMessage(string.Format("POs numbered {0} downloaded to cache", this.textBoxDownLoadPONumber.Text));

                foreach ( int id in downloadedIDs )
                {
                    CacheOrder cachedOrder = cfiClient.Cache.GetOrder( id );
                    appendMessage(cachedOrder.ToXml());
                }
            }
            else if ( result == WebServiceAPIResult.ConnectivityFail )
            {
                appendMessage(string.Format("POs numbered {0} Did not download because of a connection failure", this.textBoxDownLoadPONumber.Text));

            }
            else if ( result == WebServiceAPIResult.Fail )
            {
                appendMessage(string.Format("POs numbered {0} not downloaded to cache.\r\n{1}", this.textBoxDownLoadPONumber.Text, errorMessage));
            }
        }

        private void buttonGetOrdersByMultipleCriteria_Click(object sender, EventArgs e)
        {
            startTimer();
            DateRange range = new DateRange();
            try
            {
                range.Start = DateTime.Parse(textBoxStartDate.Text);
            }
            catch
            {
            }
            try
            {
                range.End = DateTime.Parse(textBoxEndDate.Text);
            }
            catch
            {
            }
            int[] orderIDs = cfiClient.WebServiceAPI.GetOrderIDsByMultipleCriteria(textBoxLastName.Text, this.textBoxPONumber.Text, this.textBoxStoreNumber.Text, range, true, true, 60000);
            downloadAndDiaplyOrders( orderIDs );
        }

        private void buttonReconnect_Click(object sender, EventArgs e)
        {
            cfiClient.Reconnect();
            updateUI();
        }

        private void buttonNetworkAvailable_Click(object sender, EventArgs e)
        {
            appendMessage(cfiClient.IsNetworkAvailable() ? "Network is AVAILABLE" : "Network is FUBAR");
            updateUI();
        }

        private void checkBoxExtendedTests_CheckedChanged(object sender, EventArgs e)
        {
            updateUI();
        }

    }
}
