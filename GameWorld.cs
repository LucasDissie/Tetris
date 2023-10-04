using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    enum GameState
    {
        Playing,
        GameOver
    }

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return random; } }
    static Random random;
    
    TetrisBlock block, nextBlock;
    
    SpriteFont font;

    
    GameState gameState;
    TetrisGrid grid;
    

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        RandomBlock();
        SpawnBlock();
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        grid = new TetrisGrid();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        block.HandleInput(inputHelper);
    }

    public void Update(GameTime gameTime)
    {
        block.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch, font);
        block.Draw(gameTime, spriteBatch);
        nextBlock.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
    public void SpawnBlock()
    {
        block = nextBlock;
        block.ControlledBlock = true;
        RandomBlock();
        Debug.WriteLine("high");
    }

    public void RandomBlock()
    {
        int blockindex = random.Next(0, 7);
        if (blockindex == 0)
            nextBlock = new S_Block(random.Next(0, 7), false);
        else if (blockindex == 1)
            nextBlock = new J_Block(random.Next(0, 7), false);
        else if (blockindex == 2)
            nextBlock = new O_Block(random.Next(0, 7), false);
        else if (blockindex == 3)
            nextBlock = new T_Block(random.Next(0, 7), false);
        else if (blockindex == 4)
            nextBlock = new L_Block(random.Next(0, 7), false);
        else if (blockindex == 5)
            nextBlock = new I_Block(random.Next(0, 7), false);
        else
            nextBlock = new Z_Block(random.Next(0, 7), false);
        nextBlock.ControlledBlock = false;
    }
    public void Reset()
    {
    }
    public TetrisGrid TetrisGrid
    {
        get { return grid; }
    }
    public TetrisBlock Block
    {
        get { return block; }
        set { block = value; }
    }
    public TetrisBlock NextBlock
    {
        get { return nextBlock; }
        set { nextBlock = value; }
    }
}
