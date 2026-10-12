using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Grupowanie
{
    public partial class Form1 : Form
    {
        private readonly List<PointF> punkty = new List<PointF>();

        public Form1()
        {
            InitializeComponent();

            ClientSize = new Size(800, 700);
            pictureBox1.Dock = DockStyle.None;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.BackColor = Color.White;

            Load += (s, e) => WczytajPunkty();
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.Resize += (s, e) => pictureBox1.Invalidate();
        }

        private void WczytajPunkty()
        {
            string sciezka = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "spiralka.csv");

            punkty.Clear();

            foreach (string wiersz in File.ReadLines(sciezka))
            {
                string[] dane = wiersz.Split(',');

                float x = float.Parse(dane[0], CultureInfo.InvariantCulture);
                float y = float.Parse(dane[1], CultureInfo.InvariantCulture);

                punkty.Add(new PointF(x, y));
            }

            Text = $"Wczytano {punkty.Count} punktów";
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float cx = pictureBox1.ClientSize.Width / 2f;
            float cy = pictureBox1.ClientSize.Height / 2f;
            float skala = (Math.Min(cx, cy) - 40) / 2.2f;

            if (skala <= 0) return;

            g.DrawLine(Pens.Gray, 0, cy, 2 * cx, cy);
            g.DrawLine(Pens.Gray, cx, 0, cx, 2 * cy);

            g.DrawString("x1", Font, Brushes.Black, 2 * cx - 25, cy + 5);
            g.DrawString("x2", Font, Brushes.Black, cx + 5, 5);

            for (int i = -2; i <= 2; i++)
            {
                float x = cx + i * skala;
                float y = cy - i * skala;

                g.DrawLine(Pens.Gray, x, cy - 3, x, cy + 3);
                g.DrawLine(Pens.Gray, cx - 3, y, cx + 3, y);

                g.DrawString(i.ToString(), Font, Brushes.Black, x - 8, cy + 5);

                if (i != 0)
                    g.DrawString(i.ToString(), Font, Brushes.Black, cx + 5, y - 8);
            }

            foreach (PointF p in punkty)
                g.FillRectangle(Brushes.Black,
                    cx + p.X * skala - 3,
                    cy - p.Y * skala - 3,
                    6, 6);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}