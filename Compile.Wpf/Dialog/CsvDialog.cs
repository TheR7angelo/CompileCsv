namespace Compile.Wpf.Dialog;

public class CsvDialog : FileDialog
{
    private new static IEnumerable<string> Extension => new List<string>
    {
        ".csv"
    };

    private new static string ErrorMessage => "Attendu un csv";

    public CsvDialog(string? titleOpenFile = null, string? titleSaveFile = null, bool multiSelect = false) :
        base(titleOpenFile, titleSaveFile, multiSelect, Extension, ErrorMessage, "Fichier Csv")
    {
    }
}