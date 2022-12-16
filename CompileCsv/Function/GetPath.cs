using Ookii.Dialogs.Wpf;

namespace CompileCsv.Function;

public static class GetPath
{
    public static string GetSavePath()
    {
        var dialog = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
        dialog.ShowDialog();

        return dialog.SelectedPath;
    }
}