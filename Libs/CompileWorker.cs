using Libs.Type;

namespace Libs;

public class CompileWorker
{
    private List<string>? ErrorHeader { get; set; }
    
    private List<SIndexedDict> Datas { get; }

    public CompileWorker(IEnumerable<SIndexedDict> datas)
    {
        ErrorHeader = new List<string>();
        Datas = datas.OrderBy(s => s.Index).ToList();
    }

    public async Task CheckHeader()
    {
        var mainHeader = Datas.First().Dictionary.First().Keys;

        await Parallel.ForEachAsync(Datas.Skip(1), (data, _) =>
        {
            var keys = data.Dictionary.First().Keys;
            if (!mainHeader.SequenceEqual(keys)) ErrorHeader?.Add(data.FilePath);
            
            return default;
        });
    }

    public IEnumerable<string>? GetErrorHeader() => ErrorHeader.Count.Equals(0) ? null : ErrorHeader;
}