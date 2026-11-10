using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
class Program
{
    static void Main(string[] args)
    {
        string? rawLenh = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(rawLenh))
        {
            Console.WriteLine("Lenh khong hop le");
            return;
        }
        string lenhLine = rawLenh.Trim();
        string noiDung = Console.In.ReadToEnd();

        string[] parts = lenhLine.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string cmd = parts[0].ToUpper();
        string param = parts.Length > 1 ? parts[1].Trim() : string.Empty;
        try
        {
            XmlDocument doc = new XmlDocument();
            if (!string.IsNullOrWhiteSpace(noiDung))
            {
                doc.LoadXml(noiDung);
            }
            switch (cmd)
            {
                case "LUONG_TREN":
                    HandleLuongTren(doc, param);
                    break;
                    case "PHONG":
                    HandlePhong(doc, param);
                    break;
                case "DEM":
                HandleDem(doc);
                    break;
                case "XPATH":
                    HandleXPath(doc, lenhLine.Length > 5 ? lenhLine.Substring(5).Trim() : "");
                    break;
                    case "THUONG":
                    HandleThuong(doc);
                    break;
                default:
                Console.WriteLine("Lenh khong hop le");
                    break;
            }
        }
        catch (Exception)
        {
            if (cmd == "XPATH")
            {
                Console.WriteLine("Bieu thuc XPath khong hop le");
            }
            else
            {
                Console.WriteLine("Lenh khong hop le");
            }
        }
    }
    private static void HandleLuongTren(XmlDocument doc, string paramStr)
    {
        if (!double.TryParse(paramStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double x))
        {
            Console.WriteLine("Lenh khong hop le");
            return;
        }
        string xpath = $"//nhanvien[luong > {x.ToString(CultureInfo.InvariantCulture)}]";
        XmlNodeList? nodes = doc.SelectNodes(xpath);

        if (nodes == null || nodes.Count == 0)
        {
            Console.WriteLine("Khong co nhan vien");
            return;
        }
        foreach (XmlNode node in nodes)
        {
            string ma = node.Attributes?["ma"]?.Value ?? "";
            string hoten = node["hoten"]?.InnerText ?? "";
            string luong = node["luong"]?.InnerText ?? "";
            Console.WriteLine($"{ma} - {hoten} - {luong}");
        }
    }
    private static void HandlePhong(XmlDocument doc, string maPhong)
    {
        if (string.IsNullOrEmpty(maPhong))
        {
            Console.WriteLine("Lenh khong hop le");
            return;
        }
        XmlNode? phongNode = doc.SelectSingleNode($"//phongban[@ma='{maPhong}']");
        if (phongNode == null)
        {
            Console.WriteLine($"Khong co phong {maPhong}");
            return;
        }
        XmlNodeList? nhanViens = phongNode.SelectNodes("nhanvien");
        if (nhanViens == null || nhanViens.Count == 0)
        {
            Console.WriteLine($"Phong {maPhong} chua co nhan vien");
            return;
        }
        foreach (XmlNode nv in nhanViens)
        {
            string ma = nv.Attributes?["ma"]?.Value ?? "";
            string hoten = nv["hoten"]?.InnerText ?? "";
            Console.WriteLine($"{ma} - {hoten}");
        }
        private static void HandleDem(XmlDocument doc)
    {
        XPathNavigator nav = doc.CreateNavigator();
        object? resPhong = nav.Evaluate("count(//phongban)");
        object? resNv = nav.Evaluate("count(//nhanvien)");

        int soPhong = resPhong != null ? Convert.ToInt32(resPhong, CultureInfo.InvariantCulture) : 0;
        int soNv = resNv != null ? Convert.ToInt32(resNv, CultureInfo.InvariantCulture) : 0;
        Console.WriteLine($"So phong ban: {soPhong}");
        Console.WriteLine($"So nhan vien: {soNv}");
    }
  private static void HandleXPath(XmlDocument doc, string xpathExpr)
    {
        if (string.IsNullOrEmpty(xpathExpr))
        {
            Console.WriteLine("Bieu thuc XPath khong hop le");
            return;
        }
        XPathNavigator nav = doc.CreateNavigator();
        try
        {
            object result = nav.Evaluate(xpathExpr);
            if (result is XPathNodeIterator iterator)
            {
                bool hasResult = false;
                while (iterator.MoveNext())
                {
                    if (iterator.Current != null)
                    {
                        hasResult = true;
                        Console.WriteLine($"{iterator.Current.Name}: {iterator.Current.Value}");
                    }
                }
                if (!hasResult)
                {
                    Console.WriteLine("Khong co ket qua");
                }
            }
            else if (result is double d)
            {
                Console.WriteLine(d.ToString(CultureInfo.InvariantCulture));
            }
            else if (result is bool b)
            {
                Console.WriteLine(b ? "true" : "false");
            }
            else if (result is string s)
            {
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine(result?.ToString() ?? "Khong co ket qua");
            }
        }
        catch (XPathException)
        {
            Console.WriteLine("Bieu thuc XPath khong hop le");
        }
   

