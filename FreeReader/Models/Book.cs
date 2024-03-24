using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeReader.Model
{
    public class Book
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 书名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { set; get; }

        /// <summary>
        /// 上次读到的行数
        /// </summary>
        public int LastFileLineNum { set; get; }

        /// <summary>
        /// 上次章节的行数
        /// </summary>
        public int LastFileChapter { set; get; }
    }
}
