using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.
    Vector2 position;

    /// The number of grid elements in the x-direction.
    public static int Width { get { return 10; } }
   
    /// The number of grid elements in the y-direction.
    public static int Height { get { return 21; } }
    private int score;
    private Color[,] grid = new Color[TetrisGrid.Width, TetrisGrid.Height];


    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = new Vector2(5 * emptyCell.Width, 0);
        for (int x = 0; x <= 9; x++)
        {
            for(int y = 0; y <= 20; y++)
            {
                if (y != 20)
                    grid[x, y] = Color.White;
                else
                    grid[x, y] = Color.Black;
            }
        }
        Clear();
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                position = new Vector2(x * emptyCell.Width, y * emptyCell.Height);
                spriteBatch.Draw(emptyCell, position, Color.White);
                spriteBatch.Draw(emptyCell, position, grid[x,y]);
                spriteBatch.DrawString(font, "Score:" + score, new Vector2(300, 0), Color.Blue);
            }
        }

    }
    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
        //grid = new Color[Width,Height];
    }
    public void CheckGrid()
    {
        int ColoredAmount = 0, DeletedRows = 0;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (grid[x, y] != Color.White)
                {
                    ColoredAmount++;
                }
            }
            if (ColoredAmount == 10)
            {
                DeleteRow(y);
                DeletedRows += 1;
                ColoredAmount = 0;
            }
            else
                ColoredAmount = 0;
        }
        UpdateScore(DeletedRows);
    }

    public void UpdateScore(int DeletedRows)
    {
        switch (DeletedRows)
        {
            case 1: score += 40;
                break;
            case 2: score += 100;
                break;
            case 3: score += 300;
                break;
            case 4: score += 1200;
                break;
        }

    }
        
    public void DeleteRow(int DeleteThisRow)
    {
        int RowIndicator = 0;
        for (int y = 0; y < DeleteThisRow; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                grid[x, DeleteThisRow - RowIndicator] = grid[x, DeleteThisRow - 1 - RowIndicator];
            }
            RowIndicator++;
        }
        for (int x = 0; x < 10; x++)
        {
            grid[x, 0] = Color.White;
        }

    }
    public Texture2D Emptycell
    {
        get { return emptyCell; }
    }
    public Color[,] Grid
    {
        get { return grid; }
        set { grid = value; }
    }
}

