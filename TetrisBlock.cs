using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




class TetrisBlock
{
    protected bool[,] blockLayout, oldblockLayout;
    private Vector2 blockPosition = new Vector2(4, 0), oldblockPosition;
    private Color[,] latestGrid = new Color[10,21];
    private float FallTimer = 1f;
    private bool randomRotate = true, controlledBlock;
    protected Color BlockColor;

    public TetrisBlock(int blockColor, bool controlled)
    {
        if (controlled == true)
        {
            controlledBlock = true;
        }
        else
        {
            controlledBlock = false;
        }
        switch (blockColor)
        {
            case 1:
                BlockColor = Color.Red;
                break;
            case 2:
                BlockColor = Color.Blue;
                break;
            case 3:
                BlockColor = Color.Green;
                break;
            case 4:
                BlockColor = Color.Yellow;
                break;
            case 5:
                BlockColor = Color.DarkOrange;
                break;
            case 6:
                BlockColor = Color.Fuchsia;
                break;
            case 0:
                BlockColor = Color.Cyan;
                break;
        }
    }
    public bool[,] BlockLayout
    {
        get { return blockLayout; }
        set { blockLayout = value; }
    }

    public void Update(GameTime gameTime)
    {
        FallTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (FallTimer <= 0)
        {
            oldblockLayout = blockLayout;
            oldblockPosition = blockPosition;
            blockPosition.Y += 1;
            FallTimer = 1f;
            if (CheckInput() == 2)
            {
                blockLayout = oldblockLayout;
                blockPosition = oldblockPosition;
                SetBlock();
            }
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (randomRotate)
        {
            for (int i = 0; i < GameWorld.Random.Next(0,4); i++)
            {
                blockLayout = Rotate(blockLayout, true);
            }
            randomRotate = false;
        }
        if (controlledBlock)
        {
            oldblockLayout = blockLayout;
            oldblockPosition = blockPosition;
            latestGrid = TetrisGame.GameWorld.TetrisGrid.Grid;
            for (int y = 0; y < blockLayout.GetLength(0); y++)
            {
                for (int x = 0; x < blockLayout.GetLength(0); x++)
                {
                    if (blockLayout[x, y])
                    {
                        Vector2 position = new Vector2((x + blockPosition.X) * TetrisGame.GameWorld.TetrisGrid.Emptycell.Width, (y + blockPosition.Y) * TetrisGame.GameWorld.TetrisGrid.Emptycell.Height);
                        spriteBatch.Draw(TetrisGame.GameWorld.TetrisGrid.Emptycell, position, BlockColor);
                    }
                }
            }
        }
        else
        {
            for (int y = 0; y < blockLayout.GetLength(0); y++)
            {
                for (int x = 0; x < blockLayout.GetLength(0); x++)
                {
                    if (blockLayout[x, y])
                    {
                        Vector2 position = new Vector2(500 + x * TetrisGame.GameWorld.TetrisGrid.Emptycell.Width, 500 + y * TetrisGame.GameWorld.TetrisGrid.Emptycell.Height);
                        spriteBatch.Draw(TetrisGame.GameWorld.TetrisGrid.Emptycell, position, BlockColor);
                    }
                }
            }
          
        }
    }
    public void HandleInput(InputHelper inputHelper)
    {
        bool Set = false; 
        if (inputHelper.KeyPressed(Keys.Left))
        {
            blockPosition.X -= 1;
        }
        else if (inputHelper.KeyPressed(Keys.Right))
        {
            blockPosition.X += 1;
        }
        else if (inputHelper.KeyPressed(Keys.Space))
        {
            while (CheckInput() != 2)
            {
                oldblockPosition = blockPosition;
                blockPosition.Y += 1;
            }
            blockPosition = oldblockPosition;
            SetBlock();
            Set = true; 
        }
        else if (inputHelper.KeyPressed(Keys.Down))
        {
            blockPosition.Y += 1;
        }
        else if (inputHelper.KeyPressed(Keys.A))
        {
            blockLayout = Rotate(blockLayout, false);
        }
        else if (inputHelper.KeyPressed(Keys.D))
        {
            blockLayout = Rotate(blockLayout, true);
        }
        if (CheckInput() == 1)
        {
            blockLayout = oldblockLayout;
            blockPosition = oldblockPosition;
        }
        else if (CheckInput() == 2 && !Set)
        {
            blockLayout = oldblockLayout;
            blockPosition = oldblockPosition;
            SetBlock();
        }
    }

    public void SetBlock()
    {
        for (int y = 0; y < blockLayout.GetLength(0); y++)
        {
            for (int x = 0; x < blockLayout.GetLength(0); x++)
            {
                if (blockLayout[x, y])
                {
                    latestGrid[x + (int)blockPosition.X, y + (int)blockPosition.Y] = BlockColor;
                    
                }
            }
        }
        TetrisGame.GameWorld.SpawnBlock();
        randomRotate = true;
        TetrisGame.GameWorld.TetrisGrid.Grid = latestGrid;
        TetrisGame.GameWorld.TetrisGrid.CheckGrid();
    }
    public int CheckInput()
    {
        Color[,] Grid = new Color[10, 21];
        Grid = TetrisGame.GameWorld.TetrisGrid.Grid;
        for (int y = 0; y < blockLayout.GetLength(0); y++)
        {
            for (int x = 0; x < blockLayout.GetLength(0); x++)
            {
                if (blockLayout[x, y])
                {
                    if (x + blockPosition.X > 9 || x + blockPosition.X < 0)
                    {
                        return 1;
                    }
                    if (Grid[x + (int)blockPosition.X, y + (int)blockPosition.Y] != Color.White)
                    {
                        return 2;
                    }
                }
            }
        }
        return 0;
    }

   
    public bool[,] Rotate(bool[,] block, bool clockwise)
    {
        int dim = block.GetLength(0);
        bool[,] nblock = new bool[dim, dim];

        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
            {
                if (clockwise)
                    nblock[j, i] = block[i, dim - 1 - j];
                else
                    nblock[j, i] = block[dim - 1 - i, j];
            }

        return nblock;
    }
    public bool ControlledBlock
    {
        set { controlledBlock = value; }
    }
}

class I_Block : TetrisBlock
{
    private bool[,] blockLayoutI;
    
    public I_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[4, 4];
        oldblockLayout = new bool[4, 4];
        blockLayoutI = new bool[4, 4]
        {
            {false, false, false, false},
            {true, true, true, true},
            {false, false, false, false},
            {false, false, false, false}
        };
        BlockLayout = blockLayoutI;
    }
}

class T_Block : TetrisBlock
{
    private bool[,] blockLayoutT;
    public T_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[3, 3];
        oldblockLayout = new bool[3, 3];
        blockLayoutT = new bool[3, 3]
        {
            {false, true, false},
            { true, true, true},
            {false, false, false},
        };
        BlockLayout = blockLayoutT;
    }
}

class S_Block : TetrisBlock
{
    private bool[,] blockLayoutS;
    public S_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[3, 3];
        oldblockLayout = new bool[3, 3];
        blockLayoutS = new bool[3, 3]
        {
            {false, true, true},
            { true, true, false},
            {false, false, false},
        };
        BlockLayout = blockLayoutS;
    }   
}

class L_Block : TetrisBlock
{
    private bool[,] blockLayoutL;
    public L_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[3, 3];
        oldblockLayout = new bool[3, 3];
        blockLayoutL = new bool[3, 3]
        {
            {false, false, true},
            { true, true, true},
            {false, false, false},
        };
        BlockLayout = blockLayoutL;
    }

}

class Z_Block : TetrisBlock
{
    private bool[,] blockLayoutZ;

    public Z_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[3, 3];
        oldblockLayout = new bool[3, 3];
        blockLayoutZ = new bool[3, 3]
        {
            {true, true, false},
            { false, true, true},
            {false, false, false},
        };
        BlockLayout = blockLayoutZ;
    }
}

class O_Block : TetrisBlock
{
    private bool[,] blockLayoutO;
    public O_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[2, 2];
        oldblockLayout = new bool[2, 2];
        blockLayoutO = new bool[2, 2]
        {
            {true, true},
            {true, true},
           
        };
        BlockLayout = blockLayoutO;
    }
}

class J_Block : TetrisBlock
{
    private bool[,] blockLayoutJ;
    public J_Block(int blockColor, bool controlled) : base(blockColor, controlled)
    {
        blockLayout = new bool[3, 3];
        oldblockLayout = new bool[3, 3];
        blockLayoutJ = new bool[3, 3]
        {
            {true, false, false},
            { true, true, true},
            {false, false, false},
        };
        BlockLayout = blockLayoutJ;
    }
}