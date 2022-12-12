using Libs.Type;

namespace Libs.Csv;

public class Reader
{
    private string Separator { get; }
    private IEnumerable<string> FilePath { get; }
    
    private List<SIndexedDict> ListOfResult { get; } = new();

    public Reader(string filePath, string separator=";")
    {
        FilePath = new List<string>{ filePath };
        Separator = separator;
    }
    
    public Reader(IEnumerable<string> filesPath, string separator=";")
    {
        FilePath = filesPath;
        Separator = separator;
    }

    public async Task ReadAll()
    {
        await Parallel.ForEachAsync(GetIndex(), async (file, token) =>
        {
            var lines = await GetLines(file.value);
            var header = GetHeader(lines);

            var dict = GetDict(header, lines.Skip(1));
            ListOfResult.Add(new SIndexedDict
            {
                Index = file.index,
                Dictionary = dict,
                FilePath = file.value
            });
        });
    }

    private async Task<List<List<string>>> GetLines(string filePath)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var fileReader = new StreamReader(fileStream, true);
        var str = await fileReader.ReadToEndAsync();
        var li = str.Split('\n');

        li = li.Where(s => s != string.Empty).ToArray();

        return li.Select(s => s.Split(Separator).ToList()).ToList();
    }
    
    private static IEnumerable<string> GetHeader(IReadOnlyList<List<string>> lines) => lines[0];

    private static IEnumerable<Dictionary<string, string>> GetDict(IEnumerable<string> header, IEnumerable<List<string>> lines) 
        => lines.Select(line => header.Zip(line).ToDictionary(x => x.First, x => x.Second)).ToList();

    private IEnumerable<(string value, int index)> GetIndex() 
        => FilePath.Select((value, index) => (value, index));

    public IEnumerable<SIndexedDict> GetResult() => ListOfResult;
}