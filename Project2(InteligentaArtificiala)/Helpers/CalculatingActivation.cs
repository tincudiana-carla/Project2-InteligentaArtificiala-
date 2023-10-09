namespace Project2_InteligentaArtificiala_.Helpers
{
    public class CalculatingActivation
    {
        public static double Step(double input) //treapta
        {
            return input >= 0 ? 1.0 : 0.0;
        }
        public static double Sigmoid(double input, double g, double theta) //sigmoid
        {
            return 1.0 / (1 + (Math.Exp(-g * (input - theta))));
        }
        public static double Sign(double input) //semn
        {
            return input >= 0 ? 1.0 : -1.0;
        }
        public static string Tanh(double input, double g, double theta) //tangenta hiperbolica
        {
            double e1 = Math.Exp(g * (input - theta));
            double e2 = Math.Exp(-g * (input - theta));
            return (((e1 - e2) / (e1 + e2))).ToString("F20");
        }
        public static double LinearRamp(double input, double a) //rampa(functia liniara)
        {
            if (input < -a)
            {
                return -1.0;
            }
            else if (input >= -a && input <= a)
            {
                return input / a;
            }
            else
            {
                return 1.0;
            }
        }
    }
}
