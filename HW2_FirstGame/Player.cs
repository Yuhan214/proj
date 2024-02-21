using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HW2_FirstGame
{
    /// <summary>
    /// Inherits from GameObject.
    /// </summary>
    internal class Player : GameObject
    {
        // ---- Fields ----
        private int levelScore;  // Score of the current level
        private int totalScore;  // Player's total score
        private int windowHeight;
        private int windowWidth;

        // ---- Properties ----
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        // ---- Consturctor ----
        public Player(Texture2D texture, Rectangle position, int windowWidth, int windowHeight) 
                        : base (texture, position)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.levelScore = 0;
            this.totalScore = 0;
        }

        // ---- Methods ----
        public override void Update(GameTime gameTime)
        {
            // Get the current state of the keyboard
            KeyboardState state = Keyboard.GetState();

            int speed = 50; 
            int movementX = 0;
            int movementY = 0;

            if (state.IsKeyDown(Keys.Up)) movementY -= speed;
            if (state.IsKeyDown(Keys.Down)) movementY += speed;
            if (state.IsKeyDown(Keys.Left)) movementX -= speed;
            if (state.IsKeyDown(Keys.Right)) movementX += speed;
            
            position.X += movementX;
            position.Y += movementY;

            if (position.X < 0)
            {
                position.X = windowWidth - position.Width;
            }
            else if (position.X + position.Width > windowWidth)
            {
                position.X = 0;
            }

            if (position.Y < 0)
            {
                position.Y = windowHeight - position.Height;
            }
            else if (position.Y + position.Height > windowHeight)
            {
                position.Y = 0;
            }
        }

        public void Center()
        {
            position.X = (windowWidth / 2) - (position.Width / 2);
            position.Y = (windowHeight / 2) - (position.Height / 2);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
