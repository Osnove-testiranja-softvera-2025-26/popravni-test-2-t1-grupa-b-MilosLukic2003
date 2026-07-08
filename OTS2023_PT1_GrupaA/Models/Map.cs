

namespace OTS2026_PT1_GrupaB.Models
{
    public class Map
    {
        public Field[,] Fields { get; set; }
        public static readonly int MapSize = 30;

        public Map()
        {
            Fields = new Field[MapSize, MapSize];
        }

        public void InitializeMap()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Fields[i, j] = new Field(Zone.Invalid);
                }
            }

            CreateRectangleZone(Zone.Valid, 6, 0, 15, 10);
            CreateRectangleZone(Zone.Valid, 10, 10, 9, 10);
            CreateRectangleZone(Zone.Valid, 0, 20, 30, 10);

        }


        private void CreateRectangleZone(Zone zone, int upperLeftCornerX, int upperLeftCornerY, int width, int height)
        {
            for (int i = upperLeftCornerX; i < upperLeftCornerX + width; i++)
            {
                for (int j = upperLeftCornerY; j < upperLeftCornerY + height; j++)
                {
                    Fields[i, j].Zone = zone;
                }
            }
        }

        public void AddContentToFieldOnPosition(FieldContent content, Position position)
        {
            Field field = Fields[position.X, position.Y];

            if (!field.Zone.Equals(Zone.Invalid))
            {
                field.Content = content;
            }
        }

        public void EmptyTileOnPosition(Position position)
        {
            int x = position.X;
            int y = position.Y;
            Fields[x, y].Content = FieldContent.Empty;

        }
    }
}
