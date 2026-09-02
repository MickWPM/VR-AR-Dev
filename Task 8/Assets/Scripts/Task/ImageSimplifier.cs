using UnityEngine;
using System.Collections.Generic;

//Static helper class - image setup only
public static class ImageSimplifier
{
    //Colour quantisation; take an image and number of colours and return an image that only has that number of colours
    //Use a helper struct to provide the list of colours as well

    //    public static QuantisedImage GetQuantised(Texture2D image, Color[] quantisedColours)
    //    {
    //        QuantisedImage quantisedImage = new QuantisedImage();
    //        quantisedImage.numLevels = quantisedColours.Length;
    ////        quantisedImage.quantisedColours = quantisedColours;
    //        quantisedImage.image = QuantiseImage(image, quantisedColours);

    //        return quantisedImage;
    //    }


    //    public static QuantisedImage GetQuantised(Texture2D image, int numLevels)
    //    {
    //        Color[] quantisedColours = GetColours(image, numLevels);
    //        return GetQuantised(image, quantisedColours);
    //    }

    //    private static QuantisedColours[] GetColours(Texture2D image, int levels)
    //    {
    //        QuantisedColours[] colours = new Color[levels];




    //        return colours;
    //    }

    //private static Color[] GetQuantisedLevels(Texture2D image, int numLevels)
    //{
    //    //Find the numLevels closest colours
    //}


    public static (Texture2D, Dictionary<Color, List<int>>) GetQuantisedTexture(Texture2D image, Color[] quantisedColours)
    {
        return QuantiseImage(image, quantisedColours);
    }

    public static (Texture2D, Dictionary<Color, List<int>>) GetQuantisedTexture(Texture2D image, int levels)
    {
        Color[] quantisedColours = GetQuantisedLevels(image, levels);
        return GetQuantisedTexture(image, quantisedColours);
    }

    //Simple k means..
    //1 - randomly pick k centriods
    //2 - assign each pixel to closest centriod
    //3 - recalculate centriod as average of all within group
    //
    private static Color[] GetQuantisedLevels(Texture2D image, int numLevels)
    {
        Color[] pixels = image.GetPixels();
        Color[] centroids = new Color[numLevels];

        // Random centriods
        for (int i = 0; i < numLevels; i++)
        {
            centroids[i] = pixels[UnityEngine.Random.Range(0, pixels.Length)];
        }

        int maxIterations = 10;
        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            //assign to closest centriod
            Color[] runningSums = new Color[numLevels];
            int[] counts = new int[numLevels];
            for (int i = 0; i < pixels.Length; i++)
            {
                int closestIndex = 0;
                float closestDist = float.MaxValue;

                for (int j = 0; j < numLevels; j++)
                {
                    float dist = ColourDistance(pixels[i], centroids[j]);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestIndex = j;
                    }
                }

                runningSums[closestIndex] += pixels[i];
                counts[closestIndex]++;
            }

            //recalculate centriods
            bool changed = false;
            for (int j = 0; j < numLevels; j++)
            {
                if (counts[j] > 0)
                {
                    Color newCentroid = runningSums[j] / (float)counts[j];
                    if (ColourDistance(centroids[j], newCentroid) > 0.001f)
                    {
                        changed = true;
                    }
                    centroids[j] = newCentroid;
                }
            }
            if (!changed) break; 
        }
        return centroids;
    }


    private static (Texture2D, Dictionary<Color, List<int>>) QuantiseImage(Texture2D image, Color[] quantisedColours)
    {
        Texture2D quantisedImage = new Texture2D(image.width, image.height);
        Dictionary<Color, List<int>> QuantisedColoursDict = new Dictionary<Color, List<int>>();

        for (int i = 0; i < quantisedColours.Length; i++)
        {
            if (QuantisedColoursDict.ContainsKey(quantisedColours[i]))
            {
                Debug.LogError($"{quantisedColours[i]} included multiple times in desired quantised colours. Skipping");
            }
            else
            {
                QuantisedColoursDict.Add(quantisedColours[i], new List<int>());
            }
        }

        Color[] pixels = image.GetPixels();
        Color[] newPixels = new Color[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixelColour = pixels[i];
            Color closestColour = Color.black;
            float closestDist = float.PositiveInfinity;
            for (int j = 0; j < quantisedColours.Length; j++)
            {
                float d = ColourDistance(pixelColour, quantisedColours[j]);
                if (d < closestDist)
                {
                    closestColour = quantisedColours[j];
                    closestDist = d;
                }
            }
            QuantisedColoursDict[closestColour].Add(i);
            newPixels[i] = closestColour;
        }

        quantisedImage.SetPixels(newPixels);
        quantisedImage.Apply();
        return (quantisedImage, QuantisedColoursDict);
    }

    private static float ColourDistance(Color c1, Color c2)
    {
        Vector3 v1 = new Vector3(c1.r, c1.g, c1.b);
        Vector3 v2 = new Vector3(c2.r, c2.g, c2.b);
        return Vector3.SqrMagnitude(v1 - v2);
    }

    //public struct QuantisedImage
    //{
    //    public int numLevels;
    //    public QuantisedColours[] quantisedColours;
    //    public Texture2D image;
    //}

    //public struct QuantisedColours
    //{
    //    public Color colour;
    //    public List<int> pixelIndexes;
    //}
}
