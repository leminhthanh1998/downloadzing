using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for Video.xaml
    /// </summary>
    public partial class Video : UserControl
    {
        public Video()
        {
            InitializeComponent();
            Disable();
        }
        GetLink getLink= new GetLink();

        void Disable()
        {
            RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled=ButtonViewOnWeb.IsEnabled = false;

        }
        //check link mv
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if (txbLinkVideo.Text.StartsWith("http://mp3.zing.vn/video-clip") && txbLinkVideo.Text != "")
            {
                string id = GetLink.GetID(txbLinkVideo.Text);
                getLink.GetLinkMV(id);
                if (getLink.linkMV1080 != null && getLink.linkMV720 != null && getLink.linkMv480 != null)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled=ButtonViewOnWeb.IsEnabled = true;
                }
                else if (getLink.linkMV1080 == null && getLink.linkMV720 != null && getLink.linkMv480 != null)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled  =
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled = ButtonViewOnWeb.IsEnabled= true;
                    RadioButton1080.IsEnabled = false;
                }
                else if (getLink.linkMV1080 == null && getLink.linkMV720 == null && getLink.linkMv480 != null)
                {
                    RadioButton480.IsEnabled = 
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled= ButtonViewOnWeb.IsEnabled = true;
                    RadioButton1080.IsEnabled=RadioButton720.IsEnabled  = false;
                }
                else if (getLink.linkMV1080 == null && getLink.linkMV720 == null && getLink.linkMv480 == null && getLink.check==1)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled= ButtonViewOnWeb.IsEnabled = false;
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
                }
                else if (getLink.linkMV1080 == null && getLink.linkMV720 == null && getLink.linkMv480 == null &&
                         getLink.check == 2)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                 ButtonDownloadVideo.IsEnabled= ButtonViewOnWeb.IsEnabled = false;
                    txbPathVideo.Clear();
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
            }
            else
            {
                RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                 ButtonDownloadVideo.IsEnabled= ButtonViewOnWeb.IsEnabled = false;
                txbPathVideo.Clear();
                ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
        }

        private string filename = "", path = "";

        private void ButtonDownloadVideo_Click(object sender, RoutedEventArgs e)
        {
            if (getLink.check == 3)
            {
                ModernDialog.ShowMessage("Đã có lỗi, hãy chắc là máy tính bạn có cài đặt phần mềm IDM !", "Lỗi",
                    MessageBoxButton.OK);
            }
            else if(checkPath==false)
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
            }
            else if (RadioButton720.IsChecked == true && txbLinkVideo.Text != "" && path != "")
            {
                getLink.IDM(filename, path, getLink.linkMV720);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else if (RadioButton1080.IsChecked == true && txbLinkVideo.Text != "" && path != "")
            {
                getLink.IDM(filename, path, getLink.linkMV1080);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else if (RadioButton480.IsChecked == true && txbLinkVideo.Text != null && path != "")
            {
                getLink.IDM(filename, path, getLink.linkMv480);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else
                ModernDialog.ShowMessage("Bạn chưa chọn chất lượng file tải hoặc nơi lưu và tên file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }

        private void buttonPath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "MP4 File|*.mp4|All Files|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.AddExtension = true;
            saveDialog.FileName = GetLink.GetName(txbLinkVideo.Text, GetLink.GetID(txbLinkVideo.Text));
            Nullable<bool> kq = saveDialog.ShowDialog();
            if (kq == true)
            {
                filename = System.IO.Path.GetFileName(saveDialog.FileName);
                path = System.IO.Path.GetDirectoryName(saveDialog.FileName);
            }
            txbPathVideo.Text = path + "\\" + filename;
        }

        private void ButtonViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            if(RadioButton1080.IsChecked==true)
            {
                Process.Start(getLink.linkMV1080);
                Clear();
            }
            else if(RadioButton720.IsChecked==true)
            {
                Process.Start(getLink.linkMV720);
                Clear();
            }
            else if(RadioButton480.IsChecked==true)
            {
                Process.Start(getLink.linkMv480);
                Clear();
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn chất lượng file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }
        private bool checkPath = true;
        private void txbPathVideo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPathVideo.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
                checkPath = false;
            }
            if (!txbPathVideo.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                checkPath = true;
            }
        }

        private void txbLinkVideo_TextChanged(object sender, TextChangedEventArgs e)
        {
            RadioButton480.IsEnabled = RadioButton720.IsEnabled = RadioButton1080.IsEnabled =
                buttonPath.IsEnabled = ButtonDownloadVideo.IsEnabled = ButtonViewOnWeb.IsEnabled = false;
            txbPathVideo.Clear();
        }

        void Clear()
        {
            getLink.check = 0;
        }
    }
}
