using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public interface ITask
    {
        public Task<int> RunAsync();
    }
}
