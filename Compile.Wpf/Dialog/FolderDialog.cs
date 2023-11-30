using Ookii.Dialogs.Wpf;

namespace Compile.Wpf.Dialog;

public class FolderDialog : IDialog
{
    private readonly VistaFolderBrowserDialog _folderBrowser = new();

    public string ErrorMessage => string.Empty;

    public IEnumerable<string> Extension => Array.Empty<string>();

    public FolderDialog(string? titleOpenFile = null, string? titleSaveFile = null, bool multiSelect = false)
    {
        _folderBrowser.Description = titleOpenFile;
        _folderBrowser.UseDescriptionForTitle = true;
        _folderBrowser.Multiselect = multiSelect;
    }

    public string[]? GetFile()
    {
        var result = _folderBrowser.ShowDialog();
        return result.Equals(true) ? new []{ _folderBrowser.SelectedPath } : null;
    }

    public string? SaveFile()
    {
        throw new NotImplementedException();
    }
}