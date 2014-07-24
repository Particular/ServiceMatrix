using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NServiceBusStudio.Automation.Dialog
{
    public sealed class CueBannerBehavior : Behavior<ComboBox>
    {
        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(CueBannerBehavior), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty ForegroundProperty =
           DependencyProperty.Register("Foreground", typeof(Brush), typeof(CueBannerBehavior), new FrameworkPropertyMetadata(Brushes.Gray));

        private readonly Brush cueBrush;

        public CueBannerBehavior()
        {
            var label = new Label();
            BindingOperations.SetBinding(label, ContentControl.ContentProperty, new Binding("Text") { Source = this });
            BindingOperations.SetBinding(label, Control.ForegroundProperty, new Binding("Foreground") { Source = this });

            cueBrush = new VisualBrush(label)
            {
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Center,
                Stretch = Stretch.None
            };
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            DetachFromTemplateControls();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, System.EventArgs e)
        {
            AttachFromTemplateControls();
        }

        private void AttachFromTemplateControls()
        {
            TextBox textBox;
            FrameworkElement contentHost;
            if (!GetTemplateControls(out textBox, out contentHost))
            {
                return;
            }

            // link background for content host
            textBox.SetValue(Control.BackgroundProperty, contentHost.GetValue(Control.BackgroundProperty));
            var binding = new Binding("Background") { RelativeSource = RelativeSource.TemplatedParent };
            BindingOperations.SetBinding(contentHost, Control.BackgroundProperty, binding);

            UpdateTextBoxBackground(textBox);
            textBox.TextChanged += TextBoxOnTextChanged;
            //textBox.IsKeyboardFocusedChanged += TextBoxOnIsKeyboardFocusedChanged;
        }

        private void DetachFromTemplateControls()
        {
            TextBox textBox;
            FrameworkElement contentHost;
            GetTemplateControls(out textBox, out contentHost);
            if (textBox != null)
            {
                textBox.TextChanged -= TextBoxOnTextChanged;
                //textBox.IsKeyboardFocusedChanged -= TextBoxOnIsKeyboardFocusedChanged;
            }
        }

        private bool GetTemplateControls(out TextBox textBox, out FrameworkElement contentHost)
        {
            if ((textBox = (TextBox)FindChild(AssociatedObject, "PART_EditableTextBox")) == null)
            {
                contentHost = null;
                return false;
            }

            if ((contentHost = FindChild(textBox, "PART_ContentHost")) == null)
            {
                return false;
            }

            return true;
        }

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            UpdateTextBoxBackground((TextBox)sender);
        }

        //private void TextBoxOnIsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        //{
        //    UpdateTextBoxBackground((TextBox)sender);
        //}

        private void UpdateTextBoxBackground(TextBox textBox)
        {
            //textBox.Background = (!textBox.IsKeyboardFocused && string.IsNullOrEmpty(textBox.Text))
            //    ? cueBrush
            //    : Brushes.Transparent;
            textBox.Background = string.IsNullOrEmpty(textBox.Text) ? cueBrush : Brushes.Transparent;
        }

        private FrameworkElement FindChild(FrameworkElement root, string partName)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i) as FrameworkElement;
                if (child != null)
                {
                    var match = (child.Name == partName) ? child : FindChild(child, partName);
                    if (match != null)
                    {
                        return match;
                    }
                }
            }

            return null;
        }
    }
}