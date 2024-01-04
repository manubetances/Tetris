using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        // Tiles position in the rotation state
        protected abstract Position[][] Tiles { get; }
        // Where the block will start on the grid
        protected abstract Position StartOffset { get; }
        // IDs to identify each block
        public abstract int Id { get; }

        private int rotationState; // Current rotation of the block
        private Position offset; // Current offset

        public Block()
        {
            // Offset should be initialized to the starting offset position
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        // Know the grid positions of the current block in control using the rotation and offset
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // Rotate the block 90 clockwise
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // And Rotate counterclockwise
        public void RotateCCW()
        {
            if(rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        // Move the block in the desired position the player/users wants it
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        // Reset method to reset any position change or rotation made
        public void Reset() 
        { 
            rotationState = 0; 
            offset.Row = StartOffset.Row; 
            offset.Column = StartOffset.Column;
        }
    }
}
