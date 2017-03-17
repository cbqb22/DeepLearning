using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DeepLearning.MNIST
{
    public class DigitImage : ViewModelBase
    {
        public int width; // 28
        public int height; // 28
        public byte[][] pixels; // 0(white) - 255(black)
        public byte label; // '0' - '9'
        public byte[] imageBytes;

        public DigitImage(int width, int height,
          byte[][] pixels, byte label)
        {
            this.width = width; this.height = height;
            this.pixels = new byte[height][];
            for (int i = 0; i < this.pixels.Length; ++i)
                this.pixels[i] = new byte[width];
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    this.pixels[i][j] = pixels[i][j];
            this.label = label;
        }


        private BitmapSource _bitmapSource;
        public BitmapSource BitmapSource { get => _bitmapSource; set { Set(ref _bitmapSource, value); } }


    }
}
