using CadMacro.Simulator.CommonControls.Helpers;
using CadMacro.Simulator.Utilities;
using SiemensCycleFormCreation;
using SiemensXmlCreation.helpers;
using SiemensXmlCreation.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
namespace SiemensXmlCreation.ViewModel
{
    internal class XmlCreationViewModel:BaseNotifier
    {
        private object _currentPanel;
        private readonly string location ;
        public string _previewImage;
        private string _currentFile;
        private object _previewForm;
        private MacroTreeNode _currentCopyNode;


        public string PreviewImage
        {
            get => _previewImage;
            set
            {
                _previewImage = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<MacrosNodeProperty> PropCollection { get; set; }
        public string MacrosText { get; set; }
        public ObservableCollection<MacroTreeNode> TreeCollection { get; }
        public static XmlCreationViewModel Instance { get; private set; }= new XmlCreationViewModel();
        public ICommand SelectedItemChangedCommand { get; private set; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand GenerateXmlCommand { get; }
        public ICommand LoadPreviewerCommand { get; }
        public ICommand LoadXmlCommand { get; }
        private MacroTreeNode _selectedItem;
        public MacroTreeNode SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        public object PreviewForm
        {
            get => _previewForm;
            set
            {
                _previewForm = value;
                NotifyPropertyChanged();
            }
        }
        public object CurrentPanel
        {
            get => _currentPanel;
            set
            {
                _currentPanel = value;
                NotifyPropertyChanged();
            }
        }
        public XmlCreationViewModel() 
        {
            location = SetInitialResourcePath()+"\\";

            SelectedItemChangedCommand = new InteractiveCommand(NodeSelectionChangedExecute);
            SelectionChangedCommand = new InteractiveCommand(PropSelectionChangedExecute);
            GenerateXmlCommand = new InteractiveCommand(p=>GenerateXml());
            LoadPreviewerCommand = new InteractiveCommand(p=>LoadPreviewer());
            LoadXmlCommand = new InteractiveCommand(p=>LoadMacrosFile());

            TreeCollection = new ObservableCollection<MacroTreeNode>();

            MacrosNode macroNode = new MacrosNode();
            macroNode.Name = "Root";

            MacroTreeNode treeNode = new MacroTreeNode(macroNode);
            TreeCollection.Add(treeNode);
        }
        private string SetInitialResourcePath()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = dialog.SelectedPath;
                    return selectedPath;
                }
            }
            MessageBox.Show("failed to select folder");
            return null;
        }
        private void NodeSelectionChangedExecute(object obj)
        {
            if (!obj.IsTypeOf(out RoutedPropertyChangedEventArgs<object> args) 
                || !args.NewValue.IsTypeOf(out MacroTreeNode marcrosNode))
                return;

            SelectedItem = marcrosNode;
            CurrentPanel = new NodeView() { DataContext = new NodeViewModel(SelectedItem, marcrosNode.CurrentValue) };
            UpdateAndShowProps(marcrosNode);

            ShowPreview(marcrosNode.CurrentValue.ImageSource);

        }

        //private ContextMenu GetContext()
        //{
        //    MenuItem item1 =new MenuItem();
        //    item1.Header = "AddNode";
        //    item1.Click+=AddNode;

        //    MenuItem item2 =new MenuItem();
        //    item2.Header = "RemoveNode";
        //    item2.Click += RemoveNode;

        //    MenuItem item3 = new MenuItem();
        //    item3.Header = "Addprops";
        //    item3.Click += AddProperties;

        //    MenuItem item4 = new MenuItem();
        //    item4.Header = "Removeprops";
        //    item4.Click += RemoveProperties;

        //    MenuItem[] menuItems = new MenuItem[] {item1,item2,item3,item4 };
        //    return new ContextMenu() { ItemsSource = menuItems };
        //}
        public void ShowNodeForm(MacroTreeNode node)
        {
            CurrentPanel = new NodeView() { DataContext = new NodeViewModel(node) };
        }
        public void ShowPropForm(MacroTreeNode node)
        {
            CurrentPanel = new NodePropView() { DataContext = new NodePropViewModel(node) };
        }
        public void UpdateAndShowProps(MacroTreeNode node)
        {
            if (node.CurrentValue.NodeProps is null || !node.CurrentValue.NodeProps.Any())
            {
                PropCollection?.Clear();
                return;
            }
            PropCollection = new ObservableCollection<MacrosNodeProperty>(node.CurrentValue.NodeProps);
            NotifyPropertyChanged(nameof(PropCollection));
        }
        public void Refresh()
        {
            NotifyPropertyChanged(nameof(TreeCollection));
        }
        private void PropSelectionChangedExecute(object sender)
        {
            if (!sender.IsTypeOf(out SelectionChangedEventArgs args) || args.AddedItems.Count<=0)
                return;

            MacrosNodeProperty prop = args.AddedItems[0] as MacrosNodeProperty ; 
            CurrentPanel = new NodePropView() { DataContext = new NodePropViewModel(SelectedItem, prop) };

            ShowPreview(prop.ImageSource);
        }
        public async Task<bool> TakeSnap(string fileName)
        {
            if(!ScreenSnapper.Instance.CanTakeSnap())
            {
                MessageBox.Show("Need to set snap region");
                return false;
            }

            Application.Current.MainWindow.WindowState = WindowState.Minimized;
            await Task.Delay(200);

            string path = location + fileName + ".png";
            if(File.Exists(path))
            {
                MessageBox.Show("This image is already exists");
                return false;
            }
            bool isTaken = ScreenSnapper.Instance.TakeRegionSnap(location+fileName+".png");

            await Task.Delay(1000);
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
            if (isTaken)
            {
                ShowPreview(path,true);
                return true;
            }
            return false;
        }
        public void SetSnapPoints()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

            SnapWindow window = new SnapWindow();
            window.ShowDialog();

            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
        }
        public void PasteNode(MacroTreeNode node)
        {
            node.AddChild(_currentCopyNode.Clone());
            _currentCopyNode = null;
        }
        public void CopyNode(MacroTreeNode node)
        {
            _currentCopyNode = node;
        }
        private void GenerateXml()
        {
            if (TreeCollection.Count != 1)
            {
                MessageBox.Show("Failed to Generate");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml";
            dialog.Title = "Save XML File";
            dialog.FileName = "data.xml";



            MacroTreeNode node = TreeCollection.FirstOrDefault();
            if (node is null)
                return;

            MacrosNode tempNode = node.CurrentValue;
            string rootName = node.CurrentValue.Name;
            bool isNotNeeded = node.CurrentValue?.NodeProps is null
                || !node.CurrentValue.NodeProps.Any();

            //replace to store desire value
            if (isNotNeeded)
            {
                node.CurrentValue = null;
            }
            else
            {
                node.CurrentValue.Name = null;
            }


            //file selection process
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var serializer = new XmlSerializer(typeof(MacroTreeNode));
            _currentFile = dialog.FileName;
            using (var writer = new StreamWriter(_currentFile))
            {
                serializer.Serialize(writer, TreeCollection.First());
            }
            ReplaceTrash(_currentFile);

            if (isNotNeeded)
                node.CurrentValue = tempNode;
            else
                node.CurrentValue.Name = rootName;
        }
        private void ShowPreview(string path,bool isFull=false)
        {
            if(string.IsNullOrEmpty(path))
            { return; }

            PreviewImage = isFull?path: location + "\\"+path;
        }
        private void ReplaceTrash(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            string xmlContent = File.ReadAllText(path);

            xmlContent = xmlContent.Replace(" xsi:type=\"MacroTreeNode\"", "");

            File.WriteAllText(path, xmlContent);
        }
        private void AppendTrash(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            string xmlContent = File.ReadAllText(path);

            xmlContent = xmlContent.Replace("<ChildNodes>", "<ChildNodes xsi:type=\"MacroTreeNode\">");

            File.WriteAllText(path, xmlContent);
        }
        private void LoadPreviewer()
        {
            if (string.IsNullOrEmpty(_currentFile) || !File.Exists(_currentFile))
            {
                MessageBox.Show("Generete before load");
                return;
            }

            PreviewForm = new PanelView(_currentFile,location+"\\");
        }
        public void AddToMacroExecute(MacroTreeNode node)
        {
            if(node is null || node.CurrentValue.Name is null)
            {
                MessageBox.Show("Invalid value");
                return;
            }
            MacrosText =$"{MacrosText}#{node.CurrentValue.Name},";
            NotifyPropertyChanged(nameof(MacrosText));
        }
        public void AddToMacroExecute(MacrosNodeProperty node)
        {
            if(node?.HeaderTitle != null)
            {
                MacrosText = $"{MacrosText}#{node.HeaderTitle},";
                NotifyPropertyChanged(nameof(MacrosText));
            }
            if(node?.FooterTitle != null)
            {
                MacrosText = $"{MacrosText}#{node.FooterTitle},";
                NotifyPropertyChanged(nameof(MacrosText));
            }
        }
        public void BindMacrosExecute(MacroTreeNode node)
        {
            if(node is null)
            {
                MessageBox.Show("Should select node");
                return;
            }
            else if (node.Children.Any())
            {
                MessageBox.Show("Should bind in leaf node");
                return;
            }
            node.CurrentValue.CycleFormat = MacrosText;
        }
        private void LoadMacrosFile()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml";
            dialog.Title = "Select XML File";

            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            AppendTrash(dialog.FileName);
            MacroTreeNode node = null;
            XmlSerializer serializer = new XmlSerializer(typeof(MacroTreeNode));

            using (var writer = new StreamReader(dialog.FileName))
            {
                node = (MacroTreeNode)serializer.Deserialize(writer);
            }

            if(node is null)
            {
                MessageBox.Show("failed to deserialize");
                return;
            }

            if (node.CurrentValue is null)
                node.CurrentValue = new MacrosNode() { Name = "Root" };
            else
                node.CurrentValue.Name = "Root";

            node.MakeParentRefer();
            TreeCollection.Clear();
            TreeCollection.Add(node);
        }
    }
    public class InteractiveCommand:ICommand
    {
        private Action<object> _action;
        public InteractiveCommand(Action<object> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
    public static class CommonExtension
    {
        public static bool IsTypeOf<T>(this object sender,out T castedObj)
        {
            castedObj = default;
            if(sender is T value)
            {
                castedObj = value;
                return true;
            }
            return false;
        }
    }
}
