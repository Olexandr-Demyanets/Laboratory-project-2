<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" encoding="utf-8" indent="yes" />

	<xsl:template match="/">
		<html>
			<head>
				<title>Успішність студентів</title>
				<style>
					body { font-family: Segoe UI, Arial, sans-serif; margin: 20px; }
					h1 { color: #2c3e50; }
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid #999; padding: 6px 10px; text-align: left; vertical-align: top; }
					th { background: #2c3e50; color: white; }
					tr:nth-child(even) { background: #f2f2f2; }
					ul { margin: 0; padding-left: 18px; }
					.low { color: #c0392b; font-weight: bold; }
				</style>
			</head>
			<body>
				<h1>Успішність студентів</h1>
				<p>
					Кількість студентів: <xsl:value-of select="count(Students/Student)" />
				</p>

				<table>
					<tr>
						<th>ПІБ</th>
						<th>Факультет</th>
						<th>Кафедра</th>
						<th>Курс</th>
						<th>Група</th>
						<th>Форма навчання</th>
						<th>Стипендія</th>
						<th>Дисципліни та оцінки</th>
					</tr>

					<xsl:for-each select="Students/Student">
						<xsl:sort select="@Group" />
						<tr>
							<td>
								<xsl:value-of select="@FullName" />
								<xsl:if test="@Headman = 'так'"> (староста)</xsl:if>
							</td>
							<td>
								<xsl:value-of select="@Faculty" />
							</td>
							<td>
								<xsl:value-of select="@Department" />
							</td>
							<td>
								<xsl:value-of select="@Course" />
							</td>
							<td>
								<xsl:value-of select="@Group" />
							</td>
							<td>
								<xsl:value-of select="@FormOfStudy" />
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="@Scholarship">
										<xsl:value-of select="@Scholarship" />
									</xsl:when>
									<xsl:otherwise>—</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<ul>
									<xsl:for-each select="Discipline">
										<li>
											<xsl:if test="@Grade &lt; 65">
												<xsl:attribute name="class">low</xsl:attribute>
											</xsl:if>
											<xsl:value-of select="@Name" />: <xsl:value-of select="@Grade" />
										</li>
									</xsl:for-each>
								</ul>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
