namespace WorkflowCoreUI
{
    partial class FEventSample
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
            this.tbMyText = new System.Windows.Forms.TextBox();
            this.bnSubmit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWorkflowId = new System.Windows.Forms.TextBox();
            this.bnGetWorkflowId = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type your text here:";
            // 
            // tbMyText
            // 
            this.tbMyText.Enabled = false;
            this.tbMyText.Location = new System.Drawing.Point(22, 90);
            this.tbMyText.Multiline = true;
            this.tbMyText.Name = "tbMyText";
            this.tbMyText.Size = new System.Drawing.Size(498, 79);
            this.tbMyText.TabIndex = 1;
            // 
            // bnSubmit
            // 
            this.bnSubmit.Location = new System.Drawing.Point(387, 182);
            this.bnSubmit.Name = "bnSubmit";
            this.bnSubmit.Size = new System.Drawing.Size(133, 29);
            this.bnSubmit.TabIndex = 2;
            this.bnSubmit.Text = "Send Reply";
            this.bnSubmit.UseVisualStyleBackColor = true;
            this.bnSubmit.Click += new System.EventHandler(this.bnSubmit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Workflow ID:";
            // 
            // tbWorkflowId
            // 
            this.tbWorkflowId.Location = new System.Drawing.Point(123, 9);
            this.tbWorkflowId.Name = "tbWorkflowId";
            this.tbWorkflowId.Size = new System.Drawing.Size(397, 27);
            this.tbWorkflowId.TabIndex = 4;
            // 
            // bnGetWorkflowId
            // 
            this.bnGetWorkflowId.Location = new System.Drawing.Point(355, 48);
            this.bnGetWorkflowId.Name = "bnGetWorkflowId";
            this.bnGetWorkflowId.Size = new System.Drawing.Size(164, 29);
            this.bnGetWorkflowId.TabIndex = 5;
            this.bnGetWorkflowId.Text = "Get WorkflowId";
            this.bnGetWorkflowId.UseVisualStyleBackColor = true;
            this.bnGetWorkflowId.Click += new System.EventHandler(this.bnGetWorkflowId_Click);
            // 
            // FEventSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 224);
            this.Controls.Add(this.bnGetWorkflowId);
            this.Controls.Add(this.tbWorkflowId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bnSubmit);
            this.Controls.Add(this.tbMyText);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEventSample";
            this.Text = "FEventSample";
            this.Load += new System.EventHandler(this.FEventSample_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMyText;
        private System.Windows.Forms.Button bnSubmit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWorkflowId;
        private System.Windows.Forms.Button bnGetWorkflowId;
    }
}