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
    /*MouseHandler:
     * Provides for mouse control and button interaction
     */
    class mouseHandler
    {
        private Texture2D cursorTexture;
        public Vector2 position;
        private MouseState mouseState;
        private MouseState prevState;

        //public mouseHandler(Vector2 position, Texture2D cursorTexture)
        //{
        //    this.position = position; 
        //    this.cursorTexture = cursorTexture; 
        //}

        public mouseHandler(Texture2D cursorTexture)
        {
            this.cursorTexture = cursorTexture;
            mouseState = Mouse.GetState();
            position.X = mouseState.X;
            position.Y = mouseState.Y;
        }
            
        
        public void Update()
        {
            prevState = mouseState;
            mouseState = Mouse.GetState();
            if (mouseState.X > 800)
                this.position.X = 800;
            else if (mouseState.X < 0)
                this.position.X = 0;
            else
                this.position.X = mouseState.X;
            if (mouseState.Y > 600)
                this.position.Y = 600;
            else if (mouseState.Y < 0)
                this.position.Y = 0;
            else
                this.position.Y = mouseState.Y;
        }
        
        public void Draw(SpriteBatch batch) 
        {
            batch.Draw(this.cursorTexture, this.position, Color.White); 
        }
       
        public bool overObject(node Node)
        {
            if (this.position.X >= Node.drawRectangle.X && this.position.Y >= Node.drawRectangle.Y && this.position.Y < Node.drawRectangle.Bottom && this.position.X < Node.drawRectangle.Right)
                return true;
            else
                return false;
        }
        public bool overObject(Button button)
        {
            if (this.position.X >= button.drawRectangle.X && this.position.Y >= button.drawRectangle.Y && this.position.Y < button.drawRectangle.Bottom && this.position.X < button.drawRectangle.Right)
                return true;
            else
                return false;
        }
        public bool overObject(Wall wall)
        {
            if (this.position.X >= wall.boundingRectangle.X && this.position.Y >= wall.boundingRectangle.Y && this.position.Y < wall.boundingRectangle.Bottom && this.position.X < wall.boundingRectangle.Right)
                return true;
            else
                return false;
        }
        public MouseState getState()
        {
            return mouseState;
        }
        public MouseState getPrevState()
        {
            return prevState;
        }
    }
}
