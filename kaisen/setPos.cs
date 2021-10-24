using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kaisen
{
  class setPos
  {
    const int sizeXmap = gameForm.sizeXmap;
    const int sizeYmap = gameForm.sizeYmap;

    int[,] map_b = new int[sizeXmap, sizeYmap];
    Button[,] map = new Button[sizeXmap, sizeYmap];


    public setPos(int[,] map_b, Button[,] map)
    {
      this.map_b = map_b;
      this.map = map;

    }


    public bool CheckPos(int x, int y, int ship_size, bool suichoku_matawa_suihei)
    {
      int count = 0;
      // vertical
      if (suichoku_matawa_suihei)
      {
        // checking
        if (x > 0 && x < sizeXmap - ship_size && y > 0 && y < sizeXmap - 1)
        {
          while (count <= ship_size)
          {
            // Если корабль не с кем не пересекается и не стоит у самых границ
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1 || map_b[x + count, y + 1] == 1 || map_b[x - 1, y - 1] == 1 || map_b[x - 1, y] == 1 || map_b[x - 1, y + 1] == 1 || map_b[x + ship_size, y - 1] == 1 || map_b[x + ship_size, y] == 1 || map_b[x + ship_size, y + 1] == 1))
            {
              count++;
            }
            else
            {
              return false;

            }
          }
          count = 0;
        }
        else if (x == 0 && y == 0)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y + 1] == 1 || map_b[x + ship_size, y] == 1 || map_b[x + ship_size, y + 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
          count = 0;
        }
        else if (x > 0 && y == 0 && x < sizeXmap - ship_size)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y + 1] == 1 || map_b[x - 1, y] == 1 || map_b[x + ship_size, y] == 1 || map_b[x - 1, y + 1] == 1 || map_b[x + ship_size, y + 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
          count = 0;
        }
        else if (x == 0 && y > 0 && y < sizeXmap - 1)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1 || map_b[x + count, y + 1] == 1 || map_b[x + ship_size, y] == 1 || map_b[x + ship_size, y - 1] == 1 || map_b[x + ship_size, y + 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (x >= sizeXmap - ship_size && y > 0 && y < sizeXmap - 1)
        {
          x = sizeXmap - ship_size;
          while (count < ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1 || map_b[x + count, y + 1] == 1 || map_b[x - 1, y] == 1 || map_b[x - 1, y - 1] == 1 || map_b[x - 1, y + 1] == 1))
            {
              count += 1;
            }
            else
            {
              return false;
            }
          }

          //MessageBox.Show("x");
        }
        else if (x >= sizeXmap - ship_size && y == 0)
        {
          x = sizeXmap - ship_size;
          while (count < ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y + 1] == 1 || map_b[x - 1, y] == 1 || map_b[x - 1, y + 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (x >= sizeXmap - ship_size && y == sizeXmap - 1)
        {
          x = sizeXmap - ship_size;
          while (count < ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1 || map_b[x - 1, y] == 1 || map_b[x - 1, y - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (x > 0 && y == sizeXmap - 1 && x < sizeXmap - ship_size)
        {
          //x = sizeXmap - ship_size;
          while (count <= ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1 || map_b[x - 1, y] == 1 || map_b[x - 1, y - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (x == 0 && y == sizeXmap - 1)
        {
          //x = sizeXmap - ship_size;
          while (count <= ship_size)
          {
            if (!(map_b[x + count, y] == 1 || map_b[x + count, y - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }

        return true;
      }
      else
      { 
        // horizontal
        if (x > 0 && x < sizeXmap - 1 && y > 0 && y < sizeYmap - ship_size)
        {
          while (count <= ship_size)
          {

            if (!(map_b[x, y + count] == 1 || map_b[x - 1, y + count] == 1 || map_b[x + 1, y + count] == 1 || map_b[x, y - 1] == 1 || map_b[x + 1, y - 1] == 1 || map_b[x - 1, y - 1] == 1 || map_b[x, y + ship_size] == 1 || map_b[x - 1, y + ship_size] == 1 || map_b[x + 1, y + ship_size] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
          count = 0;
        }
        else if (x == 0 && y == 0)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x + 1, y + count] == 1 || map_b[x, y + ship_size] == 1 || map_b[x + 1, y + ship_size] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
          count = 0;
        }
        else if (x == 0 && y > 0 && y < sizeYmap - ship_size)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x + 1, y + count] == 1 || map_b[x, y - 1] == 1 || map_b[x + 1, y - 1] == 1 || map_b[x, y + ship_size] == 1 || map_b[x + 1, y + ship_size] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
          count = 0;
        }
        else if (y == 0 && x > 0 && x < sizeXmap - 1)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x - 1, y + count] == 1 || map_b[x + 1, y + count] == 1 || map_b[x, y + ship_size] == 1 || map_b[x + 1, y + ship_size] == 1 || map_b[x - 1, y + ship_size] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (y == 0 && x == sizeXmap - 1)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x - 1, y + count] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (y > 0 && x == sizeXmap - 1 && y < sizeXmap - ship_size)
        {
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x - 1, y + count] == 1 || map_b[x, y - 1] == 1 || map_b[x - 1, y - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (y >= sizeXmap - ship_size && x == sizeXmap - 1)
        {
          y = sizeXmap - ship_size;
          while (count < ship_size)
          {
            if (!(map_b[x, y + count] == 1 || map_b[x - 1, y + count] == 1 || map_b[x, y - 1] == 1 || map_b[x - 1, y - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (y >= sizeXmap - ship_size && x > 0 && x < sizeXmap - 1)
        {
          y = sizeXmap - ship_size;
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count - 1] == 1 || map_b[x - 1, y + count - 1] == 1 || map_b[x + 1, y + count - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }
        else if (y >= sizeXmap - ship_size && x == 0)
        {
          y = sizeXmap - ship_size;
          while (count <= ship_size)
          {
            if (!(map_b[x, y + count - 1] == 1 || map_b[x + 1, y + count - 1] == 1))
            {
              count++;
            }
            else
            {
              return false;
            }
          }
        }

        return true;
      }
    }


    public int[,] funeosetchi(int x, int y, int funenonagasaki, bool suichoku_matawa_suihei)
    {
      int acc = 1;
      if (suichoku_matawa_suihei)
      {

        for (int i = 0; i < funenonagasaki; i++)
        {
          if (x + i < sizeXmap)
          {
            map_b[x + i, y] = 1;
          }
          else
          {
            map_b[x - acc, y] = 1;
            acc++;
          }
        }
      }
      else
      {
        for (int i = 0; i < funenonagasaki; i++)
        {
          if (y + i < sizeXmap)
          {
            map_b[x, y + i] = 1;
          }
          else 
          {
            map_b[x, y - acc] = 1;
            acc++;
          }
        }
      }
      return map_b;
    }
  }


}
