using System.Reflection;
using System.Windows;
using System.Windows.Media;
using AutoUpdaterWpf;
using AutoUpdaterWpf.Object.Enum;

namespace Compile.Wpf;

public partial class MainWindow
{
    private static Updater _updater = null!;
    private static string? XmlFile { get; set; }

    public MainWindow()
    {
        var asembly = Assembly.GetEntryAssembly()!;
        var applicationName = asembly.GetName().Name!;

        _updater = new Updater(@"T:\- 4 Suivi Appuis\18-Partage\BARRENTO ANTUNES Raphael\7_CSharp\Centre macro.db",
            applicationName);

        Console.WriteLine(applicationName);

        var currentVersion = asembly.GetName().Version!;

        // updater.SetAutoUpdater(AutoUpdaterParameterShowing.ReportErrors, true);
        _updater.SetAutoUpdater(EParameterShowing.ShowSkipButton, false);
        _updater.SetAutoUpdater(EParameterShowing.LetUserSelectRemindLater, true);

        XmlFile = _updater.GenerateXmlFile();
        var lastVersion = _updater.LastVersion;

        InitializeComponent();

        Title = applicationName;

        if (XmlFile is null)
        {
            Footer.Background = currentVersion < lastVersion ? Brushes.Crimson : Brushes.ForestGreen;
            LabelVersion.Content = currentVersion.ToString();
            return;
        }

        _updater.Start(XmlFile);

        Footer.Background = currentVersion < lastVersion ? Brushes.Crimson : Brushes.ForestGreen;
        LabelVersion.Content = currentVersion.ToString();
    }

    private void MenuItemUpdate_OnClick(object sender, RoutedEventArgs e)
    {
        if (XmlFile is null) return;

        _updater.Start(XmlFile);
    }

    private void MenuItemChangelog_OnClick(object sender, RoutedEventArgs e)
        => _updater.ShowChangelog();
}