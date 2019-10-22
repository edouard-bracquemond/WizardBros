using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WizardBros.Model;
using WizardBros.View;
using Microsoft.Xna.Framework.Graphics;
using static WizardBros.WizardBrosGame;

namespace WizardBros.Controller
{
    public class EditorMapController
    {
        public static bool mouse = true;
        int timer = 0;
        bool nomouse;


        public GameState Update(EditorMap editor, GameState currentState, ref MouseState oldMouseState, ref KeyboardState oldKeyboardState)
        {
            KeyboardState kbstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            if (!nomouse) //parce que la molette marche par valeur pas par mouvement
            {
                editor.typeSelect = Math.Abs(mstate.ScrollWheelValue / 120) % 7 + 1;
            }
            int x = mstate.X;
            int y = mstate.Y;

            if (kbstate.IsKeyDown(Keys.Escape) || kbstate.IsKeyDown(Keys.Back))
            {
                editor.getBlocks().RemoveAll((a) => a is CollisionObject);
                currentState = GameState.Start;
            }
            else
            {
                if (kbstate.IsKeyDown(Keys.D) || kbstate.IsKeyDown(Keys.Right))
                    editor.moveRight();
                if (kbstate.IsKeyDown(Keys.Q) || kbstate.IsKeyDown(Keys.Left))
                    editor.moveLeft();
                if (kbstate.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up)) //autre moyen de choisir le type (non cumulables)
                {
                    if (editor.typeSelect <= 7)
                    {
                        editor.typeSelect++;
                    }
                    else
                    {
                        editor.typeSelect = 1;
                    }
                    nomouse = true;
                }

                if (kbstate.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down))
                {
                    if (editor.typeSelect > 1)
                    {
                        editor.typeSelect--;
                    }
                    else
                    {
                        editor.typeSelect = 7;
                    }
                    nomouse = true;
                }

                if (kbstate.IsKeyDown(Keys.A) && kbstate.IsKeyUp(Keys.Q) && kbstate.IsKeyUp(Keys.D) || (oldMouseState.LeftButton == ButtonState.Released && mstate.LeftButton == ButtonState.Pressed))
                {
                    editor.addBlock(x, y);
                }

                if (kbstate.IsKeyDown(Keys.Delete) && kbstate.IsKeyUp(Keys.Q) && kbstate.IsKeyUp(Keys.D) || (oldMouseState.RightButton == ButtonState.Released && mstate.RightButton == ButtonState.Pressed))
                {
                    editor.removeBlock(x, y);
                }

                if (kbstate.IsKeyDown(Keys.X) || kbstate.IsKeyDown(Keys.Enter))
                {
                    editor.SaveMap();
                    currentState = GameState.Start;
                }
            }
               

            oldKeyboardState = kbstate;
            oldMouseState = mstate;
            return currentState;
        }


    }
}
