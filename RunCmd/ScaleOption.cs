using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RunCmd
{
    public partial class MainWindow
    {
        private List<string> nt = new List<string> { "1", "2", "3", "4" };
        private List<string> crf = new List<string> { "16", "17", "18", "19", "20", "21", "22", "23", "24", };
        private List<string> half = new List<string> { "True", "False" };
        private List<string> tile = new List<string> { "0", "1", "2", "3", "4" };
        private List<string> models = new List<string>();

        private void InitScaleOption()
        {
            ntType.ItemsSource = nt;
            ntType.SelectedIndex = 1;
            crfType.ItemsSource = crf;
            crfType.SelectedIndex = 4;
            halfType.ItemsSource = half;
            halfType.SelectedIndex = 0;
            tileType.ItemsSource = tile;
            tileType.SelectedIndex = 0;
            GetModels();
            modelsType.ItemsSource = models;
            modelsType.SelectedIndex = 0;
        }

        private void GetModels()
        {
            var path = $"{Environment.CurrentDirectory}/weights_v3";
            var files = Directory.GetFiles(path).Where(x => x.EndsWith(".pth"));
            models.AddRange(files.Select(x => Path.GetFileNameWithoutExtension(Path.GetFileName(x))));
        }
    }
}
