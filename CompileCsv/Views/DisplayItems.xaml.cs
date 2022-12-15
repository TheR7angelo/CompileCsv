using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CompileCsv.Type;

namespace CompileCsv.Views;

public partial class DisplayItems
{
    public MainView MainView { get; set; } = null!;

    public DisplayItems()
    {
        InitializeComponent();
    }

    public int CountSelectedFile() => GetFiles().Count(s => s.IsChecked);
    
    public IEnumerable<SelectedFile> GetFiles() => ListBoxFiles.Items.Cast<SelectedFile>();

    private void CheckBoxFile_OnClick(object sender, RoutedEventArgs e) =>
        MainView.LabelNumberSelected.Content = CountSelectedFile();
}