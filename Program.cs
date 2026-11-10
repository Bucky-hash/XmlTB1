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
    
