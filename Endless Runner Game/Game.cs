using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Endless_Runner_Game
{
    public partial class Game : Form
    {
        bool jumping = false;
        int jumpSpeed = 0;

        // ПРОМЕНЕНО: Начална сила за овална траектория. 
        // Стойността 14 ни дава по-дълъг скок с плавно заобляне в горната точка.
        int force = 14;
        int defaultForce = 14;

        int score = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        int position;
        bool isGameOver = false;

        public Game()
        {
            InitializeComponent();

            // Прозорецът може да се увеличава и намалява свободно с мишката
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            SetupGameLayout();
            GameReset();

            this.KeyDown -= this.keyisdown;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyisdown);

            this.KeyUp -= this.keyisup;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyisup);
        }

        private void SetupGameLayout()
        {
            // Черната земя се вижда наполовина най-долу
            pictureBox1.Height = 100;
            pictureBox1.Width = this.ClientSize.Width + 1000;
            pictureBox1.Left = -10;
            pictureBox1.Top = this.ClientSize.Height - 50;
            pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // ПОПРАВЕНО: Върнат оригинален малък размер на T-Rex (без уголемяване)
            Trex.SizeMode = PictureBoxSizeMode.AutoSize;
            Trex.Left = 80;
            Trex.Top = pictureBox1.Top - Trex.Height;

            // ПОПРАВЕНО: Върнат оригинален малък размер и на кактусите, за да съответстват на динозавъра
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Name.ToLower().StartsWith("picturebox") && x.Name != "pictureBox1" && x.Name != "Trex")
                {
                    PictureBox cactus = (PictureBox)x;
                    cactus.SizeMode = PictureBoxSizeMode.AutoSize;
                    cactus.Top = pictureBox1.Top - cactus.Height;
                }
            }

            txtScore.Font = new Font("Arial", 14, FontStyle.Bold);
        }

        private void Game_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            Trex.Top += jumpSpeed;
            txtScore.Text = "Score: " + score;

            if (jumping == true)
            {
                // ПРОМЕНЕНО: Скоростта зависи от силата (force). 
                // В началото скача бързо нагоре, в пика скоростта става 0 (заобляне), а после пада надолу.
                jumpSpeed = -force;

                // Намаляваме силата плавно, за да симулираме гравитация
                force -= 1;
            }
            else
            {
                // Ако не е в режим на активен отскок, просто прилагаме стандартна скорост на падане
                jumpSpeed = 12;
            }

            // Приземяване точно върху по-ниската черна кутия
            int trexFloor = pictureBox1.Top - Trex.Height;
            if (Trex.Top > trexFloor)
            {
                jumping = false; // Спираме скока при докосване на земята
                force = defaultForce;
                Trex.Top = trexFloor;
                jumpSpeed = 0;
            }

            // Движение на кактусите
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Name.ToLower().StartsWith("picturebox") && x.Name != "pictureBox1" && x.Name != "Trex")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {
                        // Балансирано хубаво разстояние между малките кактуси
                        x.Left = this.ClientSize.Width + rand.Next(500, 1500);
                        x.Top = pictureBox1.Top - x.Height;
                        score++;
                    }

                    if (Trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        txtScore.Text += "  Game Over! Press R to Restart!";
                        isGameOver = true;
                    }
                }
            }

            if (score > 6)
            {
                obstacleSpeed = 14;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            int trexFloor = pictureBox1.Top - Trex.Height;
            if (e.KeyCode == Keys.Space && jumping == false && Trex.Top >= trexFloor)
            {
                jumping = true;
                force = defaultForce;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }
        }

        private void GameReset()
        {
            force = defaultForce;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;

            Trex.Image = Properties.Resources.running_1_;
            isGameOver = false;
            Trex.Top = pictureBox1.Top - Trex.Height;

            int spacing = 0;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Name.ToLower().StartsWith("picturebox") && x.Name != "pictureBox1" && x.Name != "Trex")
                {
                    spacing += rand.Next(500, 1100);
                    x.Left = this.ClientSize.Width + spacing;
                    x.Top = pictureBox1.Top - x.Height;
                }
            }

            gameTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pictureBox1 != null && Trex != null)
            {
                pictureBox1.Top = this.ClientSize.Height - 50;
                if (!jumping && jumpSpeed == 0)
                {
                    Trex.Top = pictureBox1.Top - Trex.Height;
                }
            }
        }

        // Празни методи за стабилност на дизайнера
        private void Trex_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
    }
}