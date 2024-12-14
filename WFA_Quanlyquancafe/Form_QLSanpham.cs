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
using System.Xml;
using System.Xml.Xsl;

namespace WFA_Quanlyquancafe
{
    public partial class Form_QLSanpham : Form
    {
        private string connectionString = "Data Source=DESKTOP-GFNS1RA;Initial Catalog=dbQuanLyQuanCaPhe;Integrated Security=True";

        public Form_QLSanpham()
        {
            InitializeComponent();
        }

      private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                // Kết nối cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lệnh SQL để lấy dữ liệu bảng SanPham
                    string query = "SELECT * FROM SanPham";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Tạo DataTable và điền dữ liệu
                        DataTable table = new DataTable("SanPham");
                        adapter.Fill(table);

                        // Chuyển DataTable thành XML
                        using (StringWriter stringWriter = new StringWriter())
                        {
                            table.WriteXml(stringWriter, XmlWriteMode.WriteSchema);

                            // Lưu XML vào file
                            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SanPham.xml");
                            File.WriteAllText(filePath, stringWriter.ToString());

                            // Thông báo hoàn thành
                            MessageBox.Show($"File XML đã được lưu tại: {filePath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Đường dẫn đến tệp XML đã tạo từ Button1
                string xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SanPham.xml");

                // Kiểm tra xem tệp XML có tồn tại không
                if (File.Exists(xmlFilePath))
                {
                    // Đường dẫn đến tệp HTML để lưu
                    string htmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SanPham.html");

                    // Tạo HTML từ XML
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xmlFilePath);

                    StringBuilder htmlBuilder = new StringBuilder();

                    htmlBuilder.AppendLine("<html>");
                    htmlBuilder.AppendLine("<head><title>Danh sách sản phẩm</title></head>");
                    htmlBuilder.AppendLine("<body>");
                    htmlBuilder.AppendLine("<h1>Danh sách sản phẩm</h1>");
                    htmlBuilder.AppendLine("<table border='1' cellspacing='0' cellpadding='5'>");

                    // Thêm tiêu đề bảng
                    htmlBuilder.AppendLine("<tr>");
                    foreach (DataColumn column in dataSet.Tables[0].Columns)
                    {
                        htmlBuilder.AppendLine($"<th>{column.ColumnName}</th>");
                    }
                    htmlBuilder.AppendLine("</tr>");

                    // Thêm dữ liệu
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        htmlBuilder.AppendLine("<tr>");
                        foreach (var item in row.ItemArray)
                        {
                            htmlBuilder.AppendLine($"<td>{item}</td>");
                        }
                        htmlBuilder.AppendLine("</tr>");
                    }

                    htmlBuilder.AppendLine("</table>");
                    htmlBuilder.AppendLine("</body>");
                    htmlBuilder.AppendLine("</html>");

                    // Lưu file HTML
                    File.WriteAllText(htmlFilePath, htmlBuilder.ToString());

                    // Mở file HTML trong trình duyệt mặc định
                    System.Diagnostics.Process.Start(htmlFilePath);
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM HoaDon"; // Lấy toàn bộ hóa đơn
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable table = new DataTable("HoaDon");
                    adapter.Fill(table);

                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HoaDon.xml");
                    table.WriteXml(filePath, XmlWriteMode.WriteSchema);

                    MessageBox.Show($"Tệp XML đã được lưu tại: {filePath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo tệp XML từ cơ sở dữ liệu
                TaoXML taoXML = new TaoXML();
                string sql = "SELECT * FROM HoaDon"; // Câu truy vấn lấy toàn bộ hóa đơn
                string fileXML = "HoaDon.xml"; // Tên file XML
                taoXML.taoXML(sql, "HoaDon", fileXML);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // Đường dẫn đến các tệp XML, XSLT và HTML
                string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HoaDon.xml");
                string xsltPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HoaDon.xslt");
                string htmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HoaDon.html");

                // Kiểm tra xem các tệp XML và XSLT có tồn tại không
                if (!File.Exists(xmlPath))
                {
                    MessageBox.Show($"Tệp XML không tồn tại tại: {xmlPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(xsltPath))
                {
                    MessageBox.Show($"Tệp XSLT không tồn tại tại: {xsltPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Khởi tạo đối tượng XSLT để thực hiện chuyển đổi
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltPath);  // Tải tệp XSLT

                // Thực hiện chuyển đổi XML sang HTML
                using (StreamWriter writer = new StreamWriter(htmlPath))
                {
                    xslt.Transform(xmlPath, null, writer);  // Không cần truyền tham số thêm nếu không có
                }

                // Mở tệp HTML trong trình duyệt mặc định
                System.Diagnostics.Process.Start(htmlPath);
            }
            catch (Exception ex)
            {
                // Xử lý và thông báo lỗi
                MessageBox.Show($"Lỗi: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Lấy số bàn từ TextBox
            string soBan = textBox1.Text.Trim();

            // Kiểm tra nếu TextBox trống
            if (string.IsNullOrEmpty(soBan))
            {
                MessageBox.Show("Vui lòng nhập số bàn cần tìm.");
                return;
            }

            // Tìm kiếm trong XML
            TimKiemHoaDon(soBan);
        }

        private void TimKiemHoaDon(string soBan)
        {
            try
            {
                // Đường dẫn tới file XML
                string fileXML = "HoaDon.xml";  // Sửa lại đúng tên file XML của bạn

                // Tạo một đối tượng XmlDocument
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path.Combine(Application.StartupPath, fileXML));

                // Dùng XPath để tìm các hóa đơn có số bàn trùng với giá trị nhập vào
                XmlNodeList nodes = xmlDoc.SelectNodes($"/NewDataSet/HoaDon[MaBan='{soBan}']");

                // Kiểm tra nếu có kết quả tìm thấy
                if (nodes.Count > 0)
                {
                    // Sử dụng StringBuilder để tạo nội dung HTML
                    StringBuilder htmlContent = new StringBuilder();

                    // Bắt đầu HTML
                    htmlContent.Append("<html><body>");
                    htmlContent.Append("<h1>Danh sách hóa đơn cho bàn " + soBan + "</h1>");
                    htmlContent.Append("<table border='1'><tr><th>Mã Hóa Đơn</th><th>Bàn</th><th>Ngày Tạo Hóa Đơn</th><th>Ngày Xuất Hóa Đơn</th></tr>");

                    // Duyệt qua các hóa đơn tìm được và thêm vào nội dung HTML
                    foreach (XmlNode node in nodes)
                    {
                        htmlContent.Append("<tr>");
                        htmlContent.Append("<td>" + node.SelectSingleNode("MaHoaDon")?.InnerText + "</td>");
                        htmlContent.Append("<td>" + node.SelectSingleNode("MaBan")?.InnerText + "</td>");
                        htmlContent.Append("<td>" + node.SelectSingleNode("NgayTaoHoaDon")?.InnerText + "</td>");
                        htmlContent.Append("<td>" + node.SelectSingleNode("NgayXuatHoaDon")?.InnerText + "</td>");
                        htmlContent.Append("</tr>");
                    }

                    // Kết thúc bảng và HTML
                    htmlContent.Append("</table>");
                    htmlContent.Append("</body></html>");

                    // Ghi nội dung HTML ra một file tạm thời
                    string tempHtmlFile = Path.Combine(Application.StartupPath, "temp_result.html");
                    File.WriteAllText(tempHtmlFile, htmlContent.ToString());

                    // Mở file HTML bằng trình duyệt mặc định
                    System.Diagnostics.Process.Start(tempHtmlFile);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn cho bàn này.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm hóa đơn: {ex.Message}");
            }
        }

    }
}