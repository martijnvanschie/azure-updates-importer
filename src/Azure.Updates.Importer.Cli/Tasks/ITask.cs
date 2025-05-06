using Spectre.Console;

namespace Azure.Updates.Importer.Cli.Tasks
{
    public interface ITask
    {
        StatusContext StatusContext { get; set; }
        Task<int> RunAsync();
    }
}
