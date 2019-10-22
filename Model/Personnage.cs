using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardBros.Model
{

    public class Personnage : CollisionObject
    {
        protected int lives;
        protected double health;
        protected int frame = 0;
        int memsens;

        public int sens { get; set; }
        public int timerframe;
        public int posXAbsolue { get; set; }
        public bool jumping;
        public bool canJump;

        public Personnage(int posX, int posY, int h, int w, int nblives) : base(posX, posY, h, w)
        {
            lives = nblives;
            canJump = true;
            health = 100;
            posXAbsolue = 270;
        }

        public int getFrame()
        {
            return frame;
        }
        public double getHealth()
        {
            return health;
        }
        public void resetHealth()
        {
            health=100;
        }
        public void loseHealth(int damage)
        {
            health -= damage;
        }
        public int getLives()
        {
            return lives;
        }
        public void addLives(int nb)
        {
            lives += nb;
        }

        public override Rectangle getRectangle()
        {
            return new Rectangle(posX + 16, posY+18, 30, 42);
        }
        public Rectangle getRectangleRight()
        {
            return new Rectangle(posX + 45, posY + 1, 2, 59);
        }
        public Rectangle getRectangleLeft()
        {
            return new Rectangle(posX + 12, posY + 1, 2, 59);
        }
        public Rectangle getRectangleBot()
        {
            return new Rectangle(posX + 18, posY + 58, 26, 2);
        }
        public Rectangle getRectangleTop()
        {
            return new Rectangle(posX + 18, posY + 20, 26, 2);
        }

        public void ResetPos()
        {
            posX = (WizardBrosGame.width / 2) - 30;
            posY = WizardBrosGame.height - 120;
        }

        public void UpdateFrame()
        {
            timerframe--;
            if (timerframe !=0 && sens==memsens)
                return;
            else
                timerframe = 6;

            if (sens == 0){
                switch (frame)
                {
                    case (2):
                        frame = 3;
                        break;
                    case (3):
                    default:
                        frame = 2;
                        break;
                }
            }else{
                switch (frame)
                {
                    case (4):
                        frame = 5;
                        break;
                    case (5):
                    default:
                        frame = 4;
                        break;
                }
            }
            memsens = sens;
        }
    }
}
