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
    public class BooksManager
    {
        private readonly string m_JsonPath = AppDomain.CurrentDomain.BaseDirectory + "config/books.json";

        private static BooksManager m_Instance;
        private List<Book> m_BookList;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static BooksManager Instance
        {
            get { return m_Instance ?? (m_Instance = new BooksManager()); }
        }

        public List<Book> BookList
        {
            get
            {
                return m_BookList;
            }
        }

        private BooksManager()
        {

        }

        public void UpdateBook(Book book)
        {
            int index = this.m_BookList.FindIndex(v => v.Id == book.Id);
            this.m_BookList[index] = book;
            this.SaveToFile();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public void LoadJsonFile()
        {
            List<Book> books = JsonHelper.LoadJsonToList<Book>(this.m_JsonPath);
            if (books == null)
            {
                this.m_BookList = new List<Book>();
            }
            else
            {
                this.m_BookList = books;
            }
        }

        public void SaveToFile()
        {
            JsonHelper.SaveToFile(this.BookList, this.m_JsonPath);
        }
    }
}
