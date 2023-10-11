namespace Project2_InteligentaArtificiala_.Models
{
    public class NeuronModelView
    {
        public static List<InputModelView> Inputs { get; set; } = new List<InputModelView>();
        public List<double> Weights { get; set; }
        public static double GIN { get; set; }
        public int Id { get; set; }
        public double x { get; set; }
        public double w { get; set; }
        public static double Activation { get; set; }
        public static double g = 1;
        public static double a = 1;
        public static double theta = 0;
        public static string function;
        public static string operationFunction;
        private static int currentId = 0;
        public static int currentLayerID {  get; set; }
        public static int currentNeuronID { get; set; }
        public  int currentNeuronId { get; set; }
        public static double OutputResult { get; set; }
        public int LayerIndex { get; set; }
        public NeuronModelView()
        {
            Inputs = new List<InputModelView>();
        }
        public void AddNeuron()
        {
            Inputs.Add(new InputModelView { Id = ++currentId, x = 0.0, w = 0.0 });
        }

        public void SubtractNeuron()
        {
            if (Inputs.Count > 1)
            {
                Inputs.RemoveAt(Inputs.Count - 1);
            }
        }
    

    }

}
