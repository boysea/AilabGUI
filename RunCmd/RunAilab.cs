using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace RunCmd
{
    public partial class MainWindow
    {
        private bool IsStop = true;
        private readonly List<string> ImgExts = new List<string> { "png", "jpg", "jpeg", "bmp" };
        private readonly List<string> VideoExts = new List<string> { "mp4", "mkv", "avi", "mov" };
        private List<string> AllFiles = new List<string>();

        private void InitRunAilab()
        {
            if (!File.Exists($"{Environment.CurrentDirectory}/config.py.back"))
            {
                File.Copy($"{Environment.CurrentDirectory}/config.py", $"{Environment.CurrentDirectory}/config.py.back");
            }
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

        private void Run(string file)
        {
            SwitchAllElement(false);

            if (!IsStop)
            {
                Dispatcher.Invoke(() =>
                {
                    this.CurrentFile.Content = file;
                });

                SetConfig(file);

                RunCmd();
            }

            SwitchAllElement(true);
        }

        private void GetAllFiles(string path, List<string> files)
        {
            if (File.Exists(path))
            {
                var ext = Path.GetExtension(path).Replace(".", "");
                if (ImgExts.Contains(ext) || VideoExts.Contains(ext))
                {
                    files.Add(path);
                }
                return;
            }

            foreach (var pathFiles in Directory.GetFiles(path))
            {
                var ext = Path.GetExtension(pathFiles).Replace(".", "");
                if (ImgExts.Contains(ext) || VideoExts.Contains(ext))
                {
                    files.Add(pathFiles);
                }
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                GetAllFiles(directory, files);
            }
        }

        private void RunCmd()
        {
            var proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            var output = string.Empty;
            Dispatcher.Invoke(() =>
            {
                output = this.outputType.Text;
            });
            startInfo.CreateNoWindow = output == "不显示";
            startInfo.FileName = "cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            proc.StartInfo = startInfo;
            proc.Start();
            proc.StandardInput.WriteLine($"packages110\\execc.exe");

            var thread = new Thread(() => WatchThread());

            thread.IsBackground = true;

            thread.Start();

            proc.WaitForExit();
        }

        private void WatchThread()
        {
            PerformanceCounter pp = new PerformanceCounter();
            pp.CategoryName = "Process";
            pp.CounterName = "% Processor Time";
            pp.InstanceName = "execc";
            pp.MachineName = ".";
            var i = 0;
            var j = 0;
            while (true)
            {
                try
                {
                    var rate = pp.NextValue();
                    if (rate == 0)
                    {
                        i++;
                    }
                    else
                    {
                        i = 0;
                        j = 0;
                    }
                    if (i == 60)
                    {
                        break;
                    }
                }
                catch
                {
                    j++;
                    if (j == 60)
                    {
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                this.StopAilabButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            });
        }
    }
}
