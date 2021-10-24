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
    setPos setPosNewObj;

    string name;

    public int numberPoints  = 0;

    public MyNewBot(int[,] enemyMapBin, int[,] myMapBin, Button[,] enemyMap, Button[,] myMap)
    {
      this.enemyMapBin = enemyMapBin;
      this.myMapBin = myMapBin;
      this.enemyMap = enemyMap;
      this.myMap = myMap;

      for (int i = 0; i < gameForm.sizeXmap; i++)
      {
        for (int j = 0; j < gameForm.sizeXmap; j++)
        {
          this.enemyMapBin[i, j] = 0;
          this.myMapBin[i, j] = 0;
        }
      }
      setPosNewObj = new setPos(myMapBin, myMap);
    }

    public bool shoot()
    {
      bool hit = false;
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
        enemyMap[posX, posY].BackColor = Color.Orange;
        enemyMap[posX, posY].Text = "X";
        numberPoints += 1;

      }
      else
      {
        enemyMap[posX, posY].BackColor = Color.LightBlue;
        enemyMap[posX, posY].Text = "X";
      }

      return hit;
    }

    public void generateCoord(int funenonagasa)
    {
      int x;
      int y;
      bool suichoku_matawa_suihei;

      while (true)
      {
        x = r.Next(0, gameForm.sizeXmap);
        y = r.Next(0, gameForm.sizeXmap);
        suichoku_matawa_suihei = (r.Next(0, 2) == 1) ? true : false;
        if (setPosNewObj.CheckPos(x, y, funenonagasa, suichoku_matawa_suihei))
          break;

      }

      myMapBin = setPosNewObj.funeosetchi(x, y, funenonagasa, suichoku_matawa_suihei);
    }

    public int[,] ConfigureShips()
    {
      generateCoord(4);
      Thread.Sleep(30);
      
      generateCoord(3);
      Thread.Sleep(30);

      generateCoord(3);
      Thread.Sleep(30);

      generateCoord(2);
      Thread.Sleep(30);

      generateCoord(2);
      Thread.Sleep(30);
      generateCoord(2);
      Thread.Sleep(30);

      generateCoord(1);
      Thread.Sleep(30);
      generateCoord(1);
      Thread.Sleep(30);
      generateCoord(1);
      Thread.Sleep(30);
      generateCoord(1);
      Thread.Sleep(30);

      return myMapBin;
    }

    public void SetName(string name)
    {
      this.name = name;
    }

    public string GetName()
    {
      return this.name;
    }

  }
}
