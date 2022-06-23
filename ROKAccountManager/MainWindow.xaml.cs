using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string AppDataPath = Environment.ExpandEnvironmentVariables(@"%localappdata%\ROKAccountManager");


        public MainWindow()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            this.MouseDown += Window_MouseDown;
            InitializeComponent();
            LoadAccounts();
        }

        private void Account_Save(object sender, RoutedEventArgs e)
        {
            if(MessageWindow.Show(this, "Press OK After Logged In To Your Account", "Save", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            var accountName = UserInput.GetInput(this, "Account Name");
            if (accountName == null)
            {
                return;
            }

            string installDir = new string[]
            {
               Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Program Files (x86)\ROK"),
               Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Program Files\ROK"),
            }.FirstOrDefault(x => Directory.Exists(x));

            if (installDir == null)
            {
                installDir = UserInput.GetInput(this, "ROK Installtion Not Found, Enter Custom Installtion Path");
                if (installDir == null)
                {
                    return;
                }
            }

            var loginData = System.IO.Path.Combine(installDir, @"save\LoginData");
            if (!File.Exists(loginData))
            {
                MessageWindow.Show(this, "Login Info Not Found, Login And Try Again", "File Not Found", MessageBoxButton.OK);
                return;
            }

            var profile = System.IO.Path.Combine(AppDataPath, "Profiles", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(profile);
            File.WriteAllText(System.IO.Path.Combine(profile, "name"), accountName);
            File.WriteAllText(System.IO.Path.Combine(profile, "path"), installDir);
            File.Copy(loginData, System.IO.Path.Combine(profile, "LoginData"), true);
            LoadAccounts();
        }

        void LoadAccounts()
        {
            try
            {
                var profiles = new List<string>();
                foreach (var profile in Directory.GetDirectories(System.IO.Path.Combine(AppDataPath, "Profiles")))
                {
                    profiles.Add(System.IO.Path.GetFileName(profile));
                }
                profiles.Sort();

                _accounts.Items.Clear();
                foreach (var profile in profiles)
                {
                    try
                    {
                        var name = File.ReadAllText(System.IO.Path.Combine(AppDataPath, "Profiles", profile, "name"));
                        var account = new AccountItem
                        {
                            AccountName = name,
                            AccountId = profile
                        };
                        account.OnOpen += Account_OnOpen;
                        account.OnDelete += Account_OnDelete;
                        _accounts.Items.Add(account);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void Account_OnOpen(object sender, string profile)
        {
            try
            {
                var installDir = File.ReadAllText(System.IO.Path.Combine(AppDataPath, "Profiles", profile, "path"));
                var processFilename = System.IO.Path.GetFullPath(System.IO.Path.Combine(installDir, "MASS.exe"));
                var process = Process.GetProcessesByName("MASS").Where(x => System.IO.Path.GetFullPath(x.MainModule.FileName).Equals(processFilename, StringComparison.OrdinalIgnoreCase)).ToArray();
                if (process.Length > 0)
                {
                    if (MessageWindow.Show(this, "Application Is Running, Exit?", "Application Is Running", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }

                    foreach(var p in process)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception e1)
                        {
                        }
                    }
                }

                File.Copy(System.IO.Path.Combine(AppDataPath, "Profiles", profile, "LoginData"), System.IO.Path.Combine(installDir, @"save\LoginData"), true);
                var launcher = System.IO.Path.Combine(installDir, "launcher.exe");
                ShellExecute(IntPtr.Zero, "open", launcher, null, installDir, 1);
                this.WindowState = WindowState.Minimized;
            }
            catch(Exception e2)
            {
            }
        }

        private void Account_OnDelete(object sender, string e)
        {
            try
            {
                Directory.Delete(System.IO.Path.Combine(AppDataPath, "Profiles", e), true);
            }
            catch(Exception)
            {
            }
            LoadAccounts();
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString(), e.Exception.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        private void Contact_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ShellExecute(IntPtr.Zero, "open", "mailto:contact@struct.de", null, null, 1);
        }
    }
}
