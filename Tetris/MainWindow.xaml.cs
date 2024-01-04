using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Array containing tiles images
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        // Array containing the blocks images
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        // Array to control each cell in the grid
        private readonly Image[,] imageControls;
        // Delay constants to add difficulty to the game
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            MainMenu.Visibility = Visibility.Hidden;
            GameCanvas_Loaded();
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            // Look for every column and row on the game grid
            // And create an image control for each cell
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); // r - 2 is to hide the first 2 rows from the GUI
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        // Draw Game Grid
        private void DrawGrid(GameGrid grid)
        {
            // Look into all positions on the grid
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0;c < grid.Columns; c++)
                {
                    // For each position we get its ID, and set the image using the ID
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1; // Reseting opacity of the block (Check Ghost Block)
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        // To draw the blocks we loop through the tile positions and update the image on them
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1; // Reseting opacity of the block (Check Ghost Block)
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
                HoldImage.Source = blockImages[0];
            else
                HoldImage.Source = blockImages[heldBlock.Id];
        }

        // Preview of the next upcoming block
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        // Draw where the block is going to land
        // A ghost block
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        // Draw Game Grid and Blocks together
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease)); // Blocks go faster as score increases
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible; // Make Game Over Screen Visible
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // If game is over dont do anything
            if (gameState.GameOver)
                return;

            switch (e.Key)
            {
                case Key.Left: // ARROWS
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right: // ARROWS
                    gameState.MoveBlockRight();
                    break;
                case Key.Down: // ARROWS 
                    gameState.MoveBlockDown();
                    break;
                case Key.Up: // ARROWS
                    gameState.RotateBlockCW();
                    break;
                case Key.Space: // SPACE
                    gameState.RotateBlockCCW();
                    break;
                case Key.RightShift: // Shift
                    gameState.HoldBlock();
                    break;
                case Key.Enter:
                    gameState.DropBlock();
                    break;
                case Key.A: // WASD
                    gameState.MoveBlockLeft();
                    break;
                case Key.D: // WASD
                    gameState.MoveBlockRight();
                    break;
                case Key.S: // WASD
                    gameState.MoveBlockDown();
                    break;
                case Key.W: // WASD
                    gameState.RotateBlockCW();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }



        private async void GameCanvas_Loaded()
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
