using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ROKAccountManager
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        MessageBoxResult resultValue = MessageBoxResult.Cancel;

        public MessageWindow()
        {
            InitializeComponent();
        }

        void Addbutton(string text, MessageBoxResult result)
        {
            var btn = new Button();
            btn.DataContext = result;
            btn.Click += Result_Click;
            btn.Content = text;
            btn.BorderThickness = new Thickness(0, 0, 0, 0);
            btn.Background = Brushes.Transparent;
            btn.FontWeight = FontWeights.Bold;
            btn.FontFamily = this.FontFamily;
            btn.FontSize = 22;
            _buttons.Children.Add(btn);
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            resultValue = (MessageBoxResult)(sender as Button).DataContext;
            this.Close();
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            var wnd = new MessageWindow();
            wnd.Owner = owner;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.Title = caption;
            wnd.Text.Text = messageBoxText;
            if (button == MessageBoxButton.OK)
            {
                wnd.Addbutton("OK", MessageBoxResult.OK);
                wnd.resultValue = MessageBoxResult.OK;
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                wnd.Addbutton("OK", MessageBoxResult.OK);
                wnd.Addbutton("Cancel", MessageBoxResult.Cancel);
                wnd.resultValue = MessageBoxResult.Cancel;
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                wnd.Addbutton("Yes", MessageBoxResult.Yes);
                wnd.Addbutton("No", MessageBoxResult.No);
                wnd.Addbutton("Cancel", MessageBoxResult.Cancel);
                wnd.resultValue = MessageBoxResult.Cancel;
            }
            else if (button == MessageBoxButton.YesNo)
            {
                wnd.Addbutton("Yes", MessageBoxResult.Yes);
                wnd.Addbutton("No", MessageBoxResult.No);
                wnd.resultValue = MessageBoxResult.No;
            }
            wnd.ShowDialog();
            return wnd.resultValue;
        }

    }
}
