using FreeReader.DAL;
using FreeReader.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeReader
{
    public class SettingsManager
    {
        private readonly string m_JsonPath = AppDomain.CurrentDomain.BaseDirectory + "config/setting.json";

        private static SettingsManager m_Instance;
        private SettingsModel m_ReadSettings;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static SettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new SettingsManager()); }
        }

        public SettingsModel ReadSettings
        {
            get
            {
                return m_ReadSettings;
            }
        }

        private SettingsManager()
        {

        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public void LoadSettings()
        {
            m_ReadSettings = JsonHelper.LoadJsonToObject<SettingsModel>(this.m_JsonPath);
        }

        public void SaveSettings()
        {
            JsonHelper.SaveToFile(this.m_ReadSettings, this.m_JsonPath);
        }
    }
}
