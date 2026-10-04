using CadMacro.Simulator.CommonControls.AttachedBehaviours;
using CadMacro.Simulator.CommonControls.Helpers;
using SiemensXmlCreation.helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SiemensXmlCreation.ViewModel
{
    internal class NodePropViewModel:BaseNotifier
    {
        private MacrosNodeProperty _nodeProperty;
        private MacroTreeNode _node;

        private Visibility _headerValueVisibility;
        private Visibility _headerContentTypeVisibility;
        private Visibility _headerInputTypeVisibility;
        private Visibility _headerItemSourceVisibility;
        private Visibility _headerItemSourceTypeVisibility;
        private Visibility _footerTitleVisibility;
        private Visibility _footerValueVisibility;
        private Visibility _footerContentTypeVisibility;
        private Visibility _footerInputTypeVisibility;
        private Visibility _footerItemSourceVisibility;
        private Visibility _footerItemSourceTypeVisibility;
        private Visibility _unitVisibility;


        public Array ControlTypes { get; set; }
        public Array CanShowHeaderTypes { get; set; }
        public Array HeaderContentTypes { get; set; }
        public Array HeaderInputTypes { get; set; }
        public Array HeaderItemSourceTypes { get; set; }
        public Array FooterContentTypes { get; set; }
        public Array FooterInputTypes { get; set; }
        public Array CanShowTypes { get; set; }
        public Array FooterItemSourceTypes { get; set; }





        public Visibility HeaderValueVisibility 
        { 
            get => _headerValueVisibility;
            set => SetIfDiff(ref _headerValueVisibility, value);
        }
        public Visibility HeaderContentTypeVisibility
        {
            get => _headerContentTypeVisibility;
            set => SetIfDiff(ref _headerContentTypeVisibility, value);
        }
        public Visibility HeaderInputTypeVisibility
        {
            get => _headerInputTypeVisibility;
            set => SetIfDiff(ref _headerInputTypeVisibility, value);
        }
        public Visibility HeaderItemSourceVisibility
        {
            get => _headerItemSourceVisibility;
            set => SetIfDiff(ref _headerItemSourceVisibility, value);
        }
        public Visibility HeaderItemSourceTypeVisibility
        {
            get => _headerItemSourceTypeVisibility;
            set => SetIfDiff(ref _headerItemSourceTypeVisibility, value);
        }
        public Visibility FooterTitleVisibility
        {
            get => _footerTitleVisibility;
            set => SetIfDiff(ref _footerTitleVisibility, value);
        }
        public Visibility FooterValueVisibility
        {
            get => _footerValueVisibility;
            set => SetIfDiff(ref _footerValueVisibility, value);
        }
        public Visibility FooterContentTypeVisibility
        {
            get => _footerContentTypeVisibility;
            set => SetIfDiff(ref _footerContentTypeVisibility, value);
        }
        public Visibility FooterInputTypeVisibility
        {
            get => _footerInputTypeVisibility;
            set => SetIfDiff(ref _footerInputTypeVisibility, value);
        }
        public Visibility FooterItemSourceVisibility
        {
            get => _footerItemSourceVisibility;
            set => SetIfDiff(ref _footerItemSourceVisibility, value);
        }
        public Visibility FooterItemSourceTypeVisibility
        {
            get => _footerItemSourceTypeVisibility;
            set => SetIfDiff(ref _footerItemSourceTypeVisibility, value);
        }
        public Visibility UnitVisibility
        {
            get => _unitVisibility;
            set => SetIfDiff(ref _unitVisibility, value);
        }


        public ICommand AddCommand { get; }
        public ICommand ModifyCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand SnapCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand AddToMacrosCommand { get; }
        public ICommand LoadImageNameCommand { get; }

        public NodePropViewModel(MacroTreeNode node, MacrosNodeProperty nodeProperty=null)
        {
            _node = node;
            ControlTypes = Enum.GetValues(typeof(ControlType));
            CanShowHeaderTypes = new bool[] { true ,false};
            HeaderContentTypes = Enum.GetValues(typeof(ContentType));
            HeaderInputTypes = Enum.GetValues(typeof(TextboxBehavior.InputType));
            HeaderItemSourceTypes = Enum.GetValues(typeof(ItemSourceType));
            FooterContentTypes = Enum.GetValues(typeof(ContentType));
            FooterInputTypes = Enum.GetValues(typeof(TextboxBehavior.InputType));
            FooterItemSourceTypes = Enum.GetValues(typeof(ItemSourceType));
            CanShowTypes = new bool[] {true,false};
            _nodeProperty = nodeProperty ?? new MacrosNodeProperty();

            AddCommand = new InteractiveCommand(Add);
            RemoveCommand = new InteractiveCommand(Remove);
            ModifyCommand = new InteractiveCommand(Modify);
            SnapCommand = new InteractiveCommand(p=>_=TakeSnap());
            SettingsCommand = new InteractiveCommand(p => SnapSettingsExecute());
            AddToMacrosCommand = new InteractiveCommand(p => AddToMacrosExecute());
            LoadImageNameCommand = new InteractiveCommand(p => LoadTreeNames());

            ResetVisibility();
            SetVisibility(_nodeProperty.ControlType);
            
        }

        public string HeaderTitle
        {
            get => _nodeProperty.HeaderTitle;
            set
            {
                _nodeProperty.HeaderTitle = value;
                NotifyPropertyChanged();
            }
        }
        
        public string HeaderValue
        {
            get => _nodeProperty.HeaderValue;
            set
            {
                _nodeProperty.HeaderValue = value;
                NotifyPropertyChanged();
            }
        }
        public bool CanShowHeader
        {
            get => _nodeProperty.CanShowHeader;
            set
            {
                _nodeProperty.CanShowHeader = value;
                NotifyPropertyChanged();
            }
        }

        public ContentType HeaderContentType
        {
            get => _nodeProperty.HeaderContentType;
            set
            {
                _nodeProperty.HeaderContentType = value;
                SetHeaderDependentVisibility(value);
                NotifyPropertyChanged();
            }
        }

        public TextboxBehavior.InputType HeaderInputType
        {
            get => _nodeProperty.HeaderInputType;
            set
            {
                _nodeProperty.HeaderInputType = value;
                NotifyPropertyChanged();
            }
        }

        public string[] HeaderItemSource
        {
            get => _nodeProperty.HeaderItemSource;
            set
            {
                _nodeProperty.HeaderItemSource = value;
                NotifyPropertyChanged();
            }
        }

        public ItemSourceType HeaderItemSourceType
        {
            get => _nodeProperty.HeaderItemSourceType;
            set
            {
                _nodeProperty.HeaderItemSourceType = value;
                NotifyPropertyChanged();
            }
        }




        public string FooterTitle
        {
            get => _nodeProperty.FooterTitle;
            set
            {
                _nodeProperty.FooterTitle = value;
                NotifyPropertyChanged();
            }
        }

        public string FooterValue
        {
            get => _nodeProperty.FooterValue;
            set
            {
                _nodeProperty.FooterValue = value;
                NotifyPropertyChanged();
            }
        }

        public ContentType FooterContentType
        {
            get => _nodeProperty.FooterContentType;
            set
            {
                _nodeProperty.FooterContentType = value;
                SetFooterDependentVisibility(value);
                NotifyPropertyChanged();
            }
        }

        public TextboxBehavior.InputType FooterInputType
        {
            get => _nodeProperty.FooterInputType;
            set
            {
                _nodeProperty.FooterInputType = value;
                NotifyPropertyChanged();
            }
        }

        public string[] FooterItemSource
        {
            get => _nodeProperty.FooterItemSource;
            set
            {
                _nodeProperty.FooterItemSource = value;
                NotifyPropertyChanged();
            }
        }

        public ItemSourceType FooterItemSourceType
        {
            get => _nodeProperty.FooterItemSourceType;
            set
            {
                _nodeProperty.FooterItemSourceType = value;
                NotifyPropertyChanged();
            }
        }



        public string Unit
        {
            get => _nodeProperty.Unit;
            set
            {
                _nodeProperty.Unit = value;
                NotifyPropertyChanged();
            }
        }

        public ControlType ControlType
        {
            get => _nodeProperty.ControlType;
            set
            {
                _nodeProperty.ModifyControlType(value);
                SetVisibility(value);
                NotifyPropertyChanged(null);
            }
        }

        public string ImageSource
        {
            get => _nodeProperty.ImageSource;
            set
            {
                _nodeProperty.ImageSource = value;
                NotifyPropertyChanged();
            }
        }

        public string GifSource
        {
            get => _nodeProperty.GifSource;
            set
            {
                _nodeProperty.GifSource = value;
                NotifyPropertyChanged();
            }
        }

        public string ToolTip
        {
            get => _nodeProperty.ToolTip;
            set
            {
                _nodeProperty.ToolTip = value;
                NotifyPropertyChanged();
            }
        }



        private void SetVisibility(ControlType controlType)
        {
            ResetVisibility();
            switch (controlType)
            {
                case ControlType.Mono:
                    break;
                case ControlType.Dual:
                    HeaderContentTypeVisibility = Visibility.Visible;
                    ShowHeaderValues();
                    break;
                case ControlType.Tri:
                    HeaderValueVisibility = Visibility.Visible;
                    UnitVisibility = Visibility.Visible;
                    break;
                case ControlType.Quad:
                    HeaderContentTypeVisibility = Visibility.Visible;
                    FooterContentTypeVisibility = Visibility.Visible;
                    ShowHeaderValues();
                    ShowFooterValues();
                    break;
            }
        }
        private void ShowHeaderValues()
        {
            if (HeaderContentType is ContentType.TextBox)
            {
                HeaderValueVisibility = Visibility.Visible;
                HeaderItemSourceVisibility = Visibility.Collapsed;
            }
            else
            {
                HeaderValueVisibility = Visibility.Collapsed;
                HeaderItemSourceVisibility = Visibility.Visible;
                HeaderItemSourceTypeVisibility = Visibility.Visible;
            }
        }
        private void ShowFooterValues()
        {
            if (FooterContentType is ContentType.TextBox)
            {
                FooterTitleVisibility = Visibility.Visible;
                FooterValueVisibility = Visibility.Visible;
                FooterItemSourceVisibility = Visibility.Collapsed;
            }
            else
            {
                FooterTitleVisibility = Visibility.Visible;
                FooterValueVisibility = Visibility.Collapsed;
                FooterItemSourceVisibility = Visibility.Visible;
                FooterItemSourceTypeVisibility = Visibility.Visible;
            }
        }
        private void ResetVisibility()
        {
            HeaderContentTypeVisibility = Visibility.Collapsed;
            HeaderValueVisibility = Visibility.Collapsed;
            HeaderInputTypeVisibility = Visibility.Collapsed;
            HeaderItemSourceVisibility = Visibility.Collapsed;
            HeaderItemSourceTypeVisibility = Visibility.Collapsed;
            FooterTitleVisibility = Visibility.Collapsed;
            FooterValueVisibility = Visibility.Collapsed;
            FooterContentTypeVisibility = Visibility.Collapsed;
            FooterInputTypeVisibility = Visibility.Collapsed;
            FooterItemSourceVisibility = Visibility.Collapsed;
            FooterItemSourceTypeVisibility = Visibility.Collapsed;
            UnitVisibility = Visibility.Collapsed;
        }
        private string GetImageName(string name, MacroTreeNode node)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(name);

            stringBuilder = GetRecursiveName(stringBuilder, _node);
            return stringBuilder.ToString();
        }
        private StringBuilder GetRecursiveName(StringBuilder stringBuilder, MacroTreeNode node)
        {
            if (node.Parent is null)
                return stringBuilder;

            stringBuilder.Insert(0, '_');
            stringBuilder.Insert(0, node.CurrentValue.Name);
            return GetRecursiveName(stringBuilder, node.Parent as MacroTreeNode);
        }
        private void SetHeaderDependentVisibility(ContentType contentType)
        {
            HeaderItemSourceTypeVisibility = Visibility.Collapsed;
            HeaderItemSourceVisibility = Visibility.Collapsed;
            HeaderValueVisibility = Visibility.Collapsed;

            if (contentType is ContentType.TextBox)
            {
                HeaderValueVisibility = Visibility.Visible;
            }
            else
            {
                HeaderItemSourceTypeVisibility = Visibility.Visible;
                HeaderItemSourceVisibility = Visibility.Visible;
            }
        }
        private void SetFooterDependentVisibility(ContentType contentType)
        {
            FooterItemSourceTypeVisibility = Visibility.Collapsed;
            FooterItemSourceVisibility = Visibility.Collapsed;
            FooterValueVisibility = Visibility.Collapsed;

            if (contentType is ContentType.TextBox)
            {
                FooterTitleVisibility = Visibility.Visible;
                FooterValueVisibility = Visibility.Visible;
            }
            else
            {
                FooterTitleVisibility = Visibility.Visible;
                FooterItemSourceTypeVisibility = Visibility.Visible;
                FooterItemSourceVisibility = Visibility.Visible;
            }
        }
        private void AddToMacrosExecute()
        {
            XmlCreationViewModel.Instance.AddToMacroExecute(_nodeProperty);
        }


        //private MacrosNodeProperty GetCloneProp()
        //{
        //    MacrosNodeProperty macrosNodeProperty = new MacrosNodeProperty();
        //    macrosNodeProperty.HeaderTitle = this.HeaderTitle;
        //    macrosNodeProperty.HeaderValue = HeaderValue;
        //    macrosNodeProperty.HeaderContentType = HeaderContentType;
        //    macrosNodeProperty.HeaderInputType = HeaderInputType;
        //    macrosNodeProperty.HeaderItemSource = HeaderItemSource;
        //    macrosNodeProperty.HeaderItemSourceType = HeaderItemSourceType;
        //    macrosNodeProperty.FooterTitle = FooterTitle;

        //    macrosNodeProperty.FooterValue = FooterValue;
        //    macrosNodeProperty.FooterContentType = FooterContentType;

        //    macrosNodeProperty.FooterInputType = FooterInputType;
        //    macrosNodeProperty.FooterItemSource = FooterItemSource;
        //    macrosNodeProperty.FooterItemSourceType = FooterItemSourceType;
        //    macrosNodeProperty.Unit = Unit;
        //    macrosNodeProperty.ControlType = ControlType;
        //    macrosNodeProperty.ImageSource = ImageSource;
        //    macrosNodeProperty.GifSource = GifSource;

        //    macrosNodeProperty.ToolTip = ToolTip;
        //    return macrosNodeProperty;
        //}
        private void Add(object sender)
        {
            if (_node.CurrentValue.NodeProps is null)
                _node.CurrentValue.NodeProps = new MacrosNodeProperty[] { };

            MacrosNodeProperty prop = _nodeProperty.Clone();
            _node.CurrentValue.NodeProps = _node.CurrentValue.NodeProps.Append(prop).ToArray();
            XmlCreationViewModel.Instance.UpdateAndShowProps(_node);
        }
        private void Remove(object sender)
        {
            _node.CurrentValue.NodeProps = _node.CurrentValue.NodeProps.Where(prop=>prop != _nodeProperty).ToArray();
            XmlCreationViewModel.Instance.UpdateAndShowProps(_node);
        }
        private void Modify(object sender)
        {
            XmlCreationViewModel.Instance.UpdateAndShowProps(_node);
        }
        private async Task TakeSnap()
        {
            if (string.IsNullOrEmpty(ImageSource))
            {
                MessageBox.Show("fill image name");
                return;
            }
            else if(ImageSource.EndsWith(".png"))
            {
                MessageBox.Show("Give new name for new image using |");
                return;
            }

            string[] images = ImageSource.Split('|');
            if (string.IsNullOrEmpty(images.Last()))
            {
                MessageBox.Show("fill image name after '|'");
                return;
            }
            bool value = await XmlCreationViewModel.Instance.TakeSnap(images.Last());
            if (!value)
                return;
            ImageSource = ImageSource + ".png";
        }
        private void SnapSettingsExecute()
        {
            XmlCreationViewModel.Instance.SetSnapPoints();
        }
        private void LoadTreeNames()
        {
            ImageSource = Utils.GetParentImageNames(_nodeProperty.HeaderTitle, _node);
            NotifyPropertyChanged(nameof(ImageSource));
        }
    }
}
