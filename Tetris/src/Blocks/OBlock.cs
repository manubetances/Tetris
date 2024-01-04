namespace Tetris
{
    public class OBlock : Block
    {
        // All different rotation states of the I Block
        // Just have one state since is the O Block (a cube)
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0,0), new(0, 1), new(1, 0), new(1,1) }
        };

        public override int Id => 4; // Block ID
        protected override Position StartOffset => new Position(0, 4); // Spawn block position
        protected override Position[][] Tiles => tiles; // Current position of the block based on the rotation states
    }
}
