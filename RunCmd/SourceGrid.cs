using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace RunCmd
{
    public partial class MainWindow
    {
        private ObservableCollection<DisplaySource> SourcePaths = new ObservableCollection<DisplaySource>();

        private void InitSourceGrid()
        {
            SourceGrid.ItemsSource = SourcePaths;
        }

        private void AddFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                SourcePaths.Add(new DisplaySource { Id = Guid.NewGuid().ToString(), SourcePath = folderPath });
            }
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                SourcePaths.Add(new DisplaySource { Id = Guid.NewGuid().ToString(), SourcePath = filePath });
            }
        }

        private void DeleteSource(object sender, RoutedEventArgs e)
        {
            var ids = SourceGrid.SelectedItems.Cast<DisplaySource>().Select(x => x.Id);

            if (ids == null || !ids.Any())
            {
                return;
            }

            SourcePaths.RemoveAll(x => ids.Contains(x.Id));
        }
    }
}
