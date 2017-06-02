using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Application = System.Windows.Forms.Application;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace DownloadZingModernUI.Pages
{
    class ZingRadioDownload
    {
        GetLink getLink = new GetLink();
        private string path;
        private string idmPath;
        private string link = "";
        private bool  laco = true;
        private bool checkLink = true;

        public string IdmPath { get => idmPath; set => idmPath = value; }
        public bool Laco { get => laco; set => laco = value; }
        public bool CheckLink { get => checkLink; set => checkLink = value; }
        public void SetLink(string link)
        {
            this.link = link;
        }
        public void SetPath(string path)
        {
            this.path = path;
        }
        //Đẩy link download sang IDM
        public void DownloadRadio()
        {
            CheckLink = true;
            Laco = true;
            List<string> dsLink = new List<string>();
            List<string> dsLinkDownload = new List<string>();
            List<string> dsTen = new List<string>();
            //Lấy đường dẫn IDM
            try
            {
                string name = "SOFTWARE\\DownloadManager";
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(name);
                IdmPath = registryKey.GetValue("ExePath", "").ToString();
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("Máy tính bạn chưa cài đặt phần mềm IDM, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
            //Lấy link tất cả các bài hat trên Zing RAdio
            try
            {
                var web = new WebClient();
                link = web.DownloadString(link);
                MatchCollection ms = Regex.Matches(link, @"\b(http://radio.zing.vn/xml)\S+\b");
                string link2 = ms[0].Value;
                string xml = web.DownloadString(link2);
                MatchCollection baihat = Regex.Matches(xml, @"\b(http://mp3.zing.vn/bai-hat/)\S+\b");
                foreach (Match item in baihat)
                {
                    dsLink.Add(item.Value.Replace(@"]]></link", ""));
                }
            }
            catch (Exception)
            {
                CheckLink = false;
                
            }
            if (dsLink.Count != 0)
            {
                foreach (string linkBaiHat in dsLink)
                {
                    dsTen.Add(GetLink.GetName(linkBaiHat, GetLink.GetID(linkBaiHat.ToString())) +
                                    ".mp3");
                    getLink.GetLinkMp3(GetLink.GetID(linkBaiHat.ToString()));
                    if (getLink.link320 != null)
                    {
                        dsLinkDownload.Add(getLink.link320);
                    }
                    else
                    {
                        dsLinkDownload.Add(getLink.link128);
                    }
                }
            }
            else Laco = false;
            try
            {
                int k = 0;
                foreach (string item in dsLinkDownload)
                {
                    bool isSendingLink = true;
                    new Thread(_function =>
                    {
                        string pr = " /a /d " + item + " /p " + path + " /f " + dsTen[k];
                        k++;
                        Process.Start(IdmPath, pr);
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
                Laco = false;
            }
        }
        //Mở file playlist.html trong trình duyệt
        public void ViewOnWeb()
        {
            Laco = true;
            CheckLink = true;
            List<string> dsLink = new List<string>();
            List<string> dsLinkDownload = new List<string>();
            List<string> dsTen = new List<string>();
            List<string> sourceHTML = new List<string>();
            //Lấy link 
            try
            {
                var web = new WebClient();
                link = web.DownloadString(link);
                MatchCollection ms = Regex.Matches(link, @"\b(http://radio.zing.vn/xml)\S+\b");
                string link2 = ms[0].Value;
                string xml = web.DownloadString(link2);
                MatchCollection baihat = Regex.Matches(xml, @"\b(http://mp3.zing.vn/bai-hat/)\S+\b");
                foreach (Match item in baihat)
                {
                    dsLink.Add(item.Value.Replace(@"]]></link", ""));
                }
            }
            catch (Exception)
            {
                CheckLink = false;
            }
            if (dsLink.Count != 0)
            {
                foreach (string linkBaiHat in dsLink)
                {
                    dsTen.Add(GetLink.GetName(linkBaiHat, GetLink.GetID(linkBaiHat.ToString())) +
                                    ".mp3");
                    getLink.GetLinkMp3(GetLink.GetID(linkBaiHat.ToString()));
                    if (getLink.link320 != null)
                    {
                        dsLinkDownload.Add(getLink.link320);
                    }
                    else
                    {
                        dsLinkDownload.Add(getLink.link128);
                    }
                }
            }
            else Laco = false;
            //Add link vô source html
            if (Laco)
            {
                string playlist1 = Properties.Resources.testplaylist1;
                sourceHTML.Add(playlist1);
                sourceHTML.Add(@"<source type=""audio/mp3"" src=""" + dsLinkDownload[0] + "\">");
                sourceHTML.Add("Trình duyệt của bạn không hỗ trợ HTML5!");
                sourceHTML.Add("</audio>");
                sourceHTML.Add(@"<ul id=""playlist"">");
                sourceHTML.Add(@"<li class=""active""><a href=""" + dsLinkDownload[0] + "\">" + dsTen[0] + @"</a></li>");
                for (int i = 1; i < dsLinkDownload.Count; i++)
                {
                    sourceHTML.Add(@"<li><a href=""" + dsLinkDownload[i] + "\">" + dsTen[i] + @"</a></li>");
                }
                string playlist2 = Properties.Resources.testplaylist2;
                sourceHTML.Add(playlist2);
                File.WriteAllLines("playlist.html", sourceHTML);
            }
        }
    }
}
