using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunCmd
{
    public partial class MainWindow
    {
        private void SetConfig(string file)
        {
            var scale = GetScale();
            var model = GetModel();
            var mode = GetMode(file);
            var half = Gethalf();
            var tile = Gettile();
            if (mode == "image")
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/temp");
                File.Copy(file, $"{Environment.CurrentDirectory}/temp/{file.Replace("/", "").Replace("\\", "").Replace(":", "")}");
            }
            var outDir = GetOutdir();
            var nt = Getnt();
            var crf = Getcrf();

            var config = $@"
scale={scale}

model_path2=""weights_v3/{ model}.pth""
model_path3=""weights_v3/{ model}.pth""
model_path4=""weights_v3/{ model}.pth""

mode=""{ mode}""

half={half}
tile={tile}

device = ""cuda:0""

input_dir = ""{Environment.CurrentDirectory.Replace("\\", "/")}/temp""
output_dir = ""{outDir.Replace("\\", "/")}""

inp_path = ""{file.Replace("\\", "/")}""
opt_path = ""{outDir.Replace("\\","/")}/{file.Replace("/", "").Replace("\\", "").Replace(":","")}""

nt={nt}

n_gpu=1

p_sleep=(0.005,0.012)
decode_sleep=0.002

encode_params=['-crf','{crf}','-preset','medium']
";

            File.Delete($"{Environment.CurrentDirectory}/config.py");
            File.WriteAllText($"{Environment.CurrentDirectory}/config.py", config);
        }

        private string GetScale()
        {
            var model = string.Empty;
            Dispatcher.Invoke(() =>
            {
                model = this.modelsType.Text;
            });
            return model.Split('-')[0].Replace("up", "").Replace("x", "");
        }

        private string GetModel()
        {
            var model = string.Empty;
            Dispatcher.Invoke(() =>
            {
                model = this.modelsType.Text;
            });
            return model;
        }

        private string GetMode(string path)
        {
            var ext = Path.GetExtension(path).Replace(".", "").ToLower();
            if (ImgExts.Contains(ext))
            {
                return "image";
            }
            if (VideoExts.Contains(ext))
            {
                return "video";
            }
            return "";
        }

        private string Gethalf()
        {
            var half = string.Empty;
            Dispatcher.Invoke(() =>
            {
                half = this.halfType.Text;
            });
            return half;
        }

        private string Gettile()
        {
            var tile = string.Empty;
            Dispatcher.Invoke(() =>
            {
                tile = this.tileType.Text;
            });
            return tile;
        }

        private string GetOutdir()
        {
            var outDir = string.Empty;
            Dispatcher.Invoke(() =>
            {
                outDir = this.OutDir.Text;
            });
            return outDir;
        }

        private string Getnt()
        {
            var nt = string.Empty;
            Dispatcher.Invoke(() =>
            {
                nt = this.ntType.Text;
            });
            return nt;
        }

        private string Getcrf()
        {
            var crf = string.Empty;
            Dispatcher.Invoke(() =>
            {
                crf = this.crfType.Text;
            });
            return crf;
        }
    }
}
