using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GestureRecognitionWith_1.Source;
using System.Collections;

namespace GestureRecognitionWith_1
{
    public partial class Form1 : Form
    {
        private bool clickApretado = false;
        private Trayectoria trayectoriaOriginal;
        private Trayectoria trayectoriaProcesada;
        private Hashtable gestos;
        private const int MINPUNTOS = 5;
        private Reconocedor reconocedor;

        public Form1()
        {
            InitializeComponent();
            trayectoriaOriginal = new Trayectoria();
            trayectoriaProcesada = new Trayectoria();
            gestos = new Hashtable();
            reconocedor = new Reconocedor();
        }

        private void dibujarPunto(Panel panel, int x, int y)
        {
            Graphics g = panel.CreateGraphics();
            g.FillEllipse(Brushes.Green, new Rectangle(x-3, y-3, 5, 5));
        }

        private void dubujarCentroide(Panel panel, int x, int y)
        {
            Graphics g = panel.CreateGraphics();
            Brush brush = new SolidBrush(Color.Red);
            g.FillEllipse(brush, new Rectangle(x - 5, y - 5, 9, 9));
        }

        private void resetPanel(Panel panel)
        {
            Graphics g = panel.CreateGraphics();
            Brush brush = new SolidBrush(panel.BackColor);
            g.FillRectangle(brush, new Rectangle(0, 0, panel.Size.Width, panel.Size.Height));
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            this.clickApretado = true;
            clrearAll();
            this.trayectoriaOriginal.Add(new Vector2(e.X, e.Y));
            dibujarPunto(this.panel1, e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            clickApretado = false;
            Vector2 centroide = trayectoriaOriginal.getMedia();
            dubujarCentroide(this.panel1, (int)centroide.X, (int)centroide.Y);
            trayectoriaProcesada = reconocedor.remuestrear(this.trayectoriaOriginal.clon());
            for (int i = 0; i < trayectoriaProcesada.Count; i++)
                dibujarPunto(this.panel2, (int)trayectoriaProcesada[i].X, (int)trayectoriaProcesada[i].Y);
            centroide = trayectoriaProcesada.getMedia();
            dubujarCentroide(this.panel2, (int)centroide.X, (int)centroide.Y);
            
            trayectoriaProcesada = reconocedor.rotarHastaAngulo0(trayectoriaProcesada);
            for (int i = 0; i < trayectoriaProcesada.Count; i++)
                dibujarPunto(this.panel3, (int)trayectoriaProcesada[i].X, (int)trayectoriaProcesada[i].Y);
            centroide = trayectoriaProcesada.getMedia();
            dubujarCentroide(this.panel3, (int)centroide.X, (int)centroide.Y);

            trayectoriaProcesada.centrar();
            for (int i = 0; i < trayectoriaProcesada.Count; i++)
                dibujarPunto(this.panel4, (int)trayectoriaProcesada[i].X + this.panel4.Size.Height / 2, (int)trayectoriaProcesada[i].Y + this.panel4.Size.Height / 2);
            centroide = trayectoriaProcesada.getMedia();
            dubujarCentroide(this.panel4, (int)centroide.X + this.panel4.Size.Height / 2, (int)centroide.Y + this.panel4.Size.Height / 2);

            trayectoriaProcesada = reconocedor.encuadrar(trayectoriaProcesada, panel5.Size.Height);
            for (int i = 0; i < trayectoriaProcesada.Count; i++)
                dibujarPunto(this.panel5, (int)trayectoriaProcesada[i].X + this.panel5.Size.Height / 2, (int)trayectoriaProcesada[i].Y + this.panel5.Size.Height / 2);
            centroide = trayectoriaProcesada.getMedia();
            dubujarCentroide(this.panel5, (int)centroide.X + this.panel5.Size.Height / 2, (int)centroide.Y + this.panel5.Size.Height / 2);

            string gesto;
            float probabilidad;
            reconocedor.reconocer(trayectoriaProcesada, gestos, out gesto, out probabilidad, this.panel1.Size.Height);
            label6.Text = "Gesto detectado: " + gesto + " Probabilidad: " + probabilidad;


            //string gindex = reconocedor.reconocerUsandoSM(trayectoriaProcesada, gestos);
            //label6.Text = "Gesto detectado: " + gindex;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickApretado)
            {
                dibujarPunto(this.panel1, e.X, e.Y);
                this.trayectoriaOriginal.Add(new Vector2(e.X, e.Y));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.trayectoriaOriginal.Count > MINPUNTOS) && this.textBox1.Text != "")
            {
                if (!this.gestos.ContainsKey(this.textBox1.Text))
                {
                    this.gestos.Add(this.textBox1.Text, this.trayectoriaProcesada);
                    this.clrearAll();
                }
            }
        }

        private void clrearAll()
        {
            this.trayectoriaOriginal.Clear();
            this.textBox1.Clear();
            resetPanel(this.panel1);
            resetPanel(this.panel2);
            resetPanel(this.panel3);
            resetPanel(this.panel4);
            resetPanel(this.panel5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.clrearAll();
        }

    }
}
