using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WizardBros.Model
{
    public static class MapXml
    {
        public static void MaptoXml(List<CollisionObject> blocks)
        {

            XmlDocument xmlDoc = new XmlDocument();
            String filename = "mapDuJeu.xml";
            XmlNode niveauNode;


            List<int> niveaux = new List<int>();
            int max;
            niveaux = getListNiveauxName();
            if (niveaux.Count == 0)
            {
                max = 0;
            }
            else
            {
                max = niveaux.Max();
            }
            xmlDoc.Load(filename);
            niveauNode = xmlDoc.CreateElement("Niveau");
            XmlAttribute niveauID = xmlDoc.CreateAttribute("id");


            niveauID.Value = (max + 1).ToString();
            niveauNode.Attributes.Append(niveauID);
            xmlDoc.DocumentElement.AppendChild(niveauNode);
            niveauNode.Attributes.Append(niveauID);
            XmlNode blocksNode = xmlDoc.CreateElement("Blocks");
            niveauNode.AppendChild(blocksNode);
            for (int i = 0; i < blocks.Count; i++)
            {
                XmlNode block = xmlDoc.CreateElement("Block");
                XmlAttribute id = xmlDoc.CreateAttribute("id");
                id.Value = i.ToString();
                block.Attributes.Append(id);
                XmlNode type = xmlDoc.CreateElement("Type");
                XmlNode posX = xmlDoc.CreateElement("X");
                XmlNode posY = xmlDoc.CreateElement("Y");
                XmlNode hauteur = xmlDoc.CreateElement("Hauteur");
                XmlNode largeur = xmlDoc.CreateElement("Largeur");
                if (blocks[i] is Enemy)
                {
                    type.InnerText = (blocks[i].type + 10).ToString();
                }
                else
                {
                    type.InnerText = blocks[i].type.ToString();
                }


                posX.InnerText = blocks[i].getPosX().ToString();
                posY.InnerText = blocks[i].getPosY().ToString();
                hauteur.InnerText = blocks[i].getH().ToString();
                largeur.InnerText = blocks[i].getW().ToString();
                block.AppendChild(type);
                block.AppendChild(posX);
                block.AppendChild(posY);
                block.AppendChild(hauteur);
                block.AppendChild(largeur);
                blocksNode.AppendChild(block);
                xmlDoc.DocumentElement.AppendChild(niveauNode);
            }

            /* else
             {
                 rootNode = xmlDoc.CreateElement("Niveaux");
                 xmlDoc.AppendChild(rootNode);
                 niveauNode = xmlDoc.CreateElement("Niveau");
                 XmlAttribute niveauID = xmlDoc.CreateAttribute("id");
                 niveauID.Value = "0";
                 niveauNode.Attributes.Append(niveauID);
                 XmlNode blocksNode = xmlDoc.CreateElement("Blocks");
                 niveauNode.AppendChild(blocksNode);
                 for (int i = 0; i < blocks.Count; i++)
                 {
                     XmlNode block = xmlDoc.CreateElement("Block");
                     XmlAttribute id = xmlDoc.CreateAttribute("id");
                     id.Value = i.ToString();
                     block.Attributes.Append(id);
                     XmlNode type = xmlDoc.CreateElement("Type");
                     XmlNode posX = xmlDoc.CreateElement("X");
                     XmlNode posY = xmlDoc.CreateElement("Y");
                     XmlNode hauteur = xmlDoc.CreateElement("Hauteur");
                     XmlNode largeur = xmlDoc.CreateElement("Largeur");
                     type.InnerText = blocks[i].type.ToString();
                     posX.InnerText = blocks[i].getPosX().ToString();
                     posY.InnerText = blocks[i].getPosY().ToString();
                     hauteur.InnerText = blocks[i].getH().ToString();
                     largeur.InnerText = blocks[i].getW().ToString();
                     block.AppendChild(type);
                     block.AppendChild(posX);
                     block.AppendChild(posY);
                     block.AppendChild(hauteur);
                     block.AppendChild(largeur);
                     blocksNode.AppendChild(block);
                     rootNode.AppendChild(niveauNode);
                     xmlDoc.AppendChild(rootNode);
                 }
             }*/







            xmlDoc.Save(filename);


        }
        public static List<int> getListNiveauxName()
        {

            List<int> listeNiveauxID = new List<int>();
            XmlDocument xmlDoc = new XmlDocument();
            String filename = "mapDuJeu.xml";

            xmlDoc.Load(filename);



            XmlNodeList niveauNodes = xmlDoc.SelectNodes("//Niveaux/Niveau");
            foreach (XmlNode niveauNode in niveauNodes)
            {
                int id = int.Parse(niveauNode.Attributes["id"].Value);
                listeNiveauxID.Add(id);
            }
            return listeNiveauxID;
        }
        public static List<CollisionObject> getNiveau(int idNiveau)
        {
            int xValue = 0;
            int yValue = 0;
            List<CollisionObject> blocks = new List<CollisionObject>();
            XmlDocument xmlDoc = new XmlDocument();
            String filename = "mapDuJeu.xml";
            xmlDoc.Load(filename);
            XmlNodeList BNodeL = xmlDoc.SelectNodes("//Niveau[@id='" + idNiveau.ToString() + "']/Blocks/Block");
            int i = 0;
            foreach (XmlNode bloc in BNodeL)
            {

                XmlNode x = xmlDoc.SelectSingleNode("//Niveau[@id='" + idNiveau.ToString() + "']/Blocks/Block[@id='" + i.ToString() + "']/X");
                String innerX = x.InnerText;
                xValue = int.Parse(innerX);

                XmlNode y = xmlDoc.SelectSingleNode("//Niveau[@id='" + idNiveau.ToString() + "']/Blocks/Block[@id='" + i.ToString() + "']/Y");
                String innerY = y.InnerText;
                yValue = int.Parse(innerY);
                XmlNode typeNode = xmlDoc.SelectSingleNode("//Niveau[@id='" + idNiveau.ToString() + "']/Blocks/Block[@id='" + i.ToString() + "']/Type");
                String innerType = typeNode.InnerText;
                int typeValue = int.Parse(innerType);
                if (typeValue > 10)
                {
                    blocks.Add(new Enemy(xValue, yValue, 60, 120, typeValue - 10, 20, 1));
                }
                else
                {
                    blocks.Add(new CollisionObject(xValue, yValue, 60, 60, typeValue));
                }

                i++;
            }




            return blocks;
        }

    }
}
