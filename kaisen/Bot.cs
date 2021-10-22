using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;


namespace test1
{
    public class Bot
    {
        public bool[,] b_map1 = new bool[Form1.size_map, Form1.size_map];
        public bool[,] b_map2 = new bool[Form1.size_map, Form1.size_map];

        public Button[,] ta1 = new Button[Form1.size_map, Form1.size_map];
        public Button[,] ta2 = new Button[Form1.size_map, Form1.size_map];


        public Bot(bool[,] b_map1, bool [,] b_map2, Button[,] ta1, Button[,] ta2)
        {
            this.b_map1 = b_map1;
            this.b_map2 = b_map2;
            this.ta1 = ta1;
            this.ta2 = ta2;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.b_map1[i, j] = false;
                    this.b_map2[i, j] = false;
                }
            }
        }




        public bool shoot()
        {
            bool hit = false;
            Random r = new Random();
            int posX;
            int posY;

            while (true)
            {
                posX = r.Next(0, Form1.size_map);
                posY = r.Next(0, Form1.size_map);

                if (ta2[posX, posY].Text != "X") break;
            }



            if (b_map2[posX, posY])
            {
                //b_map2[posX, posY] = false;
                ta2[posX, posY].BackColor = Color.Red;
                ta2[posX, posY].Text = "X";
                //b_map2[posX, posY] = false;
            } 
            else
            {
                ta2[posX, posY].BackColor = Color.LightBlue;
                ta2[posX, posY].Text = "X";
            }

            return hit;
        }




        public void generateCoord(int length, bool p)
        {
            int posX;
            int posY;
            int pos;
            bool p1;
            Random r = new Random();

            

            while (true)
            {

                posX = r.Next(1, Form1.size_map - 1);
                posY = r.Next(1, Form1.size_map - 1);

                pos = (p) ? posX : posY;

                if (pos - 1 + length < Form1.size_map)
                {
                    p1 = true;
                    for (int i = pos; i < pos + length; i++)
                    {
                        if (p)
                            if (b_map1[pos, posY - 1] || 
                                b_map1[pos, posY] || 
                                b_map1[pos, posY + 1] || 
                                b_map1[pos - 1, posY - 1] || 
                                b_map1[pos - 1, posY] || 
                                b_map1[pos - 1, posY + 1] ||
                                b_map1[pos + length - 1, posY - 1] ||
                                b_map1[pos + length - 1, posY] ||
                                b_map1[pos + length - 1, posY + 1]                     
                                )
                            {
                                p1 = false;
                                break;
                            }
                        else
                            if (b_map1[posX, pos])
                            {
                                p1 = false;
                                break;
                            }
                    }
                    if (p1) break;
                }
            }

            if (p)
                for (int i = pos; i < pos + length; i++)
                {
                    b_map1[i, posY] = true;
                    // MessageBox.Show(i.ToString() + " " + posY.ToString());
                }
            else
                for (int i = pos; i < pos + length; i++)
                {
                    b_map1[posX, i] = true;
                    // MessageBox.Show(i.ToString() + " " + posY.ToString());
                }
        }


        public bool[,] ConfigureShips()
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



            return b_map1;
        }



            
    }
}
