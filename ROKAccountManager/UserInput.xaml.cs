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
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        public bool HasInput;
        
        public UserInput()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            HasInput = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            HasInput = false;
            this.Close();
        }

        public static string GetInput(Window parent, string title)
        {
            var wnd = new UserInput();
            wnd.Owner = parent;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.Title = title;
            wnd.ShowDialog();
            return wnd.HasInput ? wnd.Text.Text : null;
        }
    }
}
