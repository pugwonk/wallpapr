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

namespace ReworkDesignTemp.View
{
    /// <summary>
    /// Interaction logic for LabelledImage.xaml
    /// </summary>
    public partial class LabelledImage : UserControl
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(LabelledImage));

        public LabelledImage()
        {
            InitializeComponent();
        }
    }
}
