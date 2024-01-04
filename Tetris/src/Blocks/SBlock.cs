﻿namespace Tetris
{
    internal class SBlock : Block
    {
        // All different rotation states of the S Block
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0,1), new(0,2), new(1,0), new(1,1) }, // State 0
            new Position[] { new(0,1), new(1,1), new(1,2), new(2,2) }, // State 1
            new Position[] { new(1,1), new(1,2), new(2,0), new(2,1) }, // State 2
            new Position[] { new(0,0), new(1,0), new(1,1), new(2,1) } // State 3
        };

        public override int Id => 5; // Block ID
        protected override Position StartOffset => new Position(0, 3); // Spawn block position
        protected override Position[][] Tiles => tiles; // Current position of the block based on the rotation states
    }
}
