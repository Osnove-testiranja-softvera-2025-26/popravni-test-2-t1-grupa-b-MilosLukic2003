using NUnit.Framework;
using OTS2026_PT1_GrupaB;
using OTS2026_PT1_GrupaB.Exceptions;
using OTS2026_PT1_GrupaB.Models;
using System;

namespace OTS2026_PT1_GrupaB.Test
{
    [TestFixture]
    public class GameTests
    {
        private Game game;
        private Position defaultPlayerPos;
        private Position defaultDogPos;

        [SetUp]
        public void Setup()
        {
            defaultPlayerPos = new Position(10, 10);
            defaultDogPos = new Position(6, 0);

            game = new Game(defaultPlayerPos, defaultDogPos);
        }

        #region F1 - Inicijalizacija igre

        [Test]
        public void Initialize_ValidPositions_SuccessfullyInitializes()
        {
            Position playerPos = new Position(10, 10);
            Position dogPos = new Position(6, 0);

            Assert.DoesNotThrow(() => new Game(playerPos, dogPos));

            Game testGame = new Game(playerPos, dogPos);
            Assert.AreEqual(playerPos, testGame.Player.Position);
        }

        [Test]
        public void Initialize_InvalidPlayerPosition_ThrowsInvalidPositionException()
        {
            Position invalidPlayerPos = new Position(0, 0);
            Position validDogPos = new Position(6, 0);

            Assert.Throws<InvalidPositionException>(() => new Game(invalidPlayerPos, validDogPos));
        }

        [Test]
        public void Initialize_InvalidDogPosition_ThrowsInvalidPositionException()
        {
            Position validPlayerPos = new Position(10, 10);
            Position invalidDogPos = new Position(0, 0);

            Assert.Throws<InvalidPositionException>(() => new Game(validPlayerPos, invalidDogPos));
        }

        #endregion

        #region F2 - ValidatePosition

        [TestCase(10, 10, true, ExpectedResult = true)]
        [TestCase(0, 0, false, ExpectedResult = false)]
        [TestCase(-1, 5, false, ExpectedResult = false)]
        [TestCase(30, 5, false, ExpectedResult = false)]
        public bool ValidatePosition_BasicCases(int x, int y, bool hasDog)
        {
            game.Player.HasSheepDog = hasDog;
            return game.ValidatePosition(new Position(x, y));
        }

        // Testiranje uslova za sheep polje (zavisnost od sakupljenog psa ovcara)
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool ValidatePosition_OnSheepField_DependsOnSheepDog(bool hasSheepDog)
        {
            Position sheepPosition = new Position(10, 11);
            game.Map.Fields[sheepPosition.X, sheepPosition.Y].Content = FieldContent.Sheep;
            game.Player.HasSheepDog = hasSheepDog;

            return game.ValidatePosition(sheepPosition);
        }

        #endregion

        #region F3 - ResolvePlayerPosition

        [Test]
        public void ResolvePlayerPosition_OnClover_IncreasesCloverAndEmptiesField()
        {
            Position pos = new Position(10, 10);
            game.Player.Position = pos;
            game.Map.Fields[pos.X, pos.Y].Content = FieldContent.Clover;
            int initialClover = game.Player.AmountOfClover;

            game.ResolvePlayerPosition();

            Assert.AreEqual(initialClover + 1, game.Player.AmountOfClover);
            Assert.AreEqual(FieldContent.Empty, game.Map.Fields[pos.X, pos.Y].Content);
        }

        [Test]
        public void ResolvePlayerPosition_OnSheepDog_SetsHasSheepDogTrueAndEmptiesField()
        {
            Position pos = new Position(10, 10);
            game.Player.Position = pos;
            game.Map.Fields[pos.X, pos.Y].Content = FieldContent.SheepDog;

            game.ResolvePlayerPosition();

            Assert.IsTrue(game.Player.HasSheepDog);
            Assert.AreEqual(FieldContent.Empty, game.Map.Fields[pos.X, pos.Y].Content);
        }

        [Test]
        public void ResolvePlayerPosition_OnSheepWithClover_IncreasesSheepDecreasesCloverAndEmptiesField()
        {
            Position pos = new Position(10, 10);
            game.Player.Position = pos;
            game.Map.Fields[pos.X, pos.Y].Content = FieldContent.Sheep;
            game.Player.AmountOfClover = 3;
            int initialSheep = game.Player.AmountOfSheep;

            game.ResolvePlayerPosition();

            Assert.AreEqual(2, game.Player.AmountOfClover);
            Assert.AreEqual(initialSheep + 1, game.Player.AmountOfSheep);
            Assert.AreEqual(FieldContent.Empty, game.Map.Fields[pos.X, pos.Y].Content);
        }

        #endregion

        #region F4 - CalculateScore

        // Primena Black Box tehnike, Decision Table i granične vrednosti
        [TestCase(15, 5, false, Game.Score.Good)]
        [TestCase(10, 11, true, Game.Score.Good)]
        [TestCase(2, 11, true, Game.Score.Average)]
        [TestCase(10, 5, true, Game.Score.Bad)]
        [TestCase(2, 5, false, Game.Score.Bad)]
        [TestCase(2, 11, false, Game.Score.Bad)]
        public void CalculateScore_ReturnsExpectedScore(int sheep, int clover, bool hasDog, Game.Score expectedScore)
        {
            game.Player.AmountOfSheep = sheep;
            game.Player.AmountOfClover = clover;
            game.Player.HasSheepDog = hasDog;

            Game.Score actualScore = game.CalculateIncome();

            Assert.AreEqual(expectedScore, actualScore);
        }

        #endregion
    }
}