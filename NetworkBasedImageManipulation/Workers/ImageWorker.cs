using System.Drawing;
using System;

namespace NetworkBasedImageManipulation.Workers
{
    public static class ImageWorker
    {

        private static double CorrectionKoef = 8 * Math.Log(2) / 255d;

        public static int BilinearValue(
            float x, float y,
            int x1, int y1, int x2, int y2,
            int Q11, int Q12, int Q21, int Q22)
        {
            int R1 = Convert.ToInt16((x2 - x) * Q11 + (x - x1) * Q21);
            int R2 = Convert.ToInt16((x2 - x) * Q12 + (x - x1) * Q22);
            int P = Convert.ToInt16((y2 - y) * R1 + (y - y1) * R2);

            if (P < 0) P = 0;
            if (P > 255) P = 255;
            return P;
        }

        public static Color ChangeGammaCorrection(Color pixel, int gamma)
        {
            double calculatedGamma = gamma / 100d;
            return Color.FromArgb(
                ChangeColorGamma(pixel.R, calculatedGamma),
                ChangeColorGamma(pixel.G, calculatedGamma),
                ChangeColorGamma(pixel.B, calculatedGamma)
            );
        }

        public static int ChangeColorGamma(int color, double calculatedGamma)
        {
            double result = Math.Pow(color / 255d, calculatedGamma) * 255;
            if (result > 255) return 255;
            return Convert.ToInt16(result);
        }

        public delegate int CorrectionFunction(int color);

        public static int CorrectionBySin(int color)
        {
            double result = (255 / 2d) * Math.Sin(Math.PI / 255 * color - Math.PI / 2) + (255 / 2d);
            return Convert.ToInt16(result);
        }

        public static int CorrectionByExp(int color)
        {
            double result = Math.Exp(CorrectionKoef * color) - 1;
            return Convert.ToInt16(result);
        }

        public static int CorrectionByLog(int color)
        {
            double result = Math.Log(color + 1) / CorrectionKoef;
            return Convert.ToInt16(result);
        }

        public static Color ChangeCorrection(Color pixel, CorrectionFunction fn)
        {
            return Color.FromArgb(fn(pixel.R), fn(pixel.G), fn(pixel.B));
        }

        public static Color ChangeContrast(Color pixel, int value)
        {
            float percent = value / 100f;
            return Color.FromArgb(
                ChangeColorContrast(pixel.R, percent),
                ChangeColorContrast(pixel.G, percent),
                ChangeColorContrast(pixel.B, percent)
            );
        }

        public static int ChangeColorContrast(int color, float percent)
        {
            int result;
            if (percent < 0)
                result = Convert.ToInt16(color + percent * (color - 128));
            else
                result = Convert.ToInt16(128 + (color - 128) / (1 - percent));

            if (result > 255) return 255;
            if (result < 0) return 0;
            return result;
        }

        public static Color ChangeBrightness(Color pixel, int value)
        {
            float percent = value / 100f;
            return Color.FromArgb(
                ChangeColorBrightness(pixel.R, percent),
                ChangeColorBrightness(pixel.G, percent),
                ChangeColorBrightness(pixel.B, percent)
            );
        }

        public static int ChangeColorBrightness(int color, float percent)
        {
            int result = Convert.ToInt16(color + percent * 128);
            if (result > 255) return 255;
            if (result < 0) return 0;
            return result;
        }

        public static Color GreyoutPixel(Color pixel)
        {
            int avg = (pixel.R + pixel.G + pixel.B + 1) / 3;
            return Color.FromArgb(avg, avg, avg);
        }

        public static Color PreparePixel(bool isGrey, int brightness, int contrast, int correctionIndex, int gamma, Color pixel)
        {
            if (isGrey)
                pixel = GreyoutPixel(pixel);
            if (brightness != 0)
                pixel = ChangeBrightness(pixel, brightness);
            if (contrast != 0)
                pixel = ChangeContrast(pixel, contrast);
            switch (correctionIndex)
            {
                case 0: break;
                case 1:
                    pixel = ChangeCorrection(pixel, CorrectionBySin);
                    break;
                case 2:
                    pixel = ChangeCorrection(pixel, CorrectionByExp);
                    break;
                case 3:
                    pixel = ChangeCorrection(pixel, CorrectionByLog);
                    break;
            }
            if (gamma != 100)
                pixel = ChangeGammaCorrection(pixel, gamma);
            return pixel;
        }
    }
}
