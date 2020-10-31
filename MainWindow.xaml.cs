using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace WinShutdown
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double seconds = 0;
        TimeSpan t = new TimeSpan();
        DateTime shutdown = new DateTime();
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            
            timer.Interval = new TimeSpan(1000);
            timer.Tick += Timer_Tick;

            DateTime curTime = DateTime.Now;

            sp_info.Visibility = Visibility.Hidden;

            for (int i = 0; i < 60; i++)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = i.ToString();
                cb_minuten.Items.Add(ci);
            }
            cb_minuten.SelectedIndex = 0;

            for (int i = 0; i < 24; i++)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = i.ToString();
                cb_stunden.Items.Add(ci);
            }
            cb_stunden.SelectedIndex = 0;

            timepicker.Value = curTime;
        }

        

        private void rb_minutes_Click(object sender, RoutedEventArgs e)
        {
            sp_minutes.IsEnabled = true;
            rb_minutes.IsChecked = true;

            sp_time.IsEnabled = false;
            rb_time.IsChecked = false;
        }

        private void rb_time_Click(object sender, RoutedEventArgs e)
        {
            sp_time.IsEnabled = true;
            rb_time.IsChecked = true;

            sp_minutes.IsEnabled = false;
            rb_minutes.IsChecked = false;
        }

        private void btn_push_Click(object sender, RoutedEventArgs e)
        {
            
            
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            if ((bool)rb_time.IsChecked == true)
            {
                t = new TimeSpan();
                t = (DateTime)timepicker.Value - DateTime.Now;

                seconds = Math.Round(t.TotalSeconds, 0);

                if (seconds >= 60)
                {
                    btn_push.IsEnabled = false;
                    btn_reset.IsEnabled = true;

                    startInfo.Arguments = "/C shutdown -s -t " + seconds.ToString();
                }

                shutdown = DateTime.Now + t;
                
            }
            else
            {
                double seconds1 = cb_minuten.SelectedIndex * 60;
                double seconds2 = cb_stunden.SelectedIndex * 60 * 60;

                seconds = seconds1 + seconds2;

                t = TimeSpan.FromSeconds(seconds);
                

                if (seconds >= 60)
                {
                    btn_push.IsEnabled = false;
                    btn_reset.IsEnabled = true;

                    startInfo.Arguments = "/C shutdown -s -t " + seconds.ToString();
                }

                shutdown = DateTime.Now + t;
            }

            process.StartInfo = startInfo;
            process.Start();

            sp_info.Visibility = Visibility.Visible;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = shutdown - DateTime.Now;

            tb_time.Content = ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            btn_push.IsEnabled = true;
            btn_reset.IsEnabled = false;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -a";
            process.StartInfo = startInfo;
            
            process.Start();

            Process[] chromes = Process.GetProcessesByName("*");
            Console.WriteLine("{0} chrome processes", chromes.Length);

            sp_info.Visibility = Visibility.Hidden;
            timer.Stop();
        }

        
    }
}
