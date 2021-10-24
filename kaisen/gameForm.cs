using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace kaisen
{
  public partial class gameForm :Form
  {
    int min = 0, sec = 0, msec = 0;
    bool toogle_v_h = false;
    bool enableColorButton = true;
    public const int sizeXmap = 10;
    public const int sizeYmap = 10;

    bool govno_govno = true;
    int posX = 10;
    int posY = 10;
    int cellWidth = 30;
    int cellHeight = 30;
    int ship;
    int canIclick = 1;
    int acc = 0;

    string timestamp1;
    string timestamp2;

    public Button[,] myMap = new Button[sizeXmap, sizeYmap];
    public Button[,] enemyMap = new Button[sizeXmap, sizeYmap];
    public int[,] myMapBin = new int[sizeXmap, sizeYmap];
    public int[,] enemyMapBin = new int[sizeXmap, sizeYmap];
    public MyNewBot Bot;
    char[] Alphabet = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К' };

    // constructor
    public gameForm()
    {
      InitializeComponent();
      this.CenterToScreen();
      timer1.Interval = 10;

      //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      createMap();
      Bot = new MyNewBot(myMapBin, enemyMapBin, myMap, enemyMap);
      enemyMapBin = Bot.ConfigureShips();

    }

    // player shoot
    public void player_shoot(object sender, EventArgs e)
    {
      Button button = sender as Button;

      if (!shoot(enemyMapBin, button))
      {
        while (Bot.shoot())
        {
          if (Bot.acc == 20)
          {
            MessageBox.Show("貴方が　負けます");
            lock_map(enemyMap);
            break;
          }
        }
      }


    }

    // shoot
    public bool shoot(int[,] map, Button pressedButton)
    {
      bool hit = false;
      int x, y;
      y = (pressedButton.Location.X - 10 - 330) / 30;
      x = (pressedButton.Location.Y - 10) / 30;

      if (map[x, y] == 1)
      {
        hit = true;
        pressedButton.BackColor = Color.Orange;
        pressedButton.Text = "X";
        enemyMapBin[x, y] = 0;
        acc += 1;
      }
      else
      {
        hit = false;
        pressedButton.BackColor = Color.LightBlue;
        pressedButton.Text = "X";
      }
      pressedButton.Enabled = false;
      if (acc == 20)
      {
        MessageBox.Show("貴方が　勝った");
        lock_map(myMap);
      }
      return hit;
    }

    // createMap
    public void createMap()
    {
      //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Width = 330 * 2 + 40 + 20;
      for (int i = 0; i < sizeXmap; i++)
      {
        for (int j = 0; j < sizeYmap; j++)
        {
          // две карты состоящих из кнопок
          myMap[i, j] = new Button();
          enemyMap[i, j] = new Button();
          // размер каждой кнопки и их положение (для меня)
          myMap[i, j].Size = new Size(cellWidth, cellHeight);
          myMap[i, j].Location = new Point(posX, posY);
          // для бота
          enemyMap[i, j].Size = new Size(cellWidth, cellHeight);
          enemyMap[i, j].Location = new Point(posX + 350, posY);
          // смещение по горизонтали
          posX = posX + 30;
          // обнуляем матрицу положения кораблей
          if (i < sizeXmap && j < sizeYmap) myMapBin[i, j] = 0;
          // внешний вид
          myMap[i, j].BackColor = Color.White;
          enemyMap[i, j].BackColor = Color.White;
          // обработчики событий для каждой кнопки (клик, наведение, leave)
          myMap[i, j].Click += new EventHandler(set_pos);
          myMap[i, j].MouseEnter += new EventHandler(hover_temporaryRedZone);
          myMap[i, j].MouseLeave += new EventHandler(leave_temporaryRedZone);
          // дабавляем обработчик для кликов
          enemyMap[i, j].Click += new EventHandler(player_shoot);
          // на вражеской блокируем все кнопки
          enemyMap[i, j].Enabled = false;
          // добавляем созданные кнопки на форму
          this.Controls.Add(myMap[i, j]);
          this.Controls.Add(enemyMap[i, j]);
        }
        // смещение по вертикали
        posY += 30;
        // возврат на начальное положение по оси Х
        posX = 10;
      }
    }

    // hower into temporaryRedZone
    public void hover_temporaryRedZone(object sender, EventArgs e)
    {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;
      //int tmp1 = 1;
      setColor(x, y, ship, Color.Lime, toogle_v_h, false, enableColorButton);

    }

    // leave into temporaryRedZone
    public void leave_temporaryRedZone(object sender, EventArgs e)
    {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;

      setColor(x, y, ship, Color.White, toogle_v_h, false, enableColorButton);

    }

    // click to button
    public void set_pos(object sender, EventArgs e)
    {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;

      // 1 - yes
      // 2 - no
      if (canIclick == 1)
      {
        canIclick = 2;
        setColor(x, y, ship, Color.Black, toogle_v_h, true, true);
        // myMap[x, y].AutoSiz;
        
        //setPosShips(x, y, ship, toogle_v_h);
        enableColorButton = govno_govno;

      }

    }

    // x, y - coord, ship_size, color_name, toogleLocation, access_to_write - 読み取り専用？, enable_this_function - lock or unlock
    public void setColor(int x, int y, int ship_size, Color color_name, bool toogleLocation, bool access_to_write = false, bool enable_this_function = false)
    {
      // для случая, когда индекс выходит за границу массива
      int acc = 1, count = 0;
      // иногда необходимо отключить функциональность обработчика, по умолчанию false
      if (enable_this_function)
      {
        // vertical
        if (toogleLocation)
        {

          // checking
          while (count <= ship_size)
          {
            if (x > 0 && x < sizeXmap - ship_size && y > 0 && y < sizeXmap - 1 && !(myMapBin[x + count, y] == 1 || myMapBin[x + count, y - 1] == 1 || myMapBin[x + count, y + 1] == 1 || myMapBin[x - 1, y - 1] == 1 || myMapBin[x - 1, y] == 1 || myMapBin[x - 1, y + 1] == 1 || myMapBin[x + ship_size, y - 1] == 1 || myMapBin[x + ship_size, y] == 1 || myMapBin[x + ship_size, y + 1] == 1))
            {
              count++;
            }
            else
            {
              canIclick = 1;
              govno_govno = true;
              return;

            }
          }
          govno_govno = false;




          for (int i = 0; i < ship_size; i++)
          {
            // карабль поместится в поле
            if (x + i < sizeXmap)
            {
              // если истинно то записываем координаты в другой массив
              if (access_to_write)
              {

                myMapBin[x + i, y] = 1;
                myMap[x + i, y].BackColor = color_name;
              }
              myMap[x + i, y].BackColor = color_name;


            }
            else // корабль не поместится в данное поле
            {
              // если истинно то записываем координаты в другой массив
              if (access_to_write)
              {
                myMapBin[x - acc, y] = 1;
                myMap[x - acc, y].BackColor = color_name;

              }
              myMap[x - acc, y].BackColor = color_name;
              acc++;
            }
          }
        }
        else
        { // horizontal


          // checking
          while (count <= ship_size)
          {

            if (x > 0 && x < sizeXmap - 1 && y > 0 && y < sizeYmap - ship_size && !(myMapBin[x, y + count] == 1 || myMapBin[x - 1, y + count] == 1 || myMapBin[x + 1, y + count] == 1 || myMapBin[x, y - 1] == 1 || myMapBin[x + 1, y - 1] == 1 || myMapBin[x - 1, y - 1] == 1 || myMapBin[x, y + ship_size] == 1 || myMapBin[x - 1, y + ship_size] == 1 || myMapBin[x + 1, y + ship_size] == 1))
            {
              count++;
            }
            else
            {
              canIclick = 1;
              govno_govno = true;
              return;
            }


          }
          govno_govno = false;

          for (int i = 0; i < ship_size; i++)
          {
            // карабль поместится в поле
            if (y + i < sizeXmap)
            {
              // если кликаем то записываем координаты в другой массив
              if (access_to_write)
              {
                myMapBin[x, y + i] = 1;
                myMap[x, y + i].BackColor = color_name;
                enableColorButton = true;
              }
              myMap[x, y + i].BackColor = color_name;

            }
            else // корабль не поместится в данное поле
            {
              // если кликаем то записываем координаты в другой массив
              if (access_to_write)
              {
                myMapBin[x, y - acc] = 1;
                myMap[x, y - acc].BackColor = color_name;
              }
              myMap[x, y - acc].BackColor = color_name;

              acc++;
            }
          }
        }
      }
    }
    // vertical or horizontal
    private void button253_Click(object sender, EventArgs e)
    {
      Button button = sender as Button;
      //MessageBox.Show(e.ToString());
      //System.Windows.Forms.MessageBox.Show("hell");
      if (toogle_v_h)
      {
        button.Text = "--";
        toogle_v_h = false;
      }
      else
      {
        button.Text = "|";
        toogle_v_h = true;
      }
    }

    // bottom panel of button for setting ships
    private void button252_Click(object sender, EventArgs e)
    {
      Button button = sender as Button;
      button.Enabled = false;
      button.BackColor = SystemColors.Control;
      ship = Convert.ToInt16(button.Text);
      canIclick = 1;
      enableColorButton = true;

    }

    // exit
    private void button2_Click(object sender, EventArgs e)
    {
      Environment.Exit(0);
    }

    // button show
    private void button3_Click(object sender, EventArgs e)
    {

      
      for (int i = 0; i < sizeXmap; i++)
      {
        for (int j = 0; j < sizeXmap; j++)
        {
          if (myMapBin[i, j] == 1)
          {
            myMap[i, j].BackColor = Color.Black;
          }
          if (enemyMapBin[i, j] == 1)
          {
            enemyMap[i, j].BackColor = Color.Black;
          }
        }

      }
      
    }

    // play
    private void buttonPlay_Click(object sender, EventArgs e)
    {
      timer1.Enabled = true;
      timestamp1 = DateTime.Now.ToLocalTime().ToString();
      for (int i = 0; i < sizeXmap; i++)
      {
        for (int j = 0; j < sizeYmap; j++)
        {
          myMap[i, j].Enabled = false;
          enemyMap[i, j].Enabled = true;

        }
      }
    }

    // timer label4 : label3 : label2
    private void timer1_Tick(object sender, EventArgs e)
    {

      if (msec == 99)
      {
        if (sec == 59)
        {
          if (min == 59) min = 0; else min++;
          sec = 0;
        }
        else sec++;
        msec = 0;
      }
      else msec++; // Форматируем надписи табло: 

      if (min.ToString().Length == 1) label4.Text = "0" + min.ToString();
      else label4.Text = min.ToString();

      if (sec.ToString().Length == 1) label3.Text = "0" + sec.ToString();
      else label3.Text = sec.ToString();

      if (msec.ToString().Length == 1) label2.Text = "0" + msec.ToString();
      else label2.Text = msec.ToString();

    }

    // surrender
    private void button4_Click(object sender, EventArgs e)
    {
      timer1.Enabled = false;
      lock_map(enemyMap);
      button3_Click(sender, e);

      timestamp2 = DateTime.Now.ToLocalTime().ToString();
      create_history_game();

    }

    // clear
    private void button1_Click_1(object sender, EventArgs e)
    {
      for (int i = 0; i < sizeXmap; i++)
      {
        for (int j = 0; j < sizeYmap; j++)
        {
          myMapBin[i, j] = 0;
          enemyMapBin[i, j] = 0;
          myMap[i, j].BackColor = Color.White;
          enemyMap[i, j].BackColor = Color.White;
          myMap[i, j].Text = "";
          enemyMap[i, j].Text = "";
          myMap[i, j].Enabled = true;
          enemyMap[i, j].Enabled = false;
        }
      }

      enemyMapBin = Bot.ConfigureShips();
      acc = 0; Bot.acc = 0;
      timer1.Enabled = false; msec = 0; sec = 0; min = 0;

      btoggleVH.Enabled = true;
      bship41.Enabled = true;
      bship31.Enabled = true;
      bship32.Enabled = true;
      bship21.Enabled = true;
      bship22.Enabled = true;
      bship23.Enabled = true;
      bship11.Enabled = true;
      bship12.Enabled = true;
      bship13.Enabled = true;
      bship14.Enabled = true;

      btoggleVH.BackColor = Color.Lime;
      bship41.BackColor = Color.Lime;
      bship31.BackColor = Color.Lime;
      bship32.BackColor = Color.Lime;
      bship21.BackColor = Color.Lime;
      bship22.BackColor = Color.Lime;
      bship23.BackColor = Color.Lime;
      bship11.BackColor = Color.Lime;
      bship12.BackColor = Color.Lime;
      bship13.BackColor = Color.Lime;
      bship14.BackColor = Color.Lime;

    }

    // create history into file
    public void create_history_game()
    {
      try
      {
        using (FileStream fs = File.Create("history.txt"))
        {
          byte[] info = new UTF8Encoding(true).GetBytes(timestamp1 + " - " + timestamp2 + " - " + label4.Text + ":" + label3.Text + ":" + label2.Text);
          fs.Write(info, 0, info.Length);

        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

    }

    // lock map
    public void lock_map(Button[,] map)
    {
      for (int i = 0; i < sizeXmap; i++)
      {
        for (int j = 0; j < sizeYmap; j++)
        {
          map[i, j].Enabled = false;
        }
      }
    }

  }
}