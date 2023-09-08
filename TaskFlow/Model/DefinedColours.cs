using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Model
{
    /// <summary>
    /// List of colours that labels and tasks can inherit
    /// </summary>
    public enum DefinedColors
    {
        White,
        Red,
        Maroon,
        Orange,
        Pink,
        Green,
        Lime,
        Yellow,
        Lemon,
        Blue,
        Aqua,
        Purple,
        Violet,
        Brown,
        Grey,
    }
    public static class DefinedColorsExtension
    {
        private static readonly Dictionary<DefinedColors, Color> colorName = new Dictionary<DefinedColors, Color>
        {
            { DefinedColors.White, Colors.White },
            { DefinedColors.Red, Colors.Red },
            { DefinedColors.Maroon, Colors.Maroon },
            { DefinedColors.Orange, Colors.Orange },
            { DefinedColors.Pink, Colors.Pink },
            { DefinedColors.Green, Colors.Green },
            { DefinedColors.Lime, Colors.Lime },
            { DefinedColors.Yellow, Colors.Yellow },
            { DefinedColors.Lemon, Colors.LemonChiffon },
            { DefinedColors.Blue, Colors.Blue },
            { DefinedColors.Aqua, Colors.Aqua },
            { DefinedColors.Purple, Colors.Purple },
            { DefinedColors.Violet, Colors.Violet },
            { DefinedColors.Brown, Colors.Brown },
            { DefinedColors.Grey, Colors.Grey },
        };

        public static Color GetValue(this DefinedColors color)
        {
            return colorName[color];
        }

        public static Color Parse(this string str)
        {
            try
            {
                DefinedColors dc = (DefinedColors)Enum.Parse(typeof(DefinedColors), str);
                return dc.GetValue();
            }
            catch 
            {
                return null;
            }
        }
    }
}
