using SiemensXmlCreation;
using SiemensXmlCreation.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace CadMacro.Simulator.CommonControls.Helpers
{
    /// <summary>
    /// Specific tree node
    /// </summary>
    public class MacrosNode : BaseNotifier,IEquatable<MacrosNode>, IPreviewSource,IClone<MacrosNode>
    {
        private string _name;
        private int _selectedIndex;

        /// <summary>
        /// Name represents header
        /// </summary>
        [XmlAttribute]
        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(DisplayName));
            }
        }

        /// <summary>
        /// Represents selected index of current collection
        /// </summary>
        [XmlAttribute]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(nameof(DisplayName));
            }
        }

        /// <summary>
        /// Represents comboBox collection
        /// </summary>
        [XmlAttribute]
        public string[] ItemSource { get; set; }

        /// <summary>
        /// Represents itemSource type like comboBox should contain image path or value
        /// </summary>
        [XmlAttribute]
        public ItemSourceType ItemSourceType { get; set; } = ItemSourceType.Value;

        /// <summary>
        /// Dependent props which are ideal
        /// </summary>
        [XmlElement("NodeProps")]
        public MacrosNodeProperty[] NodeProps { get; set; }
        /// <summary>
        /// Dependent props which are ideal
        /// </summary>
        [XmlElement("CycleOutput")]
        public string CycleFormat { get; set; }

        /// <summary>
        /// Respective image for this node
        /// </summary>
        [XmlAttribute]
        public string ImageSource { get; set; }

        /// <summary>
        /// Respective gif for this node
        /// </summary>
        [XmlAttribute]
        public string GifSource { get; set; }
        
        /// <summary>
        /// Represents tooltip of element
        /// </summary>
        [XmlAttribute]
        public string ToolTip { get; set; }

        /// <summary>
        /// Represents tooltip of element
        /// </summary>
        [XmlAttribute]
        public bool CanShowHeader { get; set; } = true;


        public bool ShouldSerializeItemSourceType()
        {
            return ItemSourceType != ItemSourceType.Value;
        }
        public bool ShouldSerializeCanShowHeader()
        {
            return !CanShowHeader;
        }
        public bool Equals(MacrosNode other)
        {
            return Name == other.Name && SelectedIndex == SelectedIndex;
        }

        public MacrosNode Clone()
        {
            MacrosNode macrosNode = new MacrosNode();
            macrosNode.Name = Name;
            macrosNode.SelectedIndex = SelectedIndex;
            macrosNode.ItemSource = ItemSource;
            macrosNode.ItemSourceType = ItemSourceType;
            macrosNode.NodeProps = NodeProps?.Select(prop=>prop.Clone()).ToArray();
            macrosNode.CycleFormat = CycleFormat;
            macrosNode.ImageSource = ImageSource;
            macrosNode.GifSource = GifSource;
            macrosNode.ToolTip = ToolTip;
            macrosNode.CanShowHeader = CanShowHeader;
            return macrosNode;
        }

        [XmlIgnore]
        public string DisplayName => $"{Name} | {SelectedIndex}";
    }
}
