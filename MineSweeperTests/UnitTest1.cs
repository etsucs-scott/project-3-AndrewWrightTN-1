using MineSweeper;

namespace MineSweeperTests
{

    /// <summary>
    /// 10 tests just to see if core function works properly
    /// </summary>
    public class UnitTest1
    {
        [Fact]
        public void Board_Creation_SetsCorrectSize()
        {
            var board = new GameBoard(8, 10, 123);
            Assert.Equal(8, board.Size);
        }

        [Fact]
        public void Board_HasCorrectMineCount()
        {
            var board = new GameBoard(8, 10, 123);
            int mines = 0;
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (board.Tiles[r, c].IsMine) mines++;
            Assert.Equal(10, mines);
        }

        [Fact]
        public void Reveal_EmptyTile_SetsIsRevealed()
        {
            var board = new GameBoard(8, 1, 123);
            board.Reveal(0, 0);
            Assert.True(board.Tiles[0, 0].IsRevealed);
        }

        [Fact]
        public void ToggleFlag_FlagsAndUnflagsTile()
        {
            var board = new GameBoard(8, 1, 123);
            board.ToggleFlag(0, 0);
            Assert.True(board.Tiles[0, 0].IsFlagged);
            board.ToggleFlag(0, 0);
            Assert.False(board.Tiles[0, 0].IsFlagged);
        }

        [Fact]
        public void HasWon_ReturnsFalseBeforeWin()
        {
            var board = new GameBoard(8, 1, 123);
            Assert.False(board.HasWon());
        }

        [Fact]
        public void HasWon_ReturnsTrueAfterRevealingAllNonMines()
        {
            var board = new GameBoard(3, 1, 123);
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (!board.Tiles[r, c].IsMine) board.Reveal(r, c);
            Assert.True(board.HasWon());
        }

        [Fact]
        public void Reveal_Mine_TriggersMine()
        {
            var board = new GameBoard(3, 1, 123);
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board.Tiles[r, c].IsMine) board.Reveal(r, c);
            Assert.True(board.Explosion);
        }

        [Fact]
        public void InvalidCoordinates_ThrowsException()
        {
            var board = new GameBoard(8, 1, 123);
            Assert.Throws<InvalidMoveException>(() => board.Reveal(-1, 0));
        }

        [Fact]
        public void Reveal_ZeroAdjacent_Cascades()
        {
            var board = new GameBoard(3, 1, 123);
            board.Reveal(0, 0);
            int revealed = 0;
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board.Tiles[r, c].IsRevealed) revealed++;
            Assert.True(revealed > 1);
        }

        [Fact]
        public void FlaggedTile_CannotBeRevealed()
        {
            var board = new GameBoard(3, 1, 123);
            board.ToggleFlag(0, 0);
            Assert.Throws<InvalidMoveException>(() => board.Reveal(0, 0));
        }
    }
}