using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Drawing;

namespace CGC.Advent.Core.Classes
{
    public class SpaceImage
    {
        public int Width { get; private set; } = int.MinValue;
        public int Height { get; private set; } = int.MinValue;

        public int[] Data { get; private set; } = null;
        public List<int[]> Layers { get; private set; } = null;

        public SpaceImage(string imageData, int width, int height)
        {
            //< Assign the pixel width/height
            this.Width = width;
            this.Height = height;
            //< Parse the image data
            this.Data = ParseData(imageData);
            //< Get all the layers
            GetLayers();
        }

        private static int[] ParseData(string imageData)
        {
            string data = null;
            if (imageData.EndsWith(".txt"))
            {
                data = File.ReadAllText(imageData);
            }
            else
            {
                data = imageData;
            }
            return data.Select(c => int.Parse(c.ToString())).ToArray();
        }

        private void GetLayers()
        {
            //< Calculate the overall length of the layers
            int layerLen = this.Width * this.Height;
            //< Get the Layer container
            this.Layers = new List<int[]>();
            //< Iterate through all the pixels and build the layers
            int idx = 0;
            var currLayer = new List<int>();
            while (idx < this.Data.Length)
            {
                //< Add to the layer until at capacity
                if (currLayer.Count < layerLen)
                {
                    currLayer.Add(this.Data[idx]);
                }
                //< Make a new layer and add this field to it
                else
                {
                    this.Layers.Add(currLayer.ToArray());
                    currLayer = new List<int>();
                    currLayer.Add(this.Data[idx]);
                }

                idx += 1;
            }

            //< Cleanup
            if (currLayer.Count > 0)
            {
                this.Layers.Add(currLayer.ToArray());
            }
        }

        public int[] GetLayerWithMinimumNumValues(int value)
        {
            int minIdx = -1;
            int currCount = int.MaxValue;

            //< Iterate over all layers, find the layer with the fewest of a given digit
            for (int i = 0; i < this.Layers.Count; i++)
            {
                var count = this.Layers[i].Count(px => px == value);
                if (count < currCount)
                {
                    minIdx = i;
                    currCount = count;
                }
            }

            //< Return that layer
            return this.Layers[minIdx];
        }

        public static int GetNumDigits(int[] layer, int value)
        {
            return layer.Count(px => px == value);
        }

        public void GenerateFinalImage(string imageName)
        {
            var size = this.Width * this.Height;
            var imageData = new int[size];

            //< Iterate over each pixel in the final image
            for (int i = 0; i < size; i++)
            {
                imageData[i] = GetFinalPixelValue(i);
            }

            //< Generate the image
            var bmp = GenerateImage(imageData, this.Width, this.Height);
            bmp.Save(imageName, System.Drawing.Imaging.ImageFormat.Png);
        }

        private int GetFinalPixelValue(int currIdx)
        {
            //< Iterate over each layer, return the first non-transparent value
            for (int i = 0; i < this.Layers.Count; i++)
            {
                var layerValue = this.Layers[i][currIdx];
                if (layerValue != 2)
                {
                    return layerValue;
                }
            }
            //< If nothing non-transparent encountered, we're transparent mafk
            return 2;
        }

        private static Bitmap GenerateImage(int[] image, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            for (int i = 0; i < image.Length; i++)
            {
                int x = i % width; //< Just need the left-over to see where we are in X
                int y = i / width; //< Just need to find which row we're on

                bmp.SetPixel(x, y, GetColor(image[i]));
            }
            return bmp;
        }

        public static Color GetColor(int value)
        {
            switch (value)
            {
                case 0:
                    return Color.Black;
                case 1:
                    return Color.White;
                default:
                    return Color.Transparent;
            }
        }
    }
}
