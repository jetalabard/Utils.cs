
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Utils
{
    public static class Color
    {
        public static MColor FromRgb(byte R,byte G, byte B)
        {
            return MColor.FromRgb(R, G, B);
        }

        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static DColor ToDrawingColor(this MColor color)
        {
            return DColor.FromArgb(color.A, color.R, color.G, color.B);
        }


        public static DColor ToDrawingColor(ExcelColor color)
        {
            return (DColor)new ColorConverter().ConvertFromString("#"+color.Rgb);
        }

        public static MColor ToMediaColor(ExcelColor color)
        {
            return ToMediaColor(ToDrawingColor(color));
        }
    }
}
