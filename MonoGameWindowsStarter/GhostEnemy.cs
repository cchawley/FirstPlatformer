﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    enum GhostState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    public class GhostEnemy
    {
        // The speed of the walking animation
        const int FRAME_RATE = 500;

        

        // The ghost sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The ghost's animation state
        GhostState animationState = GhostState.WalkingLeft;

        // The ghost's speed
        int speed = 3;

        
        bool jumping = false;

        
        bool falling = false;

        // A timer for jumping
        TimeSpan jumpTimer;

        // A timer for animations
        TimeSpan animationTimer;

        // The currently applied SpriteEffects
        SpriteEffects spriteEffects = SpriteEffects.None;

        // The color of the sprite
        Color color = Color.White;

        // The origin of the sprite (centered on its feet)
        Vector2 origin = new Vector2(10, 21);

        public Vector2 Position = new Vector2(1, 600);

        /// <summary>
        /// Constructs a new ghost
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public GhostEnemy(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = GhostState.WalkingLeft;
        }

        /// <summary>
        /// Updates the ghost, applying movement and physics
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            if(animationState == GhostState.WalkingLeft)
            {
                Position.X -= speed;
                if (Position.X - 16 < 0)
                {
                    Position.X = 16;
                    animationState = GhostState.WalkingRight;
                }
            }
            else if(animationState == GhostState.WalkingRight)
            {
                Position.X += speed;
                if (Position.X + 20 > 1600)
                {
                    Position.X = 1580;
                    animationState = GhostState.WalkingLeft;
                }
            }

            switch (animationState)
            {
                case GhostState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 0 & 1                                                                                                            
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05)))   //this slight adjustment fixes the issue of seeing a small blip of frame 11 pop up
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE;
                    break;
                case GhostState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 0 & 1                                                                                                            
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05)))   //this slight adjustment fixes the issue of seeing a small blip of frame 11 pop up
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE;
                    break;
            }

        }

        /// <summary>
        /// Render the ghost sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }
}
