using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace DownloadZingModernUI
{
    class AlbumDownloader
    {
        private bool checkMang = true;
        private bool checkLink = true;
        private bool checkIDM = true;
        private List<string> dsLink = new List<string>();
        private List<string> dsLinkBaiHatCSN = new List<string>();
        private string text = "";
        private string idmPath = "";
        private string path;
        private string link;
        public bool CheckMang { get => checkMang; set => checkMang = value; }
        public bool CheckLink { get => checkLink; set => checkLink = value; }
        public List<string> DsLink { get => dsLink; set => dsLink = value; }
        public string Path { get => path; set => path = value; }
        public string Link { get => link; set => link = value; }
        public string IdmPath { get => idmPath; set => idmPath = value; }
        public bool CheckIDM { get => checkIDM; set => checkIDM = value; }


        /// <summary>
        /// Download album từ Nhac.vui.vn
        /// </summary>
        /// <param name="link"></param>
        public void GetLinkNhacVui()
        {
            DsLink.Clear();
            CheckLink = CheckMang = true;
            try
            {
                WebClient wc = new WebClient();
                text = wc.DownloadString(Link);
            }
            catch (Exception)
            {
                CheckMang = false;
            }
            if (CheckMang)
            {
                try
                {
                    MatchCollection linkdownload = Regex.Matches(text, "(?i)\\s*http://static-mp3.nhac.vui.vn\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
                    for (int i = linkdownload.Count - 1; i >= 0; i--)
                    {
                        if (linkdownload[i].Value.Contains("320k") || linkdownload[i].Value.Contains("320"))
                        {
                            DsLink.Add(linkdownload[i].Value.Replace(" ", "%20"));
                            i--;
                        }
                        else
                        {
                            DsLink.Add(linkdownload[i].Value.Replace(" ", "%20"));
                        }
                    }
                    if (DsLink.Count == 0) CheckLink = false;
                }
                catch (Exception)
                {
                    CheckLink = false;
                }
            }
            if (CheckLink) IDM();
        }
        /// <summary>
        /// Download các bài hát trong playlist từ Nhaccuatui
        /// </summary>
        public void GetLinkNCT()
        {
            DsLink.Clear();
            CheckLink = CheckMang = true;
            try
            {
                WebClient wc = new WebClient();
                text = wc.DownloadString(Link);
            }
            catch (Exception)
            {
                CheckMang = false;
            }
            if (CheckMang)
            {
                try
                {
                    MatchCollection linkxmlNCT = Regex.Matches(text, "(?i)\\s*http://www.nhaccuatui.com/flash/xml\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
                    var wc2 = new WebClient();
                    string text2 = wc2.DownloadString(linkxmlNCT[0].Value);
                    MatchCollection linkDownloadNCT = Regex.Matches(text2, "(?i)\\s*http://\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
                    foreach (Match item in linkDownloadNCT)
                    {
                        if (item.Value.Contains(".mp3")) DsLink.Add(item.Value.Replace("]]", ""));
                    }
                    if (DsLink.Count == 0) CheckLink = false;
                }
                catch (Exception)
                {
                    CheckLink = false;
                }
            }
            if (CheckLink) IDM();
        }

        #region Chiasenhac.vn
        /// <summary>
        /// Lấy link các bài hát trong album từ Chiasenhac.vn
        /// </summary>
        /// <param name="link"></param>
        public void GetLinkBaiHatTrongAlbum()
        {
            DsLink.Clear();
            dsLinkBaiHatCSN.Clear();
            CheckLink = CheckMang = true;
            try
            {
                WebClient wc = new WebClient();
                text = wc.DownloadString(Link);
            }
            catch (Exception)
            {
                CheckMang = false;
            }
            if (CheckMang)
            {
                try
                {
                    MatchCollection linkdownload = Regex.Matches(text, "(?i)\\s*http://chiasenhac.vn/nghe-album\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
                    foreach (Match item in linkdownload)
                    {
                        dsLinkBaiHatCSN.Add(GetLinkDownloadCSN(item.Value));
                        //dsLinkBaiHatCSN.Add(item.Value.Replace(".html","")+"_download.html");
                    }
                    dsLinkBaiHatCSN = AlbumDownloader.xoaTrungLap(dsLinkBaiHatCSN);
                    if (dsLinkBaiHatCSN.Count == 0) CheckLink = false;
                }
                catch (Exception)
                {
                    CheckLink = false;
                }
            }
            if (CheckLink)
            {
                try
                {
                    MatchCollection sobaihat = Regex.Matches(text, "(?i)\\s*playlist-\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
                    for (int i = 0; i < sobaihat.Count - 2; i++)
                    {
                        DsLink.Add(GetLinkBaiHatCSN(dsLinkBaiHatCSN[i]));
                    }
                    if (sobaihat.Count > 0)
                        IDM();
                    else CheckLink = false;
                }
                catch (Exception) { CheckLink = false; }
            }
        }

        public string GetLinkDownloadCSN(string link)
        {
            WebClient wc = new WebClient();
            string text = wc.DownloadString(link);
            MatchCollection linkdownload = Regex.Matches(text, "(?i)\\s*http://chiasenhac.vn/mp3\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
            foreach (Match item in linkdownload)
            {
                if (item.Value.Contains("_download.html"))
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy link download trực tiếp bài hát từ Chiasenhac.vn
        /// </summary>
        public string GetLinkBaiHatCSN(string link)
        {
            WebClient wc = new WebClient();
            text = wc.DownloadString(link);

            MatchCollection linkdownload = Regex.Matches(text, "(?i)\\s*http://data.chiasenhac.com/download\\s*\\s*(\"([^\"]*\")|'[^']*'|([^'\">]+))");
            for (int i = linkdownload.Count - 1; i >= 0; i--)
            {
                if (linkdownload[i].Value.Contains("[MP3 320kbps]"))
                {
                    return linkdownload[i].Value.Replace(" ", "%20").Replace("[", "%5B").Replace("]", "%5D");
                }
                else if (linkdownload[i].Value.Contains("[MP3 128kbps]")) { return linkdownload[i].Value.Replace(" ", "%20").Replace("[", "%5B").Replace("]", "%5D"); }
            }

            return null;
        }

        /// <summary>
        /// Xóa các link bài hát trùng lập
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
#endregion

        ///Xóa các link trùng lập
        public static List<string> xoaTrungLap(List<string> ds)
        {
            List<string> list = new List<string>();
            foreach (string item in ds)
            {
                if (!AlbumDownloader.Contains(list, item))
                {
                    list.Add(item);
                }
            }
            return list;
        }
        private static bool Contains(List<string> list, string comparedValue)
        {
            bool result;
            foreach (string current in list)
            {
                if (current == comparedValue)
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }


        /// <summary>
        /// Đấy link sang hàng đợi của IDM
        /// </summary>
        private void IDM()
        {
            try
            {
                string name = "SOFTWARE\\DownloadManager";
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(name);
                IdmPath = registryKey.GetValue("ExePath", "").ToString();
            }
            catch (Exception)
            {
                CheckIDM = false;
            }
            if (CheckIDM)
                foreach (string item in DsLink)
                {
                    if (item == null)
                        continue;
                    else
                    {
                        string pr = " /a /d " + item + " /p " + Path;
                        Process.Start(IdmPath, pr);
                        Thread.Sleep(500);
                    }
                }
        }

        private void ViewOnWeb()
        {

        }
    }
}
