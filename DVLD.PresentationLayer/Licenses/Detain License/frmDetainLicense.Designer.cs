namespace DVLD.PresentationLayer.Licenses.Detain_License
{
    partial class frmDetainLicense
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
            this.label1 = new System.Windows.Forms.Label();
            this.lnkNewLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.lnkLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.btnDetain = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlDriverLicenseInfoWithFilter1 = new DVLD.PresentationLayer.Licenses.Controls.ctrlDriverLicenseInfoWithFilter();
            this.gbDetainInfo = new System.Windows.Forms.GroupBox();
            this.txtFees = new System.Windows.Forms.TextBox();
            this.lblDetainId = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblLicenseId = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDetainDate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbDetainInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1268, 78);
            this.label1.TabIndex = 16;
            this.label1.Text = "Detain License";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkNewLicenseInfo
            // 
            this.lnkNewLicenseInfo.AutoSize = true;
            this.lnkNewLicenseInfo.Enabled = false;
            this.lnkNewLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNewLicenseInfo.Location = new System.Drawing.Point(225, 1055);
            this.lnkNewLicenseInfo.Name = "lnkNewLicenseInfo";
            this.lnkNewLicenseInfo.Size = new System.Drawing.Size(172, 25);
            this.lnkNewLicenseInfo.TabIndex = 29;
            this.lnkNewLicenseInfo.TabStop = true;
            this.lnkNewLicenseInfo.Text = "Show License Info";
            this.lnkNewLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewLicenseInfo_LinkClicked);
            // 
            // lnkLicenseHistory
            // 
            this.lnkLicenseHistory.AutoSize = true;
            this.lnkLicenseHistory.Enabled = false;
            this.lnkLicenseHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLicenseHistory.Location = new System.Drawing.Point(19, 1055);
            this.lnkLicenseHistory.Name = "lnkLicenseHistory";
            this.lnkLicenseHistory.Size = new System.Drawing.Size(200, 25);
            this.lnkLicenseHistory.TabIndex = 28;
            this.lnkLicenseHistory.TabStop = true;
            this.lnkLicenseHistory.Text = "Show License History";
            this.lnkLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLicenseHistory_LinkClicked);
            // 
            // btnDetain
            // 
            this.btnDetain.Enabled = false;
            this.btnDetain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetain.Location = new System.Drawing.Point(1130, 1044);
            this.btnDetain.Name = "btnDetain";
            this.btnDetain.Size = new System.Drawing.Size(153, 45);
            this.btnDetain.TabIndex = 31;
            this.btnDetain.Text = "Detain";
            this.btnDetain.UseVisualStyleBackColor = true;
            this.btnDetain.Click += new System.EventHandler(this.btnDetain_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(967, 1044);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(153, 46);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlDriverLicenseInfoWithFilter1
            // 
            this.ctrlDriverLicenseInfoWithFilter1.BackColor = System.Drawing.Color.White;
            this.ctrlDriverLicenseInfoWithFilter1.FilterEnabled = true;
            this.ctrlDriverLicenseInfoWithFilter1.FilterText = "";
            this.ctrlDriverLicenseInfoWithFilter1.Location = new System.Drawing.Point(12, 131);
            this.ctrlDriverLicenseInfoWithFilter1.Name = "ctrlDriverLicenseInfoWithFilter1";
            this.ctrlDriverLicenseInfoWithFilter1.Size = new System.Drawing.Size(1265, 639);
            this.ctrlDriverLicenseInfoWithFilter1.TabIndex = 0;
            this.ctrlDriverLicenseInfoWithFilter1.OnLicenseSelected += new System.Action<int>(this.ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected);
            // 
            // gbDetainInfo
            // 
            this.gbDetainInfo.Controls.Add(this.txtFees);
            this.gbDetainInfo.Controls.Add(this.lblDetainId);
            this.gbDetainInfo.Controls.Add(this.lblCreatedBy);
            this.gbDetainInfo.Controls.Add(this.lblLicenseId);
            this.gbDetainInfo.Controls.Add(this.label7);
            this.gbDetainInfo.Controls.Add(this.lblDetainDate);
            this.gbDetainInfo.Controls.Add(this.label5);
            this.gbDetainInfo.Controls.Add(this.label4);
            this.gbDetainInfo.Controls.Add(this.label3);
            this.gbDetainInfo.Controls.Add(this.label2);
            this.gbDetainInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDetainInfo.Location = new System.Drawing.Point(18, 776);
            this.gbDetainInfo.Name = "gbDetainInfo";
            this.gbDetainInfo.Size = new System.Drawing.Size(1265, 247);
            this.gbDetainInfo.TabIndex = 32;
            this.gbDetainInfo.TabStop = false;
            this.gbDetainInfo.Text = "Detain Info";
            // 
            // txtFees
            // 
            this.txtFees.Location = new System.Drawing.Point(169, 198);
            this.txtFees.Name = "txtFees";
            this.txtFees.Size = new System.Drawing.Size(160, 35);
            this.txtFees.TabIndex = 9;
            // 
            // lblDetainId
            // 
            this.lblDetainId.AutoSize = true;
            this.lblDetainId.Location = new System.Drawing.Point(158, 66);
            this.lblDetainId.Name = "lblDetainId";
            this.lblDetainId.Size = new System.Drawing.Size(92, 29);
            this.lblDetainId.TabIndex = 8;
            this.lblDetainId.Text = "label10";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(932, 124);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(79, 29);
            this.lblCreatedBy.TabIndex = 7;
            this.lblCreatedBy.Text = "label9";
            // 
            // lblLicenseId
            // 
            this.lblLicenseId.AutoSize = true;
            this.lblLicenseId.Location = new System.Drawing.Point(926, 66);
            this.lblLicenseId.Name = "lblLicenseId";
            this.lblLicenseId.Size = new System.Drawing.Size(79, 29);
            this.lblLicenseId.TabIndex = 6;
            this.lblLicenseId.Text = "label8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(788, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 29);
            this.label7.TabIndex = 5;
            this.label7.Text = "Created By:";
            // 
            // lblDetainDate
            // 
            this.lblDetainDate.AutoSize = true;
            this.lblDetainDate.Location = new System.Drawing.Point(185, 134);
            this.lblDetainDate.Name = "lblDetainDate";
            this.lblDetainDate.Size = new System.Drawing.Size(79, 29);
            this.lblDetainDate.TabIndex = 4;
            this.lblDetainDate.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(788, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 29);
            this.label5.TabIndex = 3;
            this.label5.Text = "License ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 29);
            this.label4.TabIndex = 2;
            this.label4.Text = "Fine Fees:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "Detain Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Detain ID:";
            // 
            // frmDetainLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1292, 1095);
            this.Controls.Add(this.gbDetainInfo);
            this.Controls.Add(this.btnDetain);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lnkNewLicenseInfo);
            this.Controls.Add(this.lnkLicenseHistory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrlDriverLicenseInfoWithFilter1);
            this.Name = "frmDetainLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detain License";
            this.gbDetainInfo.ResumeLayout(false);
            this.gbDetainInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ctrlDriverLicenseInfoWithFilter ctrlDriverLicenseInfoWithFilter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkNewLicenseInfo;
        private System.Windows.Forms.LinkLabel lnkLicenseHistory;
        private System.Windows.Forms.Button btnDetain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbDetainInfo;
        private System.Windows.Forms.TextBox txtFees;
        private System.Windows.Forms.Label lblDetainId;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblLicenseId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDetainDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}