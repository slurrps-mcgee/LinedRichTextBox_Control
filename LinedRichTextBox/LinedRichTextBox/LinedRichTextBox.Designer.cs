namespace LinedRichTextBox
{
    partial class LinedRichTextBox
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
            this.rtbLinedBox = new System.Windows.Forms.RichTextBox();
            this.rtbLineNums = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbLinedBox
            // 
            this.rtbLinedBox.AcceptsTab = true;
            this.rtbLinedBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rtbLinedBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLinedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLinedBox.EnableAutoDragDrop = true;
            this.rtbLinedBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLinedBox.Location = new System.Drawing.Point(64, 0);
            this.rtbLinedBox.Name = "rtbLinedBox";
            this.rtbLinedBox.Size = new System.Drawing.Size(611, 367);
            this.rtbLinedBox.TabIndex = 0;
            this.rtbLinedBox.TabStop = false;
            this.rtbLinedBox.Text = "";
            this.rtbLinedBox.WordWrap = false;
            this.rtbLinedBox.VScroll += new System.EventHandler(this.RtbLinedBox_VScroll);
            this.rtbLinedBox.TextChanged += new System.EventHandler(this.RtbLinedBox_TextChanged);
            this.rtbLinedBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtbLinedBox_KeyDown);
            this.rtbLinedBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RtbLinedBox_KeyPress);
            // 
            // rtbLineNums
            // 
            this.rtbLineNums.BackColor = System.Drawing.Color.Gray;
            this.rtbLineNums.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLineNums.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtbLineNums.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLineNums.Location = new System.Drawing.Point(0, 0);
            this.rtbLineNums.Name = "rtbLineNums";
            this.rtbLineNums.ReadOnly = true;
            this.rtbLineNums.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbLineNums.Size = new System.Drawing.Size(64, 367);
            this.rtbLineNums.TabIndex = 1;
            this.rtbLineNums.Text = "1";
            this.rtbLineNums.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RtbLineNums_MouseClick);
            this.rtbLineNums.MouseEnter += new System.EventHandler(this.RtbLineNums_MouseEnter);
            // 
            // LinedRichTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbLinedBox);
            this.Controls.Add(this.rtbLineNums);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LinedRichTextBox";
            this.Size = new System.Drawing.Size(675, 367);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtbLineNums;
        public System.Windows.Forms.RichTextBox rtbLinedBox;
    }
}
