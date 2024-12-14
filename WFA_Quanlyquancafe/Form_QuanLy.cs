using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_Quanlyquancafe
{
    public partial class Form_QuanLy : Form
    {
        public Form_QuanLy()
        {
            InitializeComponent();
        }

        private void Form_QuanLy_Load(object sender, EventArgs e)
        {

        }

        private void Hienthiformcon(Form childForm)
        {
            // Đảm bảo đóng form cũ trước khi mở form mới
            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Form form)
                {
                    form.Close();
                }
            }
            // Đặt form con vào panel
            childForm.TopLevel = false;  // Đảm bảo không phải là một form độc lập
            childForm.FormBorderStyle = FormBorderStyle.None; // Ẩn viền của form
            childForm.Dock = DockStyle.Fill; // Để form chiếm hết diện tích panel
            pnlContent.Controls.Add(childForm); // Thêm form vào panel
            childForm.Show(); // Hiển thị form con
        }

        private void quảnLýDanhMụcSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_QLBan form = new Form_QLBan();
            Hienthiformcon(form);
        }

        private void quảnLýBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_QLSanpham form = new Form_QLSanpham();
            Hienthiformcon(form);
        }
    }
}
