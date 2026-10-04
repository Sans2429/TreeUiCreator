using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CadMacro.Simulator.CommonControls.AttachedBehaviours
{
    public class TextboxBehavior
    {
        public enum InputType { Text, Number }
        public static readonly DependencyProperty InputTypeProperty =
        DependencyProperty.RegisterAttached(
            "InputType",
            typeof(InputType),
            typeof(TextboxBehavior),
            new PropertyMetadata(InputType.Text, OnInputTypeChanged));

        public static InputType GetInputType(DependencyObject obj)
            => (InputType)obj.GetValue(InputTypeProperty);

        public static void SetInputType(DependencyObject obj, InputType value)
            => obj.SetValue(InputTypeProperty, value);

        private static void OnInputTypeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue is InputType inputType && inputType is InputType.Number)
                {
                    textBox.PreviewTextInput += OnPreviewTextInput;
                    DataObject.AddPastingHandler(textBox, OnPaste);
                }
                else
                {
                    textBox.PreviewTextInput -= OnPreviewTextInput;
                    DataObject.RemovePastingHandler(textBox, OnPaste);
                }
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            string fullText = GetFullText(textBox, e.Text);

            e.Handled = !double.TryParse(
                fullText,
                NumberStyles.Float,
                CultureInfo.CurrentCulture,
                out _);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var textBox = sender as TextBox;
                string pasteText = (string)e.DataObject.GetData(typeof(string));

                string fullText = GetFullText(textBox, pasteText);

                if (!double.TryParse(
                    fullText,
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out _))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static string GetFullText(TextBox textBox, string newText)
        {
            return textBox.Text.Remove(
                       textBox.SelectionStart,
                       textBox.SelectionLength)
                   .Insert(textBox.SelectionStart, newText);
        }
    }
}
