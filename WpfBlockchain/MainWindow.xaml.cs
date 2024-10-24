using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;

namespace WpfBlockchain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BlockChain BlkChain {  get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            BlkChain = new BlockChain(true);
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = BlkChain.Chain;
        }

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            Block b = new Block(DateTime.Now, $"{{{tbData.Text}}}");
            BlkChain.AddBlock(b);
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = BlkChain.Chain;
        }

        private void btnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Json Datei (.json)|*.json";
            dialog.ShowDialog();

            // from System.IO.Path
            string path = Path.GetDirectoryName(dialog.FileName) ?? "";
            cbPath.Items.Add(path);
            cbPath.SelectedIndex = cbPath.Items.IndexOf(path);
        }

        private void btnChainSave_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    string dirPath = cbPath.Text;
            //    string fullPath = dirPath + "\\blockchain.json";
            //    lbxLog.Items.Add("save chain: " + fullPath);

            //    string json = JsonSerializer.Serialize<BlockChain>(
            //        BlkChain,
            //        new JsonSerializerOptions() { WriteIndented = true });
            //    File.WriteAllText(fullPath, json);
            //}
            //catch (Exception ex)
            //{
            //    lbxLog.Items.Add("Fehler: " + ex);
            //}
        }
    }
}