using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GestureRecognitionWith_1.Source
{
    public class Reconocedor
    {
        private static readonly int N = 10;
        private static readonly float SIZE = 250;
        private static readonly float PHI = (float)(0.5f * (-1f + Math.Sqrt(5)));
        public static readonly float DIAGONAL = (float)Math.Sqrt(SIZE * SIZE + SIZE * SIZE);


        private float distanciaDelCamino(Trayectoria trayectoria)
        {
            float distancia = 0;
            for (int i = 1; i < trayectoria.Count; i++)
                distancia += Vector2.getDistancia(trayectoria[i - 1], trayectoria[i]);
            return distancia;
        }

        public Trayectoria remuestrear(Trayectoria trayectoria)
        {
            float incremento = distanciaDelCamino(trayectoria) / (N-1);
            float D = 0;
            Trayectoria trayectoriaRemuestreada = new Trayectoria();
            trayectoriaRemuestreada.Add(trayectoria[0]);
            for (int i = 1; i < trayectoria.Count; i++)
            {
                float distancia = Vector2.getDistancia(trayectoria[i - 1], trayectoria[i]);
                if ((D + distancia) >= incremento)
                {
                    Vector2 nuevo = new Vector2();
                    nuevo.X = trayectoria[i - 1].X + ((incremento - D) / distancia) * (trayectoria[i].X - trayectoria[i - 1].X);
                    nuevo.Y = trayectoria[i - 1].Y + ((incremento - D) / distancia) * (trayectoria[i].Y - trayectoria[i - 1].Y);
                    trayectoriaRemuestreada.Add(nuevo);
                    trayectoria.Insert(i, nuevo);
                    D = 0;
                }
                else
                    D += distancia;
            }
            if (trayectoriaRemuestreada.Count < N)
            {
                trayectoriaRemuestreada.Add(trayectoria[trayectoria.Count - 1]);
                Console.WriteLine("Son menos que los que necesito");
            }
            return trayectoriaRemuestreada;
        }

        public Trayectoria rotarHastaAngulo0(Trayectoria trayectoria)
        {
            Vector2 centroide = trayectoria.getMedia();
            float angulo = (float)Math.Atan2(centroide.Y - trayectoria[0].Y, centroide.X - trayectoria[0].X);
            return trayectoria.rotar(-angulo, centroide);
        }

        public Trayectoria encuadrar(Trayectoria trayectoria, float size)
        {
            Contorno contorno = trayectoria.getContorno();
            return trayectoria.encuadrar(contorno, size);
        }

        private float distanciaEntreCaminos(Trayectoria t1, Trayectoria t2)
        {
            float distancia = 0;
            for (int i = 0; i < t1.Count && i < t2.Count; i++)
                distancia += Vector2.getDistancia(t1[i], t2[i]);
            return distancia / t1.Count;
        }

        private float distanciaEnAngulo(Trayectoria t1, Trayectoria t2, float angulo)
        {
            Vector2 centroide = t1.getMedia();
            Trayectoria nueva = t1.rotar(angulo, centroide);
            return distanciaEntreCaminos(nueva, t2);
        }

        public float distanciaEnMejorAngulo(Trayectoria t1, Trayectoria t2, float anguloA, float anguloB, float anguloD)
        {
            float x1 = PHI * anguloA + (1 - PHI) * anguloB;
            float f1 = distanciaEnAngulo(t1, t2, x1);

            float x2 = (1 - PHI) * anguloA + PHI * anguloB;
            float f2 = distanciaEnAngulo(t1, t2, x2);

            while (Math.Abs(anguloA - anguloB) > anguloD)
            {
                if (f1 < f2)
                {
                    anguloB = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = PHI * anguloA + (1 - PHI) * anguloB;
                    f1 = distanciaEnAngulo(t1, t2, x1);
                }
                else
                {
                    anguloA = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = (1 - PHI) * anguloA + PHI * anguloB;
                    f2 = distanciaEnAngulo(t1, t2, x2);
                }
            }
            return Math.Min(f1, f2);
        }

        public void reconocer(Trayectoria trayectoria, Hashtable gestos, out string gesto, out float probabilidad, float size)
        {
            float b = 100000000;
            gesto = "ninguno";
            foreach (DictionaryEntry template in gestos)
            {
                float d = distanciaEnMejorAngulo(trayectoria, (Trayectoria)template.Value, DegreeToRadian(-45), DegreeToRadian(45), DegreeToRadian(2));
                if (d < b)
                {
                    b = d;
                    gesto = (string)template.Key;
                }
            }
            probabilidad = (float)(1 - b / (0.5 * DIAGONAL));
        }

        private float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        public string reconocerUsandoSM(Trayectoria trayectoria, Hashtable gestos)
        {
            Hashtable caracteres = new Hashtable();
            caracteres.Add(0f, "A");
            //caracteres.Add("B", 45f);
            caracteres.Add(90f, "C");
            //caracteres.Add("D", 135f);
            caracteres.Add(180f, "E");
            //caracteres.Add("F", 225f);
            caracteres.Add(270f, "G");
            //caracteres.Add("H", 315f);
            caracteres.Add(360f, "A");
            List<string> secuencia = codificar(trayectoria, caracteres);
            List<string> secuenciaraleada = ralear(secuencia);
            string ts = convertToString(secuenciaraleada);

            Hashtable GestosString = new Hashtable();
            foreach (DictionaryEntry template in gestos)
            {
                secuencia.Clear();
                secuenciaraleada.Clear();
                secuencia = codificar((Trayectoria)template.Value, caracteres);
                secuenciaraleada = ralear(secuencia);
                string s = convertToString(secuenciaraleada);
                GestosString.Add(s,(string)template.Key);
                Console.WriteLine(s);
            }

            string ret = "no hay";
            if (GestosString.ContainsKey(ts))
            {
                return GestosString[ts].ToString();
            }
            return ret;
        }

        private List<string> codificar(Trayectoria trayectoria, Hashtable caracteres)
        {
            List<string> secuencia = new List<string>();
            for (int i = 1; i < trayectoria.Count; i++)
            {
                secuencia.Add(codificar(trayectoria[i] - trayectoria[i - 1], caracteres));
            }
            return secuencia;
        }

        private string codificar(Vector2 v, Hashtable caracteres)
        {
            float angulo = v.getAnguloEuler();
            string clave = "";
            float minDistancia = 10000;
            foreach (DictionaryEntry a in caracteres)
            {
                float distancia = Math.Abs(angulo - (float)a.Key);
                if (minDistancia > distancia)
                {
                    minDistancia = distancia;
                    clave = (string)a.Value;
                }
            }
            return clave;
        }

        private static string convertToString(List<string> cadena)
        {
            string result = "";
            for (int i = 0; i < cadena.Count; i++)
                result += cadena[i].ToString();
            return result;
        }

        //tener en cuenta que si no hay 3 direcciones seguidas entonces no se agrega
        private static List<string> preRalear(List<string> secuencia)
        {
            List<string> result = new List<string>();
            string caracter = secuencia[0];
            int repeticiones = 0;
            for (int i = 1; i < secuencia.Count; i++)
            {
                if (secuencia[i] == caracter)
                {
                    repeticiones++;
                    if (repeticiones == 3)
                    {
                        repeticiones = 0;
                        result.Add(caracter);
                    }
                }
                else
                {
                    repeticiones = 0;
                    caracter = secuencia[i];
                }
            }
            return result;
        }

        private static List<string> ralear(List<string> secuencia)
        {
            secuencia = preRalear(secuencia);
            List<string> result = new List<string>();
            result.Add(secuencia[0]);
            for (int i = 0; i < secuencia.Count - 1; i++)
                if (secuencia[i] != secuencia[i + 1])
                    result.Add(secuencia[i + 1]);
            return result;
        }
    }
}
