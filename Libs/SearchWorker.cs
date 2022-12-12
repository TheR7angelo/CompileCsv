namespace Libs;

public class SearchWorker
{
    private const string Pattern = "*.csv";
    
    private string FileSeach { get; }
    private bool SubDirectory { get; }
    
    private List<string> ListOfResult { get; }

    public SearchWorker(string fileSeach, bool subDirectory)
    {
        FileSeach = fileSeach;
        SubDirectory = subDirectory;

        ListOfResult = new List<string>();
    }

    public List<string> GetResults() => ListOfResult;

    public async Task FindAll(string? path=null)
    {
        path ??= FileSeach;

        var files = Directory.EnumerateFiles(path, Pattern, SearchOption.TopDirectoryOnly);
        ListOfResult.AddRange(files);

        if (!SubDirectory) return;

        var dirs = Directory.EnumerateDirectories(path);

        await Parallel.ForEachAsync(dirs, async (dir, token) =>
        {
            await Task.Run(async () => await FindAll(dir), token);
        });
    }
}