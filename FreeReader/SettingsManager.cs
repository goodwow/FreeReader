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
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "config.json";
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.Default))
                    {
                        string configJson = sr.ReadToEnd().Replace("\r", "").Replace("\n", "");
                        m_ReadSettings = JsonConvert.DeserializeObject<SettingsModel>(configJson);
                    }
                }
                else
                {
                    m_ReadSettings = new SettingsModel();
                }
            }
            catch (Exception e)
            {
                m_ReadSettings = new SettingsModel();
            }
        }

        public void SaveSettings()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "config.json";
            if (!System.IO.File.Exists(path))
            {
                FileStream stream = System.IO.File.Create(path);
                stream.Close();
                stream.Dispose();
            }
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                string configJson = JsonConvert.SerializeObject(m_ReadSettings);
                writer.Write(configJson);
            }
        }
    }
}
