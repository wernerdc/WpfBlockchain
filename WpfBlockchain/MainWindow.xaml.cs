using Microsoft.Win32;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace WpfBlockchain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _fileName = "\\blockchain.json";

        public BlockChain BlkChain {  get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            BlkChain = new BlockChain(true);
            
            // DEBUG datagrid: error that causes an empty row at the end of the dataset
            // as soon as the Block class has an empty construcor implemented
            //////////////////////////////////////////////////////////////////////////////
            //dataGrid1.ItemsSource = null;
            //dataGrid1.ItemsSource = BlkChain.Chain;
        }

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            Block b = new Block(DateTime.Now, $"{{{tbData.Text}}}");
            BlkChain.AddBlock(b);
            UpdateDataGrid();
        }

        private void btnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();
            dialog.Title = "Ordner auswählen...";
            dialog.ShowDialog();
            if (dialog.FolderName.Length == 0)
                return;
            
            string path = dialog.FolderName;
            cbPath.Items.Add(path);
            cbPath.SelectedIndex = cbPath.Items.IndexOf(path);
        }

        private void btnChainSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullPath = cbPath.Text + _fileName;
                JsonSerializerOptions jsOptions = new() { WriteIndented = true };
                string json = JsonSerializer.Serialize<BlockChain>(BlkChain, jsOptions);
                File.WriteAllText(fullPath, json);
                AddLogMessage("Save chain [OK]: " + fullPath);
            }
            catch (Exception ex)
            {
                AddLogMessage("Save chain [FAIL]: " + ex, false);
                Debug.WriteLine(ex);
            }
        }

        private void btnChainLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullPath = cbPath.Text + _fileName;
                string json = File.ReadAllText(fullPath);
                BlkChain = JsonSerializer.Deserialize<BlockChain>(json) ?? BlkChain;        // ?? inline null check
                UpdateDataGrid();
                AddLogMessage("Load chain [OK]: " + fullPath);
            }
            catch (Exception ex)
            {
                AddLogMessage("Load chain [FAIL]: " + ex, false);
                Debug.WriteLine(ex);
            }
        }

        private void btnChainCheck_Click(object sender, RoutedEventArgs e)
        {
            int validationState = BlkChain.ValidateChain();
            if (validationState == -1)
            {
                AddLogMessage("ChainValidation [OK]: Die Blockchain enthält keine Fehler.");
            } 
            else
            {
                AddLogMessage("ChainValidation [FAIL]: Fehler bei Index" + validationState, false);
            }
        }

        private void btnBlockDelete_Click(object sender, RoutedEventArgs e)
        {
            // check not null + cast as Block
            if (dataGrid1.SelectedItem is not Block b)
            {
                AddLogMessage("Delete block [FAIL]: Kein Block markiert/gefunden.", false);
                return;
            }
            BlkChain.RemoveBlock(b);
            UpdateDataGrid();
            AddLogMessage("Delete block [OK]: Block wurde entfernt und Hashes neu berechnet.");
        }

        private void UpdateDataGrid()
        {
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = BlkChain.Chain;
        }

        private void AddLogMessage(string msg, bool isOk = true) 
        { 
            ListBoxItem item = new();
            item.Content = msg;
            item.Foreground = (isOk) ? Brushes.SeaGreen : Brushes.Firebrick;
            lbxLog.Items.Insert(0, item);
        }
    }
}