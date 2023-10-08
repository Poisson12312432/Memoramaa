using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;


namespace Memorama
{
    public partial class Form1 : Form
    {
        Button[] b = new Button[30];
        List<int> imagenes = new List<int>();
        int primerClic = -1;
        int coincidencias = 0;
        Button btnIniciarPartida = new Button();

        int turnoJugador = 1;
        int puntosJugador1 = 0;
        int puntosJugador2 = 0;

        Label lblJugador1;
        Label lblJugador2;
        private System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
        private int tiempoRestante = 10;


        public Form1()
        {
            InitializeComponent();
            Text = "Memorama";
            this.btnIniciarPartida.Text = "Iniciar Partida";
            Controls.Add(this.btnIniciarPartida);
            this.btnIniciarPartida.Click += BtnIniciarPartida_Click;

            for (int i = 1; i <= 15; i++)
            {
                imagenes.Add(i);
                imagenes.Add(i);
            }

            for (int i = 0; i < 30; i++)
            {
                b[i] = Controls.Find("button" + (i + 2), true).FirstOrDefault() as Button;
            }

            foreach (Button btn in b)
            {
                btn.Click += BotonClic;
            }
            lblJugador1 = new Label();
            lblJugador1.Text = "Jugador 1: 0 puntos";
            lblJugador1.Location = new Point(10, 50);
            Controls.Add(lblJugador1);

            lblJugador2 = new Label();
            lblJugador2.Text = "Jugador 2: 0 puntos";
            lblJugador2.Location = new Point(10, 80);
            Controls.Add(lblJugador2);

            temporizador.Interval = 1000;
            temporizador.Tick += timer1_Tick_1;
            temporizador.Start();
            aleatorio();
        }

        private void InicializarBotones()
        {
            for (int i = 0; i < 30; i++)
            {
                b[i] = Controls.Find("button" + (i + 2), true).FirstOrDefault() as Button;
                b[i].Image = null;
            }
        }

        private void BotonClic(object? sender, EventArgs e)
        {
            Button? botonClic = sender as Button;
            if (botonClic == null || botonClic.Image != null)
            {
                return;
            }

            int indice = Array.IndexOf(b, botonClic);
            if (indice != -1)
            {
                try
                {
                    string rutaImagen = Path.Combine("Imagenes", $"imagen{imagenes[indice]}.png");
                    if (File.Exists(rutaImagen))
                    {
                        botonClic.Image = Image.FromFile(rutaImagen);
                    }
                    else
                    {
                        Console.WriteLine($"La imagen no existe en la ruta: {rutaImagen}");
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                }

                if (primerClic == -1)
                {
                    primerClic = indice;
                }
                else
                {
                    if (imagenes[primerClic] == imagenes[indice])
                    {
                        coincidencias++;
                        if (coincidencias == 15)
                        {
                            MessageBox.Show($"¡Jugador {turnoJugador} ha ganado!");
                            ReiniciarJuego();
                        }
                        CambiarTurno();
                        ActualizarEtiquetasPuntos();


                    }
                    else
                    {
                        b[primerClic].Image = null;
                        botonClic.Image = null;
                    }

                }


            }
        }
        private void CambiarTurno()
        {
            turnoJugador = (turnoJugador == 1) ? 2 : 1;
        }
        private void ActualizarEtiquetasPuntos()
        {
            lblJugador1.Text = $"Jugador 1: {puntosJugador1} puntos";
            lblJugador2.Text = $"Jugador 2: {puntosJugador2} puntos";
        }
        private void BtnIniciarPartida_Click(object sender, EventArgs e)
        {
            if (coincidencias == 15)
            {
                ReiniciarJuego();
            }
            else
            {
                DetenerTemporizador();
                aleatorio();
                ReiniciarJuego();
                IniciarTemporizador();
            }

        }

        private void IniciarNuevaPartida()
        {
            ReiniciarJuego();
        }
        public void aleatorio()
        {
            imagenes = Shuffle(imagenes);
            for (int i = 0; i < 30; i++)
            {
                b[i].ImageIndex = imagenes[i];
            }
        }

        private void ReiniciarJuego()
        {
            puntosJugador1 = 0;
            puntosJugador2 = 0;

            ActualizarEtiquetasPuntos();

            foreach (Button btn in b)
            {
                btn.ImageIndex = -1;
            }

            DetenerTemporizador();
            IniciarTemporizador();
            imagenes = Shuffle(imagenes);
            coincidencias = 0;
            primerClic = -1;
            temporizador.Stop();
            tiempoRestante = 10;
            temporizador.Start();
            aleatorio();
        }
        private void DetenerTemporizador()
        {
            temporizador.Stop();
        }
        private void IniciarTemporizador()
        {
            temporizador.Start();
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            tiempoRestante--;

            if (tiempoRestante <= 0)
            {
                CambiarTurno();


                ActualizarEtiquetasPuntos();


                tiempoRestante = 10;
            }
            if (coincidencias == 15)
            {
                temporizador.Stop();
            }

        }

        private List<T> Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

    }
}