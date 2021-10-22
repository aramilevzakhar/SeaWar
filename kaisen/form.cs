using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test1
{
    public partial class Form1 : Form
    {
        public bool isPlay = true;
        public const int size_map = 10;
        public const int btw_size = 31;
        public int count = 0;
        public bool stop = false;


        public int c1, c2;

        public char[] lru = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };


        public bool[,] b_map1 = new bool[size_map, size_map];
        public bool[,] b_map2 = new bool[size_map, size_map];

        public Button[,] ta1 = new Button[size_map, size_map];
        public Button[,] ta2 = new Button[size_map, size_map];

        Button ClearMap = new Button();
        Button PositionOfShip = new Button();
        Button Play = new Button();

        Label count_point = new Label();

        public Bot bot;


        public Form1()
        {
            //InitializeComponent();
            this.Text = "Морской бой";
            Init();



        }


        public void Init()
        {
            MenuControl();
            CreateMap();
            bot = new Bot(b_map2, b_map1, ta2, ta1);
            b_map2 = bot.ConfigureShips();
        }


        public void MenuControl()
        {

            
            this.Controls.Add(Play);
            Play.Height = 40;
            Play.Width = 40;
            Play.Top = size_map * btw_size + 20;
            Play.Left = 0;
            Play.Text = "Start";
            Play.BackColor = Color.DarkGray;
            Play.Click += new EventHandler(start);
            //Play.Enabled = false;

            
            this.Controls.Add(ClearMap);
            ClearMap.Height = 40;
            ClearMap.Width = 40;
            ClearMap.Top = size_map * btw_size + 20;
            ClearMap.Left = 41;
            ClearMap.Text = "Clear Map";
            ClearMap.BackColor = Color.DarkGray;
            ClearMap.Click += new EventHandler(method_clear_map);

            
            this.Controls.Add(PositionOfShip);
            PositionOfShip.Height = 40;
            PositionOfShip.Width = 40;
            PositionOfShip.Top = size_map * btw_size + 20;
            PositionOfShip.Left = 41 * 2;
            PositionOfShip.Text = " - ";
            PositionOfShip.BackColor = Color.DarkGray;
            PositionOfShip.Click += new EventHandler(pos_ships);

            this.Controls.Add(count_point);
            count_point.Height = 30;
            count_point.Width = 30;
            count_point.Top = size_map * btw_size + 25;
            count_point.Left = 41 * 3;
            count_point.BackColor = Color.DarkGray;
            

            //count_point.BorderStyle = 0;


        }



        public void CreateMap()
        {
            int tmp = 31 * size_map + 50;
            int l1 = 0;
            int t1 = 0;
            int l2 = tmp;
            int t2 = 0;

            int size_button = 30;
            

            for (int i = 0; i < size_map; i++)
            {
                for (int j = 0; j < size_map; j++)
                {
                    ta1[i, j] = new Button();
                    this.Controls.Add(ta1[i, j]);
                    ta1[i, j].Height = size_button;
                    ta1[i, j].Width = size_button;
                    ta1[i, j].Left = l1;
                    ta1[i, j].Top = t1;
                    ta1[i, j].BackColor = Color.LightBlue;;
                    l1 += btw_size;

                    ta2[i, j] = new Button();
                    this.Controls.Add(ta2[i, j]);
                    ta2[i, j].Height = size_button;
                    ta2[i, j].Width = size_button;
                    ta2[i, j].Left = l2;
                    ta2[i, j].Top = t2;
                    ta2[i, j].BackColor = Color.LightBlue;;
                    ta2[i, j].Enabled = false;
                    l2 += btw_size;


                    ta1[i, j].Click += new EventHandler(ConfigureShips);
                    ta2[i, j].Click += new EventHandler(player_shut);

                }
                l2 = tmp;
                t2 += btw_size;
                l1 = 0;
                t1 += btw_size;


            }



            for (int i = 0; i < size_map; i++)
            {
                for (int j = 0; j < size_map; j++)
                {
                    b_map1[i, j] = false;
                    b_map2[i, j] = false;
                }
            }
        }

        // функция запуска игры не расстоновки а игры
        public void start(object sender, EventArgs e)
        {
            // MessageBox.Show("hello");
            isPlay = false;
            Button pressedButton = sender as Button;
            pressedButton.Enabled = false;
            pressedButton.BackColor = Color.Gray;
            //ClearMap.Enabled = false;
            ClearMap.BackColor = Color.Gray;
            PositionOfShip.Enabled = false;
            PositionOfShip.BackColor = Color.Gray;
            for (int i = 0; i < size_map; i++)
            {
                for (int j = 0; j < size_map; j++)
                {
                    ta2[i, j].Enabled = true;
                    ta1[i, j].Enabled = false;
                }

            }
            

        }

        public void method_clear_map(object sender, EventArgs e)
        {
            for (int i = 0; i < size_map; i++)
            {
                for (int j = 0; j < size_map; j++)
                {
                    if (b_map2[i, j])
                        ta2[i, j].BackColor = Color.Black;
                }
            }
        }

        public void ConfigureShips(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            int y = pressedButton.Location.X / btw_size;
            int x = pressedButton.Location.Y / btw_size;
            

            bool tmp = b_map1[x, y];

            if (isPlay)
            {
                if (x == 0 && y == 0)
                {
                    if (!b_map1[x + 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x == 0 && y > 0 && y < size_map - 1)
                {
                    if (!b_map1[x + 1, y - 1] &&
                        !b_map1[x + 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x == 0 && y == size_map - 1)
                {
                    if (!b_map1[x + 1, y - 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x > 0 && y == size_map - 1 && x < size_map - 1)
                {
                    if (!b_map1[x - 1, y - 1] &&
                        !b_map1[x + 1, y - 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x == size_map - 1 && y == size_map - 1)
                {
                    if (!b_map1[x - 1, y - 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x == size_map - 1 && y < size_map - 1 && y > 0)
                {
                    if (!b_map1[x - 1, y - 1] &&
                        !b_map1[x - 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x == size_map - 1 && y == 0)
                {
                    if (!b_map1[x - 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else if (x < size_map - 1 && x > 0 && y == 0)
                {
                    if (!b_map1[x - 1, y + 1] &&
                        !b_map1[x + 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }
                else
                {
                    if (!b_map1[x - 1, y - 1] &&
                        !b_map1[x - 1, y + 1] &&
                        !b_map1[x + 1, y - 1] &&
                        !b_map1[x + 1, y + 1] &&
                        !b_map1[x, y] &&
                        count != 20)
                    {
                        pressedButton.BackColor = Color.Black;
                        b_map1[x, y] = true;
                    }
                    else
                    {
                        pressedButton.BackColor = Color.LightBlue;;
                        b_map1[x, y] = false;
                    }
                }

                if (b_map1[x, y] != tmp)
                {
                    if (b_map1[x, y] && !stop) count += 1;
                    else count -= 1;
                }

                // if (count == 20)
            }

            //Play.Enabled = (count == 20) ? true : false;
            if (count == 20)
            {
                Play.Enabled = true;
                Play.BackColor = Color.Green;
            }
            else
            {
                //Play.Enabled = false;
                Play.BackColor = Color.Gray;
            }


            

            count_point.Text = count.ToString();
        }

        public void pos_ships(object sender, EventArgs e)
        {
            Button pressButton = sender as Button;
            if (pressButton.Text == " - ")
                pressButton.Text = " | ";
            else if (pressButton.Text == " | ")
                pressButton.Text = " - ";
        }

        public void player_shut(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            Shoot(b_map2, pressedButton);
            bot.shoot();
        }

        public bool Shoot(bool[,] map, Button pressedButton)
        {
            //Button pressedButton = sender as Button;
            bool hit = false;
            if (!isPlay)
            {

                int y = (pressedButton.Location.X - 31 * size_map - 50) / btw_size;
                int x = pressedButton.Location.Y / btw_size;
                if (map[x, y])
                {
                    hit = true;
                    pressedButton.BackColor = Color.Orange;
                    pressedButton.Text = "X";
                    b_map2[x, y] = false;
                }
                else
                {
                    hit = false;
                    pressedButton.BackColor = Color.LightBlue;
                    pressedButton.Text = "X";
                }
                //MessageBox.Show(x.ToString() + " " + y.ToString());

                int a = 1;
                for (int i = 0; i < size_map; i++)
                {
                    for (int j = 0; j < size_map; j++)
                    {
                        if (b_map2[i, j] == true)
                        {
                            a = 2;
                            break;
                        }
                    }
                    if (a == 2) break;

                }
                if (a == 1)
                {
                    for (int i = 0; i < size_map; i++)
                    {
                        for (int j = 0; j < size_map; j++)
                        {
                            ta2[i, j].Enabled = false;
                        }
                    }
                    MessageBox.Show("You winned!");
                }

            }
            return hit;
        }
    }
}
