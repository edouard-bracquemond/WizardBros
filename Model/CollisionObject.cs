using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardBros.Model
{
    public class CollisionObject : GameObject
    {
        protected int h;
        protected int w;
        protected bool kill; 
        public int type { get; private set; }

        public CollisionObject(int x, int y, int h, int w) : base(x, y)
        {
            this.h = h;
            this.w = w;
            kill = false;
        }
        public CollisionObject(int posX, int posY, int h, int w, int typeObject) : base(posX, posY)
        {
            this.h = h;
            this.w = w;
            type = typeObject;
        }

        public int getH()
        {
            return this.h;
        }
        public int getW()
        {
            return this.w;

        }
        public virtual Rectangle getRectangle()
        {
            return new Rectangle(posX, posY, w, h);
        }

        public void destroy()
        {
            kill = true;
        }
        public bool isdead()
        {
            return kill;
        }
        public bool hasBlockUnder(List<CollisionObject> list)
        {
            foreach (CollisionObject element in list)
            {
                if(posY == element.posY - 60 && posX==element.posX)
                {
                    return true;
                }
            }
            return false;
        }
        public bool hasBlockOver(List<CollisionObject> list)
        {
            foreach (CollisionObject element in list)
            {
                if (posY == element.posY + 60 && posX == element.posX)
                {
                    return true;
                }
            }
            return false;
        }
        public bool hasBlockToTheRight(List<CollisionObject> list)
        {
            foreach (CollisionObject element in list)
            {
                if (posY == element.posY && posX == element.posX - 60)
                {
                    return true;
                }
            }
            return false;
        }
        public bool hasBlockToTheLeft(List<CollisionObject> list)
        {
            foreach (CollisionObject element in list)
            {
                if (posY == element.posY && posX == element.posX + 60)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
