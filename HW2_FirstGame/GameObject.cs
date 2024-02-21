using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HW2_FirstGame
{
    /// <summary>
    /// The base class for the player and collectibles.
    /// </summary>
    internal abstract class GameObject
    {
        // ---- Fields ----
        protected Texture2D texture;
        protected Rectangle position;

        // ---- Properties -----
        public Rectangle Position
        {
            get { return position; }
        }

        // ---- Constructor ----
        public GameObject(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
        }

        // ---- Methods ----
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, Color.White);
        }

        public abstract void Update(GameTime gameTime);
    }
}
