
using Microsoft.Xna.Framework.Input;
using WizardBros.Model;
using static WizardBros.WizardBrosGame;
using WizardBros.View;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace WizardBros.Controller
{
    public class MainController
    {
        public static bool mouse = true;
        int timer = 0;

        public GameState Update(World niveau, GameState currentState, ref MouseState oldMouseState, ref KeyboardState oldKeyboardState, int posX, int posY)
        {
            KeyboardState kbstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            int x = mstate.X;
            int y = mstate.Y;

            timer++;
            if (oldKeyboardState.IsKeyUp(Keys.P) && kbstate.IsKeyDown(Keys.P))
            {
                currentState = GameState.Paused;

            }
            else if (!niveau.spawnlock)
            {
                if ((kbstate.IsKeyDown(Keys.D) || kbstate.IsKeyDown(Keys.Right)) && kbstate.IsKeyUp(Keys.Q) && kbstate.IsKeyUp(Keys.Left) && !mouse || ( mouse && x > posX+50 && kbstate.IsKeyUp(Keys.LeftShift)))
                {
                    niveau.moveRight();
                    niveau.getJoueur().sens = 0;
                    niveau.getJoueur().UpdateFrame();
                }
                if ((kbstate.IsKeyDown(Keys.Q) || kbstate.IsKeyDown(Keys.Left)) && kbstate.IsKeyUp(Keys.D) && kbstate.IsKeyUp(Keys.Right) && !mouse || ( mouse && x < posX && kbstate.IsKeyUp(Keys.LeftShift))) //shift pour viser en mode souris (déconseillé)
                {
                    niveau.moveLeft();
                    niveau.getJoueur().sens = 1;
                    niveau.getJoueur().UpdateFrame();
                }
                if (kbstate.IsKeyDown(Keys.Z) && oldKeyboardState.IsKeyUp(Keys.Z)  && timer > 50)
                {
                    niveau.fire(mstate.X, mstate.Y);
                    timer = 0;
                }
                if ( (kbstate.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space) && !mouse) || (mstate.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && mouse))
                {
                    if (niveau.getJoueur().canJump)
                    {
                        niveau.getJoueur().canJump = false;
                        niveau.getJoueur().jumping = true;
                        niveau.jumpCharge++;
                    }
                }
                if ((kbstate.IsKeyDown(Keys.Space) || mstate.LeftButton == ButtonState.Pressed) && niveau.jumpCharge > 0)
                {
                    if (niveau.jumpCharge < 41)
                        niveau.jumpCharge++;
                }

                if (niveau.getJoueur().jumping) // si le perso est en train de sauter
                    niveau.JumpSpeed();
                else
                    niveau.Gravity();

                if (niveau.completed)
                {
                    View.NiveauxView.completed[WizardBrosGame.level] = true;
                    currentState = GameState.Niveau;
                }

                if (niveau.getJoueur().getPosY() > height || niveau.getJoueur().getHealth() <= 0) //mort
                {
                    niveau.getJoueur().addLives(-1);
                    if (niveau.getJoueur().getLives() < 0)
                    {
                        currentState = GameState.GameOver;
                    }
                    else currentState = GameState.Death;
                }

                niveau.UpdateWorld();
                niveau.getHUD().UpdateHearts();

            }

            else
            { //spawnlock
                niveau.Gravity();
                if (niveau.getJoueur().getPosY() > height || niveau.getJoueur().getHealth() <= 0) 
                {
                    niveau.getJoueur().addLives(-1);
                    if (niveau.getJoueur().getLives() < 0)
                    {
                        currentState = GameState.GameOver;
                    }
                    else currentState = GameState.Death;
                }
            }

            oldKeyboardState = kbstate;
            oldMouseState = mstate;
            return currentState;
        }

    }
}
