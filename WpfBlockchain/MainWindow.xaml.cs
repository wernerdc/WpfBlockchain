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
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = BlkChain.Chain;
        }

        private void btnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
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
                string fullPath = cbPath.Text + "\\blockchain.json";
                JsonSerializerOptions jsOptions = new() { WriteIndented = true };
                string json = JsonSerializer.Serialize<BlockChain>(BlkChain, jsOptions);
                File.WriteAllText(fullPath, json);
                
                lbxLog.Items.Insert(0, new ListBoxItem { 
                        Content = "Save chain [OK]: " + fullPath, 
                        Foreground = Brushes.SeaGreen });
            }
            catch (Exception ex)
            {
                lbxLog.Items.Insert(0, new ListBoxItem {
                        Content = "Save chain [FAIL]: " + ex,
                        Foreground = Brushes.Firebrick });
                Debug.WriteLine(ex);
            }
        }

        private void btnChainLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullPath = cbPath.Text + "\\blockchain.json";
                string json = File.ReadAllText(fullPath);
                dataGrid1.ItemsSource = null;
                BlkChain = JsonSerializer.Deserialize<BlockChain>(json) ?? BlkChain;        // ?? inline null check
                dataGrid1.ItemsSource = BlkChain.Chain;
                
                lbxLog.Items.Insert(0, new ListBoxItem {
                        Content = "Load chain [OK]: " + fullPath,
                        Foreground = Brushes.SeaGreen });
            }
            catch (Exception ex)
            {
                lbxLog.Items.Insert(0, new ListBoxItem {
                        Content = "Load chain [FAIL]: " + ex,
                        Foreground = Brushes.Firebrick });
                Debug.WriteLine(ex);
            }
        }

        private void btnChainCheck_Click(object sender, RoutedEventArgs e)
        {
            int validationState = BlkChain.ValidateChain();
            if (validationState == -1)
            {
                lbxLog.Items.Insert(0, new ListBoxItem {
                        Content = "ChainValidation [OK]: Die Blockchain enthält keine Fehler.",
                        Foreground = Brushes.SeaGreen });
            } 
            else
            {
                lbxLog.Items.Insert(0, new ListBoxItem {
                        Content = "ChainValidation [FAIL]: Fehler bei Index" + validationState,
                        Foreground = Brushes.Firebrick });
            }
        }
    }
}