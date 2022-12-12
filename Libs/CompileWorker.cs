using Libs.Type;

namespace Libs;

public class CompileWorker
{
    private List<string> ErrorHeader { get; }
    
    private List<SIndexedDict> Datas { get; }
    
    private List<Dictionary<string, string>> ListOfResult { get; set; }

    public CompileWorker(IEnumerable<SIndexedDict> datas)
    {
        ErrorHeader = new List<string>();
        Datas = datas.OrderBy(s => s.Index).ToList();

        ListOfResult = new List<Dictionary<string, string>>();
    }

    public async Task CheckHeader()
    {
        var mainHeader = Datas.First().Dictionary.First().Keys;

        await Parallel.ForEachAsync(Datas.Skip(1), (data, _) =>
        {
            var keys = data.Dictionary.First().Keys;
            if (!mainHeader.SequenceEqual(keys)) ErrorHeader.Add(data.FilePath);
            
            return default;
        });
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GetCompile()
    {
        await Compile();
        return ListOfResult;
    }

    private Task Compile()
    {
        ListOfResult.Clear();
        foreach (var data in Datas)
        {
            if (ErrorHeader.Contains(data.FilePath)) continue;
            
            foreach (var line in data.Dictionary)
            {
                ListOfResult.Add(line);
            }
        }

        return Task.CompletedTask;
    }

    public IEnumerable<string>? GetErrorHeader() => ErrorHeader.Count.Equals(0) ? null : ErrorHeader;
}