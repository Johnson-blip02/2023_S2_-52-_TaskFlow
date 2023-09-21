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

        /// <summary>
        /// Get the color associated with the DefinedColor key
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetValue(this DefinedColors color)
        {
            return colorName[color];
        }

        /// <summary>
        /// Gets the DefinedColor key associated with the color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static DefinedColors GetValue(this Color color)
        {
            var enumerator = colorName.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.Equals(color))
                    return enumerator.Current.Key;
            }
            return DefinedColors.White;
        }

        /// <summary>
        /// Parse a colour to its respective string representation
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Parse(this Color color)
        {
            try
            {
                DefinedColors dc = color.GetValue();
                return dc.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Parse a name of a colour to convert to the appropriate dictionary colour.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
