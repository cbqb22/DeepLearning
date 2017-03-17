using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace DeepLearning.Common.Draw
{
    public static class BitmapConveter
    {
        //using System.Drawing;
        //using System.Drawing.Imaging;

        /// <summary>
        /// 指定された画像から1bppのイメージを作成する
        /// </summary>
        /// <param name="img">基になる画像</param>
        /// <returns>1bppに変換されたイメージ</returns>
        public static Bitmap Create1bppImage(Bitmap img)
        {
            //1bppイメージを作成する
            Bitmap newImg = new Bitmap(img.Width, img.Height,
                PixelFormat.Format1bppIndexed);

            //Bitmapをロックする
            BitmapData bmpDate = newImg.LockBits(
                new Rectangle(0, 0, newImg.Width, newImg.Height),
                ImageLockMode.WriteOnly, newImg.PixelFormat);

            //新しい画像のピクセルデータを作成する
            byte[] pixels = new byte[bmpDate.Stride * bmpDate.Height];
            for (int y = 0; y < bmpDate.Height; y++)
            {
                for (int x = 0; x < bmpDate.Width; x++)
                {
                    //ピクセルデータの位置
                    int pos = (x >> 3) + bmpDate.Stride * y;

                    //明るさが0.5以上の時は白くする
                    if (0.5f <= img.GetPixel(x, y).GetBrightness())
                    {
                        //白くする
                        pixels[pos] = 0;
                        //pixels[pos] |= (byte)(0x80 >> (x & 0x7));
                    }
                    else
                    {
                        //黒くする
                        pixels[pos] = 1;
                    }
                }
            }
            //作成したピクセルデータをコピーする
            IntPtr ptr = bmpDate.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);

            //ロックを解除する
            newImg.UnlockBits(bmpDate);

            return newImg;
        }

        // ==============
        // Imageをbmp形式でByte型配列(バイナリ)に変換
        // 第１引数: Image型画像情報
        // 戻り値: Byte型配列(バイナリ)の画像情報
        public static byte[] ConvertImageToBmpBytes(Image img)
        {
            // 入力引数の異常時のエラー処理
            if (img == null)
            {
                return null;
            }

            // 返却用バイト型配列
            byte[] ImageBytes;

            // メモリストリームの生成
            using (var ms = new System.IO.MemoryStream())
            {
                // Image画像を、bmp形式でストリームに保存
                img.Save(ms, ImageFormat.Bmp);

                // ストリームのデーターをバイト型配列に変換
                ImageBytes = ms.ToArray();

                // ストリームのクローズ
                ms.Close();
            }
            return ImageBytes;
        }


        public static double[] ByteArrayToDoubleArray(byte[] bytes)
        {
            return bytes.ToList().Select(x => (double)x).ToArray();
        }
    }
}
