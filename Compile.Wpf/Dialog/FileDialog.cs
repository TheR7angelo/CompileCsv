using Microsoft.Win32;

namespace Compile.Wpf.Dialog;

public abstract class FileDialog : IDialog
{
    public string ErrorMessage { get; }
    public string FilterText { get; }
    public IEnumerable<string> Extension { get; }

    private readonly OpenFileDialog _openFileDialog;
    private readonly SaveFileDialog _saveFileDialog;

    protected FileDialog(string? titleOpenFile, string? titleSaveFile, bool multiselect, IEnumerable<string> extension, string errorMessage, string filerText)
    {
        ErrorMessage = errorMessage;
        Extension = extension;
        FilterText = filerText;

        var filter = GetFilter();

        _openFileDialog = new OpenFileDialog
        {
            Title = titleOpenFile,
            Multiselect = multiselect,
            Filter = filter
        };

        _saveFileDialog = new SaveFileDialog
        {
            Title = titleSaveFile,
            Filter = filter
        };
    }

    private string GetFilter()
    {
        var lst = string.Join(';', Extension.Select(ext => $"*{ext}"));
        return $"{FilterText} ({lst})|{lst}";
    }

    public string[]? GetFile()
    {
        var result = _openFileDialog.ShowDialog();

        if (result.Equals(true))
        {
            return _openFileDialog.Multiselect ? _openFileDialog.FileNames : new[] { _openFileDialog.FileName };
        }

        return null;

    }

    public string? SaveFile()
    {
        var result = _saveFileDialog.ShowDialog();
        return result.Equals(true) ? _saveFileDialog.FileName : null;
    }
}