
namespace WFA_Quanlyquancafe
{
    partial class Form_QuanLy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_QuanLy));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.quảnLýLịchLàmViệcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýBànToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xemThốngKêVàBáoCáoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip1.BackgroundImage")));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quảnLýLịchLàmViệcToolStripMenuItem,
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem,
            this.quảnLýBànToolStripMenuItem,
            this.xemThốngKêVàBáoCáoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1121, 64);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // quảnLýLịchLàmViệcToolStripMenuItem
            // 
            this.quảnLýLịchLàmViệcToolStripMenuItem.Name = "quảnLýLịchLàmViệcToolStripMenuItem";
            this.quảnLýLịchLàmViệcToolStripMenuItem.Size = new System.Drawing.Size(183, 60);
            this.quảnLýLịchLàmViệcToolStripMenuItem.Text = "Quản lý lịch làm việc";
            // 
            // quảnLýDanhMụcSảnPhẩmToolStripMenuItem
            // 
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem.AutoSize = false;
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem.Name = "quảnLýDanhMụcSảnPhẩmToolStripMenuItem";
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem.Size = new System.Drawing.Size(200, 60);
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem.Text = "Quản lý bàn";
            this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem.Click += new System.EventHandler(this.quảnLýDanhMụcSảnPhẩmToolStripMenuItem_Click);
            // 
            // quảnLýBànToolStripMenuItem
            // 
            this.quảnLýBànToolStripMenuItem.Name = "quảnLýBànToolStripMenuItem";
            this.quảnLýBànToolStripMenuItem.Size = new System.Drawing.Size(228, 60);
            this.quảnLýBànToolStripMenuItem.Text = "Quản danh mục sản phẩm";
            this.quảnLýBànToolStripMenuItem.Click += new System.EventHandler(this.quảnLýBànToolStripMenuItem_Click);
            // 
            // xemThốngKêVàBáoCáoToolStripMenuItem
            // 
            this.xemThốngKêVàBáoCáoToolStripMenuItem.Name = "xemThốngKêVàBáoCáoToolStripMenuItem";
            this.xemThốngKêVàBáoCáoToolStripMenuItem.Size = new System.Drawing.Size(222, 60);
            this.xemThốngKêVàBáoCáoToolStripMenuItem.Text = "Xem thống kê và báo cáo";
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 64);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1121, 490);
            this.pnlContent.TabIndex = 1;
            // 
            // Form_QuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1121, 554);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_QuanLy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_QuanLy";
            this.Load += new System.EventHandler(this.Form_QuanLy_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem quảnLýLịchLàmViệcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýDanhMụcSảnPhẩmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýBànToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xemThốngKêVàBáoCáoToolStripMenuItem;
        private System.Windows.Forms.Panel pnlContent;
    }
}