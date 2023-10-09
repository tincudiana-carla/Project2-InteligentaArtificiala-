using Project2_InteligentaArtificiala_.Models;

namespace Project2_InteligentaArtificiala_.Helpers
{
    public class CalculatingOutputResult
    {
        public static double Step(double activation) //treapta
        {
            activation = NeuronModelView.Activation;
            return activation;
        }
        public static double Sigmoid(double activation) //sigmoid
        {
            if (activation >= 0.5)
                return 1;
            else return 0;
        }
        public static double Sign(double activation) //semn
        {
            activation = NeuronModelView.Activation;
            return activation;
        }
        public static double Tanh(double activation) //tangenta hiperbolica
        {
            activation = NeuronModelView.Activation;
            if (activation >= 0)
                return 1;
            return -1;
        }
        public static double LinearRamp(double activation) //rampa(functia liniara)
        {
            activation = NeuronModelView.Activation;
            if (activation >= 0)
                return 1;
            return -1;
        }
    }
}
