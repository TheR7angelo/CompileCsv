namespace Libs.Csv;

public class Writer
{
    private IEnumerable<Dictionary<string, string>> Datas { get; }

    public Writer(IEnumerable<Dictionary<string, string>> datas)
    {
        Datas = datas;
    }

    public void Write(string filePath, string separator=";", IProgress<float>? progress=null)
    {
        float maxmium = Datas.Count();
        var actual = 0;
        
        using var file = new StreamWriter(filePath);
        
        var keys = Datas.First().Keys;

        var header = string.Join(separator, keys);
        file.WriteLine(header);

        foreach (var data in Datas)
        {
            var values = string.Join(separator, data.Values);
            file.WriteLine(values);

            actual += 1;
            var value = actual / maxmium * 100;
            progress?.Report(value);
            Thread.Sleep(1);
        }
        file.Flush();
        file.Close();
    }
}