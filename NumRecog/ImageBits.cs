using System.Collections.Generic;
using System.Drawing;

namespace NumRecog
{
    public class ImageBits 
    {
        List<int> pixels;
        int label;

        public ImageBits(List<int> pixels, int label)
        {
            this.pixels = pixels;
            this.label = label;
        }
        public ImageBits(List<int> data)
        {
            pixels = data.GetRange(0, (data.Count - 2));
            pixels.Add(0);

            label = data[data.Count - 1];
        }

        public int getLabel()
        {
            return label;
        }

        public List<int> getPixels()
        {
            return pixels;
        }

        public Bitmap toBitmap()
        {
            Bitmap bm = new Bitmap(28, 28);

            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    int greyscaleValue = pixels[y * 28 + x];
                    Color greyscaleColor = Color.FromArgb(  greyscaleValue,
                                                            greyscaleValue,
                                                            greyscaleValue);
                    bm.SetPixel(x, y, greyscaleColor);
                }
            }

            return bm;
        }
    }
}