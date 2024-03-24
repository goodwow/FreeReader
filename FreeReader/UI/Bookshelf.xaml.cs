using FreeReader.Utils;
using FreeReader.DAL;
using FreeReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FreeReader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Bookshelf : UserControl
    {
        public List<Book> BookList
        {
            get
            {
                return BooksManager.Instance.BookList;
            }
        }

        public Bookshelf()
        {
            InitializeComponent();

            this.DataContext = this.BookList;
        }

        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //获取书籍ID
            String bookId = CommonConvert.ToString(((MenuItem)sender).Tag);
            Book book = this.BookList.Find(v =>
            {
                return v.Id == bookId;
            });

            if (MessageBox.Show("确定要删除《" + book.Name + "》？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                this.BookList.Remove(book);
                this.UpdateView();
            }
        }

        /// <summary>
        /// 阅读书籍
        /// </summary>
        private void Read_Click(object sender, RoutedEventArgs e)
        {
            String bookId = CommonConvert.ToString(((MenuItem)sender).Tag);
            Book book = this.BookList.Find(v =>
            {
                return v.Id == bookId;
            });
            this.ShowReadWindow(book);
        }


        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String fileName = fileDialog.SafeFileName;
                fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                if (fileName.Contains("《"))
                {
                    string tempName = Tool.GetRegexStr(fileName, "《([\\s\\S]*?)》").Trim();
                    if (!String.IsNullOrEmpty(tempName))
                    {
                        fileName = tempName;
                    }
                }
                Book book = new Book()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    FilePath = fileDialog.FileName,
                    Name = fileName
                };

                this.BookList.Add(book);
                this.UpdateView();
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image image = sender as Image;
            image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/no-cover.jpg", UriKind.RelativeOrAbsolute));
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            Book book = listBox.SelectedItem as Book;
            if (book != null)
            {
                this.ShowReadWindow(book);
            }
        }

        private void ShowReadWindow(Book book)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            ReadingBook bookReading = new ReadingBook(book);
            bookReading.Closed += (object s, EventArgs arg) =>
            {
                this.UpdateView();
                try
                {
                    mainWindow.Show();
                }
                catch (Exception)
                {
                }
            };
            bookReading.Show();
            mainWindow.Hide();
        }

        private void UpdateView()
        {
            this.DataContext = null;
            this.DataContext = this.BookList;
        }
    }
}
