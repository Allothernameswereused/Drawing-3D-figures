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

namespace Graphics_3_Yaroslavsky
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const double PI = 3.14159; //Константы для вычисления
        private const double rd = 0.3535534;
        private Graphics gln;  //Графика
        long px, py;
        float px0, py0;
        private static double z1, z2, z3;

        private static long xx, yy;

        int cbh;
        public pointcubic[] cubeexample; //Куб для инициализации, по нему измеряются величины
        public pointcubic[] cubebuild; //Рабочая переменная, в ней хранится информация по рисуемому кубу
        public pointcubic[] cubes; //Рабочая переменная, нужна для обработки вращения куба
        public Point[] cubeproject; //Проекции куба
        private int point_number = 16; //Число точек на контуре куба
        public int Mx, My;

        private double scale = 1.0;

        double a = 0, sn = 1.0, cs = 0, h = PI / 20, sinh, cosh; 
        pointcubic z;


        private void button1_Click(object sender, EventArgs e) //Рисует спираль
        {
            double x, y, t, a, b;

            Color maincolor = Color.Blue;
            int penwidth = 1;
            Pen pn = new Pen(maincolor, penwidth);

            Mx = panel1.Width / 2;
            My = panel1.Height / 5;

            gln = panel1.CreateGraphics();

            px0 = 0;
            py0 = 0;

            for (int counter = 0; counter <= 450; counter++)
            {
                t = counter * PI / 180.0;
                a = 0.3 * t;
                b = 10 * t;
                z1 = a;
                z2 = Math.Cos(b);
                z3 = Math.Sin(b);
                x = z2 - rd * z3;
                y = z1 - rd * z3;
                px = (int)Math.Round(Mx + 100 * x);
                py = (int)Math.Round(My + 100 * y);
                if (counter == 0)
                {
                    px0 = px;
                    py0 = py;
                }

                gln.DrawLine(pn, px0, py0, px, py);
                px0 = px;
                py0 = py;
            }
        }

        private void button2_Click(object sender, EventArgs e) //Рисует сферу
        {
            float x, y, t, a, b, z1, z2, z3;
            Mx = panel1.Width / 2;
            My = panel1.Height / 2;
            Color maincolor = Color.Blue;
            int penwidth = 1;
            Pen pn = new Pen(maincolor, penwidth);

            gln = panel1.CreateGraphics();

            px0 = py0 = 0;

            for (int counter = 0; counter <= 620; counter++)
            {
                t = (float)(counter * PI / 180.0);
                a = (float)0.3 * t;
                b = (float)10 * t;
                z1 = (float)Math.Sin(b);
                z2 = (float)Math.Cos(b) * (float)Math.Cos(a);
                z3 = (float)Math.Cos(b) * (float)Math.Sin(a);
                x = z2 - (float)rd * z3;
                y = z1 - (float)rd * z3;
                px = (int)Math.Round(Mx + 100 * x);
                py = (int)Math.Round(My + 100 * y);
                if (counter == 0)
                {
                    px0 = px;
                    py0 = py;
                }

                gln.DrawLine(pn, px0, py0, px, py);
                px0 = px;
                py0 = py;
            }
        }

        private void button3_Click(object sender, EventArgs e) //Рисует поверхность
        {
            Color maincolor = Color.Blue;
            int penwidth = 1;
            Pen pn = new Pen(maincolor, penwidth);

            gln = panel1.CreateGraphics();

            px0 = py0 = 0;

            long Mx = panel1.Width / 2;
            long My = panel1.Height / 4;

            z3 = -1.2;

            while (z3 <= 1.2)
            {
                z2 = -1.2;
                Calculations(Mx, My);
                px0 = xx;
                py0 = yy;
                while (z2 <= 1.2)
                {
                    Calculations(Mx, My);
                    gln.DrawLine(pn, px0, py0, xx, yy);
                    z2 = z2 + 0.04;
                    px0 = xx;
                    py0 = yy;
                }

                z3 = z3 + 0.12;
            }
        }
        public static void Calculations(long Mx, long My) //Под-функция, нужна для просчёта поверзности
        {
            z1 = 2 * Math.Exp(-z2 * z2 - z3 * z3);
            xx = (long)Math.Round(Mx + 100.0 * z2 - (rd * 100) * z3);
            yy = (long)Math.Round(My + 100.0 * z1 - (rd * 100) * z3);
        }

        private void button4_Click(object sender, EventArgs e) //Рисует куб
        {
            gln = panel1.CreateGraphics();
            cbh = 50;
            Mx = panel1.Width / 2;
            My = panel1.Height / 2;
            cubebuild = new pointcubic[point_number];
            Cube_Ini();
            for (int i = 0; i < point_number; i++)
            {
                cubebuild[i] = new pointcubic();
                cubebuild[i].x = cubeexample[i].x * cbh;
                cubebuild[i].y = cubeexample[i].y * cbh;
                cubebuild[i].z = cubeexample[i].z * cbh;
                cubebuild[i].x = cubebuild[i].x + Mx;
                cubebuild[i].y = cubebuild[i].y + My;
            }
            Cube_Projections();
            Cube_Draw(gln, new Pen(Color.Red, 1));
        }

        private void button5_Click(object sender, EventArgs e) //Масштабирует куб
        {
            scale = 1;
            Pen pen = new Pen(Color.Red, 1);
            int pause = 300;         
            int i = 0;
            while (true)
            {
                if (scale > 1.4)
                {
                    break;
                }
                else
                {
                    if ((i%2)==0)
                    Cube_Projections(); Cube_Scaling((float)scale);
                    Cube_Draw(gln, new Pen(panel1.BackColor, 1));
                    Cube_Scaling((float)scale);
                    scale = scale + 0.1;
                    Cube_Projections();
                    Cube_Scaling((float)scale); 
                    Cube_Projections();
                    Cube_Draw(gln, pen);
                    Thread.Sleep(pause);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e) //Перемещает куб
        { 
            gln = panel1.CreateGraphics();
            Color clr = Color.Red;
            int pause = 50;
            while (true)
            {
                if (cubeproject[1].X < 100)
                { 
                    break;
                }
                else
                {    
                    clear_panel();
                    Cube_Projections();
                    Cube_Draw(gln, new Pen(clr, 1));
                    Thread.Sleep(pause); 
                    TranCub(5); //Перемещение куба, скорость пропорциональна величине число в функции
                }
            }
        }



        private void button7_Click(object sender, EventArgs e) //Вращает куб по оси
        {
            sinh = Math.Sin(h); cosh = Math.Cos(h); a = 0; cs = Math.Cos(a); sn = Math.Sin(a); z = new pointcubic(cubebuild[0].x, cubebuild[0].y, 0); int i = 0; cs = Math.Cos(a); sn = Math.Sin(a); cubes = new pointcubic[point_number]; for (i = 0; i < point_number; i++) cubes[i] = new pointcubic(0, 0, 0); Color clr; int pause = 300;           //if ((ii % 2) == 0)14
            int k = 0; while (true)
            {
                if (a > 2 * PI)
                    break;
                else
                {
                    if ((k % 2) == 0)
                        clr = panel1.BackColor;
                    else clr = Color.Red; 
                    k++; 
                    CirCub(sn, cs);
                    for (i = 0; i < point_number; i++)
                    {
                        cubeproject[i].X = (int)Math.Round(cubes[i].y - rd * (float)cubes[i].z);
                        cubeproject[i].Y = (int)Math.Round(cubes[i].x - rd * (float)cubes[i].z);
                    }
                    Cube_Draw(gln, new Pen(clr, 1));
                    Thread.Sleep(pause);
                    cs = cs * cosh - sn * sinh;
                    sn = cs * sinh + sn * cosh;
                    a = a + h;
                    clear_panel();
                }
            }
        }



        private void button8_Click(object sender, EventArgs e) //Очистка всего поля
        {
            clear_panel();
        }

        public struct pointcubic //Структура, точка куба с тремя координатами
        {
            public float x, y, z;
            public pointcubic(float x1, float y1, float z1) { x = x1; y = y1; z = z1; }
        };


        public void Cube_Ini() //Инициализация точек куба
        {
            cubeexample = new pointcubic[point_number];
            cubeexample[0] = new pointcubic(0, 0, 0);
            cubeexample[1] = new pointcubic(1, 0, 0);
            cubeexample[2] = new pointcubic(1, 1, 0);
            cubeexample[3] = new pointcubic(0, 1, 0);
            cubeexample[4] = new pointcubic(0, 0, 0);
            cubeexample[5] = new pointcubic(0, 0, 1);
            cubeexample[6] = new pointcubic(1, 0, 1);
            cubeexample[7] = new pointcubic(1, 0, 0);
            cubeexample[8] = new pointcubic(1, 0, 1);
            cubeexample[9] = new pointcubic(1, 1, 1);
            cubeexample[10] = new pointcubic(1, 1, 0);
            cubeexample[11] = new pointcubic(1, 1, 1);
            cubeexample[12] = new pointcubic(0, 1, 1);
            cubeexample[13] = new pointcubic(0, 1, 0);
            cubeexample[14] = new pointcubic(0, 1, 1);
            cubeexample[15] = new pointcubic(0, 0, 1);
        }

        public void Cube_Projections() //Рисует проекции куба
        {
            cubeproject = new Point[point_number];
            for (int i = 0; i < point_number; i++)
            {
                cubeproject[i] = new Point();
                cubeproject[i].X = (int)Math.Round(cubebuild[i].y - rd * (float)cubebuild[i].z);
                cubeproject[i].Y = (int)Math.Round(cubebuild[i].x - rd * (float)cubebuild[i].z);
            }
        }

        public void Cube_Draw(Graphics gg, Pen pn) //Рисует куб по проекциям
        {

            int px0, py0, px, py;
            px0 = (int)cubeproject[0].X;
            py0 = (int)cubeproject[0].Y;

            for (int i = 0; i < 16; i++)
            {
                px = cubeproject[i].X;
                py = cubeproject[i].Y;
                gg.DrawLine(pn, px0, py0, px, py);
                px0 = px;
                py0 = py;
            }
        }

        public void Cube_Scaling(float s) //Масштабирует куб
        { 
            pointcubic zf = new pointcubic(Mx, My, 0); 
            for (int i = 0; i < point_number; i++) 
            { 
                cubebuild[i].x = (float)cubebuild[i].x * s + (float)zf.x * (1 - s);
                cubebuild[i].y = (float)cubebuild[i].y * s + (float)zf.y * (1 - s);
                cubebuild[i].z = (float)cubebuild[i].z * s + (float)zf.z * (1 - s);
            } 
        }

        public void TranCub(float dz) //Перемещает куб
        { 
            for (int i = 0; i < point_number; i++) 
            {
                cubebuild[i].z = cubebuild[i].z + dz;
            } 
        }

        private void clear_panel() //Очистка поля, вызывается несколькими функциями выше
        {
            SolidBrush mass_eraser = new SolidBrush(panel1.BackColor);
            Rectangle field = new Rectangle(0,0, panel1.Width, panel1.Height); 
            gln.FillRectangle(mass_eraser, field);
        }


        public void CirCub(double sn, double cs) //"Поворачивает" куб
        {
            for (int i = 0; i < point_number; i++)
            {
                cubes[i].x = (float)(z.x + (cubebuild[i].x - z.x) * cs - (cubebuild[i].y - z.y) * sn);
                cubes[i].y = (float)(z.y + (cubebuild[i].y - z.y) * cs - (cubebuild[i].x - z.x) * sn);
                cubes[i].z = cubebuild[i].z;
            }
        }


    }
}
