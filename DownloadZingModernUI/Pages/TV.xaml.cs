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
    /// Interaction logic for TV.xaml
    /// </summary>
    public partial class TV : UserControl
    {
        public TV()
        {
            InitializeComponent();
            Disable();
        }
        GetLink getLink= new GetLink();
        void Disable()
        {
            buttonPath.IsEnabled = ButtonViewOnWeb.IsEnabled=
                ButtonDownloadTV.IsEnabled = RadioButton480.IsEnabled = RadioButton720.IsEnabled = false;
        }
        //check link zing tv
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if (txbLinkTV.Text.StartsWith("http://tv.zing.vn/video") && txbLinkTV.Text != "")
            {
                string id = GetLink.GetID(txbLinkTV.Text);
                getLink.GetLinkTv(id);
                if (getLink.link480 != null && getLink.link720 != null)
                {
                    buttonPath.IsEnabled =
                        ButtonDownloadTV.IsEnabled=ButtonViewOnWeb.IsEnabled = RadioButton480.IsEnabled = RadioButton720.IsEnabled = true;
                }
                else if (getLink.link480 != null && getLink.link720 == null)
                {
                    buttonPath.IsEnabled =
                        ButtonDownloadTV.IsEnabled=ButtonViewOnWeb.IsEnabled = RadioButton480.IsEnabled = true;
                    RadioButton720.IsEnabled = false;
                }
                else if (getLink.link720 != null && getLink.link480 == null)
                {
                    buttonPath.IsEnabled =ButtonViewOnWeb.IsEnabled=
                        ButtonDownloadTV.IsEnabled = RadioButton720.IsEnabled = true;
                    RadioButton480.IsEnabled = false;
                }
                else if (getLink.link480 == null && getLink.link720 == null && getLink.check == 1)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled = false;
                    txbPathTV.Clear();
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
                }
                else if (getLink.link480 == null && getLink.link720 == null && getLink.check == 2)
                {
                    RadioButton480.IsEnabled = RadioButton720.IsEnabled=ButtonViewOnWeb.IsEnabled = ButtonDownloadTV.IsEnabled = false;
                    txbPathTV.Clear();
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
            }
            else
            {
                RadioButton480.IsEnabled = RadioButton720.IsEnabled=ButtonDownloadTV.IsEnabled=ButtonViewOnWeb.IsEnabled = false;
                ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
        }

        private string filename = "", path = "";

        private void buttonPath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "MP4 File|*.mp4|All Files|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.AddExtension = true;
            saveDialog.FileName = GetLink.GetName(txbLinkTV.Text, GetLink.GetID(txbLinkTV.Text));
            Nullable<bool> kq = saveDialog.ShowDialog();
            if (kq == true)
            {
                filename = System.IO.Path.GetFileName(saveDialog.FileName);
                path = System.IO.Path.GetDirectoryName(saveDialog.FileName);
            }
            txbPathTV.Text = path + "\\" + filename;
        }

        private void ButtonDownloadTV_Click(object sender, RoutedEventArgs e)
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
            else if (RadioButton480.IsChecked == true && txbLinkTV.Text != "" && path !="")
            {
                getLink.IDM(filename, path, getLink.link480);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else if (RadioButton720.IsChecked == true && txbLinkTV.Text != "" && path != "" && RadioButton720.IsEnabled==true)
            {
                getLink.IDM(filename, path, getLink.link720);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn chất lượng file tải hoặc nơi lưu và tên file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }

        private void ButtonViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            if(RadioButton480.IsChecked==true)
            {
                Process.Start(getLink.link480);
                Clear();
            }
            else if(RadioButton720.IsChecked==true && RadioButton720.IsEnabled==true)
            {
                Process.Start(getLink.link720);
                Clear();
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn chất lượng, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }
        private bool checkPath = true;
        private void txbPathTV_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPathTV.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
                checkPath = false;
            }
            if (!txbPathTV.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                checkPath = true;
            }
        }

        private void txbLinkTV_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonPath.IsEnabled = ButtonViewOnWeb.IsEnabled =
                ButtonDownloadTV.IsEnabled = RadioButton480.IsEnabled = RadioButton720.IsEnabled = false;
            txbPathTV.Clear();
        }

        void Clear()
        {
            getLink.check = 0;
            getLink.link480 = getLink.link720 = null;
        }
    }
}
