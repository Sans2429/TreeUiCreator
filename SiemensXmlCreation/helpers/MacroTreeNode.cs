using CadMacro.Simulator.CommonControls.Helpers;
using CadMacro.Simulator.Utilities;
using SiemensXmlCreation.ViewModel;
using SiemensXmlCreation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SiemensXmlCreation.helpers
{
    [XmlRoot("Tree")]
    public class MacroTreeNode : TreeNode<MacrosNode>, INotifyPropertyChanged,IClone<MacroTreeNode>
    {
        [XmlIgnore]
        public ContextMenu ContextMenu { get; set; }
        [XmlElement("ChildNodes")]
        public override ObservableCollection<TreeNode<MacrosNode>> Children { get => base.Children; set => base.Children = value; }
        public MacroTreeNode(MacrosNode nodeValue) : base(nodeValue)
        {
            ContextMenu = GetContext();
        }
        public MacroTreeNode()
        {
            ContextMenu = GetContext();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RefreshNode()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentValue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MacrosNode.DisplayName)));
        }

        private ContextMenu GetContext()
        {
            MenuItem item1 = new MenuItem();
            item1.Header = "AddNode";
            item1.Click += (s, e) => AddChild();

            MenuItem item2 = new MenuItem();
            item2.Header = "RemoveNode";
            item2.Click += (s, e) => RemoveOwn();

            MenuItem item3 = new MenuItem();
            item3.Header = "Addprops";
            item3.Click += (s, e) => AddProperties();

            MenuItem item4 = new MenuItem();
            item4.Header = "Copy";
            item4.Click += (s, e) => CopyNode();

            MenuItem item5 = new MenuItem();
            item5.Header = "Paste";
            item5.Click += (s, e) => PasteNode();

            MenuItem empty = new MenuItem();

            MenuItem item6 = new MenuItem();
            item6.Header = "AddToMacros";
            item6.Click += (s, e) => AddToMacros();

            MenuItem item7 = new MenuItem();
            item7.Header = "BindMacros";
            item7.Click += (s, e) => BindMacros();


            MenuItem[] menuItems = new MenuItem[] { item1, item2, item3 ,item4,item5, empty, item6 ,item7};
            return new ContextMenu() { ItemsSource = menuItems };
        }

        private void AddToMacros()
        {
            XmlCreationViewModel.Instance.AddToMacroExecute(this);
        }

        private void PasteNode()
        {
            XmlCreationViewModel.Instance.PasteNode(this);
        }

        private void CopyNode()
        {
            XmlCreationViewModel.Instance.CopyNode(this);
        }

        private void AddChild()
        {
            XmlCreationViewModel.Instance.ShowNodeForm(this);
        }
        private void RemoveOwn()
        {
            Parent.RemoveChild(this);
        }
        private void AddProperties()
        {
            XmlCreationViewModel.Instance.ShowPropForm(this);
        }
        private void BindMacros()
        {
            XmlCreationViewModel.Instance.BindMacrosExecute(this);
        }

        public MacroTreeNode Clone()
        {
            MacrosNode macro = CurrentValue.Clone();
            MacroTreeNode node =  new MacroTreeNode(macro);
            foreach(MacroTreeNode childNode in Children.OfType<MacroTreeNode>())
            {
                node.AddChild(childNode.Clone());
            }
            return node;
        }
        public void MakeParentRefer()
        {
            foreach(MacroTreeNode node in Children)
            {
                node.Parent=this;
                node.MakeParentRefer();
            }
        }
    }
}
