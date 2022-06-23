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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROKAccountManager
{
    /// <summary>
    /// Interaction logic for AccountItem.xaml
    /// </summary>
    public partial class AccountItem : UserControl
    {
        public string AccountName { get => _label.Content as string; set { _label.Content = value; } }
        public string AccountId;
        public event EventHandler<string> OnOpen;
        public event EventHandler<string> OnDelete; 

        public AccountItem()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(sender, AccountId);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OnOpen?.Invoke(sender, AccountId);
        }
    }
}
