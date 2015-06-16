using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognitionWith_1.Source
{
    public class Trayectoria
    {
        private List<Vector2> trayectoria;

        #region Metodos base de la lista
        public Trayectoria()
        {
            this.trayectoria = new List<Vector2>();
        }

        public Trayectoria(List<Vector2> trayectoria)
        {
            this.trayectoria = trayectoria;
        }

        public int Count
        {
            get { return trayectoria.Count; }
        }

        public void Add(Vector2 value)
        {
            trayectoria.Add(value);
        }

        public void Add(Trayectoria trayectoria)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                this.trayectoria.Add(trayectoria[i]);
        }

        public void Clear()
        {
            trayectoria.Clear();
        }

        public void Insert(int index, Vector2 value)
        {
            trayectoria.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            trayectoria.RemoveAt(index);
        }

        public Vector2 this[int index]
        {
            get
            {
                return trayectoria[index];
            }
            set
            {
                trayectoria[index] = value;
            }
        }

        public Trayectoria GetRange(int index, int count)
        {
            return new Trayectoria(trayectoria.GetRange(index, count));
        }
        #endregion

        #region Metodos de una trayectoria
        public Trayectoria clon()
        {
            List<Vector2> copia = new List<Vector2>();
            for (int i = 0; i < trayectoria.Count; i++)
                copia.Add(trayectoria[i].clon());
            return new Trayectoria(copia);
        }

        public void trasladar(Vector2 posicion)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = trayectoria[i] - posicion;
        }

        // Traslada toda la Trayectoria restandole la media
        public Vector2 centrar()
        {
            Vector2 media = getMedia();
            trasladar(media);
            return media;
        }

        // Devuelve la media de una Trayectoria
        public Vector2 getMedia()
        {
            Vector2 media = new Vector2();
            for (int i = 0; i < trayectoria.Count; i++)
                media = media + trayectoria[i];
            media = media / trayectoria.Count;
            return media;
        }

        // Escala toda la Trayectoria, restandole la media y dividiendolo por la escala
        public float normalizar()
        {
            Vector2 media = getMedia();
            float s = getEscala();
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = (trayectoria[i] - media) / s;
            return s;
        }

        // Calcula la escala como la raiz cuadrada, de la suma de las desviaciones cuadradas sobre la cantidad de elementos
        public float getEscala()
        {
            Vector2 media = getMedia();
            float sumaDeCuadrados = 0;
            for (int i = 0; i < trayectoria.Count; i++)
                sumaDeCuadrados += trayectoria[i].desviacionCuadrada(media);
            return (float)Math.Sqrt(sumaDeCuadrados / trayectoria.Count);
        }

        public Contorno getContorno()
        {
            Contorno contorno = new Contorno();
            for (int i = 0; i < this.Count; i++)
            {
                if (trayectoria[i].X < contorno.XMIN)
                    contorno.XMIN = trayectoria[i].X;
                
                if (trayectoria[i].X > contorno.XMAX)
                    contorno.XMAX = trayectoria[i].X;
                
                if (trayectoria[i].Y < contorno.YMIN)
                    contorno.YMIN = trayectoria[i].Y;

                if (trayectoria[i].Y > contorno.YMAX)
                    contorno.YMAX = trayectoria[i].Y;
            }
            return contorno;
        }

        public Trayectoria encuadrar(Contorno contorno, float size)
        {
            Trayectoria encuadrada = new Trayectoria();
            for (int i = 0; i < this.Count; i++)
            {
                Vector2 nuevo = new Vector2();
                nuevo.X = this[i].X * (size / contorno.Largo);
                nuevo.Y = this[i].Y * (size / contorno.Largo);
                encuadrada.Add(nuevo);
            }
            return encuadrada;
        }

        public double[][] getArray()
        {
            double[][] ret = new double[trayectoria.Count][];
            for (int i = 0; i < trayectoria.Count; i++)
                ret[i] = new double[] { trayectoria[i].X, trayectoria[i].Y };
            return ret;
        }

        public Trayectoria rotar(float angulo, Vector2 centroide)
        {
            Trayectoria rotada = new Trayectoria();
            for (int i = 0; i < this.Count; i++)
            {
                Vector2 nuevo = new Vector2();
                nuevo.X = (float)((this[i].X - centroide.X) * Math.Cos(angulo) - (this[i].Y - centroide.Y) * Math.Sin(angulo) + centroide.X);
                nuevo.Y = (float)((this[i].X - centroide.X) * Math.Sin(angulo) + (this[i].Y - centroide.Y) * Math.Cos(angulo) + centroide.Y);
                rotada.Add(nuevo);
            }
            return rotada;
        }
        #endregion
    }
}
