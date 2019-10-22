using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WizardBros.Model
{
    public class Projectile : CollisionObject
    {
        Vector2 position;
        Vector2 aim;
        public Vector2 vector;
        float angle = 0;
        int speed;
        int power;
        public Color color;
        int timer = 0;

        public float Angle { get { return angle; } set { angle = value; } }

        public Projectile(int x, int y, int h, int w, Vector2 mouse, int vit, int pow) : base(x, y, h, w)
        {
            aim = mouse; // pointeur souris
            position = new Vector2(x, y); //coordonnées proj
            vector = aim - position; //orientation vecteur direction
            vector.Normalize(); //pour que la longueur de vector n'influe pas sur la vitesse
            speed = vit;
            power = pow;
            color = Color.Gray;
        }

        public Vector2 getPos()
        {
            return position;
        }
        public void setPos(Vector2 pos)
        {
            position = pos;
        }
        public override int getPosX()
        {
            return (int)position.X;
        }
        public override void setPosX(int x)
        {
            position.X = posX = x;
        }
        public Vector2 getAim()
        {
            return aim;
        }
        public Color getColor()
        {
            return color;
        }
        public int getTimer()
        {
            return timer;
        }

        public Rectangle getRectangleTop()
        {
            if (Angle < -2.20)
            {
                return new Rectangle(posX - 25, posY - 20, 15, 15);
            }
            if (Angle < -1.60 && Angle > -2.20)
            {
                return new Rectangle(posX - 15, posY - 30, 15, 15);
            }
            if (Angle < -1.0 && Angle > -1.60)
            {
                return new Rectangle(posX, posY - 30, 15, 15);
            }
            if (Angle < -0.6 && Angle > -1.0)
            {
                return new Rectangle(posX + 10, posY - 30, 15, 15);
            }
            else if (Angle > -0.6 && Angle < 0.0)
            {
                return new Rectangle(posX + 20, posY - 15, 15, 15);
            }
            else return new Rectangle(0, 0, 0, 0);
        }
        public Rectangle getRectangleBot()
        {
            if (Angle > 2.20)
            {
                return new Rectangle(posX - 25, posY + 5, 15, 15);
            }
            if (Angle > 1.60 && Angle < 2.20)
            {
                return new Rectangle(posX - 10, posY + 15, 15, 15);
            }
            if (Angle > 1.0 && Angle < 1.60)
            {
                return new Rectangle(posX - 10, posY + 15, 15, 15);
            }
            if (Angle > 0.6 && Angle < 1.0)
            {
                return new Rectangle(posX + 15, posY + 10, 15, 15);
            }
            if (Angle > 0 && Angle < 0.6)
            {
                return new Rectangle(posX + 15, posY, 15, 15);
            }
            else
                return new Rectangle(0, 0, 0, 0);
        }
        public Rectangle getRectangleLeft()
        {
            if (Angle > 1.5 && Angle < 2.2)
            {
                return new Rectangle(posX - 15, posY + 10, 15, 15);
            }
            if (Angle > 2.2 && Angle < 2.8)
            {
                return new Rectangle(posX - 15, posY + 10, 15, 15);
            }
            if (Math.Abs(Angle) > 2.8)
            {
                return new Rectangle(posX - 35, posY - 5, 15, 15);
            }
            if (Angle < -2.2 && Angle > -2.8)
            {
                return new Rectangle(posX - 25, posY - 20, 15, 15);
            }
            if (Angle > -2.2 && Angle < -1.5)
            {
                return new Rectangle(posX - 20, posY - 30, 15, 15);
            }
            return new Rectangle(0, 0, 0, 0);
        }
        public Rectangle getRectangleRight()
        {
            if (Angle > -1.57 && Angle < -1.0)
            {
                return new Rectangle(posX + 5, posY - 30, 15, 15);
            }
            if (Angle > -1.0 && Angle < -0.4)
            {
                return new Rectangle(posX + 15, posY - 25, 15, 15);
            }
            if (Angle > -0.4 && Angle < 0.2)
            {
                return new Rectangle(posX + 20, posY - 5, 15, 15);
            }
            if (Angle > 0.2 && Angle < 0.8)
            {
                return new Rectangle(posX + 20, posY + 15, 15, 15);
            }
            if (Angle > 0.8 && Angle < 1.5)
            {
                return new Rectangle(posX + 5, posY + 15, 15, 15);
            }
            else return new Rectangle(0, 0, 0, 0);
        }

        public void updatePos()
        {
            position += vector * speed;
            posX = (int)position.X;
            posY = (int)position.Y;
            angle = VectorToAngle(vector);
            timer++;
            if (timer > 20 && color == Color.Gray)
                color = Color.White;
        }
        float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public void bounceX()
        {
            vector.X = -vector.X;
            position += vector * 3;
            posX = (int)position.X;
            posY = (int)position.Y;
            color = Color.Yellow;
        }
        public void bounceY()
        {
            vector.Y = -vector.Y;
            position += vector * 3;
            posX = (int)position.X;
            posY = (int)position.Y;
            color = Color.Yellow;
        }
    }
}
