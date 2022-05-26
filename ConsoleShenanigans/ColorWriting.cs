namespace ConsoleShenanigans.ColorWriting
{
    public static class ColorWriter
    {
        public static void WriteColor(string text, ConsoleColorSettings colors, bool keepNewColors = false)
        {
            var earlierColors = ConsoleColorSettings.Current;

            ConsoleColorSettings.Current = colors;

            Console.Write(text);


            if (!keepNewColors)
            {
                ConsoleColorSettings.Current = earlierColors;
            }
        }

        public static void WriteColorLine(string text, ConsoleColorSettings colors, bool keepNewColors = false)
        {
            var earlierColors = ConsoleColorSettings.Current;

            ConsoleColorSettings.Current = colors;

            Console.WriteLine(text);


            if (!keepNewColors)
            {
                ConsoleColorSettings.Current = earlierColors;
            }
        }
    }

    public class ConsoleColorSettings
    {
        public ConsoleColor? BackgroundColor { get; set; }
        public ConsoleColor? ForegroundColor { get; set; }

        public NullGroundColorBehavior NullGroundColorBehavior { get; set; }

        /// <summary>
        /// Sets or gets the current background and foreground colors of <see cref="System.Console"/>.
        /// </summary>
        public static ConsoleColorSettings Current
        {
            get
            {
                return new ConsoleColorSettings()
                {
                    BackgroundColor = Console.BackgroundColor,
                    ForegroundColor = Console.ForegroundColor
                };
            }

            set
            {
                if (value.BackgroundColor != null)
                {
                    if (value.NullGroundColorBehavior == NullGroundColorBehavior.ResetColor)
                    {
                        var currentForegroundColor = Console.ForegroundColor;
                        Console.ResetColor();
                        Console.ForegroundColor = currentForegroundColor;
                    }
                }
            }
        }
    }

    public enum NullGroundColorBehavior
    {
        DoNothing = 0,
        ResetColor = 1,
    }
}