using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_Quanlyquancafe
{
    public partial class Form_QLBan : Form
    {
        private string connectionString = "Data Source=DESKTOP-GFNS1RA;Initial Catalog=dbQuanLyQuanCaPhe;Integrated Security=True";
        public Form_QLBan()
        {
            InitializeComponent();
        }
        // Hàm LoadRoles để tải danh mục vào ComboBox
        private void LoadDanhMuc()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn SQL để lấy danh mục
                    string query = "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dtDanhMuc = new DataTable();
                    adapter.Fill(dtDanhMuc);

                    // Đổ dữ liệu vào ComboBox 1 (Danh mục)
                    comboBoxdanhmuc.DataSource = dtDanhMuc;
                    comboBoxdanhmuc.DisplayMember = "TenDanhMuc"; // Hiển thị tên danh mục
                    comboBoxdanhmuc.ValueMember = "MaDanhMuc";   // Giá trị là mã danh mục
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
        }

        private void LoadSanPhamTheoDanhMuc(string maDanhMuc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn SQL để lấy sản phẩm theo mã danh mục
                    string query = "SELECT MaSanPham, TenSanPham FROM SanPham WHERE MaDanhMuc = @MaDanhMuc";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dtSanPham = new DataTable();
                    adapter.Fill(dtSanPham);

                    // Đổ dữ liệu vào ComboBox 2 (Sản phẩm)
                    comboBoxsanpham.DataSource = dtSanPham;
                    comboBoxsanpham.DisplayMember = "TenSanPham"; // Hiển thị tên sản phẩm
                    comboBoxsanpham.ValueMember = "MaSanPham";   // Giá trị là mã sản phẩm
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
        }


        private void Form_QLBan_Load_1(object sender, EventArgs e)
        {
            LoadDanhMuc();

            LoadTableStatuses();

            ban1.Click += Button_Click;
            ban2.Click += Button_Click;
            ban3.Click += Button_Click;
            ban4.Click += Button_Click;
            ban5.Click += Button_Click;
            ban6.Click += Button_Click;

        }
        private void button7_Click(object sender, EventArgs e)
        {
            string madanhmuc = comboBoxdanhmuc.SelectedValue?.ToString();
            string masanpham = comboBoxsanpham.SelectedValue?.ToString();
            string sl = soluong.Value.ToString();
            string mahoadon=null;
            

            if (string.IsNullOrEmpty(masanpham))
            {
                MessageBox.Show("Vui lòng chọn món");
                return;
            }

            if (chitietdondataGridView.Rows.Count > 0)
            {
                // Kiểm tra cột "MaHoaDon" có tồn tại và có giá trị
                if (chitietdondataGridView.Rows[0].Cells["MaHoaDon"].Value != null)
                {
                    mahoadon = chitietdondataGridView.Rows[0].Cells["MaHoaDon"].Value.ToString();
                }
                
            }
            else
            {
                mahoadon = LayMaHoaDonMoi(soban.Text);
            }


            // Gọi stored procedure để thêm chi tiết hóa đơn
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AddChiTietHoaDon", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@MaHoaDon", LayMaHoaDonMoi(soban.Text));
                    cmd.Parameters.AddWithValue("@MaSanPham", masanpham);
                    cmd.Parameters.AddWithValue("@SoLuong", sl);

                    // Thực thi stored procedure
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm món thành công!");

                    // Cập nhật giao diện (nếu cần thiết)
                    LoadInvoiceDetails(soban.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            LoadInvoiceDetails(soban.Text);
            TinhTongTien();

        }

        private string LayMaHoaDonMoi(String soban)
        {
            string maHoaDon = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Truy vấn lấy mã hóa đơn mới nhất của bàn với số bàn là 'soBan'
                    string query = @"
                SELECT TOP 1 MaHoaDon 
                FROM HoaDon 
                WHERE MaBan = (SELECT MaBan FROM Ban WHERE TenBan = @TenBan)
                ORDER BY NgayTaoHoaDon DESC"; // Lấy hóa đơn mới nhất theo ngày tạo

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenBan", soban);

                        maHoaDon = cmd.ExecuteScalar()?.ToString();  // Lấy mã hóa đơn

                        if (string.IsNullOrEmpty(maHoaDon))
                        {
                            MessageBox.Show("Không thể lấy mã hóa đơn mới.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy mã hóa đơn: " + ex.Message);
                }
            }

            return maHoaDon;
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxsanpham_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxdanhmuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxdanhmuc.SelectedValue != null)
            {
                string maDanhMuc = this.comboBoxdanhmuc.SelectedValue.ToString(); // Lấy mã danh mục
                LoadSanPham(maDanhMuc); // Gọi hàm để tải sản phẩm thuộc danh mục
            }
        }

        private void LoadSanPham(string maDanhMuc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn SQL để lấy sản phẩm theo mã danh mục
                    string query = "SELECT MaSanPham, TenSanPham FROM SanPham WHERE MaDanhMuc = @MaDanhMuc";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dtSanPham = new DataTable();
                    adapter.Fill(dtSanPham);

                    // Đổ dữ liệu vào ComboBox 2 (Sản phẩm)
                    comboBoxsanpham.DataSource = dtSanPham;
                    comboBoxsanpham.DisplayMember = "TenSanPham"; // Hiển thị tên sản phẩm
                    comboBoxsanpham.ValueMember = "MaSanPham";   // Giá trị là mã sản phẩm
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
        }


            private void ban1_Click(object sender, EventArgs e)
        {

        }
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button; // Lấy button mà người dùng đã nhấn

            if (clickedButton != null)
            {
                soban.Text = clickedButton.Text;
                string selectedBan = clickedButton.Text.ToString();
             

                int status = CheckTableStatus(selectedBan); // Kiểm tra trạng thái bàn "Ban1"

                if (status == 1)
                {
                    MessageBox.Show("Bàn đang sử dụng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (status == 0)
                {
                    clickedButton.BackColor = Color.Red;
                    taoHoaDon(selectedBan);
                }    

                LoadInvoiceDetails(selectedBan);

                TinhTongTien();
                }
        }
        private void LoadInvoiceDetails(string tableName)
        {
            try
            {
                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Câu lệnh SQL để lấy chi tiết hóa đơn của bàn theo tên bàn
                    // Lấy giá từ bảng SanPham
                    string query = @"
                SELECT
                    cthd.MaHoaDon,
                    cthd.MaSanPham,
                    sp.TenSanPham,
                    cthd.SoLuong,
                    sp.Gia,  -- Lấy giá từ bảng SanPham
                    (cthd.SoLuong * sp.Gia) AS ThanhTien  -- Tính tổng tiền cho từng sản phẩm
                FROM 
                    ChiTietHoaDon cthd
                INNER JOIN 
                    HoaDon hd ON cthd.MaHoaDon = hd.MaHoaDon
                INNER JOIN 
                    SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                WHERE 
                    hd.MaHoaDon = (SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaBan = (SELECT MaBan FROM Ban WHERE TenBan = @TenBan) ORDER BY NgayTaoHoaDon DESC)
                    AND hd.NgayXuatHoaDon IS NULL ";  // Lọc hóa đơn chưa xuất

                    // Tạo command để thực thi câu lệnh SQL
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenBan", tableName);

                        // Đọc dữ liệu từ cơ sở dữ liệu
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();

                        // Đổ dữ liệu vào DataTable
                        dataAdapter.Fill(dataTable);

                        // Cập nhật GridView với dữ liệu hóa đơn chi tiết
                        chitietdondataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TinhTongTien()
        {
            try
            {
                int tongTien = 0;

                // Lặp qua các hàng trong DataGridView để tính tổng cột ThanhTien
                foreach (DataGridViewRow row in chitietdondataGridView.Rows)
                {
                    // Kiểm tra nếu cột ThanhTien có giá trị hợp lệ
                    if (row.Cells["ThanhTien"].Value != DBNull.Value)
                    {
                        tongTien += Convert.ToInt32(row.Cells["ThanhTien"].Value);
                    }
                }

                // Hiển thị tổng số tiền lên TextBox
                textBoxtongtien.Text = tongTien.ToString(); // Chỉ hiển thị giá trị tổng tiền dưới dạng số nguyên

                // Kiểm tra và trừ giá trị giảm giá
                int giamGia = 0;
                if (!string.IsNullOrEmpty(textBoxgiamgia.Text))
                {
                    giamGia = Convert.ToInt32(textBoxgiamgia.Text); // Lấy giá trị giảm giá từ TextBox
                }

                // Tính toán lại thành tiền sau khi trừ giảm giá
                int thanhtien = tongTien - giamGia;
                textBoxthanhtien.Text = thanhtien.ToString(); // Hiển thị thành tiền sau khi trừ giảm giá
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tổng tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private int CheckTableStatus(string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Câu lệnh SQL để kiểm tra trạng thái bàn
                    string query = "SELECT TrangThai FROM Ban WHERE TenBan = @TenBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenBan", tableName);

                        SqlDataReader reader = cmd.ExecuteReader(); // Thực thi câu lệnh SQL

                        if (reader.Read()) // Nếu có dữ liệu trả về
                        {
                            string trangThai = reader["TrangThai"].ToString();

                            if (trangThai == "Trống")
                            {
                                return 0; // Bàn trống
                            }
                            else if (trangThai == "Đang sử dụng")
                            {
                                return 1; // Bàn đang sử dụng
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra trạng thái bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }

            return -1; // Trả về -1 nếu có lỗi hoặc không tìm thấy trạng thái
        }

        private void LoadTableStatuses()
        {
            // Duyệt qua tất cả các bàn từ ban1 đến ban6
            List<Button> buttons = new List<Button> { ban1, ban2, ban3, ban4, ban5, ban6 };

            foreach (Button button in buttons)
            {
                string tableName = button.Text.ToString(); // Lấy tên bàn từ button

                // Kiểm tra trạng thái của bàn
                int status = CheckTableStatus(tableName);

                // Cập nhật màu sắc của button dựa trên trạng thái
                if (status == 1)
                {
                    button.BackColor = Color.Red; // Bàn đang sử dụng
                }
                else if (status == 0)
                {
                    button.BackColor = Color.FromArgb(128, 255, 255); // Bàn trống
                }
            }
        }



        private void taoHoaDon(string tenBan)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Tạo hóa đơn mới
                    string insertHoaDonQuery = "INSERT INTO HoaDon (NgayTaoHoaDon, MaBan) VALUES (GETDATE(), (SELECT MaBan FROM Ban WHERE TenBan = @TenBan))";
                    using (SqlCommand insertCmd = new SqlCommand(insertHoaDonQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@TenBan", tenBan);
                        insertCmd.ExecuteNonQuery();
                    }

                    // Cập nhật trạng thái bàn
                    string updateBanQuery = "UPDATE Ban SET TrangThai = N'Đang sử dụng' WHERE TenBan = @TenBan";
                    using (SqlCommand updateCmd = new SqlCommand(updateBanQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@TenBan", tenBan);
                        updateCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Hóa đơn mới đã được tạo cho bàn {tenBan}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
        }

        private void buttonthanhtoan_Click(object sender, EventArgs e)
        {
            string tenban = soban.Text;

            // Câu lệnh SQL cập nhật
            string query = "UPDATE HoaDon SET NgayXuatHoaDon = GETDATE() WHERE MaHoaDon = (SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaBan = (SELECT MaBan FROM Ban WHERE TenBan = @TenBan) ORDER BY NgayTaoHoaDon DESC)" +
                "UPDATE Ban SET TrangThai = N'Trống' WHERE TenBan = @TenBan";
            // Kết nối và thực thi câu lệnh
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Thêm tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@TenBan", tenban);

                        // Thực thi câu lệnh
                        int rowsAffected = command.ExecuteNonQuery();                     

                        // Thông báo kết quả
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã thanh toán thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn để cập nhật.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            LoadTableStatuses();
            LoadInvoiceDetails(tenban);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
