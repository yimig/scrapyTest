using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using dbui.Data;
using dbui.Model;

namespace dbui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Article> articles;
        private DbConnector connector;

        public MainWindow()
        {
            InitializeComponent();
            connector=new DbConnector();
            Articles = connector.GetAllArticle();
            lvData.ItemsSource = Articles;
        }

        public List<Article> Articles
        {
            get => articles;
            set
            {
                OnPropertyChanged("Articles");
                articles = value;
            }
        }

        private void ExecuteSql()
        {
            string sql = "select * from article where " + tbSearch.Text + " order by id desc";
            Articles = connector.GetArticles(sql);
            lvData.SelectedItem = null;
            lvData.ItemsSource = Articles;
            lvData.UpdateLayout();
        }

        private void BuildFileButton(string title, string link)
        {
            var btn = new Button {Content = title, Height = 30, Margin = new Thickness(5)};
            btn.Click += (sender, args) => { System.Diagnostics.Process.Start(link); };
            spFiles.Children.Add(btn);
        }

        static async Task<string> RunPythonProcess()
        {
            string res = "";
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "C:\\Users\\zhang\\django_basic_venv\\Scripts\\python.exe";
                p.StartInfo.Arguments = @"H:/PycharmProjects/scrapyTest/mySpider/main.py";
                p.StartInfo.WorkingDirectory = @"H:/PycharmProjects/scrapyTest/mySpider";
                p.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true; //不显示程序窗口
                p.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
                {
                    // DoSth.
                    if (e.Data!=null)
                    {
                        res = $"更新了{e.Data}条数据。";
                    }
                });

                await Task.Run(() =>
                {
                    p.Start();
                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();
                    p.WaitForExit(); //等待程序执行完退出进程
                });
                p.Close();

            }

            return res;
        }

        private async void BeginRefurbish()
        {
            tbStatus.Text = "更新命令已发出";
            var res = await RunPythonProcess();
            tbStatus.Text = res;
        }

        private void LvData_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var article = lvData.SelectedItem as Article;
            if (article!=null)
            {
                tbContent.Text = article.Content;
                foreach (var filePair in article.Files)
                {
                    BuildFileButton(filePair.Key, filePair.Value);
                }
            }
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            ExecuteSql();
        }

        private void TbSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteSql();
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void MiRefurbish_OnClick(object sender, RoutedEventArgs e)
        {
            BeginRefurbish();
        }
    }
}
