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
        private readonly string m_JsonPath = AppDomain.CurrentDomain.BaseDirectory + "setting.json";

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
            if (m_ReadSettings == null)
            {
                String settingJson = @"{
                    ""FontColor"": ""#AEF0FFFF"",
                    ""Background"": ""#17000000"",
                    ""FontSize"": 16.958963282937358,
                    ""FontFamily"": ""Arial"",
                    ""FontStretch"": ""Normal"",
                    ""FontStyle"": ""Normal"",
                    ""FontWeight"": ""Normal"",
                    ""FontOpacity"": 0.68624190064794821,
                    ""BackgroundOpacity"": 0.093110151187905,
                    ""SelectedBackground"": ""#FF000000"",
                    ""SelectedFontColor"": ""#FFF0FFFF"",
                    ""WindowWidth"": 638.0,
                    ""WindowHeight"": 522.0
                }";
                m_ReadSettings = JsonHelper.DeserializeJsonToObject<SettingsModel>(settingJson);
            }
        }

        public void SaveSettings()
        {
            JsonHelper.SaveToFile(this.m_ReadSettings, this.m_JsonPath);
        }
    }
}
