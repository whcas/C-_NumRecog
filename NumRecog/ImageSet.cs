using System.Collections.Generic;
using System.Drawing;

namespace NumRecog
{
    public class ImageSet
    {
        List<ImageBits> images;
        public ImageSet(string fileName)
        {
            images = CSVtoImages(fileName);
        }

        public ImageBits getImage(int index)
        {
            return images[index];
        }

        public ImageBits[] getImages()
        {
            return images.ToArray();
        }

        public int getLabel(int index)
        {
            return images[index].getLabel();
        }

        public Bitmap imageBitmap(int index)
        {
            if (index < images.Count)
            {
                return images[index].toBitmap();
            }
            return null;
        }

        private List<ImageBits> CSVtoImages(string fileName)
        {
            List<List<int>> data = CSVProcess.toIntArray(fileName);

            return dataToImageList(data);
        }

        private List<ImageBits> dataToImageList(List<List<int>> data)
        {
            List<ImageBits> images = new List<ImageBits>();
            foreach (List<int> image in data)
            {
                images.Add(new ImageBits(image));
            }

            return images;
        }

        public void saveImage(int index, string name)
        {
            Bitmap bm = imageBitmap(index);

            bm.Save(@"greyscale" + getLabel(index) + "-" + name + ".png");
        }
    }
}