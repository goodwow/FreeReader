using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace FreeReader.Utils
{
    public class Tool
    {
        /// <summary>
        /// Text文件追加
        /// </summary>
        /// <param name="text">追加内容</param>
        /// <returns>返回追加结果</returns>
        public static bool TextAdditional(string text)
        {
            FileStream fs = null;
            string filePath = AppDomain.CurrentDomain.BaseDirectory+"MsgLog.txt";
            // 将待写的入数据从字符串转换为字节数组  
            Encoding encoder = Encoding.UTF8;
            byte[] bytes = encoder.GetBytes(DateTime.Now.ToString("G")+":"+text + "\r\n");
            try
            {
                fs = File.OpenWrite(filePath);
                // 设定书写的开始位置为文件的末尾  
                fs.Position = fs.Length;
                // 将待写入内容追加到文件末尾  
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                fs.Dispose();
                return true;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// 获取系统中文字体
        /// </summary>
        /// <returns>返回字体列表</returns>
        public static List<string> GetTypeface()
        {
            List<string> Typeface = new List<string>();
            foreach (FontFamily fontfamily in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontdics = fontfamily.FamilyNames;

                //判断该字体是不是中文字体   英文字体为en-us
                if (fontdics.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    string fontfamilyname = null;
                    if (fontdics.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out fontfamilyname))
                    {
                        Typeface.Add(fontfamilyname);
                    }
                }
            }
            return Typeface;
        }


        /// <summary>
        /// 正则查找
        /// </summary>
        /// <param name="reString">正文</param>
        /// <param name="regexCode">正则表达式</param>
        /// <returns>返回结果(单条数据)</returns>
        public static string GetRegexStr(string reString, string regexCode)
        {
            try
            {
                System.Text.RegularExpressions.Regex reg;//正则表达式变量
                reg = new System.Text.RegularExpressions.Regex(regexCode);//初始化正则对象

                System.Text.RegularExpressions.MatchCollection mc = reg.Matches(reString);//匹配;
                string temp = string.Empty;//声明一个临时变量
                for (int ic = 0; ic < mc.Count; ic++)
                {
                    System.Text.RegularExpressions.GroupCollection gc = mc[ic].Groups;//获取所有分组
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    for (int i = 0; i < gc.Count; i++)
                    {
                        temp = gc[i].Value;  //得到组对应数据
                    }
                }
                return temp;
            }
            catch
            {
                TextAdditional("正则查找失败");
                return string.Empty;
            }

        }
    }
}
