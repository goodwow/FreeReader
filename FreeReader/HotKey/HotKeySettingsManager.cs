﻿using System.Collections.ObjectModel;

namespace FreeReader.HotKey
{
    /// <summary>
    /// 快捷键设置管理器
    /// </summary>
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager m_Instance;
        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new HotKeySettingsManager()); }
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyModel>();
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.全屏.ToString(), IsUsable = false, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Q });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.打开.ToString(), IsUsable = false, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Z });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.设置.ToString(), IsUsable = false, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Space });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.退出.ToString(), IsUsable = false, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.D });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.显示隐藏.ToString(), IsUsable = true, IsSelectCtrl = false, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.D1 });
            return hotKeyList;
        }

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

    }
}
