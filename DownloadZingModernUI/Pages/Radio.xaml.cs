using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for Radio.xaml
    /// </summary>
    public partial class Radio : UserControl
    {
        public Radio()
        {
            InitializeComponent();
            Disable();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker2.DoWork += Worker2_DoWork;
            worker2.RunWorkerCompleted += Worker2_RunWorkerCompleted;
        }
#region Chay nen 2
        private void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (radio.CheckLink==false)
            {
                ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng hoặc link !", "Lỗi", MessageBoxButton.OK);
            }
            else if(radio.Laco == false)
            {
                ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link !", "Lỗi", MessageBoxButton.OK);
            }
            else
            {
                ModernDialog.ShowMessage("Trình duyệt sẽ mở playlist lên cho bạn!", "Thông báo", MessageBoxButton.OK);
                Process.Start(System.Windows.Forms.Application.StartupPath + "\\playlist.html");
            }
        }

        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            radio.ViewOnWeb();
        }
        #endregion
        #region Chay nen 1
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (radio.CheckLink)
            {
                if (CheckDowloadRadio.IsChecked == true)
                {
                    Process.Start(radio.IdmPath, " /s");
                }
                if (radio.Laco)
                {
                    ModernDialog.ShowMessage("IDM sẽ làm thực hiện việc còn lại giúp bạn !", "Thành công",
                       MessageBoxButton.OK);
                    Clear();
                }
                else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
            }
            else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            radio.DownloadRadio();
        }
#endregion
        void Disable()
        {
            buttonPathRadio.IsEnabled = ButtonDownloadRadio.IsEnabled = butViewOnWeb.IsEnabled=
                CheckDowloadRadio.IsEnabled = false;
        }
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        ZingRadioDownload radio = new ZingRadioDownload();
        private string path = "";
        bool checkPath = true;

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if(txbLinkRadio.Text.StartsWith("http://radio.zing.vn"))
            {
                buttonPathRadio.IsEnabled = ButtonDownloadRadio.IsEnabled =butViewOnWeb.IsEnabled=
                CheckDowloadRadio.IsEnabled = true;
            }
            else ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
        }

        private void buttonPathRadio_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txbPathRadio.Text = folderDlg.SelectedPath;
                path = txbPathRadio.Text;
            }
        }
        void Clear()
        {
            //buttonPathRadio.IsEnabled = ButtonDownloadRadio.IsEnabled =
            //    CheckDowloadRadio.IsEnabled = false;
            //txbLinkRadio.Clear();
            //txbPathRadio.Clear();
        }

        private void txbPathRadio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPathRadio.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
                checkPath = false;
            }
            else checkPath = true;
        }

        private void ButtonDownloadRadio_Click(object sender, RoutedEventArgs e)
        {
            if(checkPath==false)
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
            }
            else if(path!=""&&txbLinkRadio.Text!="")
            {
                radio.SetLink(txbLinkRadio.Text);
                radio.SetPath(path);
                worker.RunWorkerAsync();
                Loading.IsActive = true;
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn thư mục lưu file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }

        private void butViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
                Loading.IsActive = true;
                radio.SetLink(txbLinkRadio.Text);
                worker2.RunWorkerAsync();
        }

        private void txbLinkRadio_TextChanged(object sender, TextChangedEventArgs e)
        {
            butViewOnWeb.IsEnabled = ButtonDownloadRadio.IsEnabled=buttonPathRadio.IsEnabled = false;
        }
    }
}
