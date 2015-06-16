using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognitionWith_1.Source
{
    public class Vector2
    {
        private float x = 0;
        private float y = 0;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2()
        {
            this.set(0, 0);
        }

        public Vector2(float x, float y)
        {
            this.set(x, y);
        }

        public Vector2(Vector2 v)
        {
            this.set(v.x, v.y);
        }

        public Vector2(string xy)
        {
            xy = xy.Replace('(', ' ').Replace(')', ' ');
            string[] trim = xy.Split(',');

            this.x = System.Convert.ToInt32(trim[0]);
            this.y = System.Convert.ToInt32(trim[1]);
        }

        public void set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static float getDistancia(Vector2 v1, Vector2 v2)
        {
            float determinante = (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y);
            return (float)Math.Sqrt(determinante);
        }

        public float productoEscalar(Vector2 v)
        {
            return x * v.x + y * v.y;
        }

        public float desviacionCuadrada(Vector2 v)
        {
            Vector2 desviacion = this - v;
            return desviacion.productoEscalar(desviacion);
        }

        public Vector2 clon()
        {
            return new Vector2(this.x, this.y);
        }

        public float getAnguloEuler()
        {
            float angulo = radianToDegree(Math.Atan(y/x));
            if((x < 0) && (y > 0))
                return angulo + 180;
            if ((x < 0) && (y < 0))
                return angulo + 180;
            if ((x > 0) && (y < 0))
                return angulo + 360;

            return angulo;
        }

        private float radianToDegree(double angle)
        {
            return (float)(angle * (180f / Math.PI));
        }

        #region Sobrecarga de operadores
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator /(Vector2 v1, float f)
        {
            return new Vector2(v1.x / f, v1.y / f);
        }
        #endregion
    }
}
