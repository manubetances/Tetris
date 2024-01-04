namespace Tetris
{
    public class IBlock : Block
    {
        // All different rotation states of the I Block
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) }, // State 0
            new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) }, // State 1
            new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) }, // State 2
            new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) } // State 3
        };

        public override int Id => 1; // Block ID
        protected override Position StartOffset => new Position(-1,3); // Spawn block position
        protected override Position[][] Tiles => tiles; // Current position of the block based on the rotation states
    }
}
