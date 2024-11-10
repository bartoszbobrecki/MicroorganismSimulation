using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;

namespace FungiSimulation
{
    public class ColorGenerator
    {
        private static readonly Color BaseColor = Color.FromArgb(30, 30, 30);
        private static readonly Color FungiColor = Color.FromArgb(21, 68, 237);
        private static readonly Color FoodColor = Color.FromArgb(170, 81, 57);
        private static readonly Color WaterColor = Color.FromArgb(128, 197, 222);
        private static readonly Color ColdColor = Color.FromArgb(18, 84, 168);
        private static readonly Color HotColor = Color.FromArgb(231, 1, 4);
        private static readonly Color ObstacleColor = Color.FromArgb(159, 141, 141);

        public static Color GenerateFungiColor(int value) => GenerateColor(BaseColor, FungiColor, value);
        public static Color GenerateFoodColor(int value) => GenerateColor(BaseColor, FoodColor, value);
        public static Color GenerateWaterColor(int value) => GenerateColor(BaseColor, WaterColor, value);
        public static Color GenerateTemperatureColor(int value)
        {
            if (value <= 5)
            {
                return ColdColor;
            }
            else if (value >= 50)
            {
                return HotColor;
            }
            else if (value < 27)
            {
                double factor = (value - 5) / 22.0;
                return InterpolateColor(ColdColor, BaseColor, factor);
            }
            else
            {
                double factor = (value - 27) / 23.0;
                return InterpolateColor(BaseColor, HotColor, factor);
            }
        }

        private static Color InterpolateColor(Color startColor, Color endColor, double factor)
        {
            int red = (int)(startColor.R + (endColor.R - startColor.R) * factor);
            int green = (int)(startColor.G + (endColor.G - startColor.G) * factor);
            int blue = (int)(startColor.B + (endColor.B - startColor.B) * factor);

            return Color.FromArgb(red, green, blue);
        }
        public static Color GenerateObstacleColor(int value) => GenerateColor(BaseColor, ObstacleColor, value);

        private static Color GenerateColor(Color baseColor, Color targetColor, int value)
        {

            int red = (int)(baseColor.R + (targetColor.R - baseColor.R) * (value / 255.0));
            int green = (int)(baseColor.G + (targetColor.G - baseColor.G) * (value / 255.0));
            int blue = (int)(baseColor.B + (targetColor.B - baseColor.B) * (value / 255.0));

            return Color.FromArgb(red, green, blue);
        }
    }
}
