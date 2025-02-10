using System;
using System.Drawing;
using System.Windows.Forms;

namespace cproj
{
    public class MainForm : Form
    {
        private int score = 0;
        private Label scoreLabel;
        private PictureBox cookiePictureBox;
        private Timer clickTimer;
        private Timer hoverTimer;
        private bool isHovering = false;
        private int baseSize = 200; // Výchozí velikost cookie
        private bool isFullscreen = false;
        private Button closeButton;

        public MainForm()
        {
            // Nastavení hlavního okna
            this.Text = "Cookie Clicker";
            this.Width = 1920;
            this.Height = 1080;
            this.BackColor = Color.Pink; // Růžové pozadí
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Moderní vzhled bez rámečku

            // Nastavení ikony aplikace
            try
            {
                this.Icon = new Icon("cookie.ico"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání ikony: " + ex.Message);
            }

            cookiePictureBox = new PictureBox
            {
                Width = baseSize,
                Height = baseSize,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent // Žádné pozadí
            };

            try
            {
                cookiePictureBox.Image = Image.FromFile("cookie.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání obrázku: " + ex.Message);
            }

            cookiePictureBox.MouseEnter += CookieHoverStart;
            cookiePictureBox.MouseLeave += CookieHoverEnd;
            cookiePictureBox.Click += CookieClicked;
            this.Controls.Add(cookiePictureBox);

            // Skóre vedle cookie
            scoreLabel = new Label
            {
                Text = "0",
                Font = new Font("Arial", 32, FontStyle.Bold),
                ForeColor = Color.Gold,
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);

            // Zavírací tlačítko (červené "X")
            closeButton = new Button
            {
                Text = "X",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 40
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => Application.Exit();
            this.Controls.Add(closeButton);

            // Timer pro klikací efekt (přirozený bounce)
            clickTimer = new Timer { Interval = 10 };
            clickTimer.Tick += (s, e) => AnimateClick();

            // Timer pro hover efekt
            hoverTimer = new Timer { Interval = 10 };
            hoverTimer.Tick += (s, e) => AnimateHover();

            // Zpracování kláves (F11 = fullscreen)
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F11)
                {
                    ToggleFullscreen();
                }
            };

            CenterElements();
            this.Resize += (s, e) => CenterElements();
        }

        private void CookieClicked(object sender, EventArgs e)
        {
            score++;
            scoreLabel.Text = score.ToString();

            // Plynulý bounce efekt při kliknutí
            clickTimer.Start();
        }

        private int clickPhase = 0;
        private void AnimateClick()
        {
            if (clickPhase == 0)
            {
                cookiePictureBox.Width += 15;
                cookiePictureBox.Height += 15;
                clickPhase++;
            }
            else if (clickPhase == 1)
            {
                cookiePictureBox.Width -= 10;
                cookiePictureBox.Height -= 10;
                clickPhase++;
            }
            else if (clickPhase == 2)
            {
                cookiePictureBox.Width += 5;
                cookiePictureBox.Height += 5;
                clickPhase++;
            }
            else
            {
                clickPhase = 0;
                clickTimer.Stop();
            }
            CenterElements();
        }

        private void CookieHoverStart(object sender, EventArgs e)
        {
            isHovering = true;
            hoverTimer.Start();
        }

        private void CookieHoverEnd(object sender, EventArgs e)
        {
            isHovering = false;
            hoverTimer.Start();
        }

        private void AnimateHover()
        {
            if (isHovering && cookiePictureBox.Width < baseSize + 10)
            {
                cookiePictureBox.Width += 1;
                cookiePictureBox.Height += 1;
            }
            else if (!isHovering && cookiePictureBox.Width > baseSize)
            {
                cookiePictureBox.Width -= 1;
                cookiePictureBox.Height -= 1;
            }
            else
            {
                hoverTimer.Stop();
            }
            CenterElements();
        }

        private void CenterElements()
        {
            cookiePictureBox.Location = new Point((this.ClientSize.Width - cookiePictureBox.Width) / 2, 
                                                  (this.ClientSize.Height - cookiePictureBox.Height) / 2);

            scoreLabel.Location = new Point(cookiePictureBox.Right + 20, 
                                            cookiePictureBox.Top + (cookiePictureBox.Height / 2) - (scoreLabel.Height / 2));

            closeButton.Location = new Point(this.ClientSize.Width - closeButton.Width - 10, 10);
        }

        private void ToggleFullscreen()
        {
            if (isFullscreen)
            {
           // Zpět do normálního režimu
            this.FormBorderStyle = FormBorderStyle.Sizable; // Umožní změnu velikosti okna
           this.WindowState = FormWindowState.Normal;
            this.Width = 800; // Nastaví původní šířku
           this.Height = 600; // Nastaví původní výšku
    }
        else
         {
         // Přejít do fullscreen režimu
           this.FormBorderStyle = FormBorderStyle.None; // Skryje rámeček okna
         this.WindowState = FormWindowState.Maximized; // Okno bude maximalizováno
      }
     isFullscreen = !isFullscreen;
     CenterElements(); // Přecentrace prvků
}


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
