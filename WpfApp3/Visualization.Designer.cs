
namespace GenealogyTree
{
    partial class Visualization
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
            this.treePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // treePanel
            // 
            this.treePanel.AutoScroll = true;
            this.treePanel.Location = new System.Drawing.Point(0, 0);
            this.treePanel.Name = "treePanel";
            this.treePanel.Size = new System.Drawing.Size(1370, 700);
            this.treePanel.TabIndex = 0;
            // 
            // Visualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10000, 5000);
            this.AutoScrollMinSize = new System.Drawing.Size(1000, 500);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1284, 661);
            this.Controls.Add(this.treePanel);
            this.Name = "Visualization";
            this.Text = "Visualization";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel treePanel;
    }
}