using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for More.xaml
    /// </summary>
    public partial class More : UserControl
    {
        public More()
        {
            InitializeComponent();
            Disable();
            workerCSN.DoWork += WorkerCSN_DoWork;
            workerCSN.RunWorkerCompleted += WorkerCSN_RunWorkerCompleted;
            workerNCT.DoWork += WorkerNCT_DoWork;
            workerNCT.RunWorkerCompleted += WorkerNCT_RunWorkerCompleted;
            workerNhacVui.DoWork += WorkerNhacVui_DoWork;
            workerNhacVui.RunWorkerCompleted += WorkerNhacVui_RunWorkerCompleted;
        }

        AlbumDownloader album = new AlbumDownloader();
        private BackgroundWorker workerCSN = new BackgroundWorker();
        private BackgroundWorker workerNCT = new BackgroundWorker();
        private BackgroundWorker workerNhacVui = new BackgroundWorker();
        private string path="";
        private bool checkPath = true;

        #region Chạy nền các tác vụ tải nhạc
        private void WorkerNhacVui_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (album.CheckIDM)
            {
                if (album.CheckMang)
                {
                    if (album.CheckLink)
                    {
                        if (CheckDowloadAlbum.IsChecked == true)
                        {
                            Process.Start(album.IdmPath, " /s");
                        }
                        ModernDialog.ShowMessage("IDM sẽ làm thực hiện việc còn lại giúp bạn !", "Thành công",
                                MessageBoxButton.OK);
                    }
                    else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
                else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
            }
            else ModernDialog.ShowMessage("Máy tính bạn chưa cài đặt phần mềm IDM, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
        }

        private void WorkerNhacVui_DoWork(object sender, DoWorkEventArgs e)
        {
            album.GetLinkNhacVui();
        }

        private void WorkerNCT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (album.CheckIDM)
            {
                if (album.CheckMang)
                {
                    if (album.CheckLink)
                    {
                        if (CheckDowloadAlbum.IsChecked == true)
                        {
                            Process.Start(album.IdmPath, " /s");
                        }
                        ModernDialog.ShowMessage("IDM sẽ làm thực hiện việc còn lại giúp bạn !", "Thành công",
                                MessageBoxButton.OK);
                    }
                    else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
                else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
            }
            else ModernDialog.ShowMessage("Máy tính bạn chưa cài đặt phần mềm IDM, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
        }

        private void WorkerNCT_DoWork(object sender, DoWorkEventArgs e)
        {
            album.GetLinkNCT();
        }

        private void WorkerCSN_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.IsActive = false;
            if (album.CheckIDM)
            {
                if (album.CheckMang)
                {
                    if (album.CheckLink)
                    {
                        if (CheckDowloadAlbum.IsChecked == true)
                        {
                            Process.Start(album.IdmPath, " /s");
                        }
                        ModernDialog.ShowMessage("IDM sẽ làm thực hiện việc còn lại giúp bạn !", "Thành công",
                                MessageBoxButton.OK);
                    }
                    else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại link bạn nhập !", "Lỗi", MessageBoxButton.OK);
                }
                else ModernDialog.ShowMessage("Đã có lỗi xảy ra, hãy kiểm tra lại kết nối mạng !", "Lỗi", MessageBoxButton.OK);
            }
            else ModernDialog.ShowMessage("Máy tính bạn chưa cài đặt phần mềm IDM, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);

        }

        private void WorkerCSN_DoWork(object sender, DoWorkEventArgs e)
        {
            album.GetLinkBaiHatTrongAlbum();
        }
#endregion

        void Disable()
        {
            ButtonDownloadAlbum.IsEnabled = buttonPathAlbum.IsEnabled = CheckDowloadAlbum.IsEnabled= false;
        }
        void Enable()
        {
            ButtonDownloadAlbum.IsEnabled = buttonPathAlbum.IsEnabled = CheckDowloadAlbum.IsEnabled = true;
        }

        /// <summary>
        /// Check link nhập vào
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if(txbLinkAlbum.Text.StartsWith("http://chiasenhac.vn/nghe-album"))
            {
                Enable();
            }
            else if(txbLinkAlbum.Text.StartsWith("http://www.nhaccuatui.com/playlist"))
            {
                Enable();
            }
            else if(txbLinkAlbum.Text.StartsWith("http://nhac.vui.vn/album"))
            {
                Enable();
            }
            else ModernDialog.ShowMessage("Link bạn nhập không đúng, hãy kiểm tra lại !", "Lỗi", MessageBoxButton.OK);
        }


        private void ButtonDownloadAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (checkPath == false)
            {
                ModernDialog.ShowMessage("Đường dẫn lưu file không được có khoảng trắng !",
                    "Lỗi", MessageBoxButton.OK);
            }
            else if(checkPath==true && txbLinkAlbum.Text.StartsWith("http://chiasenhac.vn/nghe-album") && txbPathAlbum.Text!="")
            {
                album.Link = txbLinkAlbum.Text;
                album.Path = txbPathAlbum.Text;
                workerCSN.RunWorkerAsync();
                Loading.IsActive = true;
            }
            else if(checkPath==true && txbLinkAlbum.Text.StartsWith("http://www.nhaccuatui.com/playlist") && txbPathAlbum.Text!="")
            {
                album.Link = txbLinkAlbum.Text;
                album.Path = txbPathAlbum.Text;
                workerNCT.RunWorkerAsync();
                Loading.IsActive = true;
            }
            else if (checkPath == true && txbLinkAlbum.Text.StartsWith("http://nhac.vui.vn/album") && txbPathAlbum.Text != "")
            {
                album.Link = txbLinkAlbum.Text;
                album.Path = txbPathAlbum.Text;
                workerNhacVui.RunWorkerAsync();
                Loading.IsActive = true;
            }
            else if(txbPathAlbum.Text=="") ModernDialog.ShowMessage("Bạn chưa chọn thư mục lưu file, hãy chọn lại !",
                    "Lỗi", MessageBoxButton.OK);
        }



        /// <summary>
        /// Chọn thư mục để lưu các bài hát
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Kiểm tra đường dẫn có khoảng cách trăng hay không
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbPathAlbum_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txbLinkAlbum_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonDownloadAlbum.IsEnabled = buttonPathAlbum.IsEnabled = false;
        }

        
    }
}
