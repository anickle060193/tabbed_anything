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
        [Option( 'p', "process", Required=true, HelpText="The name of the process to capture." )]
        public String ProcessName { get; set; }

        [Option( 's', "startup", DefaultValue=false, HelpText="Indicates Tabbed Anything is being opened from startup." )]
        public bool Startup { get; set; }
    }
}
