<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:html="http://www.w3.org/1999/xhtml"
                version="1.0">
	<xsl:output method="html" encoding="utf-8" indent="yes" />

	<!-- Biến đầu vào tìm kiếm -->
	<xsl:param name="searchName" />

	<html:html>
		<html:head>
			<html:title>Danh sách hóa đơn</html:title>
			<html:style>
				table {
				width: 100%;
				border-collapse: collapse;
				}
				table, th, td {
				border: 1px solid black;
				}
				th {
				background-color: #f2f2f2;
				}
			</html:style>
		</html:head>
		<html:body>
			<html:h1>Danh sách hóa đơn</html:h1>
			<html:form method="get">
				<html:label for="search">Tìm kiếm theo tên bàn:</html:label>
				<html:input type="text" id="search" name="search" value="{$searchName}" />
				<html:button type="submit">Tìm kiếm</html:button>
			</html:form>
			<html:table>
				<html:tr>
					<html:th>Mã hóa đơn</html:th>
					<html:th>Ngày tạo</html:th>
					<html:th>Ngày xuất</html:th>
					<html:th>Phương thức thanh toán</html:th>
					<html:th>Tên bàn</html:th>
					<html:th>Mã nhân viên</html:th>
				</html:tr>
				<!-- Lặp qua các hóa đơn, áp dụng bộ lọc -->
				<xsl:for-each select="HoaDon[MaBan[contains(., $searchName)]]">
					<html:tr>
						<html:td>
							<xsl:value-of select="MaHoaDon" />
						</html:td>
						<html:td>
							<xsl:value-of select="NgayTaoHoaDon" />
						</html:td>
						<html:td>
							<xsl:value-of select="NgayXuatHoaDon" />
						</html:td>
						<html:td>
							<xsl:value-of select="PhuongThucThanhToan" />
						</html:td>
						<html:td>
							<xsl:value-of select="MaBan" />
						</html:td>
						<html:td>
							<xsl:value-of select="MaNhanVien" />
						</html:td>
					</html:tr>
				</xsl:for-each>
			</html:table>
		</html:body>
	</html:html>
</xsl:stylesheet>
