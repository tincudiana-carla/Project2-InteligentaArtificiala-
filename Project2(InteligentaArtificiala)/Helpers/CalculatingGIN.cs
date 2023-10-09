using Project2_InteligentaArtificiala_.Models;

namespace Project2_InteligentaArtificiala_.Helpers
{
    public class CalculatingGIN
    {
        public static double SUM(List<NeuronModelView> neurons)
        {
            double sum = 0.0;
            foreach (var neuron in neurons)
            {
                
                int id = neuron.Id;
               
                
                sum += neuron.x * neuron.w;
            }
            return sum;
        }

        public static double PROD(List<NeuronModelView> neurons)
        {
            double prod = 1.0;
            foreach (var neuron in neurons)
            {
                prod *= neuron.x * neuron.w;
            }
            return prod;
        }

        public static double MAX(List<NeuronModelView> neurons)
        {
            double maxValue = double.MinValue;
            foreach (var neuron in neurons)
            {
                double value = neuron.x * neuron.w;
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }

        public static double MIN(List<NeuronModelView> neurons)
        {
            double minValue = double.MaxValue;
            foreach (var neuron in neurons)
            {
                double value = neuron.x * neuron.w;
                if (value < minValue)
                {
                    minValue = value;
                }
            }
            return minValue;
        }
    }
}
