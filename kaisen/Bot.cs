using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kaisen {
  public class MyNewBot {
    public int[,] enemyMapBin = new int[gameForm.sizeXmap, gameForm.sizeYmap];
    public int[,] myMapBin = new int[gameForm.sizeXmap, gameForm.sizeYmap];

    public Button[,] enemyMap = new Button[gameForm.sizeXmap, gameForm.sizeYmap];
    public Button[,] myMap = new Button[gameForm.sizeXmap, gameForm.sizeYmap];
    Random r = new Random();
    setPos setPosNewObj;

    string name;

    public int numberPoints = 0;

    public MyNewBot(int[,] enemyMapBin, int[,] myMapBin, Button[,] enemyMap, Button[,] myMap) {
      this.enemyMapBin = enemyMapBin;
      this.myMapBin = myMapBin;
      this.enemyMap = enemyMap;
      this.myMap = myMap;

      for (int i = 0; i < gameForm.sizeXmap; i++) {
        for (int j = 0; j < gameForm.sizeXmap; j++) {
          this.enemyMapBin[i, j] = 0;
          this.myMapBin[i, j] = 0;
        }
      }
      setPosNewObj = new setPos(myMapBin, myMap);
    }

    public bool shoot() {
      bool hit = false;
      int X;
      int Y;

      while (true) {
        X = r.Next(0, 10);
        Y = r.Next(0, 10);

        if (enemyMap[X, Y].Text != "X") break;
      }

      if (enemyMapBin[X, Y] == 1) {
        hit = true;
        enemyMap[X, Y].BackColor = Color.Orange;
        enemyMap[X, Y].Text = "X";
        numberPoints += 1;

      } else {
        enemyMap[X, Y].BackColor = Color.LightBlue;
        enemyMap[X, Y].Text = "X";
      }

      return hit;
    }

    public void generateCoord(int funenonagasa) {
      int x, y;
      bool suichoku_matawa_suihei;

      while (true) {
        x = r.Next(0, gameForm.sizeXmap);
        y = r.Next(0, gameForm.sizeXmap);
        suichoku_matawa_suihei = (r.Next(0, 2) == 1) ? true : false;
        if (setPosNewObj.CheckPos(x, y, funenonagasa, suichoku_matawa_suihei))
          break;

      }

      myMapBin = setPosNewObj.funeosetchi(x, y, funenonagasa, suichoku_matawa_suihei);
    }
    public int[,] ConfigureShips() {
      int[] arr = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
      for (int i = 0; i < 10; i++) {
        generateCoord(arr[i]);
        Thread.Sleep(30);
      }

      return myMapBin;
    }
    public void SetName(string name) {
      this.name = name;
    }
    public string GetName() {
      return this.name;
    }
  }
}
