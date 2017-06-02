using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace DownloadZingModernUI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            Dispatcher.Invoke(CheckUpdate);
        }
        void CheckUpdate()
        {
            try
            {
                var update = new WebClient();
                string version = update.DownloadString("https://goo.gl/eEkHCb");
                if (Convert.ToInt32(version) > 480)
                {
                    ModernDialog.ShowMessage("Đã có phiên bản mới, hãy click vào Update để cập nhật DownloadZing !", "Thông báo", System.Windows.MessageBoxButton.OK);
                }
            }
            catch (System.Exception)
            {

            }

        }
    }
}
