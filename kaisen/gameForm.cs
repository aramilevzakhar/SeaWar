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

namespace kaisen {
  public partial class gameForm :Form {
    int min = 0, sec = 0, msec = 0;
    bool toogle_v_h = false;
    bool enableColorButton = true;
    public const int sizeXmap = 10;
    public const int sizeYmap = 10;

    int posX = 10;
    int posY = 10;
    int cellWidth = 30;
    int cellHeight = 30;
    int ship;
    int canIclick = 1;
    int numberPoints = 0;
    bool lockPreview = false;

    string timestamp1;
    string timestamp2;
    string winner;

    public Button[,] myMap = new Button[sizeXmap, sizeYmap];
    public Button[,] enemyMap = new Button[sizeXmap, sizeYmap];
    public int[,] myMapBin = new int[sizeXmap, sizeYmap];
    public int[,] enemyMapBin = new int[sizeXmap, sizeYmap];
    public MyNewBot Bot;
    res ans;
    setPos check_pos;
    List<Color> colors = new List<Color>() { };



    char[] Alphabet = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К' };

    // constructor
    public gameForm() {
      InitializeComponent();
      this.CenterToScreen();
      timer1.Interval = 10;

      createMap();
      Bot = new MyNewBot(myMapBin, enemyMapBin, myMap, enemyMap);
      Bot.SetName("Василий");
      ans = new res();


      check_pos = new setPos(myMapBin, myMap);
      enemyMapBin = Bot.ConfigureShips();

    }

    // player shoot
    public void player_shoot(object sender, EventArgs e) {
      Button button = sender as Button;

      if (!shoot(enemyMapBin, button)) {
        while (Bot.shoot()) {
          if (Bot.numberPoints == 20) {          
            lock_map(enemyMap);
            winner = Bot.GetName();
            MessageBox.Show(string.Format("{0} победил!", winner));
            timer1.Enabled = false;
            ans.create_history_game("history.txt", timestamp1, timestamp1, label4.Text, label3.Text, label2.Text, winner);
            break;
          }
        }
      }


    }

    // shoot
    public bool shoot(int[,] map, Button pressedButton) {
      bool hit = false;
      int x, y;
      y = (pressedButton.Location.X - 10 - 330) / 30;
      x = (pressedButton.Location.Y - 10) / 30;

      if (map[x, y] == 1) {
        hit = true;
        pressedButton.BackColor = Color.Orange;
        pressedButton.Text = "X";
        enemyMapBin[x, y] = 0;
        numberPoints += 1;
      } else {
        hit = false;
        pressedButton.BackColor = Color.LightBlue;
        pressedButton.Text = "X";
      }
      pressedButton.Enabled = false;
      if (numberPoints == 20) {
        
        lock_map(myMap);
        lock_map(enemyMap);
        winner = label1.Text.Split()[0];
        MessageBox.Show(string.Format("Игрок {0} победил", winner));
        timer1.Enabled = false;
        ans.create_history_game("history.txt", timestamp1, timestamp1, label4.Text, label3.Text, label2.Text, winner);
      }
      return hit;
    }

    // createMap
    public void createMap() {
      //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Width = 330 * 2 + 40 + 20;
      for (int i = 0; i < sizeXmap; i++) {
        for (int j = 0; j < sizeYmap; j++) {
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
    public void hover_temporaryRedZone(object sender, EventArgs e) {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;

      sel(x, y, lockPreview);
    }

    // leave into temporaryRedZone
    public void leave_temporaryRedZone(object sender, EventArgs e) {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;

      unsel(x, y, lockPreview);
      colors.Clear();

    }

    // click to button
    public void set_pos(object sender, EventArgs e) {
      Button button = sender as Button;
      int y = (button.Location.X - 10) / 30;
      int x = (button.Location.Y - 10) / 30;

      if (check_pos.CheckPos(x, y, ship, toogle_v_h) && canIclick == 1) {
        check_pos.funeosetchi(x, y, ship, toogle_v_h, true);
        canIclick = 2;
        lockPreview = false;
      }

    }

    public void sel(int x, int y, bool lockPreview=true) {
      int acc = 1;
      Color color;

      if (lockPreview) {
        if (check_pos.CheckPos(x, y, ship, toogle_v_h))
          color = Color.Lime;
        else
          color = Color.Orange;



        if (toogle_v_h) {
          for (int i = 0; i < ship; i++) {
            if (x + i < sizeXmap) {
              colors.Add(myMap[x + i, y].BackColor);
              myMap[x + i, y].BackColor = color;


            } else {
              colors.Add(myMap[x - acc, y].BackColor);
              myMap[x - acc, y].BackColor = color;
              acc++;
            }

          }

        } else {
          for (int i = 0; i < ship; i++) {
            if (y + i < sizeXmap) {
              colors.Add(myMap[x, y + i].BackColor);
              myMap[x, y + i].BackColor = color;
            } else {
              colors.Add(myMap[x, y - acc].BackColor);
              myMap[x, y - acc].BackColor = color;
              acc++;

            }
          }
        }




      }
    }

    public void unsel(int x, int y, bool lockPreview=true) {
      int acc = 1;


      if (lockPreview) {
        if (toogle_v_h) {
          for (int i = 0; i < ship; i++) {
            if (x + i < sizeXmap) {
              myMap[x + i, y].BackColor = colors[i];

            } else {
              myMap[x - acc, y].BackColor = colors[i];
              acc++;
            }

          }

        } else {
          for (int i = 0; i < ship; i++) {
            if (y + i < sizeXmap) {
              myMap[x, y + i].BackColor = colors[i];
            } else {
              myMap[x, y - acc].BackColor = colors[i];
              acc++;

            }
          }
        }
      }
    }

    // vertical or horizontal
    private void button253_Click(object sender, EventArgs e) {
      Button button = sender as Button;
      //MessageBox.Show(e.ToString());
      //System.Windows.Forms.MessageBox.Show("hell");
      if (toogle_v_h) {
        button.Text = "H";
        toogle_v_h = false;
      } else {
        button.Text = "V";
        toogle_v_h = true;
      }
    }

    // bottom panel of button for setting ships
    private void button252_Click(object sender, EventArgs e) {
      Button button = sender as Button;
      button.Enabled = false;
      ship = Convert.ToInt16(button.Text);
      canIclick = 1;
      lockPreview = true;

    }

    // exit
    private void button2_Click(object sender, EventArgs e) {
      Environment.Exit(0);
    }

    // button show
    private void button3_Click(object sender, EventArgs e) {


      for (int i = 0; i < sizeXmap; i++) {
        for (int j = 0; j < sizeXmap; j++) {
          if (myMapBin[i, j] == 1) {
            myMap[i, j].BackColor = Color.Black;
          }
          if (enemyMapBin[i, j] == 1) {
            enemyMap[i, j].BackColor = Color.Black;
          }
        }

      }

    }

    // play
    private void buttonPlay_Click(object sender, EventArgs e) {
      timer1.Enabled = true;
      timestamp1 = DateTime.Now.ToLocalTime().ToString();
      for (int i = 0; i < sizeXmap; i++) {
        for (int j = 0; j < sizeYmap; j++) {
          myMap[i, j].Enabled = false;
          enemyMap[i, j].Enabled = true;

        }
      }
    }

    // timer label4 : label3 : label2
    private void timer1_Tick(object sender, EventArgs e) {

      if (msec == 99) {
        if (sec == 59) {
          if (min == 59) min = 0; else min++;
          sec = 0;
        } else sec++;
        msec = 0;
      } else msec++; // Форматируем надписи табло: 

      if (min.ToString().Length == 1) label4.Text = "0" + min.ToString();
      else label4.Text = min.ToString();

      if (sec.ToString().Length == 1) label3.Text = "0" + sec.ToString();
      else label3.Text = sec.ToString();

      if (msec.ToString().Length == 1) label2.Text = "0" + msec.ToString();
      else label2.Text = msec.ToString();

    }

    // surrender
    private void button4_Click(object sender, EventArgs e) {
      timer1.Enabled = false;
      lock_map(enemyMap);
      button3_Click(sender, e);

      timestamp2 = DateTime.Now.ToLocalTime().ToString();

      winner = Bot.GetName();
      ans.create_history_game("history.txt", timestamp1, timestamp1, label4.Text, label3.Text, label2.Text, winner);

    }

    // clear
    private void button1_Click_1(object sender, EventArgs e) {
      for (int i = 0; i < sizeXmap; i++) {
        for (int j = 0; j < sizeYmap; j++) {
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
      numberPoints = 0; Bot.numberPoints = 0;
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
    }



    // lock map
    public void lock_map(Button[,] map) {
      for (int i = 0; i < sizeXmap; i++) {
        for (int j = 0; j < sizeYmap; j++) {
          map[i, j].Enabled = false;
        }
      }
    }

  }
}