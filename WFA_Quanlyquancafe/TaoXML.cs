using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace WFA_Quanlyquancafe
{
    class TaoXML
    {
        string strCon = "Data Source=DESKTOP-GFNS1RA;Initial Catalog=dbQuanLyQuanCaPhe;Integrated Security=True";

        // Hàm tạo XML từ cơ sở dữ liệu
        public void taoXML(string sql, string bang, string _FileXML)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    SqlDataAdapter ad = new SqlDataAdapter(sql, con);
                    DataTable dt = new DataTable(bang);
                    ad.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string filePath = Path.Combine(Application.StartupPath, _FileXML);
                        dt.WriteXml(filePath, XmlWriteMode.WriteSchema);
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để tạo file XML.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong taoXML: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }


        // Hàm load dữ liệu từ file XML
        public DataTable loadDataGridView(string _FileXML)
        {
            DataTable dt = new DataTable();
            try
            {
                string filePath = Path.Combine(Application.StartupPath, _FileXML);

                // Kiểm tra nếu file tồn tại
                if (File.Exists(filePath))
                {
                    dt.ReadXml(filePath); // Đọc dữ liệu từ file XML vào DataTable
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file XML: {ex.Message}");
            }

            return dt;
        }


        // Hàm thêm một bản ghi vào file XML
        public void Them(string FileXML, string xml)
        {
            try
            {
                XmlTextReader textread = new XmlTextReader(FileXML);
                XmlDocument doc = new XmlDocument();
                doc.Load(textread);
                textread.Close();
                XmlNode currNode;
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = xml;
                currNode = doc.DocumentElement;
                currNode.InsertAfter(docFrag, currNode.LastChild);
                doc.Save(FileXML);
            }
            catch
            {
                MessageBox.Show("Lỗi khi thêm vào XML");
            }

        }

        // Hàm xóa bản ghi từ file XML
        public void xoa(string _FileXML, string xml)
        {
            try
            {
                string fileName = _FileXML;
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode nodeCu = doc.SelectSingleNode(xml);
                doc.DocumentElement.RemoveChild(nodeCu);
                doc.Save(fileName);
            }
            catch
            {
                MessageBox.Show("Lỗi khi xóa trong XML");
            }
        }

        // Hàm xóa bản ghi trong cơ sở dữ liệu từ XML
        public void Xoa_Database(string _FileXML, string tenCot, string giaTri, string tenBang)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tenCot].ToString().Trim() == giaTri)
                { dong = i; }
            }
            if (dong > -1)
            {
                string sql = "delete from " + tenBang + " where " + tenCot + "= '" + giaTri + "'";
                exCuteNonQuery(sql);
            }
        }


        // Hàm sửa một bản ghi trong file XML
        public void sua(string FileXML, string sql, string xml, string bang)
        {
            XmlTextReader reader = new XmlTextReader(FileXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            XmlNode oldValue;
            XmlElement root = doc.DocumentElement;
            oldValue = root.SelectSingleNode(sql);
            XmlElement newValue = doc.CreateElement(bang);
            newValue.InnerXml = xml;
            root.ReplaceChild(newValue, oldValue);
            doc.Save(FileXML);
        }

        // Hàm tìm kiếm trong XML và hiển thị kết quả lên DataGridView
        public void TimKiem(string _FileXML, string xml, DataGridView dgv)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.StartupPath + _FileXML);
            string xPath = xml;
            XmlNode node = xDoc.SelectSingleNode(xPath);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            XmlNodeReader nr = new XmlNodeReader(node);
            ds.ReadXml(nr);
            dgv.DataSource = ds.Tables[0];
            nr.Close();
        }

        // Hàm lấy giá trị từ XML
        public string LayGiaTri(string duongDan, string truongA, string giaTriA, string truongB)
        {
            string giatriB = "";
            DataTable dt = loadDataGridView(duongDan);
            int soDongNhanVien = dt.Rows.Count;
            for (int i = 0; i < soDongNhanVien; i++)
            {
                if (dt.Rows[i][truongA].ToString().Trim().Equals(giaTriA))
                {
                    giatriB = dt.Rows[i][truongB].ToString();
                    return giatriB;
                }
            }
            return giatriB;
        }

        // Kiểm tra sự tồn tại của giá trị trong XML
        public bool KiemTra(string _FileXML, string truongKiemTra, string giaTriKiemTra)
        {
            DataTable dt = loadDataGridView(_FileXML);
            dt.DefaultView.RowFilter = truongKiemTra + " ='" + giaTriKiemTra + "'";
            if (dt.DefaultView.Count > 0)
                return true;
            return false;
        }

        // Hàm tạo mã tự động cho các đối tượng
        public string txtMa(string tienTo, string _FileXML, string tenCot)
        {
            string txtMa = "";
            DataTable dt = loadDataGridView(_FileXML);
            int dem = dt.Rows.Count;
            if (dem == 0)
            {
                txtMa = tienTo + "001";
            }
            else
            {
                int duoi = int.Parse(dt.Rows[dem - 1][tenCot].ToString().Substring(2, 3)) + 1;
                string cuoi = "00" + duoi;
                txtMa = tienTo + "" + cuoi.Substring(cuoi.Length - 3, 3);
            }
            return txtMa;
        }

        // Kiểm tra mã có tồn tại trong XML không
        public bool KTMa(string _FileXML, string cotMa, string ma)
        {
            bool kt = true;
            DataTable dt = loadDataGridView(_FileXML);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][cotMa].ToString().Trim().Equals(ma))
                {
                    kt = false;
                }
            }
            return kt;
        }

        // Hàm thực thi câu lệnh SQL không trả về kết quả (Insert, Update, Delete)
        public void exCuteNonQuery(string sql)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.ExecuteNonQuery();
        }

        // Hàm thêm bản ghi vào cơ sở dữ liệu từ XML
        public void Them_Database(string tenBang, string _FileXML)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = table.Rows.Count - 1;
            string sql = "insert into " + tenBang + " values(";
            for (int j = 0; j < table.Columns.Count - 1; j++)
            {
                sql += "N'" + table.Rows[dong][j].ToString().Trim() + "',";
            }
            sql += "N'" + table.Rows[dong][table.Columns.Count - 1].ToString().Trim() + "'";
            sql += ")";
            exCuteNonQuery(sql);
        }

        // Hàm sửa bản ghi trong cơ sở dữ liệu từ XML
        public void Sua_Database(string tenBang, string _FileXML, string tenCot, string giaTri)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tenCot].ToString().Trim() == giaTri)
                { dong = i; }
            }
            if (dong > -1)
            {
                string sql = "update " + tenBang + " set ";
                for (int j = 0; j < table.Columns.Count - 1; j++)
                {
                    sql += table.Columns[j].ToString() + " = N'" + table.Rows[dong][j].ToString().Trim() + "', ";
                }
                sql += table.Columns[table.Columns.Count - 1].ToString() + " = N'" + table.Rows[dong][table.Columns.Count - 1].ToString().Trim() + "' ";
                sql += "where " + tenCot + "= '" + giaTri + "'";
                exCuteNonQuery(sql);
            }
        }

    }
}