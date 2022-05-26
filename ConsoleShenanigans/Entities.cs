using ConsoleShenanigans.ColorWriting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShenanigans.Entities
{
    public class ConsoleScreen
    {
        public Entity[]? Entities { get; set; }

        public void Render()
        {
            var testEntities = new[]
            {
                new Entity()
                {
                    Z = 0,
                    FillBrush = new Brush()
                    {
                        Character = 'x',
                        Colors = new ConsoleColorSettings() { BackgroundColor = ConsoleColor.DarkGray, ForegroundColor = ConsoleColor.DarkYellow },
                    },
                    Dimensions = new Dimensions()
                    {
                        X = 25,
                        Y = 19,
                        Width = 4,
                        Height = 4
                    },
                    OuterBorder = new Border()
                    {
                        Brush = new Brush()
                        {
                            Colors = new ConsoleColorSettings() { BackgroundColor = ConsoleColor.DarkRed, ForegroundColor = ConsoleColor.White },
                            Character = 'o'
                        },
                        Thickness = 3
                    }
                }
            };

            Entities = testEntities;

            // rendering:

            int xCounter = 0;
            int yCounter = 0;

            while (yCounter < Console.WindowHeight)
            {
                while (xCounter < Console.WindowWidth)
                {
                    var brush = GetBrushForCoordinate(xCounter, yCounter);
                    if (brush != null)
                    {
                        if (brush.Colors != null) ConsoleColorSettings.Current = brush.Colors;

                        Console.Write(brush.Character);
                    } else
                    {
                        Console.Write(' ');
                    }
                    xCounter++;
                }

                Console.WriteLine("\n");
                xCounter = 0;
                yCounter++;
            }
        }

        private Brush? GetBrushForCoordinate(int x, int y)
        {
            var entity = GetAllRectanglesInCoordinate(x, y)?.MinBy(entity => entity.Z);
            if (entity == null)
            {
                return null;
            }
            var brush = CoordinateHitsEntity(entity, x, y);

            if (brush != null)
            {
                return brush;
            } else
            {
                throw new Exception("Failed to get brush for coordinate.");
            }
        }

        private IEnumerable<Entity>? GetAllRectanglesInCoordinate(int x, int y)
        {
            return Entities!.Where(entity => CoordinateHitsEntity(entity, x, y) != null);
        }

        private Brush? CoordinateHitsEntity(Entity entity, int x, int y)
        {
            if (x >= entity.Dimensions.X &&
                x <= entity.Dimensions.X + entity.Dimensions.Width &&
                y >= entity.Dimensions.Y &&
                y <= entity.Dimensions.Y + entity.Dimensions.Height)
            {
                return entity.FillBrush;
            } else
            {
                var outerBorderEntities = GetAllOuterBorderEntities();
                if (!IsNullOrEmpty(outerBorderEntities))
                {
                    var outerBorderBrush = outerBorderEntities!.Select(entity => CoordinateHitsEntity(entity, x, y)).FirstOrDefault();
                    if (outerBorderBrush != null)
                    {
                        throw new InvalidOperationException("Couldn't get fillbrush for an outer border, for some unknown reason.");
                    } else
                    {
                        return outerBorderBrush;
                    }
                } else
                {
                    return null;
                }
            }
        }

        private bool IsNullOrEmpty<T>(T[]? array)
        {
            return (array == null || (array != null && array.Any()));
        }

        private Entity[]? OuterBorderToEntities(Entity entity)
        {
            if (entity.OuterBorder != null)
            {
                return new[]
                {
                    new Entity()
                    {
                        Z = entity.Z,
                        FillBrush = entity.OuterBorder.Brush,
                        Dimensions = new Dimensions()
                        {
                            X = entity.Dimensions.X - entity.OuterBorder.Thickness,
                            Y = entity.Dimensions.Y - entity.OuterBorder.Thickness,
                            Width = entity.OuterBorder.Thickness,
                            Height = entity.OuterBorder.Thickness * 2 + entity.Dimensions.Height
                        }
                    },
                    new Entity()
                    {
                        Z = entity.Z,
                        FillBrush = entity.OuterBorder.Brush,
                        Dimensions = new Dimensions()
                        {
                            X = entity.Dimensions.X + entity.Dimensions.Width,
                            Y = entity.Dimensions.Y - entity.OuterBorder.Thickness,
                            Width = entity.OuterBorder.Thickness,
                            Height = entity.OuterBorder.Thickness * 2 + entity.Dimensions.Height
                        }
                    },
                    new Entity()
                    {
                        Z = entity.Z,
                        FillBrush = entity.OuterBorder.Brush,
                        Dimensions = new Dimensions()
                        {
                            X = entity.Dimensions.X,
                            Y = entity.Dimensions.Y - entity.OuterBorder.Thickness,
                            Width = entity.Dimensions.Width,
                            Height = entity.OuterBorder.Thickness
                        }
                    },
                    new Entity()
                    {
                        Z = entity.Z,
                        FillBrush = entity.OuterBorder.Brush,
                        Dimensions = new Dimensions()
                        {
                            X = entity.Dimensions.X,
                            Y = entity.Dimensions.Y + entity.Dimensions.Height,
                            Width = entity.Dimensions.Width,
                            Height = entity.OuterBorder.Thickness
                        }
                    }
                };
            } else
            {
                return null;
            }
        }

        private Entity[]? GetAllOuterBorderEntities()
        {
            var result = new List<Entity>();
            
            foreach (var entity in Entities)
            {
                var borderEntities = OuterBorderToEntities(entity);
                if (borderEntities != null && borderEntities.Any())
                {
                    result.AddRange(borderEntities);
                }
            }

            return result.Any() ? result.ToArray() : null;
        }

        private IEnumerable<T>? MaybeConcat<T>(IEnumerable<T>? first, IEnumerable<T>? second)
        {
            return second == null ? first : first?.Concat(second);
        }
    }

    public class Brush
    {
        public char Character { get; set; }
        public ConsoleColorSettings? Colors { get; set; }
    }

    public class Entity
    {
        public int Z { get; set; }
        public Dimensions Dimensions { get; set; }
        public Border OuterBorder { get; set; }
        public Brush FillBrush { get; set; }
    }

    public struct Dimensions
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Border
    {
        public int Thickness { get; set; }
        public Brush Brush { get; set; }
    }

}
