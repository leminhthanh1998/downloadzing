using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Application = System.Windows.Forms.Application;
using System.Collections.Generic;

namespace DownloadZingModernUI.Pages
{
    class DownloadAlbum
    {
        public string idmPath { set; get; }
        private string linkAlbum;
        private string path;
        public bool laCo = true;
        public bool checkMang = true;
        
        public void SetLinkAlbum(string link)
        {
            this.linkAlbum = link;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }
        
        GetLink getLink = new GetLink();
        string page = "";
        List<string> danhSachBaiHat = new List<string>();
        List<string> danhSachLink = new List<string>();
        List<string> danhSachTen = new List<string>();
        //Đẩy link download sang IDM
        public void DownloadAlbumZing()
        {
            laCo = true;
            checkMang = true;
            danhSachBaiHat.Clear();
            danhSachLink.Clear();
            danhSachTen.Clear();
            //Lấy đường dẫn đến file idman.exe
            try
            {
                string name = "SOFTWARE\\DownloadManager";
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(name);
                idmPath = registryKey.GetValue("ExePath", "").ToString();
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("Máy tính bạn chưa cài đặt phần mềm IDM, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
            //Giải mã gzip và link link các bài nhạc trong album
            try
            {
                HttpWebRequest req =
                (HttpWebRequest)HttpWebRequest.Create(linkAlbum);
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("Accept-Encoding", "gzip,deflate");
                GZipStream zip = new GZipStream(req.GetResponse().GetResponseStream(),
                    CompressionMode.Decompress);
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
                var reader = new StreamReader(zip);
                page = reader.ReadToEnd();
                resp.Close();
            }
            catch (Exception e)
            {
                checkMang = false;
            }

            MatchCollection ms = Regex.Matches(page, @"\b(?:https?://mp3.zing.vn/bai-hat/)\S+\b");
            for (int i = 0; i < ms.Count - 1; i += 3)
            {
                danhSachBaiHat.Add(ms[i].Value);
            }
            if (danhSachBaiHat.Count != 0)
            {
                foreach (var linkBaiHat in danhSachBaiHat)
                {
                    danhSachTen.Add(GetLink.GetName(linkBaiHat.ToString(), GetLink.GetID(linkBaiHat.ToString())) +
                                    ".mp3");
                    getLink.GetLinkMp3(GetLink.GetID(linkBaiHat.ToString()));
                    if (getLink.link320 != null)
                    {
                        danhSachLink.Add(getLink.link320);
                    }
                    else
                    {
                        danhSachLink.Add(getLink.link128);
                    }
                }

            }
            else
            {
                laCo = false;
            }
            //Tiến hành add link đã get và IDM
            try
            {
                int k = 0;
                foreach (string link in danhSachLink)
                {
                    bool isSendingLink = true;
                    new Thread(_function =>
                    {
                        string pr = " /a /d " + link + " /p " + path + " /f " + danhSachTen[k];
                        k++;
                        Process.Start(idmPath, pr);
                        isSendingLink = false;
                    }).Start();
                    while (!isSendingLink)
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(500);

                }
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage("Hãy chắc là bạn đã cài đặt phần mềm IDM !", "Lỗi", MessageBoxButton.OK);
                laCo = false;
            }
        }
        //Mở file playlist.html
        
        public void ViewOnWeb()
        {
            laCo = true;
            checkMang = true;
            danhSachBaiHat.Clear();
            danhSachLink.Clear();
            danhSachTen.Clear();

            try
            {
                HttpWebRequest req =
                (HttpWebRequest)HttpWebRequest.Create(linkAlbum);
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("Accept-Encoding", "gzip,deflate");
                GZipStream zip = new GZipStream(req.GetResponse().GetResponseStream(),
                    CompressionMode.Decompress);
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
                var reader = new StreamReader(zip);
                page = reader.ReadToEnd();
                resp.Close();
            }
            catch (Exception e)
            {
                checkMang = false;
            }

            MatchCollection ms = Regex.Matches(page, @"\b(?:https?://mp3.zing.vn/bai-hat/)\S+\b");
            for (int i = 0; i < ms.Count - 1; i += 3)
            {
                danhSachBaiHat.Add(ms[i].Value);
            }
            if (danhSachBaiHat.Count != 0)
            {
                foreach (var linkBaiHat in danhSachBaiHat)
                {
                    danhSachTen.Add(GetLink.GetName(linkBaiHat.ToString(), GetLink.GetID(linkBaiHat.ToString())) +
                                    ".mp3");
                    getLink.GetLinkMp3(GetLink.GetID(linkBaiHat.ToString()));
                    if (getLink.link320 != null)
                    {
                        danhSachLink.Add(getLink.link320);
                    }
                    else
                    {
                        danhSachLink.Add(getLink.link128);
                    }
                }

            }
            else
            {
                laCo = false;
            }
            
        }

        public void GhiFile()
        {
            if (laCo)
            {
                List<string> sourceHTML = new List<string>();
                string playlist1 = Properties.Resources.testplaylist1;
                sourceHTML.Add(playlist1);
                //Add link vô playlist.html
                sourceHTML.Add(@"<source type=""audio/mp3"" src=""" + danhSachLink[0] + "\">");
                sourceHTML.Add("Trình duyệt của bạn không hỗ trợ HTML5!");
                sourceHTML.Add("</audio>");
                sourceHTML.Add(@"<ul id=""playlist"">");
                sourceHTML.Add(@"<li class=""active""><a href=""" + danhSachLink[0] + "\">" + danhSachTen[0] + @"</a></li>");
                for (int i = 1; i < danhSachLink.Count; i++)
                {
                    sourceHTML.Add(@"<li><a href=""" + danhSachLink[i] + "\">" + danhSachTen[i] + @"</a></li>");
                }
                string playlist2 = Properties.Resources.testplaylist2;
                sourceHTML.Add(playlist2);
                File.WriteAllLines("playlist.html", sourceHTML);
                
            }
        }
    }
}
