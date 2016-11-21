using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient klient = null;
            TcpListener serwerHTTP = new TcpListener(IPAddress.Any,80);
            serwerHTTP.Start();
            while (true)
            {
                klient = serwerHTTP.AcceptTcpClient();
                StreamReader Sreader = new StreamReader(klient.GetStream());
                StreamWriter Swriter = new StreamWriter(klient.GetStream());
                BinaryWriter Bwriter = new BinaryWriter(klient.GetStream());
                string komunikat = "";
                string temp = "";
                if (klient.Connected)
                {
                    do
                    {
                        try
                        {
                            temp = Sreader.ReadLine();
                        }
                        catch
                        {
                            return;
                        }
                        komunikat += temp + "\n";
                    } while (temp != "");
                } string sciezka = "..\\..\\www";
                sciezka += komunikat.Substring(komunikat.IndexOf("/"), komunikat.IndexOf("HTTP") - 5);
                sciezka = sciezka.Replace("/", "\\");
                if (sciezka == "..\\..\\www\\")
                    sciezka = "..\\..\\www\\index.html";

                if (komunikat.Substring(0, 3) == "GET")
                {
                    if (!Directory.Exists("..\\..\\www") || !File.Exists(sciezka))
                    {
                        ;
                    }
                    else
                    {
                        FileInfo info = new FileInfo(sciezka);
                        Swriter.Write(komunikat.Substring(komunikat.IndexOf("HTTP", 1), 8) + " 200 OK\r\nServer: ala\r\nKeep-Alive: timeout=15, max=100\r\nContent-Length: " + info.Length + "\r\nContent-Type: " + DajMime(info.Extension) + "; charset=iso-8859-2\r\n\r\n");
                        FileStream Fstream = new FileStream(sciezka, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BinaryReader reader = new BinaryReader(Fstream);
                        byte[] bufor = new byte[Fstream.Length];
                        int odebranebajty;
                        while ((odebranebajty = reader.Read(bufor, 0, bufor.Length)) != 0)
                        {
                            ;
                        }
                        Bwriter.Write(bufor, 0, (int)info.Length);
                        reader.Close();
                        Fstream.Close();
                    }
                }
                klient.Close();
            }
        }

        private static string DajMime(string p)
        {
            string MIME;
            if (p == ".html" || p == ".htm")
                MIME = "text/html";
            else if (p == ".css")
                MIME = "text/css";
            else if (p == ".png")
                MIME = "image/png";
            else if (p == ".gif")
                MIME = "image/gif";
            else if (p == ".jpg")
                MIME = "image/jpg";
            else
                MIME = "text/html";
            return MIME;
        }
    }
}
