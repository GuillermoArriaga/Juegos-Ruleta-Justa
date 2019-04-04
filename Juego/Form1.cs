using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Juego
{
    public partial class Form1 : Form
    {
        int Distancia = 130;
        Point PosicionInicial = new Point(12, 66);
        List<PictureBox> pb = new List<PictureBox>(16);
        PictureBox[] pbApuesta = new PictureBox[16];
        Random aleatorio = new Random();
        int imagenActual = 0;
        SoundPlayer sonido;
        double[] apuesta = new double[16];
        double cantidadGanada;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "     BIENVENIDO A la RULETA de PORRISTAS\n" +
                "\n" +
                "\n" +
                "Este juego paga 15:1 y se recupera la apuesta si se ganó.\n" +
                "\n" +
                "Para apostar dé doble click en la imagen que deseé y para deshacerla dé doble click en la misma imagen pero fuera de la moneda que indica la apuesta realizada.\n" +
                "\n" +
                "Si elige 8 imagenes y apuesta lo mismo a todas, entonces ganaría al final con paga equivalente a 1:1\n" +
                "De igual manera, si va con 4 con misma apuesta, la paga equivale a 3:1 y así sucesivamente.\n" +
                "\n" +
                "Este juego equivale completamente a una ruleta justa\n"
            );

            pb.Add(pb1);
            pb.Add(pb2);
            pb.Add(pb3);
            pb.Add(pb4);
            pb.Add(pb5);
            pb.Add(pb6);
            pb.Add(pb7);
            pb.Add(pb8);
            pb.Add(pb9);
            pb.Add(pb10);
            pb.Add(pb11);
            pb.Add(pb12);
            pb.Add(pb13);
            pb.Add(pb14);
            pb.Add(pb15);
            pb.Add(pb16);

            // Ubicacion de imagenes
            pb[0].Location = PosicionInicial;

            for (int i = 0; i < 16; i++)
            {
                pbApuesta[i] = new PictureBox();
                pbApuesta[i].Image = Properties.Resources.MonedaPlata;
                pbApuesta[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pbApuesta[i].Size = new Size(Distancia / 2, Distancia / 2);
                pbApuesta[i].Location = new Point(Distancia / 4, Distancia / 4);
                pbApuesta[i].Visible = false;
                pbApuesta[i].Parent = pb[i];

                apuesta[i] = new double();
                apuesta[i] = 0;

                if (i >= 1 && i <= 4) pb[i].Location = new Point(pb[i - 1].Location.X + Distancia, pb[i - 1].Location.Y);
                if (i >= 5 && i <= 8) pb[i].Location = new Point(pb[i - 1].Location.X , pb[i - 1].Location.Y + Distancia);
                if (i >= 9 && i <= 12) pb[i].Location = new Point(pb[i - 1].Location.X - Distancia, pb[i - 1].Location.Y);
                if (i >= 13 && i <= 15) pb[i].Location = new Point(pb[i - 1].Location.X, pb[i - 1].Location.Y - Distancia);
                pb[i].Size = new Size(Distancia, Distancia);
                pb[i].Image = pb[i].BackgroundImage;
                pb[i].BackgroundImage = null;
                pb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                
            }

            pbFondo.Size = new Size(Distancia*3, Distancia*3);
            pbFondo.Location= new Point(pb[1].Location.X, pb[1].Location.Y + Distancia);
            pbFondo.Image = Properties.Resources.giphy__1_;
            pbFondo.SizeMode = PictureBoxSizeMode.StretchImage;

            pbGanador.Location = new Point(Distancia/2, Distancia/2);
            pbGanador.Size = new Size(Distancia*2, Distancia*2);
            pbGanador.SizeMode = PictureBoxSizeMode.StretchImage;
            pbGanador.Visible = false;
            pbGanador.Parent = pbFondo;
            Size = new Size(3 * PosicionInicial.X + 5 * Distancia, PosicionInicial.Y + 5 * Distancia + PosicionInicial.X);

            sonido = new SoundPlayer(Properties.Resources.Cheer_Mix___Uptown_Funk__30_);
            sonido.Play();
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            timerAnimacion.Enabled = true;
            timerFin.Enabled = true;

            sonido = new SoundPlayer(Properties.Resources.Tómbola_de_Premios_para_TEDx_Pura_Vida_2012);
            sonido.Play();
        }

        private void bSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timerAnimacion_Tick(object sender, EventArgs e)
        {
            pb[(imagenActual + 16 - 1) % 16].Visible = true;    // Vuelve visible la imagen anterior del ciclo anterior
            pb[imagenActual].Visible = false;       // Vuelve invisible la imagen anterior de este ciclo
            imagenActual = (imagenActual+1) % 16;   // Ubica la imagen actual
            pb[imagenActual].Visible = true;       // Vuelve visible la imagen seleccionada de este ciclo
            pb[(imagenActual + 1) % 16].Visible = false;    // Vuelve invisible la imagen siguiente de este ciclo

            int duracionSiguiente= timerAnimacion.Interval - aleatorio.Next(70, 100);
            if (duracionSiguiente < 10) duracionSiguiente = aleatorio.Next(5, 50);

            timerAnimacion.Interval = duracionSiguiente;
        }

        private void timerFin_Tick(object sender, EventArgs e)
        {
            timerAnimacion.Enabled = false;
            timerFin.Enabled = false;
            pbGanador.Image = pb[imagenActual].Image;

            // Muestra todas las imagenes
            pb[(imagenActual + 16 - 1) % 16].Visible = true;    // Vuelve visible la imagen anterior a la ganadora
            pb[(imagenActual + 1) % 16].Visible = true;    // Vuelve visible la imagen siguiente a la ganadora

            // Muestra imagen ganadora al centro
            pbGanador.Image = pb[imagenActual].Image; // entero aleatorio entre 0 y 15
            pbGanador.Visible = true;

            cantidadGanada = apuesta[imagenActual] * 16;

            if (cantidadGanada > 0)   //Exito
            {
                sonido = new SoundPlayer(Properties.Resources.FuegosArtificiales);
                sonido.Play();
            }
            else    //Fracaso
            {
                sonido = new SoundPlayer(Properties.Resources.Risa);
                sonido.Play();
            }

            timerResultado.Enabled = true;
        }

        private void tbCredito_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double credito = Convert.ToDouble(tbCredito.Text);
                tbCredito.Text = credito.ToString();
                if (credito < 0)
                {
                    MessageBox.Show("Se inserto Crédito negativo. Se cambiará 1000");
                    tbCredito.Text = "1000";
                }
            }
            catch
            {
                MessageBox.Show("Se inserto un caracer no numerico en Crédito. Se cambiará 1000");
                tbCredito.Text = "1000";
            }

            if (Convert.ToDouble(tbApuesta.Text) > Convert.ToDouble(tbCredito.Text))
            {
                tbApuesta.Text = tbCredito.Text;
            }
        }

        private void tbApuesta_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                double apuesta = Convert.ToDouble(tbApuesta.Text);
                tbApuesta.Text = apuesta.ToString();
                if (apuesta < 0)
                {
                    MessageBox.Show("Se inserto Apuesta negativa. Se cambiará a 100");
                    tbCredito.Text = "100";
                }
            }
            catch
            {
                MessageBox.Show("Se inserto un caracer no numerico en Apuesta. Se cambiará 100.");
                tbApuesta.Text = "100";
            }
            if (Convert.ToDouble(tbApuesta.Text) > Convert.ToDouble(tbCredito.Text))
            {
                tbApuesta.Text = tbCredito.Text;
            }
        }

        private void bAumentarCredito_Click(object sender, EventArgs e)
        {
            tbCredito.Text = (Convert.ToDouble(tbCredito.Text) + 100).ToString();
        }

        private void bDisminuirCredito_Click(object sender, EventArgs e)
        {
            double cantidad = Convert.ToDouble(tbCredito.Text) - 30;
            if (cantidad < 0) cantidad = 0;
            tbCredito.Text = (cantidad).ToString();
        }

        private void bAumentarApuesta_Click(object sender, EventArgs e)
        {
            tbApuesta.Text = (Convert.ToDouble(tbApuesta.Text) + 10).ToString();
        }

        private void bDisminuirApuesta_Click(object sender, EventArgs e)
        {
            double cantidad = Convert.ToDouble(tbApuesta.Text) - 5;
            if (cantidad < 0) cantidad = 0;
            tbApuesta.Text = (cantidad).ToString();
        }

        private void bApuestaMaxima_Click(object sender, EventArgs e)
        {
            tbApuesta.Text = tbCredito.Text;
        }

        private void bApuestaMinima_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(tbCredito.Text) < 1)
            {
                tbApuesta.Text = tbCredito.Text;
            }
            else
            {
                tbApuesta.Text = "1";
            }            
        }

        // Posicion de apuestas segun click en imagen
        private void imagen_Click(object sender, EventArgs e)
        {
            int imagenNumero=Convert.ToInt32(((PictureBox)sender).Tag);

            if (pbApuesta[imagenNumero].Visible)
            {
                pbApuesta[imagenNumero].Visible = false;    // Deshace apuesta
                tbCredito.Text = (Convert.ToDouble(tbCredito.Text) + apuesta[imagenNumero]).ToString();
                apuesta[imagenNumero] = 0;
            }
            else
            {
                pbApuesta[imagenNumero].Visible = true;    // Hace apuesta
                apuesta[imagenNumero] = Convert.ToDouble(tbApuesta.Text);
                tbCredito.Text = (Convert.ToDouble(tbCredito.Text) - apuesta[imagenNumero]).ToString();
            }
        }

        private void timerResultado_Tick(object sender, EventArgs e)
        {
            timerResultado.Enabled = false;
            MessageBox.Show("Ha ganado " + cantidadGanada + " créditos. Los cuales están por bonificarse al aceptar.");

            // Reinicio para siguiente juego
            tbCredito.Text = (Convert.ToDouble(tbCredito.Text) + cantidadGanada).ToString();

            for (int i = 0; i < apuesta.Length; i++)
            {
                apuesta[i] = 0;
                pbApuesta[i].Visible = false;
            }
            pbGanador.Visible = false;

            sonido = new SoundPlayer(Properties.Resources.Cheer_Mix___Uptown_Funk__30_);
            sonido.Play();
        }
    }
}
