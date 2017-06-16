/*
 * This file is part of black desert-emu <http://necroz-project.net/>.
 *  
 * black desert-emu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * black desert-emu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with black desert-emu. If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using BlackDesertALFALauncher.contracts;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using System.IO;

namespace BlackDesertALFALauncher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /*
     * @author Karyzir
     * @modified by Maxes727
     */

    public partial class MainWindow
    {
        public static IScsServiceClient<ILauncherContract> LauncherService;
        protected string SiteUrl = "http://127.0.0.1/news.txt";
        protected WebClient NewsClient;
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                ReturnAllNews();
            }
            catch (Exception)
            {
                
                throw;
            }
            if (!File.Exists(".\\cfg.ini"))
                return;

            using (StreamReader sr = new StreamReader(".\\cfg.ini"))
            {
                string txt = sr.ReadLine();
                if (txt != null)
                {
                    string[] acc = txt.Split('@');
                    if (acc.Length > 1)
                    {
                        LoginBox.Text = acc[0];
                        PasswordBox.Password = acc[1];
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "" || PasswordBox.Password == "")
            {
                MessageBox.Show("Incorrect login or password");
                return;
            }
            try
            {

                LauncherService =
                    ScsServiceClientBuilder.CreateClient<ILauncherContract>(
                        new ScsTcpEndPoint("127.0.0.1", 6667));

                LauncherService.ConnectTimeout = 3000;
                LauncherService.Timeout = 1000;

                LauncherService.Connect();

                StartingSequence(LauncherService.ServiceProxy.RegisterClientToken(LoginBox.Text, PasswordBox.Password));


                if (RememberBox.IsChecked != null && RememberBox.IsChecked.Value)
                {
                    using (StreamWriter sw = new StreamWriter(".\\cfg.ini"))
                    {
                        sw.WriteLine("{0}@{1}", LoginBox.Text, PasswordBox.Password);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Close();
            }
        }

        private void StartingSequence(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Wrong login or password!");
                return;
            }
            string executableFile = "BlackDesert32.exe";

            if (File.Exists("BlackDesert64.exe"))
                executableFile = "BlackDesert64.exe";
           
            Process.Start(executableFile, token);
        }
        public void ReturnAllNews()
        {
            NewsClient = new WebClient();
            string content = NewsClient.DownloadString(SiteUrl);
            NewsBox.Items.Add(content);

        }

        private void NewsBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
            var item = (string)NewsBox.SelectedItem;
            if (item != null)
            {
                var url = item.Substring(item.IndexOf('@')).Replace("@", "");
                Process.Start(url);
            }
        }
    }
}
