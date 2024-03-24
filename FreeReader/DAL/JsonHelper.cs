using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FreeReader.DAL
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        public static List<T> LoadJsonToList<T>(string path) where T : class
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        string configJson = sr.ReadToEnd().Replace("\r", "").Replace("\n", "");
                        return JsonHelper.DeserializeJsonToList<T>(configJson);
                    }
                }
                else
                {
                    return default(List<T>);
                }
            }
            catch (Exception e)
            {
                return default(List<T>);
            }
        }

        public static T LoadJsonToObject<T>(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string configJson = sr.ReadToEnd().Replace("\r", "").Replace("\n", "");
                        return JsonConvert.DeserializeObject<T>(configJson);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static void SaveToFile(Object configInfo, String path)
        {
            if (!System.IO.File.Exists(path))
            {
                FileStream stream = System.IO.File.Create(path);
                stream.Close();
                stream.Dispose();
            }
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                string configJson = JsonConvert.SerializeObject(configInfo);
                writer.Write(configJson);
            }
        }
    }
}
