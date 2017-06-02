using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace DownloadZingModernUI
{
    class GetLink
    {
        public string link320 { get; set; }
        public string link128 { get; set; }
        public string link720 { get; set; }
        public string link480 { get; set; }
        public string linkMv480 { get; set; }
        public string linkMV720 { get; set; }
        public string linkMV1080 { get; set; }
        public int check { get; set; }
        private string text2;

        /// <summary>
        /// Lấy link tải nhạc chất lương cao 320 kbps trên Zing MP3
        /// </summary>
        /// <param name="id"></param>
        public void GetLinkMp3(string id)
        {
            //kiem tra ket noi mang
            try
            {
                WebClient webClient = new WebClient();
                dynamic text = JObject.Parse(webClient.DownloadString("http://api.mp3.zing.vn/api/mobile/song/getsonginfo?requestdata=%7B%22id%22:%22" + id + "%22%7D"));
                text2 = Convert.ToString(text);

            }
            catch (Exception e)
            {
                this.link320 = this.link128 = null;
                check = 1;
            }
            //kiem tra link
            try
            {
                MatchCollection ms = Regex.Matches(text2, @"\b(?:https?://api.mp3.zing.vn/api/mobile/source/song)\S+\b");
                link128 = ms[0].Value.ToString();
                try
                {
                    if (ms.Count == 2)
                    {
                        link320 = ms[1].Value.ToString();
                    }
                    else
                    link320 = ms[2].Value.ToString();
                }
                catch (Exception e)
                {
                    this.link320 = null;
                }
            }
            catch (Exception e)
            {
                this.link128 = null; this.link320 = null;
                if (check != 1)
                    check = 2;
            }

        }
        /// <summary>
        /// Lấy link tải video HD trên Zing TV
        /// </summary>
        /// <param name="id"></param>
        public void GetLinkTv(string id)
        {
            //kiem tra ket noi mang
            try
            {
                WebClient webClient = new WebClient();
                dynamic text = JObject.Parse(webClient.DownloadString("http://api.tv.zing.vn/3.0/media/info?api_key=d04210a70026ad9323076716781c223f&media_id=" + id));
                text2 = Convert.ToString(text);

            }
            catch (Exception e)
            {
                check = 1;
                this.link480 = this.link720 = null;
            }
            // kiem tra link
            try
            {
                link720 = link480 = null;
                MatchCollection m = Regex.Matches(text2, @"\b(stream)\S+\w");
                foreach (var item in m)
                {
                    if (item.ToString().Contains("Video720"))
                    {
                        link720 = item.ToString().Insert(0, "http://");
                    }
                    if (item.ToString().Contains("Video480"))
                    {
                        link480 = item.ToString().Insert(0, "http://");
                    }
                    if (link720 != null && link480 != null) break;
                }
                if (link720 == null) link480 = m[0].Value.Insert(0, "http://");

                #region getlink Tv code cu
                //link480 = m[2].Value.ToString().Insert(0, "http://");
                //try
                //{
                //    link720 = m[3].Value.ToString().Insert(0, "http://");
                //}
                //catch (Exception e)
                //{
                //    this.link720 = null;
                //}
                //try
                //{
                //    if (link480.Contains("720"))
                //    {
                //        link720 = link480;
                //        link480 = null;
                //    }
                //    if (link720.Contains("3gp"))
                //    {
                //        link480 = m[2].Value.ToString().Insert(0, "http://");
                //        link720 = null;
                //    }
                //}
                //catch (Exception)
                //{ }
                #endregion
            }
            catch (Exception e)
            {
                this.link480 = null; this.link720 = null;
                if (check != 1)
                    check = 2;
            }

        }
        /// <summary>
        /// Lấy link tải MV HD trên Zing MP3
        /// </summary>
        /// <param name="id"></param>
        public void GetLinkMV(string id)
        {
            //kiem tra ket noi mang
            try
            {
                WebClient webClient = new WebClient();
                dynamic text =
                    JObject.Parse(
                        webClient.DownloadString(
                            "http://api.mp3.zing.vn/api/mobile/video/getvideoinfo?requestdata=%7B%22id%22:%22" + id +
                            "%22%7D"));
                text2 = Convert.ToString(text);

            }
            catch (Exception e)
            {
                check = 1;
                this.linkMv480 = this.linkMV1080 = this.linkMV720 = null;

            }
            try
            {
                MatchCollection ms = Regex.Matches(text2, @"\b(?:https?://api.mp3.zing.vn/api/mobile/source/video)\S+\b");
                linkMv480 = ms[1].Value.ToString();
                try
                {
                    linkMV720 = ms[2].Value.ToString();
                }
                catch (Exception e)
                {
                    this.linkMV720 = null;
                }
                /////////
                try
                {
                    linkMV1080 = ms[3].Value.ToString();
                }
                catch (Exception e)
                {
                    this.linkMV1080 = null;
                }
            }
            catch (Exception e)
            {
                this.linkMv480 = null; this.linkMV720 = null; this.linkMV1080 = null;
                if (check != 1)
                    check = 2;
            }

        }
        /// <summary>
        /// Thực thi IDM để tiến hành tải
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="link"></param>
        public void IDM(string filename, string path, string link)
        {
            try
            {
                string name = "SOFTWARE\\DownloadManager";
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(name);
                string idmPath = registryKey.GetValue("ExePath", "").ToString();
                string pr = " /n /d " + link + " /p " + path + " /f " + filename;
                Process.Start(idmPath, pr);
            }
            catch (Exception e)
            {
                check = 3;
            }

        }
        /// <summary>
        /// Lấy ID của bài hát
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string GetID(string link)
        {
            string id = link.Trim().Remove(0, (link.Length - 13)).Replace(".html", "");
            return id;
        }
        /// <summary>
        /// Lấy tên của bài hát
        /// </summary>
        /// <param name="link"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetName(string link, string id)
        {
            string name = link.Trim().Replace("http://mp3.zing.vn/bai-hat/", "").Replace("http://mp3.zing.vn/video-clip/", "").Replace("http://tv.zing.vn/video/", "").Replace("/" + id + ".html", "");
            return name;
        }
    }
}
