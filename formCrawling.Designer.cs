namespace CrawlingProduct {
    partial class formCrawling {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent() {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabBrowser = new System.Windows.Forms.TabPage();
            this._chrome = new CefSharp.WinForms.ChromiumWebBrowser();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnShowHtml = new System.Windows.Forms.Button();
            this.btnGetProductList = new System.Windows.Forms.Button();
            this.btnComapareImage = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabProductList = new System.Windows.Forms.TabPage();
            this.gridProductList = new System.Windows.Forms.DataGridView();
            this.tabHtml = new System.Windows.Forms.TabPage();
            this.txtHtml = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnTest = new DevExpress.XtraEditors.SimpleButton();
            this.tabMain.SuspendLayout();
            this.tabBrowser.SuspendLayout();
            this.layoutTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabRight.SuspendLayout();
            this.tabProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductList)).BeginInit();
            this.tabHtml.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabBrowser);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(916, 492);
            this.tabMain.TabIndex = 0;
            // 
            // tabBrowser
            // 
            this.tabBrowser.Controls.Add(this._chrome);
            this.tabBrowser.Location = new System.Drawing.Point(4, 22);
            this.tabBrowser.Name = "tabBrowser";
            this.tabBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrowser.Size = new System.Drawing.Size(908, 466);
            this.tabBrowser.TabIndex = 0;
            this.tabBrowser.Text = "Browser";
            this.tabBrowser.UseVisualStyleBackColor = true;
            // 
            // _chrome
            // 
            this._chrome.ActivateBrowserOnCreation = false;
            this._chrome.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chrome.Location = new System.Drawing.Point(3, 3);
            this._chrome.Name = "_chrome";
            this._chrome.Size = new System.Drawing.Size(902, 460);
            this._chrome.TabIndex = 0;
            // 
            // layoutTop
            // 
            this.layoutTop.ColumnCount = 5;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTop.Controls.Add(this.lblId, 0, 1);
            this.layoutTop.Controls.Add(this.txtId, 1, 1);
            this.layoutTop.Controls.Add(this.lblPass, 0, 2);
            this.layoutTop.Controls.Add(this.txtPass, 1, 2);
            this.layoutTop.Controls.Add(this.btnStart, 2, 2);
            this.layoutTop.Controls.Add(this.btnShowHtml, 5, 2);
            this.layoutTop.Controls.Add(this.btnGetProductList, 4, 1);
            this.layoutTop.Controls.Add(this.btnComapareImage, 2, 1);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutTop.Location = new System.Drawing.Point(0, 0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 4;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutTop.Size = new System.Drawing.Size(1257, 69);
            this.layoutTop.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblId.Location = new System.Drawing.Point(3, 10);
            this.lblId.Margin = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(92, 25);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "아이디";
            this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(103, 13);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(120, 21);
            this.txtId.TabIndex = 2;
            // 
            // lblPass
            // 
            this.lblPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPass.Location = new System.Drawing.Point(3, 35);
            this.lblPass.Margin = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(92, 25);
            this.lblPass.TabIndex = 3;
            this.lblPass.Text = "패스워드";
            this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(103, 38);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(120, 21);
            this.txtPass.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(233, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(194, 19);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "자료수집 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnShowHtml
            // 
            this.btnShowHtml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowHtml.Location = new System.Drawing.Point(1110, 38);
            this.btnShowHtml.Name = "btnShowHtml";
            this.btnShowHtml.Size = new System.Drawing.Size(144, 19);
            this.btnShowHtml.TabIndex = 5;
            this.btnShowHtml.Text = "Html 보기";
            this.btnShowHtml.UseVisualStyleBackColor = true;
            this.btnShowHtml.Click += new System.EventHandler(this.btnShowHtml_Click);
            // 
            // btnGetProductList
            // 
            this.btnGetProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetProductList.Location = new System.Drawing.Point(1110, 13);
            this.btnGetProductList.Name = "btnGetProductList";
            this.btnGetProductList.Size = new System.Drawing.Size(144, 19);
            this.btnGetProductList.TabIndex = 5;
            this.btnGetProductList.Text = "상품리스트 가져오기";
            this.btnGetProductList.UseVisualStyleBackColor = true;
            this.btnGetProductList.Click += new System.EventHandler(this.btnGetProductList_Click);
            // 
            // btnComapareImage
            // 
            this.btnComapareImage.Location = new System.Drawing.Point(233, 13);
            this.btnComapareImage.Name = "btnComapareImage";
            this.btnComapareImage.Size = new System.Drawing.Size(194, 19);
            this.btnComapareImage.TabIndex = 0;
            this.btnComapareImage.Text = "이미지 비교 테스트(미작업)";
            this.btnComapareImage.UseVisualStyleBackColor = true;
            this.btnComapareImage.Click += new System.EventHandler(this.btnComapareImage_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(78, 70);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(679, 21);
            this.txtUrl.TabIndex = 6;
            this.txtUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUrl_KeyPress);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 69);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabRight);
            this.splitContainer1.Size = new System.Drawing.Size(1257, 492);
            this.splitContainer1.SplitterDistance = 916;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabRight
            // 
            this.tabRight.Controls.Add(this.tabProductList);
            this.tabRight.Controls.Add(this.tabHtml);
            this.tabRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRight.Location = new System.Drawing.Point(0, 0);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(337, 492);
            this.tabRight.TabIndex = 0;
            // 
            // tabProductList
            // 
            this.tabProductList.Controls.Add(this.btnTest);
            this.tabProductList.Controls.Add(this.gridProductList);
            this.tabProductList.Location = new System.Drawing.Point(4, 22);
            this.tabProductList.Name = "tabProductList";
            this.tabProductList.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductList.Size = new System.Drawing.Size(329, 466);
            this.tabProductList.TabIndex = 0;
            this.tabProductList.Text = "Product List";
            this.tabProductList.UseVisualStyleBackColor = true;
            // 
            // gridProductList
            // 
            this.gridProductList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProductList.Location = new System.Drawing.Point(3, 3);
            this.gridProductList.Name = "gridProductList";
            this.gridProductList.RowTemplate.Height = 23;
            this.gridProductList.Size = new System.Drawing.Size(323, 460);
            this.gridProductList.TabIndex = 0;
            // 
            // tabHtml
            // 
            this.tabHtml.Controls.Add(this.txtHtml);
            this.tabHtml.Location = new System.Drawing.Point(4, 22);
            this.tabHtml.Name = "tabHtml";
            this.tabHtml.Padding = new System.Windows.Forms.Padding(3);
            this.tabHtml.Size = new System.Drawing.Size(329, 466);
            this.tabHtml.TabIndex = 1;
            this.tabHtml.Text = "Html Source";
            this.tabHtml.UseVisualStyleBackColor = true;
            // 
            // txtHtml
            // 
            this.txtHtml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHtml.Location = new System.Drawing.Point(3, 3);
            this.txtHtml.Name = "txtHtml";
            this.txtHtml.Size = new System.Drawing.Size(323, 460);
            this.txtHtml.TabIndex = 0;
            this.txtHtml.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1257, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(1000, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(217, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(109, 27);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // formCrawling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 561);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.layoutTop);
            this.Name = "formCrawling";
            this.Text = "TaoBao Product Crawling";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formCrawling_FormClosing);
            this.tabMain.ResumeLayout(false);
            this.tabBrowser.ResumeLayout(false);
            this.layoutTop.ResumeLayout(false);
            this.layoutTop.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabRight.ResumeLayout(false);
            this.tabProductList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProductList)).EndInit();
            this.tabHtml.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabBrowser;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabProductList;
        private System.Windows.Forms.TabPage tabHtml;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnShowHtml;
        private System.Windows.Forms.Button btnGetProductList;
        private CefSharp.WinForms.ChromiumWebBrowser _chrome;
        private System.Windows.Forms.DataGridView gridProductList;
        private System.Windows.Forms.RichTextBox txtHtml;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnComapareImage;
        private DevExpress.XtraEditors.SimpleButton btnTest;
    }
}

