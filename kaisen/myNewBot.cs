using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kaisen
{
    public class MyNewBot
    {
        public int[,] enemyMapBin = new int[gameForm.sizeXmap, gameForm.sizeYmap];
        public int[,] myMapBin = new int[gameForm.sizeXmap, gameForm.sizeYmap];

        public Button[,] enemyMap = new Button[gameForm.sizeXmap, gameForm.sizeYmap];
        public Button[,] myMap = new Button[gameForm.sizeXmap, gameForm.sizeYmap];
        Random r = new Random();

        public MyNewBot(int [,]enemyMapBin, int [,]myMapBin, Button [,]enemyMap, Button [,]myMap)
        {
            this.enemyMapBin = enemyMapBin;
            this.myMapBin = myMapBin;
            this.enemyMap = enemyMap;
            this.myMap = myMap;

            for (int i = 0; i < gameForm.sizeXmap - 1; i++)
            {
                for (int j = 0; j < gameForm.sizeXmap - 1; j++)
                {
                    this.enemyMapBin[i, j] = 0;
                    this.myMapBin[i, j] = 0;
                }
            }
        }



        public bool shoot()
        {
            bool hit = false;
            if (true)
            {

                //Random r = new Random();
                int posX;
                int posY;

                while (true)
                {
                    posX = r.Next(0, 10);
                    posY = r.Next(0, 10);

                    if (enemyMap[posX, posY].Text != "X") break;
                }



                if (enemyMapBin[posX, posY] == 1)
                {
                    hit = true;
                    enemyMap[posX + 1, posY + 1].BackColor = Color.Orange;
                    enemyMap[posX + 1, posY + 1].Text = "X";
                    //enemyMapBin[posX, posY] = 0;
                }
                else
                {
                    enemyMap[posX + 1, posY + 1].BackColor = Color.LightBlue;
                    enemyMap[posX + 1, posY + 1].Text = "X";
                }
            }
            return hit;
        }

        public void generateCoord(int length, bool toggleH)
        {
            int posX;
            int posY;
            bool p1 = false;
            //Random r = new Random();



            while (true)
            {
                if (toggleH)
                {
                    posX = r.Next(0, gameForm.sizeXmap - 1);
                    posY = r.Next(0, gameForm.sizeXmap - length);
                    for (int i = 0; i < length; i++)
                    {
                        if (myMapBin[posX, posY + i] == 1)
                        {
                            p1 = true;
                            break;
                        }
                    }
                    if (!p1)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            myMapBin[posX, posY + i] = 1;
                        }
                    }

                }
                else
                {
                    posX = r.Next(0, gameForm.sizeXmap - length);
                    posY = r.Next(0, gameForm.sizeXmap - 1);
                    for (int i = 0; i < length; i++)
                    {
                        myMapBin[posX + i, posY] = 1;
                        
                    }
                }
                break;
            }
        }
   

        public int[,] ConfigureShips()
        {
            generateCoord(4, true);     
            Thread.Sleep(30);
                    
            generateCoord(3, true);     
            Thread.Sleep(30);

            generateCoord(3, true);
            Thread.Sleep(30);

            generateCoord(2, true);
            Thread.Sleep(30);
            generateCoord(2, true);
            Thread.Sleep(30);
            generateCoord(2, true);
            Thread.Sleep(30);

            generateCoord(1, true);
            Thread.Sleep(30);
            generateCoord(1, true);
            Thread.Sleep(30);
            generateCoord(1, true);
            Thread.Sleep(30);
            generateCoord(1, true);
            Thread.Sleep(30);

            return myMapBin;
        }



    }
}
