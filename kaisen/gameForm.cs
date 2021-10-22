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

namespace kaisen
{
    public partial class gameForm : Form
    {
        bool toogle_v_h = false;
        //bool isPlay = true;
        bool enableColorButton = true;

        public const int sizeXmap = 11;
        public const int sizeYmap = 11;

        int posX = 10;
        int posY = 10;
        int cellWidth = 30;
        int cellHeight = 30;

        int ship;
        int canIclick = 1;
        int painOver = 0;

        bool click_hover = false;


        string str = "";

        //bool[,] myMap = new bool[sizeX, sizeY];
        public Button[,] myMap = new Button[sizeXmap, sizeYmap];
        public Button[,] enemyMap = new Button[sizeXmap, sizeYmap];

        public int[,] myMapBin = new int[sizeXmap - 1, sizeYmap - 1];
        public int[,] enemyMapBin = new int[sizeXmap - 1, sizeYmap - 1];


        public MyNewBot Bot;
        //map bot = new map(myMapBin);


        char []Alphabet = {'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К'};
        public gameForm()
        {
            InitializeComponent();
            createMap();
            Bot = new MyNewBot(myMapBin, enemyMapBin, myMap, enemyMap);
            enemyMapBin = Bot.ConfigureShips();
          
        }


        public void player_shoot(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (!shoot(enemyMapBin, button))
                while (Bot.shoot()) ;

            
        }

        public bool shoot(int[,] map, Button pressedButton)
        {
            bool hit = false;
            if (true)
            {
                int x;
                int y;

                y = (pressedButton.Location.X - 10 - 330) / 30 - 1;
                x = (pressedButton.Location.Y - 10) / 30 - 1;

                
                if (map[x, y] == 1)
                {
                    hit = true;
                    pressedButton.BackColor = Color.Orange;
                    pressedButton.Text = "X";
                    
                    enemyMapBin[x, y] = 0;
                    //isPlay = true;
                }
                else
                {
                    hit = false;
                    pressedButton.BackColor = Color.LightBlue;
                    pressedButton.Text = "X";
                    //isPlay = false;
                }
                pressedButton.Enabled = false;
                int a = 1;
                for (int i = 0; i < sizeXmap - 1; i++)
                {
                    for (int j = 0; j < sizeXmap - 1; j++)
                    {
                        if (enemyMapBin[i, j] == 1)
                        {
                            a = 2;
                            break;
                        }
                    }
                    if (a == 2) break;

                }
                if (a == 1)
                {
                    for (int i = 0; i < sizeXmap; i++)
                    {
                        for (int j = 0; j < sizeXmap; j++)
                        {
                            enemyMap[i, j].Enabled = false;
                        }
                    }
                    MessageBox.Show("You winned!");
                }
                

            }
            return hit;
        }


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
                    if (i < sizeXmap - 1 && j < sizeYmap - 1)
                    {
                        myMapBin[i, j] = 0;

                    }

                    // для номеров строк обеих карт
                    if ((j == 0) && (i != 0))
                    {
                        myMap[i, j].Text = i.ToString();
                        myMap[i, j].Enabled = false;

                        enemyMap[i, j].Text = i.ToString();

                    }
                    else if ((i == 0) && (j != 0)) // для алфовитной строки
                    {
                        myMap[i, j].Text = Alphabet[j - 1].ToString();
                        myMap[i, j].Enabled = false;
                        enemyMap[i, j].Text = Alphabet[j - 1].ToString();

                    }
                    else
                    {
                        //myMap[i, j].FlatStyle = FlatStyle.Flat;
                        //myMap[i, j].FlatAship_sizeearance.BorderSize = 4;
                        //myMap[i, j].FlatAship_sizeearance.BorderColor = Color.Blue;
                        
                        // внешний вид
                        myMap[i, j].BackColor = Color.White;
                        enemyMap[i, j].BackColor = Color.White;

                        // обработчики событий для каждой кнопки (клик, наведение, leave)
                        myMap[i, j].Click += new EventHandler(set_pos);
                        myMap[i, j].MouseEnter += new EventHandler(hover_temporaryRedZone);
                        myMap[i, j].MouseLeave += new EventHandler(leave_temporaryRedZone);

                        // дабавляем обработчик для кликов
                        
                        enemyMap[i, j].Click += new EventHandler(player_shoot);


                    }
                 

                    myMap[0, 0].Enabled = false;
                    myMap[0, 0].BackColor = SystemColors.Control;
                    enemyMap[0, 0].BackColor = SystemColors.Control;

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

//            if (click_hover)
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
                
                setColor(x, y, ship, Color.Black, toogle_v_h, true, true);
                // myMap[x, y].AutoSiz;
                enableColorButton = false;
                canIclick = 2;
            }

        }



        // color and coord ships
        // x, y - coord, ship_size, color_name, toogleLocation, access_to_write - 読み取り専用？, enable_this_function - lock or unlock
        public void setColor(int x, int y, int ship_size, Color color_name, bool toogleLocation, bool access_to_write=false, bool enable_this_function=false)
        {
            // для случая, когда индекс выходит за границу массива
            int acc = 1;

            // иногда необходимо отключить функциональность обработчика, по умолчанию false
            if (enable_this_function)
            {
                // vertical
                if (toogleLocation)
                {

                    for (int i = 0; i < ship_size; i++)
                    {
                        // карабль поместится в поле
                        if (x + i < sizeXmap)
                        {
                            // если истинно то записываем координаты в другой массив
                            if (access_to_write)
                            {
                                myMapBin[x - 1 + i, y - 1] = 1;
                                myMap[x + i, y].BackColor = color_name;
                            }

                            // если вертикальный корабль не пересекается с другими караблями то закрашивает
                            if (myMapBin[x - 1 + i, y - 1] == 0)
                            {
                                myMap[x + i, y].BackColor = color_name;
                            }
                            else
                            {
                                myMap[x + i, y].BackColor = Color.Red;
                            }


                        }
                        else // корабль не поместится в данное поле
                        {
                            // если истинно то записываем координаты в другой массив
                            if (access_to_write)
                            {
                                myMapBin[x - 1 - acc, y - 1] = 1;
                                myMap[x - acc, y].BackColor = color_name;

                            }

                            // если вертикальный корабль не пересекается с другими караблями то закрашивает
                            if (myMapBin[x - 1 - acc, y - 1] == 0)
                            {
                                myMap[x - acc, y].BackColor = color_name;

                            }



                            acc++;

                        }

                    }

                }
                else // horizontal
                {
                    for (int i = 0; i < ship_size; i++)
                    {
                        // карабль поместится в поле
                        if (y + i < sizeXmap)
                        {
                            // если кликаем то записываем координаты в другой массив
                            if (access_to_write)
                            {
                                myMapBin[x - 1, y - 1 + i] = 1;
                                //myMap[x, y + i].Enabled = false;
                                myMap[x, y + i].BackColor = color_name;
                                enableColorButton = true;
                            }
                            else
                            {
                                enableColorButton = false;
                            }

                            // если горизонтальный корабль пересекается с другими караблями то закрашивает
                            if (myMapBin[x - 1, y - 1 + i] == 0)
                            {
                                myMap[x, y + i].BackColor = color_name;
                                enableColorButton = true;
                            }

                        }
                        else // корабль не поместится в данное поле
                        {
                            // если кликаем то записываем координаты в другой массив
                            if (access_to_write)
                            {
                                myMapBin[x - 1, y - 1 - acc] = 1;
                                myMap[x, y - acc].BackColor = color_name;
                            }
                            // если горизонтальный корабль пересекается с другими караблями то закрашивает
                            if (myMapBin[x - 1, y - 1 - acc] == 0)
                            {
                                myMap[x, y - acc].BackColor = color_name;

                            }

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

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < sizeXmap - 1; i++)
            {
                for (int j = 0; j < sizeXmap - 1; j++)
                {
                    if (myMapBin[i, j] == 1)
                    {
                        myMap[i + 1, j + 1].BackColor = Color.Black;
                    }
                    if (enemyMapBin[i, j] == 1) { 
                        enemyMap[i + 1, j + 1].BackColor = Color.Black;
                    }
                }

            }
        }
    }
}
