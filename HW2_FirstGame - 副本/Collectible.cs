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
    internal class Collectible : GameObject
    {
        // ---- Constructor ----
        public Collectible(Texture2D texture, Rectangle position) : base (texture, position)
        { 
            this.position = position;
            this.texture = texture;
        }

        // ---- Methods ----
        public bool CheckCollision(GameObject otherObject)
        {
            return position.Intersects(otherObject.Position);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime);
        }
    }
}
