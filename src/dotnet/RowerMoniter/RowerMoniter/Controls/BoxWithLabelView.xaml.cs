using RowerMoniter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RowerMoniter
{
    /// <summary>
    /// Interaction logic for BoxWithLabel.xaml
    /// </summary>
    public partial class BoxWithLabel : UserControl
    {
        public static readonly DependencyProperty LableProperty = DependencyProperty.Register("Label", typeof(string), typeof(BoxWithLabel));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BoxWithLabel));

        public string Label
        {
            get { return (string)GetValue(LableProperty); }
            set { SetValue(LableProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public BoxWithLabel()
        {
        }
    }
}
