using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Windows;
using Point = System.Windows.Point;
using Size = System.Drawing.Size;

namespace SiemensXmlCreation.helpers
{
    public class ScreenSnapper
    {
        public bool IsAtRectSelection { get;private set; }
        public static readonly ScreenSnapper Instance = new ScreenSnapper();
        public Bitmap CaptureScreen()
        {
            var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            var bitmap = new Bitmap(screenWidth, screenHeight);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            }

            return bitmap;
        }

        public Bitmap CaptureRegion(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(x, y, 0, 0, new Size(width, height));
            }

            return bitmap;
        }

        public void Save(Bitmap bitmap, string path)
        {
            bitmap.Save(path, ImageFormat.Png);
        }
        public void EnableBoundarySelection()
        {
            IsAtRectSelection = true;
        }
        public void DisableBoundarySelection()
        {
            IsAtRectSelection = true;
        }
        private Point _minPoint, _maxPoint;
        public void SetMinPoint(Point minPoint)
        {
            _minPoint  = minPoint;
        }
        public void SetMaxPoint(Point maxPoint)
        {
            _maxPoint = maxPoint;
        }
        public bool TakeRegionSnap(string path)
        {
            if(_minPoint==_maxPoint)
            {
                MessageBox.Show("Need to update snap region");
                return false;
            }
            int x = (int)Math.Min(_minPoint.X, _maxPoint.X);
            int y = (int)Math.Min(_minPoint.Y, _maxPoint.Y);
            int width = (int)Math.Abs(_minPoint.X - _maxPoint.X);
            int height = (int)Math.Abs(_minPoint.Y - _maxPoint.Y);

            Bitmap bm=CaptureRegion(x, y, width, height);
            Save(bm, path);
            return true;
        }
        public bool CanTakeSnap() => _minPoint != _maxPoint;
    }
}
