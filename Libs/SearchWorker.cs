using System.Collections.Concurrent;

namespace Libs;

public class SearchWorker
{
    private const string Pattern = "*.csv";
    
    private string FileSearch { get; }
    private bool SubDirectory { get; }
    
    private ConcurrentBag<string> ListOfResult { get; }

    public SearchWorker(string fileSearch, bool subDirectory)
    {
        FileSearch = fileSearch;
        SubDirectory = subDirectory;

        ListOfResult = new ConcurrentBag<string>();
    }

    public List<string> GetResults() => ListOfResult.ToList();

    public async Task FindAll(string? path=null)
    {
        path ??= FileSearch;

        var files = Directory.EnumerateFiles(path, Pattern, SearchOption.TopDirectoryOnly);
        await Parallel.ForEachAsync(files, (file, _) =>
        {
            ListOfResult.Add(file);
            return default;
        });

        if (!SubDirectory) return;

        var dirs = Directory.EnumerateDirectories(path);

        await Parallel.ForEachAsync(dirs, async (dir, token) =>
        {
            await Task.Run(async () => await FindAll(dir), token);
        });
    }
}