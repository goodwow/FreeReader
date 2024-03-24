using System.Windows;
using System.Windows.Controls;

namespace FreeReader
{
    public partial class GroupHeader : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(GroupHeader));


        public GroupHeader()
        {
            InitializeComponent();
        }
    }
}
