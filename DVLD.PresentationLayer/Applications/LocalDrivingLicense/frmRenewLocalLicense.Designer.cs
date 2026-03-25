namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    partial class frmRenewLocalLicense
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpLicenseInfo = new System.Windows.Forms.TabPage();
            this.btnNext = new System.Windows.Forms.Button();
            this.ctrlDriverLicenseInfoWithFilter1 = new DVLD.PresentationLayer.Licenses.Controls.ctrlDriverLicenseInfoWithFilter();
            this.tpApplicationInfo = new System.Windows.Forms.TabPage();
            this.lblApplicationDate = new System.Windows.Forms.Label();
            this.lblIssueDate = new System.Windows.Forms.Label();
            this.lblApplicationFees = new System.Windows.Forms.Label();
            this.lblLicenseFees = new System.Windows.Forms.Label();
            this.lblNewLicenseId = new System.Windows.Forms.Label();
            this.lblOldLicenseId = new System.Windows.Forms.Label();
            this.lblExpirationDate = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblTotalFees = new System.Windows.Forms.Label();
            this.lblRLApplicationId = new System.Windows.Forms.Label();
            this.btnRenew = new System.Windows.Forms.Button();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkNewLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.lnkLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpLicenseInfo.SuspendLayout();
            this.tpApplicationInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpLicenseInfo);
            this.tabControl.Controls.Add(this.tpApplicationInfo);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1046, 758);
            this.tabControl.TabIndex = 0;
            // 
            // tpLicenseInfo
            // 
            this.tpLicenseInfo.Controls.Add(this.btnNext);
            this.tpLicenseInfo.Controls.Add(this.ctrlDriverLicenseInfoWithFilter1);
            this.tpLicenseInfo.Location = new System.Drawing.Point(4, 34);
            this.tpLicenseInfo.Name = "tpLicenseInfo";
            this.tpLicenseInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpLicenseInfo.Size = new System.Drawing.Size(1038, 720);
            this.tpLicenseInfo.TabIndex = 0;
            this.tpLicenseInfo.Text = "License Info";
            this.tpLicenseInfo.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(879, 669);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(153, 45);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // ctrlDriverLicenseInfoWithFilter1
            // 
            this.ctrlDriverLicenseInfoWithFilter1.BackColor = System.Drawing.Color.White;
            this.ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            this.ctrlDriverLicenseInfoWithFilter1.FilterText = "";
            this.ctrlDriverLicenseInfoWithFilter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlDriverLicenseInfoWithFilter1.Location = new System.Drawing.Point(7, 7);
            this.ctrlDriverLicenseInfoWithFilter1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlDriverLicenseInfoWithFilter1.Name = "ctrlDriverLicenseInfoWithFilter1";
            this.ctrlDriverLicenseInfoWithFilter1.Size = new System.Drawing.Size(1024, 639);
            this.ctrlDriverLicenseInfoWithFilter1.TabIndex = 1;
            this.ctrlDriverLicenseInfoWithFilter1.OnLicenseSelected += new System.Action<int>(this.ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected);
            // 
            // tpApplicationInfo
            // 
            this.tpApplicationInfo.Controls.Add(this.lblApplicationDate);
            this.tpApplicationInfo.Controls.Add(this.lblIssueDate);
            this.tpApplicationInfo.Controls.Add(this.lblApplicationFees);
            this.tpApplicationInfo.Controls.Add(this.lblLicenseFees);
            this.tpApplicationInfo.Controls.Add(this.lblNewLicenseId);
            this.tpApplicationInfo.Controls.Add(this.lblOldLicenseId);
            this.tpApplicationInfo.Controls.Add(this.lblExpirationDate);
            this.tpApplicationInfo.Controls.Add(this.lblCreatedBy);
            this.tpApplicationInfo.Controls.Add(this.lblTotalFees);
            this.tpApplicationInfo.Controls.Add(this.lblRLApplicationId);
            this.tpApplicationInfo.Controls.Add(this.btnRenew);
            this.tpApplicationInfo.Controls.Add(this.txtNotes);
            this.tpApplicationInfo.Controls.Add(this.label7);
            this.tpApplicationInfo.Controls.Add(this.label8);
            this.tpApplicationInfo.Controls.Add(this.label9);
            this.tpApplicationInfo.Controls.Add(this.label10);
            this.tpApplicationInfo.Controls.Add(this.label11);
            this.tpApplicationInfo.Controls.Add(this.label6);
            this.tpApplicationInfo.Controls.Add(this.label5);
            this.tpApplicationInfo.Controls.Add(this.label4);
            this.tpApplicationInfo.Controls.Add(this.label3);
            this.tpApplicationInfo.Controls.Add(this.label2);
            this.tpApplicationInfo.Controls.Add(this.label1);
            this.tpApplicationInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpApplicationInfo.Location = new System.Drawing.Point(4, 34);
            this.tpApplicationInfo.Name = "tpApplicationInfo";
            this.tpApplicationInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpApplicationInfo.Size = new System.Drawing.Size(1038, 720);
            this.tpApplicationInfo.TabIndex = 1;
            this.tpApplicationInfo.Text = "Application Info";
            this.tpApplicationInfo.UseVisualStyleBackColor = true;
            // 
            // lblApplicationDate
            // 
            this.lblApplicationDate.AutoSize = true;
            this.lblApplicationDate.Location = new System.Drawing.Point(230, 142);
            this.lblApplicationDate.Name = "lblApplicationDate";
            this.lblApplicationDate.Size = new System.Drawing.Size(49, 29);
            this.lblApplicationDate.TabIndex = 23;
            this.lblApplicationDate.Text = "???";
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.AutoSize = true;
            this.lblIssueDate.Location = new System.Drawing.Point(168, 232);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(49, 29);
            this.lblIssueDate.TabIndex = 22;
            this.lblIssueDate.Text = "???";
            // 
            // lblApplicationFees
            // 
            this.lblApplicationFees.AutoSize = true;
            this.lblApplicationFees.Location = new System.Drawing.Point(235, 322);
            this.lblApplicationFees.Name = "lblApplicationFees";
            this.lblApplicationFees.Size = new System.Drawing.Size(49, 29);
            this.lblApplicationFees.TabIndex = 21;
            this.lblApplicationFees.Text = "???";
            // 
            // lblLicenseFees
            // 
            this.lblLicenseFees.AutoSize = true;
            this.lblLicenseFees.Location = new System.Drawing.Point(200, 412);
            this.lblLicenseFees.Name = "lblLicenseFees";
            this.lblLicenseFees.Size = new System.Drawing.Size(49, 29);
            this.lblLicenseFees.TabIndex = 20;
            this.lblLicenseFees.Text = "???";
            // 
            // lblNewLicenseId
            // 
            this.lblNewLicenseId.AutoSize = true;
            this.lblNewLicenseId.Location = new System.Drawing.Point(905, 67);
            this.lblNewLicenseId.Name = "lblNewLicenseId";
            this.lblNewLicenseId.Size = new System.Drawing.Size(49, 29);
            this.lblNewLicenseId.TabIndex = 19;
            this.lblNewLicenseId.Text = "???";
            // 
            // lblOldLicenseId
            // 
            this.lblOldLicenseId.AutoSize = true;
            this.lblOldLicenseId.Location = new System.Drawing.Point(840, 157);
            this.lblOldLicenseId.Name = "lblOldLicenseId";
            this.lblOldLicenseId.Size = new System.Drawing.Size(49, 29);
            this.lblOldLicenseId.TabIndex = 18;
            this.lblOldLicenseId.Text = "???";
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.AutoSize = true;
            this.lblExpirationDate.Location = new System.Drawing.Point(845, 247);
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Size = new System.Drawing.Size(49, 29);
            this.lblExpirationDate.TabIndex = 17;
            this.lblExpirationDate.Text = "???";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(801, 337);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(49, 29);
            this.lblCreatedBy.TabIndex = 16;
            this.lblCreatedBy.Text = "???";
            // 
            // lblTotalFees
            // 
            this.lblTotalFees.AutoSize = true;
            this.lblTotalFees.Location = new System.Drawing.Point(798, 427);
            this.lblTotalFees.Name = "lblTotalFees";
            this.lblTotalFees.Size = new System.Drawing.Size(49, 29);
            this.lblTotalFees.TabIndex = 15;
            this.lblTotalFees.Text = "???";
            // 
            // lblRLApplicationId
            // 
            this.lblRLApplicationId.AutoSize = true;
            this.lblRLApplicationId.Location = new System.Drawing.Point(251, 52);
            this.lblRLApplicationId.Name = "lblRLApplicationId";
            this.lblRLApplicationId.Size = new System.Drawing.Size(49, 29);
            this.lblRLApplicationId.TabIndex = 12;
            this.lblRLApplicationId.Text = "???";
            // 
            // btnRenew
            // 
            this.btnRenew.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenew.Location = new System.Drawing.Point(879, 659);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(153, 45);
            this.btnRenew.TabIndex = 4;
            this.btnRenew.Text = "Renew";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(128, 499);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(904, 132);
            this.txtNotes.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(657, 427);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 29);
            this.label7.TabIndex = 10;
            this.label7.Text = "Total Fees:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(657, 337);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(138, 29);
            this.label8.TabIndex = 9;
            this.label8.Text = "Created By:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(657, 247);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(182, 29);
            this.label9.TabIndex = 8;
            this.label9.Text = "Expiration Date:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(657, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 29);
            this.label10.TabIndex = 7;
            this.label10.Text = "Old License ID:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(657, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(242, 29);
            this.label11.TabIndex = 6;
            this.label11.Text = "Renewed License ID:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 502);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 29);
            this.label6.TabIndex = 5;
            this.label6.Text = "Notes:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 412);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 29);
            this.label5.TabIndex = 4;
            this.label5.Text = "License Fees:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Application Fees:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Issue Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "R.L Application  ID:";
            // 
            // lnkNewLicenseInfo
            // 
            this.lnkNewLicenseInfo.AutoSize = true;
            this.lnkNewLicenseInfo.Enabled = false;
            this.lnkNewLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNewLicenseInfo.Location = new System.Drawing.Point(267, 785);
            this.lnkNewLicenseInfo.Name = "lnkNewLicenseInfo";
            this.lnkNewLicenseInfo.Size = new System.Drawing.Size(265, 29);
            this.lnkNewLicenseInfo.TabIndex = 25;
            this.lnkNewLicenseInfo.TabStop = true;
            this.lnkNewLicenseInfo.Text = "Show New License Info";
            this.lnkNewLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewLicenseInfo_LinkClicked);
            // 
            // lnkLicenseHistory
            // 
            this.lnkLicenseHistory.AutoSize = true;
            this.lnkLicenseHistory.Enabled = false;
            this.lnkLicenseHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLicenseHistory.Location = new System.Drawing.Point(14, 785);
            this.lnkLicenseHistory.Name = "lnkLicenseHistory";
            this.lnkLicenseHistory.Size = new System.Drawing.Size(244, 29);
            this.lnkLicenseHistory.TabIndex = 24;
            this.lnkLicenseHistory.TabStop = true;
            this.lnkLicenseHistory.Text = "Show License History";
            this.lnkLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLicenseHistory_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(895, 785);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(153, 46);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close ❌";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmRenewLocalLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 843);
            this.Controls.Add(this.lnkNewLicenseInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lnkLicenseHistory);
            this.Controls.Add(this.tabControl);
            this.Name = "frmRenewLocalLicense";
            this.Text = "Renew Local Driving License";
            this.Load += new System.EventHandler(this.frmRenewLocalLicense_Load);
            this.tabControl.ResumeLayout(false);
            this.tpLicenseInfo.ResumeLayout(false);
            this.tpApplicationInfo.ResumeLayout(false);
            this.tpApplicationInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpLicenseInfo;
        private System.Windows.Forms.TabPage tpApplicationInfo;
        private Licenses.Controls.ctrlDriverLicenseInfoWithFilter ctrlDriverLicenseInfoWithFilter1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblApplicationDate;
        private System.Windows.Forms.Label lblIssueDate;
        private System.Windows.Forms.Label lblApplicationFees;
        private System.Windows.Forms.Label lblLicenseFees;
        private System.Windows.Forms.Label lblNewLicenseId;
        private System.Windows.Forms.Label lblOldLicenseId;
        private System.Windows.Forms.Label lblExpirationDate;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblTotalFees;
        private System.Windows.Forms.Label lblRLApplicationId;
        private System.Windows.Forms.LinkLabel lnkNewLicenseInfo;
        private System.Windows.Forms.LinkLabel lnkLicenseHistory;
    }
}