namespace CFI.ServicesTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonEcho = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.groupBoxWebServiceTests = new System.Windows.Forms.GroupBox();
            this.textBoxStoreNumber = new System.Windows.Forms.TextBox();
            this.buttonGetOrderByStoreNumber = new System.Windows.Forms.Button();
            this.buttonGetOrdersByMultipleCriteria = new System.Windows.Forms.Button();
            this.textBoxPONumber = new System.Windows.Forms.TextBox();
            this.buttongetOrderByPONumber = new System.Windows.Forms.Button();
            this.buttonUnittest = new System.Windows.Forms.Button();
            this.textBoxEndDate = new System.Windows.Forms.TextBox();
            this.textBoxStartDate = new System.Windows.Forms.TextBox();
            this.buttonGetOrdersByDate = new System.Windows.Forms.Button();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.buttonOrdersByCustomerName = new System.Windows.Forms.Button();
            this.buttonNoteTypes = new System.Windows.Forms.Button();
            this.textBoxUnitTestResults = new System.Windows.Forms.TextBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.textBoxServiceUrl = new System.Windows.Forms.TextBox();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonGetWSDL = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxCacheTests = new System.Windows.Forms.GroupBox();
            this.buttonNetworkAvailable = new System.Windows.Forms.Button();
            this.buttonRunCacheUnitTests = new System.Windows.Forms.Button();
            this.textBoxDownLoadPONumber = new System.Windows.Forms.TextBox();
            this.buttonDownLoadPoNumber = new System.Windows.Forms.Button();
            this.buttonIsConnected = new System.Windows.Forms.Button();
            this.buttonReconnect = new System.Windows.Forms.Button();
            this.checkBoxExtendedTests = new System.Windows.Forms.CheckBox();
            this.checkBoxScheduledOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxActiveOnly = new System.Windows.Forms.CheckBox();
            this.groupBoxWebServiceTests.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBoxCacheTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonEcho
            // 
            this.buttonEcho.Location = new System.Drawing.Point(6, 19);
            this.buttonEcho.Name = "buttonEcho";
            this.buttonEcho.Size = new System.Drawing.Size(119, 23);
            this.buttonEcho.TabIndex = 0;
            this.buttonEcho.Text = "Echo";
            this.buttonEcho.UseVisualStyleBackColor = true;
            this.buttonEcho.Click += new System.EventHandler(this.buttonEcho_Click);
            // 
            // textBox
            // 
            this.textBox.AcceptsReturn = true;
            this.textBox.AcceptsTab = true;
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(15, 544);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(1157, 187);
            this.textBox.TabIndex = 2;
            // 
            // groupBoxWebServiceTests
            // 
            this.groupBoxWebServiceTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxWebServiceTests.Controls.Add(this.textBoxStoreNumber);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonGetOrderByStoreNumber);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonGetOrdersByMultipleCriteria);
            this.groupBoxWebServiceTests.Controls.Add(this.textBoxPONumber);
            this.groupBoxWebServiceTests.Controls.Add(this.buttongetOrderByPONumber);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonUnittest);
            this.groupBoxWebServiceTests.Controls.Add(this.textBoxEndDate);
            this.groupBoxWebServiceTests.Controls.Add(this.textBoxStartDate);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonGetOrdersByDate);
            this.groupBoxWebServiceTests.Controls.Add(this.textBoxLastName);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonOrdersByCustomerName);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonNoteTypes);
            this.groupBoxWebServiceTests.Controls.Add(this.buttonEcho);
            this.groupBoxWebServiceTests.Location = new System.Drawing.Point(15, 133);
            this.groupBoxWebServiceTests.Name = "groupBoxWebServiceTests";
            this.groupBoxWebServiceTests.Size = new System.Drawing.Size(850, 201);
            this.groupBoxWebServiceTests.TabIndex = 3;
            this.groupBoxWebServiceTests.TabStop = false;
            this.groupBoxWebServiceTests.Text = "Service Tests";
            // 
            // textBoxStoreNumber
            // 
            this.textBoxStoreNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStoreNumber.Location = new System.Drawing.Point(605, 140);
            this.textBoxStoreNumber.Name = "textBoxStoreNumber";
            this.textBoxStoreNumber.Size = new System.Drawing.Size(233, 20);
            this.textBoxStoreNumber.TabIndex = 14;
            this.textBoxStoreNumber.TextChanged += new System.EventHandler(this.textBoxStoreNumber_TextChanged);
            // 
            // buttonGetOrderByStoreNumber
            // 
            this.buttonGetOrderByStoreNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetOrderByStoreNumber.Location = new System.Drawing.Point(396, 138);
            this.buttonGetOrderByStoreNumber.Name = "buttonGetOrderByStoreNumber";
            this.buttonGetOrderByStoreNumber.Size = new System.Drawing.Size(182, 23);
            this.buttonGetOrderByStoreNumber.TabIndex = 13;
            this.buttonGetOrderByStoreNumber.Text = "Get Order by Store Number";
            this.buttonGetOrderByStoreNumber.UseVisualStyleBackColor = true;
            this.buttonGetOrderByStoreNumber.Click += new System.EventHandler(this.buttonGetOrderByStoreNumber_Click);
            // 
            // buttonGetOrdersByMultipleCriteria
            // 
            this.buttonGetOrdersByMultipleCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetOrdersByMultipleCriteria.Location = new System.Drawing.Point(396, 167);
            this.buttonGetOrdersByMultipleCriteria.Name = "buttonGetOrdersByMultipleCriteria";
            this.buttonGetOrdersByMultipleCriteria.Size = new System.Drawing.Size(182, 23);
            this.buttonGetOrdersByMultipleCriteria.TabIndex = 12;
            this.buttonGetOrdersByMultipleCriteria.Text = "Get Orders (multiple criteria)";
            this.buttonGetOrdersByMultipleCriteria.UseVisualStyleBackColor = true;
            this.buttonGetOrdersByMultipleCriteria.Click += new System.EventHandler(this.buttonGetOrdersByMultipleCriteria_Click);
            // 
            // textBoxPONumber
            // 
            this.textBoxPONumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPONumber.Location = new System.Drawing.Point(605, 112);
            this.textBoxPONumber.Name = "textBoxPONumber";
            this.textBoxPONumber.Size = new System.Drawing.Size(233, 20);
            this.textBoxPONumber.TabIndex = 11;
            this.textBoxPONumber.TextChanged += new System.EventHandler(this.textBoxPONumber_TextChanged);
            // 
            // buttongetOrderByPONumber
            // 
            this.buttongetOrderByPONumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttongetOrderByPONumber.Location = new System.Drawing.Point(396, 110);
            this.buttongetOrderByPONumber.Name = "buttongetOrderByPONumber";
            this.buttongetOrderByPONumber.Size = new System.Drawing.Size(182, 23);
            this.buttongetOrderByPONumber.TabIndex = 10;
            this.buttongetOrderByPONumber.Text = "Get Order by PO Number";
            this.buttongetOrderByPONumber.UseVisualStyleBackColor = true;
            this.buttongetOrderByPONumber.Click += new System.EventHandler(this.buttongetOrderByPONumber_Click);
            // 
            // buttonUnittest
            // 
            this.buttonUnittest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUnittest.Location = new System.Drawing.Point(6, 167);
            this.buttonUnittest.Name = "buttonUnittest";
            this.buttonUnittest.Size = new System.Drawing.Size(131, 23);
            this.buttonUnittest.TabIndex = 8;
            this.buttonUnittest.Text = "Run Service Unit Tests";
            this.buttonUnittest.UseVisualStyleBackColor = true;
            this.buttonUnittest.Click += new System.EventHandler(this.buttonUnittest_Click);
            // 
            // textBoxEndDate
            // 
            this.textBoxEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEndDate.Location = new System.Drawing.Point(605, 77);
            this.textBoxEndDate.Name = "textBoxEndDate";
            this.textBoxEndDate.Size = new System.Drawing.Size(233, 20);
            this.textBoxEndDate.TabIndex = 7;
            this.textBoxEndDate.TextChanged += new System.EventHandler(this.textBoxEndDate_TextChanged);
            // 
            // textBoxStartDate
            // 
            this.textBoxStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStartDate.Location = new System.Drawing.Point(605, 50);
            this.textBoxStartDate.Name = "textBoxStartDate";
            this.textBoxStartDate.Size = new System.Drawing.Size(233, 20);
            this.textBoxStartDate.TabIndex = 6;
            this.textBoxStartDate.TextChanged += new System.EventHandler(this.textBoxStartDate_TextChanged);
            // 
            // buttonGetOrdersByDate
            // 
            this.buttonGetOrdersByDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetOrdersByDate.Location = new System.Drawing.Point(396, 48);
            this.buttonGetOrdersByDate.Name = "buttonGetOrdersByDate";
            this.buttonGetOrdersByDate.Size = new System.Drawing.Size(182, 23);
            this.buttonGetOrdersByDate.TabIndex = 5;
            this.buttonGetOrdersByDate.Text = "Get Orders by Date Range";
            this.buttonGetOrdersByDate.UseVisualStyleBackColor = true;
            this.buttonGetOrdersByDate.Click += new System.EventHandler(this.buttonGetOrdersByDate_Click);
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLastName.Location = new System.Drawing.Point(605, 19);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Size = new System.Drawing.Size(233, 20);
            this.textBoxLastName.TabIndex = 4;
            this.textBoxLastName.TextChanged += new System.EventHandler(this.textBoxLastName_TextChanged);
            // 
            // buttonOrdersByCustomerName
            // 
            this.buttonOrdersByCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOrdersByCustomerName.Location = new System.Drawing.Point(396, 19);
            this.buttonOrdersByCustomerName.Name = "buttonOrdersByCustomerName";
            this.buttonOrdersByCustomerName.Size = new System.Drawing.Size(182, 23);
            this.buttonOrdersByCustomerName.TabIndex = 3;
            this.buttonOrdersByCustomerName.Text = "Get Orders by Customer Name";
            this.buttonOrdersByCustomerName.UseVisualStyleBackColor = true;
            this.buttonOrdersByCustomerName.Click += new System.EventHandler(this.buttonOrdersByCustomerName_Click);
            // 
            // buttonNoteTypes
            // 
            this.buttonNoteTypes.Location = new System.Drawing.Point(6, 48);
            this.buttonNoteTypes.Name = "buttonNoteTypes";
            this.buttonNoteTypes.Size = new System.Drawing.Size(119, 23);
            this.buttonNoteTypes.TabIndex = 2;
            this.buttonNoteTypes.Text = "Get Note Types";
            this.buttonNoteTypes.UseVisualStyleBackColor = true;
            this.buttonNoteTypes.Click += new System.EventHandler(this.buttonNoteTypes_Click);
            // 
            // textBoxUnitTestResults
            // 
            this.textBoxUnitTestResults.AcceptsReturn = true;
            this.textBoxUnitTestResults.AcceptsTab = true;
            this.textBoxUnitTestResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUnitTestResults.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUnitTestResults.Location = new System.Drawing.Point(15, 340);
            this.textBoxUnitTestResults.Multiline = true;
            this.textBoxUnitTestResults.Name = "textBoxUnitTestResults";
            this.textBoxUnitTestResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxUnitTestResults.Size = new System.Drawing.Size(1157, 198);
            this.textBoxUnitTestResults.TabIndex = 9;
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(12, 13);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(68, 13);
            this.labelURL.TabIndex = 4;
            this.labelURL.Text = "Service URL";
            // 
            // textBoxServiceUrl
            // 
            this.textBoxServiceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServiceUrl.Location = new System.Drawing.Point(86, 12);
            this.textBoxServiceUrl.Name = "textBoxServiceUrl";
            this.textBoxServiceUrl.Size = new System.Drawing.Size(773, 20);
            this.textBoxServiceUrl.TabIndex = 5;
            this.textBoxServiceUrl.TextChanged += new System.EventHandler(this.textBoxServiceUrl_TextChanged);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisconnect.Location = new System.Drawing.Point(970, 12);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(99, 23);
            this.buttonDisconnect.TabIndex = 10;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.Location = new System.Drawing.Point(878, 13);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 11;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonGetWSDL
            // 
            this.buttonGetWSDL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetWSDL.Location = new System.Drawing.Point(1075, 11);
            this.buttonGetWSDL.Name = "buttonGetWSDL";
            this.buttonGetWSDL.Size = new System.Drawing.Size(75, 23);
            this.buttonGetWSDL.TabIndex = 12;
            this.buttonGetWSDL.Text = "Get WSDL";
            this.buttonGetWSDL.UseVisualStyleBackColor = true;
            this.buttonGetWSDL.Click += new System.EventHandler(this.buttonGetWSDL_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 740);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip.TabIndex = 13;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 17);
            this.statusLabel.Text = "{status goes here}";
            // 
            // groupBoxCacheTests
            // 
            this.groupBoxCacheTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCacheTests.Controls.Add(this.buttonNetworkAvailable);
            this.groupBoxCacheTests.Controls.Add(this.buttonRunCacheUnitTests);
            this.groupBoxCacheTests.Controls.Add(this.textBoxDownLoadPONumber);
            this.groupBoxCacheTests.Controls.Add(this.buttonDownLoadPoNumber);
            this.groupBoxCacheTests.Controls.Add(this.buttonIsConnected);
            this.groupBoxCacheTests.Location = new System.Drawing.Point(878, 140);
            this.groupBoxCacheTests.Name = "groupBoxCacheTests";
            this.groupBoxCacheTests.Size = new System.Drawing.Size(301, 190);
            this.groupBoxCacheTests.TabIndex = 14;
            this.groupBoxCacheTests.TabStop = false;
            this.groupBoxCacheTests.Text = "Cache Tests";
            // 
            // buttonNetworkAvailable
            // 
            this.buttonNetworkAvailable.Location = new System.Drawing.Point(148, 19);
            this.buttonNetworkAvailable.Name = "buttonNetworkAvailable";
            this.buttonNetworkAvailable.Size = new System.Drawing.Size(147, 23);
            this.buttonNetworkAvailable.TabIndex = 4;
            this.buttonNetworkAvailable.Text = "Is Network Available";
            this.buttonNetworkAvailable.UseVisualStyleBackColor = true;
            this.buttonNetworkAvailable.Click += new System.EventHandler(this.buttonNetworkAvailable_Click);
            // 
            // buttonRunCacheUnitTests
            // 
            this.buttonRunCacheUnitTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRunCacheUnitTests.Location = new System.Drawing.Point(6, 161);
            this.buttonRunCacheUnitTests.Name = "buttonRunCacheUnitTests";
            this.buttonRunCacheUnitTests.Size = new System.Drawing.Size(136, 23);
            this.buttonRunCacheUnitTests.TabIndex = 3;
            this.buttonRunCacheUnitTests.Text = "Run Cache Unit Tests";
            this.buttonRunCacheUnitTests.UseVisualStyleBackColor = true;
            this.buttonRunCacheUnitTests.Click += new System.EventHandler(this.buttonRunCacheUnitTests_Click);
            // 
            // textBoxDownLoadPONumber
            // 
            this.textBoxDownLoadPONumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDownLoadPONumber.Location = new System.Drawing.Point(148, 50);
            this.textBoxDownLoadPONumber.Name = "textBoxDownLoadPONumber";
            this.textBoxDownLoadPONumber.Size = new System.Drawing.Size(147, 20);
            this.textBoxDownLoadPONumber.TabIndex = 2;
            this.textBoxDownLoadPONumber.TextChanged += new System.EventHandler(this.textBoxDownLoadPONumber_TextChanged);
            // 
            // buttonDownLoadPoNumber
            // 
            this.buttonDownLoadPoNumber.Location = new System.Drawing.Point(7, 50);
            this.buttonDownLoadPoNumber.Name = "buttonDownLoadPoNumber";
            this.buttonDownLoadPoNumber.Size = new System.Drawing.Size(135, 23);
            this.buttonDownLoadPoNumber.TabIndex = 1;
            this.buttonDownLoadPoNumber.Text = "Download by PO Number";
            this.buttonDownLoadPoNumber.UseVisualStyleBackColor = true;
            this.buttonDownLoadPoNumber.Click += new System.EventHandler(this.buttonDownLoadPoNumber_Click);
            // 
            // buttonIsConnected
            // 
            this.buttonIsConnected.Location = new System.Drawing.Point(6, 19);
            this.buttonIsConnected.Name = "buttonIsConnected";
            this.buttonIsConnected.Size = new System.Drawing.Size(136, 23);
            this.buttonIsConnected.TabIndex = 0;
            this.buttonIsConnected.Text = "Is Connected";
            this.buttonIsConnected.UseVisualStyleBackColor = true;
            this.buttonIsConnected.Click += new System.EventHandler(this.buttonIsConnected_Click);
            // 
            // buttonReconnect
            // 
            this.buttonReconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReconnect.Location = new System.Drawing.Point(878, 43);
            this.buttonReconnect.Name = "buttonReconnect";
            this.buttonReconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonReconnect.TabIndex = 15;
            this.buttonReconnect.Text = "Reconnect";
            this.buttonReconnect.UseVisualStyleBackColor = true;
            this.buttonReconnect.Click += new System.EventHandler(this.buttonReconnect_Click);
            // 
            // checkBoxExtendedTests
            // 
            this.checkBoxExtendedTests.AutoSize = true;
            this.checkBoxExtendedTests.Location = new System.Drawing.Point(21, 43);
            this.checkBoxExtendedTests.Name = "checkBoxExtendedTests";
            this.checkBoxExtendedTests.Size = new System.Drawing.Size(197, 17);
            this.checkBoxExtendedTests.TabIndex = 16;
            this.checkBoxExtendedTests.Text = "Extended Tests (take MUCH longer)";
            this.checkBoxExtendedTests.UseVisualStyleBackColor = true;
            this.checkBoxExtendedTests.CheckedChanged += new System.EventHandler(this.checkBoxExtendedTests_CheckedChanged);
            // 
            // checkBoxScheduledOnly
            // 
            this.checkBoxScheduledOnly.AutoSize = true;
            this.checkBoxScheduledOnly.Location = new System.Drawing.Point(21, 66);
            this.checkBoxScheduledOnly.Name = "checkBoxScheduledOnly";
            this.checkBoxScheduledOnly.Size = new System.Drawing.Size(135, 17);
            this.checkBoxScheduledOnly.TabIndex = 17;
            this.checkBoxScheduledOnly.Text = "Scheduled Orders Only";
            this.checkBoxScheduledOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxActiveOnly
            // 
            this.checkBoxActiveOnly.AutoSize = true;
            this.checkBoxActiveOnly.Location = new System.Drawing.Point(21, 90);
            this.checkBoxActiveOnly.Name = "checkBoxActiveOnly";
            this.checkBoxActiveOnly.Size = new System.Drawing.Size(114, 17);
            this.checkBoxActiveOnly.TabIndex = 18;
            this.checkBoxActiveOnly.Text = "Active Orders Only";
            this.checkBoxActiveOnly.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 762);
            this.Controls.Add(this.checkBoxActiveOnly);
            this.Controls.Add(this.checkBoxScheduledOnly);
            this.Controls.Add(this.checkBoxExtendedTests);
            this.Controls.Add(this.buttonReconnect);
            this.Controls.Add(this.groupBoxCacheTests);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.textBoxUnitTestResults);
            this.Controls.Add(this.buttonGetWSDL);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.textBoxServiceUrl);
            this.Controls.Add(this.labelURL);
            this.Controls.Add(this.groupBoxWebServiceTests);
            this.Controls.Add(this.textBox);
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "MainForm";
            this.Text = "Web Service Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxWebServiceTests.ResumeLayout(false);
            this.groupBoxWebServiceTests.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBoxCacheTests.ResumeLayout(false);
            this.groupBoxCacheTests.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonEcho;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.GroupBox groupBoxWebServiceTests;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.Button buttonOrdersByCustomerName;
        private System.Windows.Forms.Button buttonNoteTypes;
        private System.Windows.Forms.TextBox textBoxEndDate;
        private System.Windows.Forms.TextBox textBoxStartDate;
        private System.Windows.Forms.Button buttonGetOrdersByDate;
        private System.Windows.Forms.Button buttonUnittest;
        private System.Windows.Forms.TextBox textBoxUnitTestResults;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox textBoxServiceUrl;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonGetWSDL;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TextBox textBoxPONumber;
        private System.Windows.Forms.Button buttongetOrderByPONumber;
        private System.Windows.Forms.GroupBox groupBoxCacheTests;
        private System.Windows.Forms.Button buttonIsConnected;
        private System.Windows.Forms.TextBox textBoxDownLoadPONumber;
        private System.Windows.Forms.Button buttonDownLoadPoNumber;
        private System.Windows.Forms.Button buttonRunCacheUnitTests;
        private System.Windows.Forms.Button buttonGetOrdersByMultipleCriteria;
        private System.Windows.Forms.Button buttonReconnect;
        private System.Windows.Forms.Button buttonNetworkAvailable;
        private System.Windows.Forms.CheckBox checkBoxExtendedTests;
        private System.Windows.Forms.TextBox textBoxStoreNumber;
        private System.Windows.Forms.Button buttonGetOrderByStoreNumber;
        private System.Windows.Forms.CheckBox checkBoxScheduledOnly;
        private System.Windows.Forms.CheckBox checkBoxActiveOnly;
    }
}

