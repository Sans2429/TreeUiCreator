using SiemensXmlCreation;
using SiemensXmlCreation.ViewModel;
using System.Xml.Serialization;
using static SiemensCycleFormCreation.Contracts.ProjectEnums;
using InputType = CadMacro.Simulator.CommonControls.AttachedBehaviours.TextboxBehavior.InputType;

namespace CadMacro.Simulator.CommonControls.Helpers
{
    /// <summary>
    /// Node's child
    /// </summary>
    public class MacrosNodeProperty:BaseNotifier, IPreviewSource,IClone<MacrosNodeProperty>
    {
        private string _headerTitle;
        [XmlAttribute]
        public string HeaderTitle 
        {
            get => _headerTitle;
            set
            {
                _headerTitle = value;
                NotifyPropertyChanged();
            }
        }
        [XmlAttribute]
        public string HeaderValue { get; set; }
        [XmlAttribute]
        public ContentType HeaderContentType { get; set; } = ContentType.TextBox;
        [XmlAttribute]
        public InputType HeaderInputType { get; set; } = InputType.Number;
        [XmlAttribute]
        public string[] HeaderItemSource { get; set; }
        [XmlAttribute]
        public ItemSourceType HeaderItemSourceType { get; set; } = ItemSourceType.Value;



        [XmlAttribute]
        public string FooterTitle { get; set; }
        [XmlAttribute]
        public string FooterValue { get; set; }
        [XmlAttribute]
        public ContentType FooterContentType { get; set; } = ContentType.TextBox;
        [XmlAttribute]
        public InputType FooterInputType { get; set; } = InputType.Number;
        [XmlAttribute]
        public string[] FooterItemSource { get; set; }
        [XmlAttribute]
        public ItemSourceType FooterItemSourceType { get; set; } = ItemSourceType.Value;


        [XmlAttribute]
        public string Unit { get; set; }
        [XmlAttribute]
        public ControlType ControlType { get; set; } = ControlType.Tri;
        [XmlAttribute]
        public string ImageSource { get; set; }
        [XmlAttribute]
        public string GifSource { get; set; }
        [XmlAttribute]
        public string ToolTip { get; set; }
        [XmlAttribute]
        public bool CanShowHeader { get; set; } = true;
        string IPreviewSource.Name => HeaderTitle;



        public bool ShouldSerializeHeaderInputType()
        {
            return HeaderInputType != InputType.Number;
        }
        public bool ShouldSerializeHeaderContentType()
        {
            return HeaderContentType != ContentType.TextBox;
        }
        public bool ShouldSerializeHeaderItemSourceType()
        {
            return HeaderItemSourceType != ItemSourceType.Value;
        }
        public bool ShouldSerializeControlType()
        {
            return ControlType != ControlType.Tri;
        }
        public bool ShouldSerializeFooterItemSourceType()
        {
            return FooterItemSourceType != ItemSourceType.Value;
        }
        public bool ShouldSerializeFooterInputType()
        {
            return FooterInputType != InputType.Number;
        }
        public bool ShouldSerializeFooterContentType()
        {
            return FooterContentType != ContentType.TextBox;
        }
        public bool ShouldSerializeCanShowHeader()
        {
            return !CanShowHeader;
        }

        public MacrosNodeProperty Clone()
        {
            MacrosNodeProperty macrosNodeProperty = new MacrosNodeProperty();
            macrosNodeProperty.HeaderTitle = HeaderTitle;
            macrosNodeProperty.HeaderValue = HeaderValue;
            macrosNodeProperty.HeaderContentType = HeaderContentType;
            macrosNodeProperty.HeaderInputType = HeaderInputType;
            macrosNodeProperty.HeaderItemSource = HeaderItemSource;
            macrosNodeProperty.HeaderItemSourceType = HeaderItemSourceType;
            macrosNodeProperty.FooterTitle = FooterTitle;

            macrosNodeProperty.FooterValue = FooterValue;
            macrosNodeProperty.FooterContentType = FooterContentType;

            macrosNodeProperty.FooterInputType = FooterInputType;
            macrosNodeProperty.FooterItemSource = FooterItemSource;
            macrosNodeProperty.FooterItemSourceType = FooterItemSourceType;
            macrosNodeProperty.Unit = Unit;
            macrosNodeProperty.ControlType = ControlType;
            macrosNodeProperty.ImageSource = ImageSource;
            macrosNodeProperty.GifSource = GifSource;

            macrosNodeProperty.ToolTip = ToolTip;
            return macrosNodeProperty;
        }
        public void ModifyControlType(ControlType controlType)
        {
            ControlType = controlType;

            HeaderTitle = default;
            HeaderValue = default;
            HeaderContentType = ContentType.TextBox;
            HeaderInputType = InputType.Number;
            HeaderItemSource = default;
            HeaderItemSourceType = ItemSourceType.Value;
            FooterTitle = default;

            FooterValue = default;
            FooterContentType = ContentType.TextBox;
            
            FooterInputType = InputType.Number;
            FooterItemSource = default;
            FooterItemSourceType = ItemSourceType.Value;
            Unit = default;
            ImageSource = default;
            GifSource = default;
            ToolTip = default;
        }
    }
}
