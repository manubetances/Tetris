namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid; // 2D array to store rows and collumns
        public int Rows { get; }
        public int Columns { get; }

        // Indexer to access array
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[Rows, Columns];
        }

        // Check if the collumn is inside the grid or not
        // From 0 to the grid/row size
        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        // Check if the cell is empty or not. If the value of the cell is 0 is empty
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        // Check if the entire row is full of blocks
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                    return false;
            }

            return true;
        }

        // Check if row is empty
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r,c] != 0)
                    return false;
            }

            return true;
        }

        // The game will be scanning rows, and as soon as it encounters a full row starts cleaning them
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        // Move Rows down after it cleans the full row(s)
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c< Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        //
        public int ClearFullRows()
        {
            int cleared = 0;
            
            // If the row encountered is full, count it and aument the cleared rows
            for (int r = Rows-1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                // If a row is cleared (1 or more) we move the rows above down by the number of clear rows
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared; // return the number of cleared rows
        }

    }
}
