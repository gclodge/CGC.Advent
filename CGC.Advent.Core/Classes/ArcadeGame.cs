using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CGC.Advent.Core.Classes
{
    public class ArcadeGrid
    {
        public Dictionary<long, Dictionary<long, long>> Grid { get; private set; } = null;

        public ArcadeGrid()
        {
            this.Grid = new Dictionary<long, Dictionary<long, long>>();
        }

        public void AddTile(long x, long y, long t)
        {
            if (!this.Grid.ContainsKey(x))
            {
                this.Grid.Add(x, new Dictionary<long, long>());
            }

            if (!this.Grid[x].ContainsKey(y))
            {
                this.Grid[x].Add(y, -1);
            }

            this.Grid[x][y] = t;    
        }

        public long CountTiles(long tileVal)
        {
            long count = 0;
            foreach (var x in this.Grid.Keys)
            {
                foreach (var y in this.Grid[x].Keys)
                {
                    if (this.Grid[x][y] == tileVal)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private const long BallValue = 4;
        private const long PaddleValue = 3;
        public long GetBallPosition()
        {
            //< Get the ball position
            var posX = GetPosition(BallValue);
            if(posX != -1)
            {
                return posX;
            }
            else
            {
                //< No ballll?
                throw new Exception("You missed the ball fuccboi");
            }
        }

        public long GetPaddlePosition()
        {
            //< Get the ball position
            var posX = GetPosition(PaddleValue);
            if (posX != -1)
            {
                return posX;
            }
            else
            {
                //< No ballll?
                throw new Exception("You missed the ball fuccboi");
            }
        }

        private long GetPosition(long val)
        {
            foreach (var x in this.Grid.Keys)
            {
                foreach (var y in this.Grid[x].Keys)
                {
                    if (this.Grid[x][y] == val)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
    }

    public class ArcadeGame
    {
        public Intcode Game { get; private set; } = null;
        public ArcadeGrid Grid { get; private set; } = null;

        public long Score { get; private set; } = 0;

        private string Source { get; } = null;
        private int CurrOutput = 0;

        public ArcadeGame(string intcodeSource)
        {
            //< Instantiate the game controller and the grid
            this.Game = new Intcode(intcodeSource);
            this.Grid = new ArcadeGrid();
            //< Hold onto the source path
            this.Source = intcodeSource;
        }

        public void TestGame()
        {
            //< Run the game
            this.Game.Process();
            //< Parse the outputs and add the tiles to the grid
            ParseOutputs();
        }

        public void AddQuarters(long quarters)
        {
            this.Game.SetValue(0, quarters);
        }

        public void RunGame()
        {
            //< Run the game
            this.Game.Process();
            //< Parse the outputs and add the tiles to the grid
            ParseOutputs();

            //< Restart the game
            this.Game = new Intcode(this.Source);
            //< Add a coupla quarters
            this.AddQuarters(2);
            while (!this.Game.IsFinished)
            {
                //< Run until next wait/halt
                this.Game.Process();
                //< Parse outputs
                ParseOutputs();
                //< Update the paddle position
                MovePaddle();
            }
        }

        private void ParseOutputs()
        {
            while (CurrOutput < this.Game.Outputs.Count)
            {
                //< Pull them values out
                var x = this.Game.Outputs[CurrOutput].Item2;
                var y = this.Game.Outputs[CurrOutput + 1].Item2;
                var t = this.Game.Outputs[CurrOutput + 2].Item2;

                if (x == -1 && y == 0)
                {
                    this.Score = t;
                }
                else
                {
                    this.Grid.AddTile(x, y, t);
                }
                CurrOutput += 3;
            }
        }

        private void MovePaddle()
        {
            //< Get the paddle movement
            var offset = GetPaddleMovement();
            //< Add it as an input
            this.Game.AddInput(offset);
        }

        private long GetPaddleMovement()
        {
            //< Get the current ball/paddle positions
            var ballX = this.Grid.GetBallPosition();
            var paddX = this.Grid.GetPaddlePosition();
            //< Find how we need to adjust the paddle
            var diff = ballX - paddX;

            if (diff == 0)
                return 0;
            else if (diff > 0)
                return 1;
            else
                return -1;
        }
    }
}
