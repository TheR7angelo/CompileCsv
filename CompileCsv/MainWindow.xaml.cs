using System.Windows.Media;

namespace CompileCsv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            if (Function.GetVersion.GetUpdate())
            {
                Function.GetVersion.Update();
                GridFooter.Background = Brushes.Crimson;
            }
            else
            {
                GridFooter.Background = Brushes.ForestGreen;
            }

            LabelVersion.Content = Function.AssemblyCl.GetVersionDeploy().ToString();
        }
    }
}