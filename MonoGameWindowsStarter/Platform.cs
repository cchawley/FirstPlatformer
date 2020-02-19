﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    public class Platform : IBoundable
    {
        BoundingRectangle bounds;

        Sprite sprite;

        int tileCount;
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Constructs a new platform
        /// </summary>
        /// <param name="bounds">The platform's bounds</param>
        /// <param name="sprite">The platform's sprite</param>
        public Platform(BoundingRectangle bounds, Sprite sprite)
        {
            this.bounds = bounds;
            this.sprite = sprite;
            tileCount = (int)bounds.Width / sprite.Width;
        }

        /// <summary>
        /// Draws the platform
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to render to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if Debug
            VisualDebugging.DrawRectangle(spriteBatch, bounds, Color.Green);
#endif
            for (int i = 0; i < tileCount; i++)
            {
                sprite.Draw(spriteBatch, new Vector2(bounds.X + i * sprite.Width, bounds.Y), Color.White);
            }
        }
    }

}

