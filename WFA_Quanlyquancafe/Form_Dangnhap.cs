using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Data.SqlClient;
namespace WFA_Quanlyquancafe
{
    public partial class Form_Dangnhap : Form
    {
        // Chuỗi kết nối tới cơ sở dữ liệu SQL Server
        private string connectionString = "Data Source=DESKTOP-GFNS1RA;Initial Catalog=dbQuanLyQuanCaPhe;Integrated Security=True";

        public Form_Dangnhap()
        {
            InitializeComponent();
        }
        private void LoadRoles()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Truy vấn lấy danh sách vai trò
                    string query = "SELECT TenVaiTro FROM VaiTro";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        cobvaitro.Items.Clear(); // Xóa các mục cũ trong ComboBox
                        while (reader.Read())
                        {
                            cobvaitro.Items.Add(reader["TenVaiTro"].ToString());
                        }
                    }
                }

                // Chọn vai trò đầu tiên nếu danh sách không rỗng
                if (cobvaitro.Items.Count > 0)
                {
                    cobvaitro.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btn_login_Click(object sender, EventArgs e)
        {
            string username = txttendangnhap.Text;
            string password = txtmatkhau.Text;

            if (cobvaitro.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedRole = cobvaitro.SelectedItem.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn để kiểm tra thông tin đăng nhập
                    string query = @"
                        SELECT tk.MaTaiKhoan, tk.TenDangNhap, tk.MatKhau, vt.TenVaiTro 
                        FROM TaiKhoan tk
                        INNER JOIN VaiTro vt ON tk.MaVaiTro = vt.MaVaiTro
                        WHERE tk.TenDangNhap = @Username AND vt.TenVaiTro = @Role";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Role", selectedRole);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Kiểm tra mật khẩu
                            if (reader["MatKhau"].ToString() != password)
                            {
                                MessageBox.Show("Mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Đăng nhập thành công
                            MessageBox.Show($"Đăng nhập thành công với vai trò: {selectedRole}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            Form_QuanLy formQuanLy = new Form_QuanLy();
                            formQuanLy.Show();  // Hiển thị form quản lý
                            this.Hide();
                           
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc vai trò không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Khi form được load, gọi LoadRoles để nạp các vai trò
        private void Form_Dangnhap_Load(object sender, EventArgs e)
        {
            
        }

        private void Form_Dangnhap_Load_1(object sender, EventArgs e)
        {
            LoadRoles();
        }
    }
}
