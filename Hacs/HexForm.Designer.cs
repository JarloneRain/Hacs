﻿namespace Hacs {
    partial class HexForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modifyBtn
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            SuspendLayout();
            // 
            // HexForm
            // 
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(Length, Length);
            Name = "HexForm";
            Text = "HexForm";
            Load += HexForm_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}