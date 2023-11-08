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
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    public partial class DragAndDropTest : Window
    {
        public DragAndDropTest()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RedRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(RedRectangle, RedRectangle.Fill.ToString(), DragDropEffects.Move);
            }
        }

        private void BlueRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(BlueRectangle, BlueRectangle.Fill.ToString(), DragDropEffects.Move);
            }
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            ((Label)sender).Content = (string)e.Data.GetData(DataFormats.Text);
        }
    }
}
