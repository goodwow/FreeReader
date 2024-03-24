using System;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace FreeReader
{
    public partial class App : Application
    {
        private NotifyIcon m_trayIcon;

        public void AppStartup(object sender, StartupEventArgs args)
        {
            SettingsManager.Instance.LoadSettings();
            BooksManager.Instance.LoadJsonFile();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            AddTrayIcon();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SettingsManager.Instance.SaveSettings();
            BooksManager.Instance.SaveToFile();
            RemoveTrayIcon();

            base.OnExit(e);
        }

        /// <summary>
        /// 添加基础信息
        /// </summary>
        private void AddTrayIcon()
        {
            if (m_trayIcon != null)
            {
                return;
            }
            m_trayIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Images/fav.ico", UriKind.RelativeOrAbsolute)).Stream),
                Text = "FreeReader"
            };
            m_trayIcon.Click += NotifyIcon_Click;
            m_trayIcon.Visible = true;


            #region 添加右键菜单内容

            //实例化右键菜单
            ContextMenu menu = new ContextMenu();

            //添加菜单的内容
            MenuItem cancelItem = new MenuItem();
            cancelItem.Text = "退出";
            cancelItem.Click += (sender, e) =>
            {
                this.Shutdown();
            };
            menu.MenuItems.Add(cancelItem);

            m_trayIcon.ContextMenu = menu;//设置右键弹出菜单          

            #endregion
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.MainWindow.Activate();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        private void RemoveTrayIcon()
        {
            if (m_trayIcon != null)
            {
                m_trayIcon.Visible = false;
                m_trayIcon.Dispose();//释放资源
                m_trayIcon = null;
            }
        }
    }
}
