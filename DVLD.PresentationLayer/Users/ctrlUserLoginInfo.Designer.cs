namespace DVLD.PresentationLayer.Users
{
    partial class ctrlUserLoginInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbUserLoginInfo = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblIsActive = new System.Windows.Forms.Label();
            this.gbUserLoginInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUserLoginInfo
            // 
            this.gbUserLoginInfo.Controls.Add(this.lblIsActive);
            this.gbUserLoginInfo.Controls.Add(this.lblUsername);
            this.gbUserLoginInfo.Controls.Add(this.lblUserID);
            this.gbUserLoginInfo.Controls.Add(this.label3);
            this.gbUserLoginInfo.Controls.Add(this.label1);
            this.gbUserLoginInfo.Controls.Add(this.label2);
            this.gbUserLoginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUserLoginInfo.Location = new System.Drawing.Point(3, 3);
            this.gbUserLoginInfo.Name = "gbUserLoginInfo";
            this.gbUserLoginInfo.Size = new System.Drawing.Size(1123, 138);
            this.gbUserLoginInfo.TabIndex = 0;
            this.gbUserLoginInfo.TabStop = false;
            this.gbUserLoginInfo.Text = "Login Information";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(100, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "User ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(449, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(824, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Is Active:";
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(197, 71);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(64, 25);
            this.lblUserID.TabIndex = 5;
            this.lblUserID.Text = "label4";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(572, 71);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(64, 25);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "label5";
            // 
            // lblIsActive
            // 
            this.lblIsActive.AutoSize = true;
            this.lblIsActive.Location = new System.Drawing.Point(932, 71);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(64, 25);
            this.lblIsActive.TabIndex = 7;
            this.lblIsActive.Text = "label6";
            // 
            // ctrlUserLoginInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbUserLoginInfo);
            this.Name = "ctrlUserLoginInfo";
            this.Size = new System.Drawing.Size(1129, 143);
            this.gbUserLoginInfo.ResumeLayout(false);
            this.gbUserLoginInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUserLoginInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIsActive;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUserID;
    }
}
