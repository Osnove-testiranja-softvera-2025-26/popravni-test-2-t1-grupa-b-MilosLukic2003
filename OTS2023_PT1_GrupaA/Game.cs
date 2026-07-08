

using OTS2026_PT1_GrupaB.Exceptions;
using OTS2026_PT1_GrupaB.Models;

namespace OTS2026_PT1_GrupaB
{
    public class Game
    {
        public Player Player { get; set; }
        public Map Map { get; set; }


        public Game(Position playerPosition, Position sheepDogPosition)
        {
            Map = new Map();
            Map.InitializeMap();

            if (!ValidatePosition(playerPosition) || !ValidatePosition(sheepDogPosition))
            {
                throw new InvalidPositionException("Player and sheep dog must be in the valid zone!");
            }

            Player = new Player(playerPosition);
            Map.Fields[sheepDogPosition.X, sheepDogPosition.Y].Content = FieldContent.SheepDog;
        }

        public void MovePlayer(Move move)
        {
            Position playerPositionAfterMove = Player.GetPositionAfterMove(move);

            if (ValidatePosition(playerPositionAfterMove))
            {
                Player.MakeMove(move);
            }
        }

        public bool ValidatePosition(Position position)
        {

            if (position == null)
                return false;

            int x = position.X;
            int y = position.Y;

            if (!ValidatePositionInsideValidZones(position))
            {
                return false;
            }
            if (Map.Fields[x, y].Content.Equals(FieldContent.Sheep))
            {
                return Player.HasSheepDog;
            }
            else
            {
                return true;
            }
        }

        private bool ValidatePositionInsideValidZones(Position position)
        {

            if (position == null)
                return false;

            int x = position.X;
            int y = position.Y;

            if (x < 0 || x >= Map.MapSize || y < 0 || y >= Map.MapSize)
            {
                return false;
            }
            if (Map.Fields[x, y].Zone.Equals(Zone.Invalid))
            {
                return false;
            }
            return true;
        }

        public void ResolvePlayerPosition()
        {
            FieldContent fieldContent = Map.Fields[Player.Position.X, Player.Position.Y].Content;

            if (fieldContent.Equals(FieldContent.Clover))
            {
                Player.AmountOfClover++;
            }
            else if (fieldContent.Equals(FieldContent.Sheep))
            {
                if (Player.AmountOfClover > 0)
                {
                    Player.CaptureSheep();
                }
                else
                {
                    return;
                }
            }
            else if (fieldContent.Equals(FieldContent.SheepDog))
            {
                Player.HasSheepDog = true;
                
            }

            Map.EmptyTileOnPosition(Player.Position);

        }

        public enum Score
        {
            Bad,
            Average,
            Good
        }


        public Score CalculateIncome()
        {
            if (Player.AmountOfSheep > 14)
            {
                return Score.Good;
            }
            if (Player.AmountOfClover >= 10 && Player.HasSheepDog)
            {
                if (Player.AmountOfSheep > 7)
                {
                    return Score.Good;
                }
                else
                {
                    return Score.Average;
                }
            }
            return Score.Bad;
        }
    }
}
