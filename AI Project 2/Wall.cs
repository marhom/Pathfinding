using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AI_Project_2
{
    class Wall
    {
        public Rectangle boundingRectangle;
        public Texture2D wallTexture;

        public Wall(Texture2D wallTexture, Rectangle boundingRectangle)
        {
            this.wallTexture = wallTexture;
            this.boundingRectangle = boundingRectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(wallTexture, boundingRectangle, Color.White);
        }



    }
}
