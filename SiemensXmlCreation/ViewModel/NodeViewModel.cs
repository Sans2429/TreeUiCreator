using CadMacro.Simulator.CommonControls.Helpers;
using CadMacro.Simulator.Utilities;
using SiemensXmlCreation.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace SiemensXmlCreation.ViewModel
{
    internal class NodeViewModel:BaseNotifier
    {
        private MacrosNode macrosNode;
        private MacroTreeNode _node;

        public Array ItemSourceTypes { get; }
        public Array CanShowTypes { get; }
        public ICommand AddCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand SnapCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand LoadImageNameCommand { get; set; }
        public NodeViewModel(MacroTreeNode node, MacrosNode macroNode=null)
        {
            _node = node;
            ItemSourceTypes = Enum.GetValues(typeof(ItemSourceType));
            CanShowTypes = new bool[] { true, false };
            macrosNode = macroNode ?? new MacrosNode();
            AddCommand = new InteractiveCommand(p=>Add());
            ModifyCommand = new InteractiveCommand(p=>Modify());
            SnapCommand = new InteractiveCommand(p => _ = TakeSnap());
            SettingsCommand = new InteractiveCommand(p => SnapSettingsExecute());
            LoadImageNameCommand = new InteractiveCommand(p => LoadTreeNames());
        }
        public string Name 
        {
            get => macrosNode.Name;
            set
            {
                macrosNode.Name = value;
                NotifyPropertyChanged();
            }
        }
        public int SelectedIndex
        { 
            get=>macrosNode.SelectedIndex;
            set=>macrosNode.SelectedIndex = value;
        }
        public string[] ItemSource
        {
            get => macrosNode.ItemSource;
            set => macrosNode.ItemSource = value;
        }
        public ItemSourceType ItemSourceType
        {
            get => macrosNode.ItemSourceType;
            set => macrosNode.ItemSourceType = value;
        }
        public List<MacrosNodeProperty> NodeProps
        {
            get => macrosNode.NodeProps?.ToList();
            set => macrosNode.NodeProps = value?.ToArray();
        }
        public string CycleFormat
        {
            get => macrosNode.CycleFormat;
            set => macrosNode.CycleFormat = value;
        }
        public bool CanShowHeader
        {
            get => macrosNode.CanShowHeader;
            set => macrosNode.CanShowHeader = value;
        }
        public string ImageSource
        {
            get => macrosNode.ImageSource;
            set => macrosNode.ImageSource = value;
        }
        public string GifSource
        {
            get => macrosNode.GifSource;
            set => macrosNode.GifSource = value;
        }
        public string ToolTip
        {
            get => macrosNode.ToolTip;
            set => macrosNode.ToolTip = value;
        }
        private void Modify()
        {
            _node.CurrentValue = macrosNode;
            _node.RefreshNode();
        }
        //private MacrosNode GetCloneMacroNode()
        //{
        //    MacrosNode macrosNode = new MacrosNode();
        //    macrosNode.Name = Name;
        //    macrosNode.SelectedIndex = SelectedIndex;
        //    macrosNode.ItemSource = ItemSource;
        //    macrosNode.ItemSourceType = ItemSourceType;
        //    macrosNode.NodeProps = macrosNode.NodeProps;
        //    macrosNode.CycleFormat = CycleFormat;
        //    macrosNode.ImageSource = ImageSource;
        //    macrosNode.GifSource= GifSource;
        //    macrosNode.ToolTip = ToolTip;
        //    return macrosNode;
        //}
        private void Add()
        {
            MacrosNode macrosNode = this.macrosNode.Clone();
            _node.AddChild(new MacroTreeNode(macrosNode));
        }
        private async Task TakeSnap()
        {
            if(string.IsNullOrEmpty(ImageSource))
            {
                MessageBox.Show("fill image name");
                return;
            }
            else if (ImageSource.EndsWith(".png"))
            {
                MessageBox.Show("Give new name for new image using |");
                return;
            }

            string[] images = ImageSource.Split('|');
            if(string.IsNullOrEmpty(images.Last()))
            {
                MessageBox.Show("fill image name after '|'");
                return;
            }
            bool value =await XmlCreationViewModel.Instance.TakeSnap(images.Last());            
            if (!value)
                return;
            ImageSource = ImageSource+".png";
            NotifyPropertyChanged(nameof(ImageSource));
        }
        private void SnapSettingsExecute()
        {
            XmlCreationViewModel.Instance.SetSnapPoints();
        }
        private void LoadTreeNames()
        {
            string currentName = Name + SelectedIndex;
            ImageSource = Utils.GetParentImageNames(currentName, _node.Parent as MacroTreeNode);
            NotifyPropertyChanged(nameof(ImageSource));
        }
    }
}
