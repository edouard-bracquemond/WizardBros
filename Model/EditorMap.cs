using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardBros.Controller;

namespace WizardBros.Model
{
    public class EditorMap
    {
        Background bg;
        int screenW;
        List<CollisionObject> blocks;
        public int typeSelect { get; set; }
        public EditorMap(int screenW, int screenH)
        {
            blocks = new List<CollisionObject>();

            this.screenW = screenW;
            bg = new Background(1, 1, screenW, screenH);
            typeSelect = 1;
        }
        public void addBlock(int posX, int posY)
        {
            int x = 0;
            bool existe = false;

            if (blocks.Count == 0)
            {
                for (int i = 60; i <= 660; i = i + 60)// dÃ©termine posX de l'Ã©lÃ©ment
                {
                    if (posX < i)
                    {
                        x = i - 60;
                        break;
                    }
                }
                for (int i = 600; i >= 0; i = i - 60)//dÃ©termine la positionY de l'Ã©lÃ©ment
                {
                    if (posY > i)
                    {

                        if (typeSelect > 6)
                        {
                            blocks.Add(new Enemy(x, i, 120, 60, typeSelect, 20, 1));
                        }
                        else
                        {
                            blocks.Add(new CollisionObject(x, i, 60, 60, typeSelect));
                        }

                        break;
                    }


                }

            }
            else
            {

                int decalage = blocks.First().getPosX() % 60;// nb bloc dÃ©calage par rapport au premier bloc posÃ©
                for (int i = 0 + decalage; i < 661; i = i + 60) // dÃ©termine posX
                {
                    if (posX < i)
                    {

                        x = i - 60;
                        break;
                    }
                }


                for (int i = 600; i >= 0; i = i - 60)  //determine posY
                {
                    if (posY > i)
                    {
                        foreach (CollisionObject element in blocks)
                        {
                            if (typeSelect > 6 && element.getPosX() - 60 == x && element.getPosY() == i)
                            {
                                existe = true;
                                break;
                            }
                 
                            if (element.getPosX() == x && element.getPosY() == i)
                            {
                                existe = true;
                                break;
                            }
                            if (element is Enemy && element.getPosX() == x - 60 && element.getPosY() == i)
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            if (typeSelect > 6)
                            {
                                blocks.Add(new Enemy(x, i, 60, 60, typeSelect, 20, 1));
                            }
                            else
                            {
                                blocks.Add(new CollisionObject(x, i, 60, 60, typeSelect));
                            }

                        }


                        break;

                    }

                }
            }

        }

        public void removeBlock(int posX, int posY)
        {
            int x = 0;
            int index = -1;

            if (blocks.Count != 0)
            {

                int decalage = blocks.First().getPosX() % 60;
                for (int i = 0 + decalage; i < 661; i = i + 60)
                {
                    if (posX < i)
                    {
                        x = i - 60;
                        break;
                    }
                }


                for (int i = 600; i >= 0; i = i - 60)
                {
                    if (posY > i)
                    {
                        foreach (CollisionObject element in blocks)
                        {
                            if (element.getPosX() == x && element.getPosY() == i)
                            {
                                index = blocks.IndexOf(element);
                                break;
                            }
                            if (element is Enemy && element.getPosX() + 60 == x && element.getPosY() == i)
                            {
                                index = blocks.IndexOf(element);
                                break;
                            }
                        }

                        break;

                    }

                }
                if (index != -1)
                {
                    blocks.RemoveAt(index);
                }
            }

        }
        public bool droitAdd(Vector2 v)
        {

            foreach (CollisionObject elem in blocks)
            {
                if (typeSelect > 6 && elem.getPosX() - 60 == v.X && elem.getPosY() == v.Y)
                {
                    return true;
                }
                if (elem is Enemy && typeSelect > 6 && elem.getPosX() - 120 == v.X && elem.getPosY() == v.Y)
                {
                    return true;
                }
                if (elem.getPosX() == v.X && elem.getPosY() == v.Y)
                {
                    return true;
                }
                if (elem is Enemy && elem.getPosX() + 60 == v.X && elem.getPosY() == v.Y)
                {
                    return true;
                }
            }
            return false;
        }
        public Vector2 previsualiser(int posX, int posY)
        {
            int x = 0;


            if (blocks.Count == 0)
            {
                for (int i = 60; i <= 660; i = i + 60)
                {
                    if (posX < i)
                    {
                        x = i - 60;
                        break;
                    }
                }
                for (int i = 600; i >= 0; i = i - 60)
                {
                    if (posY > i)
                    {


                        return new Vector2(x, i);





                    }


                }
                return new Vector2(1000, 1000);
            }
            else
            {

                int decalage = blocks.First().getPosX() % 60;
                for (int i = 0 + decalage; i < 661; i = i + 60)
                {
                    if (posX < i)
                    {
                        x = i - 60;
                        break;
                    }
                }


                for (int i = 600; i >= 0; i = i - 60)
                {
                    if (posY > i)
                    {

                        return new Vector2(x, i);



                    }




                }
                return new Vector2(1000, 1000);

            }



        }
        public void SaveMap()
        {
            if (blocks.Count > 0)
            {
                int decalage;

                blocks.Sort((a, b) => a.getPosX() - b.getPosX());

                if (blocks.First().getPosX() < 0)
                {
                    decalage = (blocks.First().getPosX());

                }
                else
                {
                    decalage = (-blocks.First().getPosX());
                }

                foreach (CollisionObject element in blocks)
                {
                    element.setPosX(element.getPosX() + decalage);
                    Console.WriteLine(element.getPosX());
                }
                MapXml.MaptoXml(blocks); //Sauvegarde la map dans le fichier Xml
            }
        }

        public Background getBackground()
        {
            return bg;
        }

        public List<CollisionObject> getBlocks()
        {
            return blocks;
        }

        public void moveRight()
        {


            if (bg.getSecondPosX() == 1)
            {
                bg.setPosX(bg.getLargeur());
            }
            else if (bg.getPosX() == 1)
            {
                bg.setSecondPosX(bg.getLargeur());
            }

            bg.setPosX(bg.getPosX() - 1);
            bg.setSecondPosX(bg.getSecondPosX() - 1);
            foreach (CollisionObject element in blocks)
            {
                element.setPosX(element.getPosX() - 3);
            }
        }

        public void moveLeft()
        {

            if (bg.getSecondPosX() == 1)
            {
                bg.setPosX(-bg.getLargeur());
            }
            else if (bg.getPosX() == 1)
            {
                bg.setSecondPosX(-bg.getLargeur());
            }

            bg.setPosX(bg.getPosX() + 1);
            bg.setSecondPosX(bg.getSecondPosX() + 1);
            foreach (CollisionObject element in blocks)
            {
                element.setPosX(element.getPosX() + 3);
            }

        }
    }
}



