using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace RunCmd
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitSourceGrid();

            InitScaleOption();

            InitRunAilab();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            if (SourcePaths.Count == 0)
            {
                MessageBox.Show("请添加源文件/源文件夹！");
                return;
            }

            if (string.IsNullOrEmpty(OutDir.Text))
            {
                MessageBox.Show("请添加输出文件夹！");
                return;
            }

            IsStop = false;

            var files = new List<string>();

            foreach (var path in SourcePaths.Select(x => x.SourcePath))
            {
                GetAllFiles(path, files);
            }

            AllFiles = files;

            RunNextFile();
        }

        private void RunNextFile()
        {
            if (AllFiles.Count > 0)
            {
                var file = AllFiles.First();

                AllFiles.RemoveAll(x => x == file);

                var thread = new Thread(() => Run(file));

                thread.IsBackground = true;

                thread.Start();
            }
            else
            {
                CurrentFile.Content = "已完成";

                SwitchAllElement(true);
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            IsStop = true;

            var ps = Process.GetProcessesByName("execc");

            foreach (var p in ps)
            {
                p.Kill();
            }

            DeleteTemp();

            this.CurrentFile.Content = "已停止";

            SwitchAllElement(true);
        }

        private void StopAilab(object sender, RoutedEventArgs e)
        {
            var ps = Process.GetProcessesByName("execc");

            foreach (var p in ps)
            {
                p.Kill();
            }

            DeleteTemp();

            RunNextFile();
        }

        private void DeleteTemp()
        {
            if (Directory.Exists($"{Environment.CurrentDirectory}/temp"))
            {
                var deleteFiles = Directory.GetFiles($"{Environment.CurrentDirectory}/temp");
                foreach (var deleteFile in deleteFiles)
                {
                    File.Delete(deleteFile);
                }
                Directory.Delete($"{Environment.CurrentDirectory}/temp");
            }

            if (Directory.Exists($"{Environment.CurrentDirectory}/tmp"))
            {
                var deleteFiles = Directory.GetFiles($"{Environment.CurrentDirectory}/tmp");
                foreach (var deleteFile in deleteFiles)
                {
                    File.Delete(deleteFile);
                }
            }
        }

        private void SwitchAllElement(bool status)
        {
            Dispatcher.Invoke(() =>
            {
                this.AddFolderButton.IsEnabled = status;
                this.AddFileButton.IsEnabled = status;
                this.DeleteSourceButton.IsEnabled = status;
                this.ntType.IsEnabled = status;
                this.crfType.IsEnabled = status;
                this.halfType.IsEnabled = status;
                this.tileType.IsEnabled = status;
                this.modelsType.IsEnabled = status;
                this.OutDir.IsEnabled = status;
                this.SelectOutDirButton.IsEnabled = status;
                this.StartButton.IsEnabled = status;
                this.outputType.IsEnabled = status;
            });
        }
    }
}
