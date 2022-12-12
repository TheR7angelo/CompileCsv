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

    public async Task FindAll(string? path)
    {
        path ??= FileSeach;

        var files = Directory.EnumerateFiles(path, Pattern, SearchOption.TopDirectoryOnly);
        await Parallel.ForEachAsync(files, (file, token) =>
        {
            ListOfResult.Add(file);
            return default;
        });

        if (!SubDirectory) return;

        var dirs = Directory.EnumerateDirectories(path);

        await Parallel.ForEachAsync(dirs, (dir, token) =>
        {
            Task.Run(() => FindAll(dir), token);
            return default;
        });
    }
}