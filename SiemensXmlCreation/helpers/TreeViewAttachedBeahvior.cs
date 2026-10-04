using CadMacro.Simulator.CommonControls.AttachedBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SiemensXmlCreation.helpers
{
    public static class TreeViewAttachedBeahvior
    {
        public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.RegisterAttached(
            "SelectedItem",
            typeof(object),
            typeof(TreeViewAttachedBeahvior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public static void SetSelectedItem(DependencyObject element, object value)
            => element.SetValue(SelectedItemProperty, value);

        public static object GetSelectedItem(DependencyObject element)
            => element.GetValue(SelectedItemProperty);

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeView treeView)
            {
                treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
            }
        }

        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            SetSelectedItem(treeView, e.NewValue);
        }
    }
}
