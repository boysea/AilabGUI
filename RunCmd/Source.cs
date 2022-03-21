using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunCmd
{
    public class Source
    {
        public string SourcePath { get; set; }
    }

    public class DisplaySource : Source
    {
        public string Id { get; set; }
    }
}