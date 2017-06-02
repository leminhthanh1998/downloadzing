using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadZingModernUI
{
    class WoimDownloader
    {
        private bool checkLink=true;
        private bool checkMang=true;
        private string linkDownload;
        private List<string> dsLink = new List<string>();
        public bool CheckLink { get => checkLink; set => checkLink = value; }
        public bool CheckMang { get => checkMang; set => checkMang = value; }
        public string LinkDownload { get => linkDownload; set => linkDownload = value; }
        public List<string> DsLink { get => dsLink; set => dsLink = value; }

        /// <summary>
        /// Lấy link download bài hát
        /// </summary>
        /// <param name="link"></param>
        public void GetLinkBaiHat(string link)
        {
            var wc = new WebClient();
            string text = ""; string text2="";
            MatchCollection linkdownload;
            try
            {
                text = wc.DownloadString(link);
            }
            catch (Exception)
            {
                CheckMang = false;
            }
            try
            {
                MatchCollection ms = Regex.Matches(text, @"\b(?:https?://www.woim.net/list/)\S+\b");
                text2 = wc.DownloadString(ms[0].Value);
                linkdownload = Regex.Matches(text2, @"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*");
                LinkDownload= linkdownload[0].Value;
            }
            catch (Exception)
            {
                CheckLink = false;
            }
        }

        public void GetLinkAlbum(string link)
        {
            WebClient wc = new WebClient();
            string text = wc.DownloadString(link);
            MatchCollection ms = Regex.Matches(text, @"\b(?:https?://www.woim.net/song/)\S+\b");
            for (int i = 0; i < ms.Count; i += 3)
            {
                DsLink.Add(ms[i].Value);
            }
        }

        private string GetName(string link)
        {
            string name="";

            return name;
        }
    }
}
