namespace Libs.Csv;

public class Writer
{
    private IEnumerable<Dictionary<string, string>> Datas { get; }

    public Writer(IEnumerable<Dictionary<string, string>> datas)
    {
        Datas = datas;
    }

    public void Write(string filePath, string separator=";")
    {
        using var file = new StreamWriter(filePath);
        
        var keys = Datas.First().Keys;

        var header = string.Join(separator, keys);
        file.WriteLine(header);

        foreach (var data in Datas)
        {
            var values = string.Join(separator, data.Values);
            file.WriteLine(values);
        }
        file.Flush();
        file.Close();
    }
}