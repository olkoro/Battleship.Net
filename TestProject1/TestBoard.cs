using Domain;
using NUnit.Framework;

namespace Tests
{
    public class TestBoard
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBoardRowSeparator()
        {
            var board = new GameBoard(1, 1);
            var separatorString = board.GetRowSeparator(1);
            Assert.AreEqual("+---+", separatorString);
        }

        [Test]
        public void TestBoardDataRow()
        {
            var board = new GameBoard(1, 1);
            var dataRowString = board.GetRowWithData(board.Board[0]);
            Assert.AreEqual("| " + board.GetBoardSquareStateSymbol(board.Board[0][0]) + " |", dataRowString);
        }

        [Test]
        public void TestBoardComplete()
        {
            var board = new GameBoard(1, 1);
            var boardString = board.GetBoardString();
            
            var expectedResult =
                "+---+\n" +
                "| " + board.GetBoardSquareStateSymbol(board.Board[0][0]) + " |\n" +
                "+---+";

            Assert.AreEqual(expectedResult, boardString);
        }
    }
}