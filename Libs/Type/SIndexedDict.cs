namespace Libs.Type;

public struct SIndexedDict
{
    public int Index { get; set; }
    public IEnumerable<Dictionary<string, string>> Dictionary { get; set; }
    public string FilePath { get; set; }
}