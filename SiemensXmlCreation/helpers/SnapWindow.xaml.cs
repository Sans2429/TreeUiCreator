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

namespace SiemensXmlCreation.helpers
{
    /// <summary>
    /// Interaction logic for SnapWindow.xaml
    /// </summary>
    public partial class SnapWindow : Window
    {
        public SnapWindow()
        {
            InitializeComponent();
            MouseDown += SnapWindow_MouseDown;
            MouseUp += SnapWindow_MouseUp;
        }
        private Point startPoint;

        private void SnapWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //startPoint=e.GetPosition(this);
            startPoint=e.GetPosition(this);
        }

        private void SnapWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(this);

            //Point screenStart = PointToScreen(startPoint);
            //Point screenEnd = PointToScreen(endPoint);

            ScreenSnapper.Instance.SetMinPoint(startPoint);
            ScreenSnapper.Instance.SetMaxPoint(endPoint);

            this.Close();
        }
    }
}
