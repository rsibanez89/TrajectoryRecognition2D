using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognitionWith_1.Source
{
    public class Contorno
    {
        private float xMin = 10000000;
        private float xMax = -10000000;
        private float yMin = 10000000;
        private float yMax = -10000000;

        public float XMIN
        {
            get { return xMin; }
            set { xMin = value; }
        }

        public float XMAX
        {
            get { return xMax; }
            set { xMax = value; }
        }

        public float YMIN
        {
            get { return yMin; }
            set { yMin = value; }
        }

        public float YMAX
        {
            get { return yMax; }
            set { yMax = value; }
        }

        public float Ancho
        {
            get { return xMax - xMin; }
        }

        public float Alto
        {
            get { return yMax - yMin; }
        }

        public float Largo
        {
            get
            {
                if (this.Ancho > this.Alto)
                    return Ancho;
                return Alto;
            }
        }

        public Contorno()
        {
        }

        public Contorno(float xMin, float xMax, float yMin, float yMax)
        {
            this.set(xMin, xMax, yMin, yMax);
        }

        public Contorno(Contorno v)
        {
            this.set(v.xMin, v.xMax, v.yMin, v.yMax);
        }

        public void set(float xMin, float xMax, float yMin, float yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }
    }
}
