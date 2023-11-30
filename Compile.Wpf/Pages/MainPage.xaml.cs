using System.Windows;

namespace Compile.Wpf.Pages;

public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void ButtonCsv_OnClick(object sender, RoutedEventArgs e)
        => nameof(MainWindow.FrameBody).NavigateTo(typeof(CsvPage));
}