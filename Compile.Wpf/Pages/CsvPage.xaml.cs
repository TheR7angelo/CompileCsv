using System.IO;
using Compile.IO.Csv;
using Compile.IO.Excel;
using Compile.Wpf.Dialog;
using Compile.Wpf.UserControls;

namespace Compile.Wpf.Pages;

public partial class CsvPage
{
    public Type FolderDialog { get; } = typeof(FolderDialog);

    public CsvPage()
    {
        InitializeComponent();
    }

    private async void Input_OnButtonPressed(object? sender, EventArgs e)
    {
        if (sender is not Input input) return;
        var directory = input.TextBoxText;

        if (string.IsNullOrEmpty(directory)) return;

        input.SetEnable(false);

        var compile = await Task.Run(() =>
        {
            var files = Directory.GetFiles(directory, "*.csv");
            return files.CompileCsvFiles();
        });

        var dialog = new CsvDialog();
        var fileToSave = dialog.SaveFile();

        if (string.IsNullOrEmpty(fileToSave))
        {
            input.SetEnable(true);
            return;
        }

        await Task.Run(() =>
        {
            compile.WriteCsv(fileToSave);
            if (compile.CsvCompileErrors is null || compile.CsvCompileErrors.Count.Equals(0)) return;
            compile.CsvCompileErrors.WriteError(fileToSave);
        });

        input.SetEnable(true);
    }
}