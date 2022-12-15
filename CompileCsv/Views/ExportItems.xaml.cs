using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Libs;
using Libs.Csv;

namespace CompileCsv.Views;

public partial class ExportItems
{
    internal MainView MainView { get; set; } = null!;

    public ExportItems()
    {
        InitializeComponent();
    }
    
    private async void ButtonCheckFile_OnClick(object sender, RoutedEventArgs e)
    {
        var files = await MainView.ReadFiles();
        var check = new CompileWorker(files);
        await check.CheckHeader();
        var errors = check.GetErrorHeader();

        if (errors is not null)
        {
            var data = MainView.DisplayItems.GetFiles().ToList();
            foreach (var error in errors)
            {
                var p = data.First(s => s.Path.Equals(error));
                p.IsChecked = false;
            }

            MainView.DisplayItems.ListBoxFiles.Items.Refresh();

            MainView.ExportItems.LabelNumberSelected.Content = MainView.DisplayItems.CountSelectedFile();
            var messageTxt = new List<string>()
            {
                "Des fichiers non compatible on étais détecter, ils ont étais automatiquement retirer.",
                "Voulez vous lancer quand même lancer la compilation ?"
            };

            var msg = MessageBox.Show(string.Join('\n', messageTxt),
                "Des erreurs ont étais détecter", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (msg.Equals(MessageBoxResult.No)) return;
        }

        var csvs = await check.GetCompile();

        var writer = new Writer(csvs);
        writer.Write("Je suis un test trop bien.csv");
        
    }
}