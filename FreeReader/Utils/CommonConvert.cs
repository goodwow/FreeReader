using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace FreeReader.Utils
{
    /// <summary>
    /// Title:  通用转换类
    /// </summary>
    public class CommonConvert
    {
        /// <summary>
        /// object转换string的函数
        /// </summary>
        public static string ToString(object value)
        {
            string result = string.Empty;
            if (value is IList<object>)
            {
                foreach (object item in (value as IList<object>))
                {
                    result += Convert.ToString(item) + ",";
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Remove(result.Length - 1);
                }
            }
            else
            {
                result = Convert.ToString(value);
            }

            return result;
        }

        /// <summary>
        /// object转换int的函数
        /// </summary>
        public static int ToInteger(object value, int defaultValue = 0)
        {
            try
            {
                return (null != value) ? Convert.ToInt32(value) : defaultValue;
            }
            catch(Exception e)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// object转换long的函数
        /// </summary>
        public static long ToLong(object value, long defaultValue = 0)
        {
            try
            {
                return (null != value) ? Convert.ToInt64(value) : defaultValue;
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// object转换double的函数
        /// </summary>
        public static double ToDouble(object value, double defaultValue = 0.0)
        {
            try
            {
                return (null != value) ? Convert.ToDouble(value) : defaultValue;
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// object转换bool的函数
        /// </summary>
        public static bool ToBoolean(object value, bool defaultValue = false)
        {
            try
            {
                return (null != value) ? Convert.ToBoolean(value) : defaultValue;
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }
    }
}