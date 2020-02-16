using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MonoGameWindowsStarter
{

    enum PlayerState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    public class Player
    {
        // The speed of the walking animation
        const int FRAME_RATE = 500;

        // The duration of a player's jump, in milliseconds
        const int JUMP_TIME = 500;

        // The player sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The player's animation state
        PlayerState animationState = PlayerState.Idle;

        // The player's speed
        int speed = 3;

        // If the player is jumping
        bool jumping = false;

        // If the player is falling 
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

        /// <summary>
        /// Gets and sets the position of the player on-screen
        /// </summary>
        public Vector2 Position = new Vector2(200, 200);

        /// <summary>
        /// Constructs a new player
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public Player(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = PlayerState.Idle;
        }

        /// <summary>
        /// Updates the player, applying movement and physics
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            // Vertical movement
            if (jumping)
            {
                jumpTimer += gameTime.ElapsedGameTime;
                // TODO: Replace jumping with platformer physics
                //Position.Y -= speed; // Naive jumping 
                Position.Y -= (250 / (float)jumpTimer.TotalMilliseconds);
                if (jumpTimer.TotalMilliseconds >= JUMP_TIME)
                {
                    jumping = false;
                    falling = true;
                }
            }
            if (falling)
            {
                Position.Y += speed;
                // TODO: This needs to be replaced with collision logic
                if (Position.Y > 200)  //currently what doesn't allow player to go below a certain limit. Need to put in logic for enemy interaction and platform interaction!
                {
                    Position.Y = 200;
                    falling = false;
                }
            }
            if (!jumping && !falling && keyboard.IsKeyDown(Keys.Space))
            {
                jumping = true;
                jumpTimer = new TimeSpan(0);
            }

            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (jumping || falling) animationState = PlayerState.JumpingLeft;
                else animationState = PlayerState.WalkingLeft;
                Position.X -= speed;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                if (jumping || falling) animationState = PlayerState.JumpingRight;
                else animationState = PlayerState.WalkingRight;
                Position.X += speed;
            }
            else
            {
                animationState = PlayerState.Idle;
            }

            // Apply animations
            switch (animationState)
            {
                case PlayerState.Idle:
                    currentFrame = 0;
                    animationTimer = new TimeSpan(0);
                    break;
                case PlayerState.JumpingLeft:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    currentFrame = 7;
                    break;
                case PlayerState.JumpingRight:
                    spriteEffects = SpriteEffects.None;
                    currentFrame = 7;
                    break;
                case PlayerState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 9 & 10
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE + 9;                                                                                         
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05 )))   //this slight adjustment fixes the issue of seeing a small blip of frame 11 pop up
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    break;
                case PlayerState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE + 9;
                    if (animationTimer.TotalMilliseconds > (FRAME_RATE * 2 - (FRAME_RATE * 0.05)))
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    break;

            }
        }

        /// <summary>
        /// Render the player sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }

}
