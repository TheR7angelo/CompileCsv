namespace CompileCsv.Type;

public class SelectedFile
{
    public bool IsChecked { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    
    public SelectedFile()
    {
    }
}