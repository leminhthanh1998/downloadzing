using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UserControl = System.Windows.Controls.UserControl;
using System.Diagnostics;

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : UserControl
    {
        public BasicPage1()
        {
            InitializeComponent();
            Disable();
        }
        GetLink getLink = new GetLink();
        //vo hieeu hoa cac button va radiobutton
        void Disable()
        {
            buttonPath.IsEnabled =ButtonViewOnWeb.IsEnabled=
            RadioButton128.IsEnabled = RadioButton320.IsEnabled =
                ButtonDownloadMp3.IsEnabled = false;
        }
        private bool checkPath = true;
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if (txbLinkMp3.Text.StartsWith("http://mp3.zing.vn/bai-hat") && txbLinkMp3.Text != "")
            {
                string id = GetLink.GetID(txbLinkMp3.Text);
                getLink.GetLinkMp3(id);
                if (getLink.link128 != null && getLink.link320 != null)
                {
                    RadioButton128.IsEnabled = RadioButton320.IsEnabled = true;
                    buttonPath.IsEnabled = ButtonDownloadMp3.IsEnabled=ButtonViewOnWeb.IsEnabled = true;
                }
                else if (getLink.link128 == null && getLink.link320 != null)
                {
                    RadioButton320.IsEnabled = true;
                    RadioButton128.IsEnabled = false;
                    buttonPath.IsEnabled = ButtonDownloadMp3.IsEnabled=ButtonViewOnWeb.IsEnabled = true;
                }
                else if (getLink.link128 != null && getLink.link320 == null)
                {
                    RadioButton128.IsEnabled = true;
                    RadioButton320.IsEnabled = false;
                    buttonPath.IsEnabled = ButtonDownloadMp3.IsEnabled=ButtonViewOnWeb.IsEnabled = true;
                }
                if (getLink.link320 == null && getLink.link128 == null && getLink.check == 1)
                {
                    RadioButton320.IsEnabled =
                        ButtonDownloadMp3.IsEnabled=ButtonViewOnWeb.IsEnabled = false;
                    txbPathMp3.Clear();
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
                }
                if (getLink.link320 == null && getLink.link128 == null && getLink.check == 2)
                {
                    RadioButton320.IsEnabled = RadioButton128.IsEnabled =
                ButtonDownloadMp3.IsEnabled=ButtonViewOnWeb.IsEnabled = false;
                    txbPathMp3.Clear();
                    ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
            }
            else
            {
                RadioButton320.IsEnabled = RadioButton128.IsEnabled=ButtonViewOnWeb.IsEnabled=
                ButtonDownloadMp3.IsEnabled = false; ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
        }

        private string filename = "", path = "";

        private void ButtonDownloadMp3_Click(object sender, RoutedEventArgs e)
        {
            if (getLink.check == 3)
            {
                ModernDialog.ShowMessage("Đã có lỗi, hãy chắc là máy tính bạn có cài đặt phần mềm IDM !", "Lỗi!",
                    MessageBoxButton.OK);
            }
            else if (checkPath == false)
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
            }
            else if (RadioButton320.IsChecked == true && txbPathMp3.Text != "" && path != "" &&RadioButton320.IsEnabled==true)
            {
                getLink.IDM(filename, path, getLink.link320);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            else if (RadioButton128.IsChecked == true && txbLinkMp3.Text != "" && path != "")
            {
                getLink.IDM(filename, path, getLink.link128);
                ModernDialog.ShowMessage("IDM sẽ tiến hành tải file về cho bạn !", "Thành công", MessageBoxButton.OK);
                Clear();
            }
            
            else ModernDialog.ShowMessage("Bạn chưa chọn chất lượng file tải hoặc nơi lưu và tên file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }

        //Lay duong dan luu file
        private void buttonPath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "MP3 File|*.mp3|All Files|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.AddExtension = true;
            saveDialog.FileName = GetLink.GetName(txbLinkMp3.Text, GetLink.GetID(txbLinkMp3.Text));
            Nullable<bool> kq = saveDialog.ShowDialog();
            if (kq == true)
            {
                filename = System.IO.Path.GetFileName(saveDialog.FileName);
                path = System.IO.Path.GetDirectoryName(saveDialog.FileName);
            }
            txbPathMp3.Text = path+"\\"+filename;
        }

        private void ButtonViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            if (RadioButton128.IsChecked == true)
            {
                Process.Start(getLink.link128);
                Clear();
            }
            else if (RadioButton320.IsChecked == true && RadioButton320.IsEnabled==true)
            {
                Process.Start(getLink.link320);
                Clear();
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn chất lượng file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }
        //Kiem tra xem duong dan co khoang trang hay khong
        private void txbPathMp3_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txbPathMp3.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
                checkPath = false;
            }
            if(!txbPathMp3.Text.Any(x=> Char.IsWhiteSpace(x)))
            {
                checkPath = true;
            }
        }

        private void txbLinkMp3_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RadioButton128.IsEnabled = RadioButton320.IsEnabled = ButtonDownloadMp3.IsEnabled = ButtonViewOnWeb.IsEnabled= buttonPath.IsEnabled  = false;
            txbPathMp3.Clear();
        }

        //
        void Clear()
        {
            getLink.check = 0;
        }
    }
}
