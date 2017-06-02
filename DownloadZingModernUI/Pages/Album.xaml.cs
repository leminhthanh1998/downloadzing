using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using FirstFloor.ModernUI.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;
using System.Threading;
using System.ComponentModel;

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for Album.xaml
    /// </summary>
    public partial class Album : UserControl
    {
        public Album()
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
            if (album.checkMang == false)
            {
                ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
            }
            else if (album.laCo == false)
            {
                ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
            }
            else
            {
                ModernDialog.ShowMessage("Trình duyệt sẽ mở playlist lên cho bạn !", "Thông báo", MessageBoxButton.OK);
                Process.Start(System.Windows.Forms.Application.StartupPath + "\\playlist.html");
            }
        }

        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
                album.ViewOnWeb();
                album.GhiFile();
        }
        #endregion
        #region Chay nen 1
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (album.checkMang)
            {
                if (CheckDowloadAlbum.IsChecked == true)
                {
                    Process.Start(album.idmPath, " /s");
                }
                if (album.laCo)
                {
                    ModernDialog.ShowMessage("IDM sẽ làm thực hiện việc còn lại giúp bạn !", "Thành công",
                            MessageBoxButton.OK);
                }
                else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
            }
            else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            album.DownloadAlbumZing();
        }
        #endregion

        void Disable()
        {
            buttonPathAlbum.IsEnabled = ButtonDownloadAlbum.IsEnabled = butViewOnWeb.IsEnabled =
                CheckDowloadAlbum.IsEnabled = false;
        }
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        DownloadAlbum album = new DownloadAlbum();
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if ((txbLinkAlbum.Text.StartsWith("http://mp3.zing.vn/album")|| txbLinkAlbum.Text.StartsWith("http://mp3.zing.vn/playlist")) && txbLinkAlbum.Text != "")
            {
                buttonPathAlbum.IsEnabled = ButtonDownloadAlbum.IsEnabled = butViewOnWeb.IsEnabled =
                CheckDowloadAlbum.IsEnabled = true;
            }
            else
            {
                ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
            }
        }

        private string path = "";
        private void buttonPathAlbum_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txbPathAlbum.Text = folderDlg.SelectedPath;
                path = txbPathAlbum.Text;
            }
        }

        
        //Tien hanh download Album
        private void ButtonDownloadAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (checkPath == false)
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
            }
            else if (path != "" && txbLinkAlbum.Text != "")
            {
                album.SetLinkAlbum(txbLinkAlbum.Text);
                album.SetPath(path);
                worker.RunWorkerAsync();
                Loading.IsActive = true;
            }
            else ModernDialog.ShowMessage("Bạn chưa chọn thư mục lưu file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }
        //Kiem tra duong dan
        private bool checkPath = true;
        private void txbPathAlbum_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txbPathAlbum.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng!",
                    "Lỗi", MessageBoxButton.OK);
                checkPath = false;
            }
            if (!txbPathAlbum.Text.Any(x => Char.IsWhiteSpace(x)))
            {
                checkPath = true;
            }
        }
        //Nghe playlist tren web
        private void butViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            if(txbLinkAlbum.Text!="")
            {
                Loading.IsActive = true;
                album.SetLinkAlbum(txbLinkAlbum.Text);
                worker2.RunWorkerAsync();
            }
        }

        private void txbLinkAlbum_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            butViewOnWeb.IsEnabled = ButtonDownloadAlbum.IsEnabled = buttonPathAlbum.IsEnabled = false;
        }

    }
}
