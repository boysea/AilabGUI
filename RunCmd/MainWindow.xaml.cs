using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace RunCmd
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<DisplaySource> SourcePaths = new ObservableCollection<DisplaySource>();

        private List<string> nt = new List<string> { "1","2", "3", "4" };
        private List<string> crf = new List<string> { "16", "17", "18", "19", "20", "21", "22", "23", "24", };
        private List<string> half = new List<string> { "True", "False" };
        private List<string> tile = new List<string> { "0", "1", "2", "3", "4" };
        private List<string> mode = new List<string> { "video", "image" };
        private List<string> Scale = new List<string> { "2", "3", "4" };


        private List<string> models = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            //var thread = new Thread(() => RunCmd());

            //thread.IsBackground = true;

            //thread.Start();

            //var thread = new Thread(() => SetProgress());

            //thread.IsBackground = true;

            //thread.Start();

            SourceGrid.ItemsSource = SourcePaths;

            ntType.ItemsSource = nt;
            ntType.SelectedIndex = 1;
            crfType.ItemsSource = crf;
            crfType.SelectedIndex = 4;
            halfType.ItemsSource = half;
            halfType.SelectedIndex = 0;
            tileType.ItemsSource = tile;
            tileType.SelectedIndex = 0;
            modeType.ItemsSource = mode;
            modeType.SelectedIndex = 0;
            GetModels();
            modelsType.ItemsSource = models;
            modelsType.SelectedIndex = 0;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            
        }

        private void GetModels()
        {
            var path = $"{Environment.CurrentDirectory}/weights_v3";
            var files = Directory.GetFiles(path).Where(x => x.EndsWith(".pth"));
            models.AddRange(files.Select(x => Path.GetFileNameWithoutExtension(Path.GetFileName(x))));
        }

        private void SelectOutDir(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                OutDir.Text = folderPath;
            }
        }

        private void RunCmd()
        {
            var cmds = GetCmd();

            foreach (var cmd in cmds)
            {
                Run(cmd);
            }
        }

        private List<string> GetCmd()
        {
            return new List<string>
            {
                "ping www.thisismyhome.top",
                "ping www.thisismyhome.top -t"
            };
        }

        private void Run(string cmd)
        {
            //var path = Environment.CurrentDirectory;
            var proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            proc.StartInfo = startInfo;
            proc.Start();
            proc.StandardInput.WriteLine($"{cmd}&exit");

            StreamReader reader = proc.StandardOutput;
            var input = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                input = reader.ReadLine();
                Dispatcher.Invoke(new Action(() =>
                {
                    this.CmdReturn.Text += input + "\n";
                    this.CmdReturn.ScrollToEnd();
                }));
            }
            proc.WaitForExit();
            proc.Close();
        }

        private void SetProgress()
        {
            for (var i = 0; i <= 100; i++)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    this.CurrenPercent.Content = $"{i}/100";
                    this.CurrentProgress.Value = i;
                }));

                Thread.Sleep(100);
            }
        }
    }
}
