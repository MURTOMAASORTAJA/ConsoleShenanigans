using ConsoleShenanigans.ColorWriting;
using NUnit.Framework;
using System;
using System.Threading;

namespace ConsoleShenanigansTests
{
    public class ConsoleColorSettingsTests
    {
        [Test]
        public void ShouldGetCorrectCurrentColors()
        {
            var random = new Random();

            var tests = 5;
            var testCounter = 0;

            while (testCounter < tests)
            {
                var backgroundColor = GetRandomColor(random);
                var foregroundColor = GetRandomColor(random);

                if (!GetsCorrectCurrentColors(backgroundColor, foregroundColor))
                {
                    Assert.Fail($"Failed to get correct current colors, which were {backgroundColor} and {foregroundColor}.");
                }

                Thread.Sleep(30);

                testCounter++;
            }

            Assert.Pass();
        }

        public bool GetsCorrectCurrentColors(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(".");

            var current = ConsoleColorSettings.Current;

            return 
                current.BackgroundColor == backgroundColor && 
                current.ForegroundColor == foregroundColor;
        }

        public ConsoleColor GetRandomColor(Random random)
        {
            var availableColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)availableColors.GetValue(random.Next(availableColors.Length))!;
        }
    }
}