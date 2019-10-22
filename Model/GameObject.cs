using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardBros.Model
{
    public class GameObject
    {
        protected int posX;
        protected int posY;
        protected int initposX;
        protected int initposY;
        public GameObject(int posX, int posY)
        {
            this.posX = initposX = posX;
            this.posY = initposY = posY;
        }
        public virtual int getPosX()
        {
            return posX;
        }
        public int getPosY()
        {
            return posY;
        }
        public virtual void setPosX(int posX)
        {
            this.posX = posX;
        }
        public virtual void setPosY(int posY)
        {
            this.posY = posY;
        }
        public void resetPos()
        {
            posX = initposX;
            posY = initposY;
        }

    }
}
