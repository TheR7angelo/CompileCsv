namespace Libs.Csv;

public class Reader
{
    private string FilePath { get; }
    
    public Reader(string filePath)
    {
        FilePath = filePath;
    }
}