using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedAnything
{
    class Arguments
    {
        [Option( 's', "startup", DefaultValue=false, HelpText="Indicates Tabbed Anything is being opened from startup." )]
        public bool Startup { get; set; }
    }
}
