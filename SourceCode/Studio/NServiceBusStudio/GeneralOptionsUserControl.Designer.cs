namespace NServiceBusStudio
{
    partial class GeneralOptionsUserControl
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
            this.grpNamingConventions = new System.Windows.Forms.GroupBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtContracts = new System.Windows.Forms.TextBox();
            this.txtInternalMessages = new System.Windows.Forms.TextBox();
            this.lblContracts = new System.Windows.Forms.Label();
            this.lblInternalMessages = new System.Windows.Forms.Label();
            this.lblNamingConventionsDescription = new System.Windows.Forms.Label();
            this.grpNamingConventions.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpNamingConventions
            // 
            this.grpNamingConventions.Controls.Add(this.lblCode);
            this.grpNamingConventions.Controls.Add(this.txtCode);
            this.grpNamingConventions.Controls.Add(this.txtContracts);
            this.grpNamingConventions.Controls.Add(this.txtInternalMessages);
            this.grpNamingConventions.Controls.Add(this.lblContracts);
            this.grpNamingConventions.Controls.Add(this.lblInternalMessages);
            this.grpNamingConventions.Controls.Add(this.lblNamingConventionsDescription);
            this.grpNamingConventions.Location = new System.Drawing.Point(3, 3);
            this.grpNamingConventions.Name = "grpNamingConventions";
            this.grpNamingConventions.Size = new System.Drawing.Size(391, 128);
            this.grpNamingConventions.TabIndex = 0;
            this.grpNamingConventions.TabStop = false;
            this.grpNamingConventions.Text = "Naming Conventions";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(6, 104);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(146, 13);
            this.lblCode.TabIndex = 6;
            this.lblCode.Text = "Project Namespace for Code:";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(219, 101);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(166, 20);
            this.txtCode.TabIndex = 5;
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProjectName_KeyPress);
            this.txtCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyUp);
            // 
            // txtContracts
            // 
            this.txtContracts.Location = new System.Drawing.Point(219, 75);
            this.txtContracts.Name = "txtContracts";
            this.txtContracts.Size = new System.Drawing.Size(166, 20);
            this.txtContracts.TabIndex = 4;
            this.txtContracts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProjectName_KeyPress);
            this.txtContracts.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtContracts_KeyUp);
            // 
            // txtInternalMessages
            // 
            this.txtInternalMessages.Location = new System.Drawing.Point(219, 49);
            this.txtInternalMessages.Name = "txtInternalMessages";
            this.txtInternalMessages.Size = new System.Drawing.Size(166, 20);
            this.txtInternalMessages.TabIndex = 3;
            this.txtInternalMessages.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProjectName_KeyPress);
            this.txtInternalMessages.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtInternalMessages_KeyUp);
            // 
            // lblContracts
            // 
            this.lblContracts.AutoSize = true;
            this.lblContracts.Location = new System.Drawing.Point(6, 78);
            this.lblContracts.Name = "lblContracts";
            this.lblContracts.Size = new System.Drawing.Size(166, 13);
            this.lblContracts.TabIndex = 2;
            this.lblContracts.Text = "Project Namespace for Contracts:";
            // 
            // lblInternalMessages
            // 
            this.lblInternalMessages.AutoSize = true;
            this.lblInternalMessages.Location = new System.Drawing.Point(6, 52);
            this.lblInternalMessages.Name = "lblInternalMessages";
            this.lblInternalMessages.Size = new System.Drawing.Size(207, 13);
            this.lblInternalMessages.TabIndex = 1;
            this.lblInternalMessages.Text = "Project Namespace for Internal Messages:";
            // 
            // lblNamingConventionsDescription
            // 
            this.lblNamingConventionsDescription.Location = new System.Drawing.Point(6, 16);
            this.lblNamingConventionsDescription.Name = "lblNamingConventionsDescription";
            this.lblNamingConventionsDescription.Size = new System.Drawing.Size(379, 26);
            this.lblNamingConventionsDescription.TabIndex = 0;
            this.lblNamingConventionsDescription.Text = "Define the project namespaces you wish to use in ServiceMatrix to create new NSer" +
    "viceBus System solutions.";
            // 
            // GeneralOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpNamingConventions);
            this.Name = "GeneralOptionsUserControl";
            this.Size = new System.Drawing.Size(398, 251);
            this.grpNamingConventions.ResumeLayout(false);
            this.grpNamingConventions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpNamingConventions;
        private System.Windows.Forms.Label lblNamingConventionsDescription;
        private System.Windows.Forms.Label lblInternalMessages;
        private System.Windows.Forms.Label lblContracts;
        private System.Windows.Forms.TextBox txtContracts;
        private System.Windows.Forms.TextBox txtInternalMessages;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
    }
}
