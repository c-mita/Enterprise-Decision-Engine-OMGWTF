﻿namespace Enterprise_Decision_Engine
{
    partial class EDEForm
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
            if (disposing && (components != null)) {
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
            this.lblRNG = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRNG
            // 
            this.lblRNG.AutoSize = true;
            this.lblRNG.Location = new System.Drawing.Point(43, 38);
            this.lblRNG.Name = "lblRNG";
            this.lblRNG.Size = new System.Drawing.Size(35, 13);
            this.lblRNG.TabIndex = 0;
            this.lblRNG.Text = "NULL";
            // 
            // EDEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblRNG);
            this.Name = "EDEForm";
            this.Text = "Enterprise Decision Engine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRNG;
    }
}
