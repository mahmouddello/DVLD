namespace DVLD.PresentationLayer.Users
{
    partial class frmUserDetails
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
            this.ctrlPersonCard1 = new DVLD.PresentationLayer.People.ctrlPersonCard();
            this.ctrlUserLoginInfo1 = new DVLD.PresentationLayer.Users.ctrlUserLoginInfo();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1115, 52);
            this.label1.TabIndex = 2;
            this.label1.Text = "User Details";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ctrlPersonCard1
            // 
            this.ctrlPersonCard1.Location = new System.Drawing.Point(22, 100);
            this.ctrlPersonCard1.Name = "ctrlPersonCard1";
            this.ctrlPersonCard1.Size = new System.Drawing.Size(1124, 382);
            this.ctrlPersonCard1.TabIndex = 3;
            // 
            // ctrlUserLoginInfo1
            // 
            this.ctrlUserLoginInfo1.Location = new System.Drawing.Point(22, 488);
            this.ctrlUserLoginInfo1.Name = "ctrlUserLoginInfo1";
            this.ctrlUserLoginInfo1.Size = new System.Drawing.Size(1129, 143);
            this.ctrlUserLoginInfo1.TabIndex = 4;
            // 
            // frmUserDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 637);
            this.Controls.Add(this.ctrlUserLoginInfo1);
            this.Controls.Add(this.ctrlPersonCard1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUserDetails";
            this.Text = "frmUserDetails";
            this.Load += new System.EventHandler(this.frmUserDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private People.ctrlPersonCard ctrlPersonCard1;
        private ctrlUserLoginInfo ctrlUserLoginInfo1;
    }
}