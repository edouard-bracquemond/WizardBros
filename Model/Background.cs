using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardBros.Model
{
    public class Background : GameObject
    {
        private int secondPosX;
        private int largeur;
        private int hauteur;


        public Background(int posX, int posY, int largeur,int hauteur) : base(posX,posY){
            
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.secondPosX = largeur;
      
        }

      
        public int getSecondPosX()
        {
            return secondPosX;
        }


        public void setSecondPosX(int secondPosX)
        {
            this.secondPosX = secondPosX;
        }

       
        public int getLargeur()
        {
            return largeur;
        }
        public int getHauteur()
        {
            return hauteur;
        }

    }
}
