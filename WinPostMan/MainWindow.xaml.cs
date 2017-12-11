using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace WinPostMan
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts;

        IHttpService _service;
        public MainWindow(): this(new HttpService())
        {
            InitializeComponent();
        }

        public MainWindow(IHttpService service)
        {
            _service = new HttpService();
        }

        /// <summary>
        /// 開始作業
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            txtResult.Text = "";

            ChangeStatus("查詢中...");
            barStatus.IsIndeterminate = true;

            // 取得Header
            var headerDict = tryGetHeader();

            // --- 開始HTTP ---
            string result = "";
            Task<string> task;
            switch (cbMethod.SelectedIndex)
            {
                case 0:
                    task = _service.tryHttpGet(headerDict, txtUrl.Text, cts.Token);
                    break;
                case 1:
                    task = _service.tryHttpPost(headerDict, txtUrl.Text, txtBody.Text, cts.Token);
                    break;
                default:
                    throw new ArgumentNullException("沒有此HTTP方法");
            }
            result = await task;
            txtResult.Text = result;
            // --- 結束HTTP ---


            ChangeStatus("查詢完畢", Brushes.Green);
            barStatus.IsIndeterminate = false;

        }

        /// <summary>
        /// 取消作業
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
                ChangeStatus("終止查詢", Brushes.Red);
            }
        }







        /// <summary>
        /// 改狀態
        /// </summary>
        /// <param name="state">文字</param>
        /// <param name="color">Brush顏色</param>
        /// <param name="isBegin">是否剛開始</param>
        void ChangeStatus(string state, SolidColorBrush color = null)
        {
            status.Content = state;
            status.Foreground = color ?? Brushes.Black;
        }

        /// <summary>
        /// 取表頭
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> tryGetHeader()
        {
            Dictionary<string, string> headerDict = new Dictionary<string, string>();
            try
            {
                // Header處理 (4個應該就夠多了)
                if (Header1Val.Text != "") headerDict.Add(Header1.Text, Header1Val.Text);
                if (Header2Val.Text != "") headerDict.Add(Header2.Text, Header2Val.Text);
                if (Header3Val.Text != "") headerDict.Add(Header3.Text, Header3Val.Text);
                if (Header4Val.Text != "") headerDict.Add(Header4.Text, Header4Val.Text);
                
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.ToString();
            }
            return headerDict;

        }
    }

}
