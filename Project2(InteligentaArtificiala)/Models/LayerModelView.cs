namespace Project2_InteligentaArtificiala_.Models
{
    public class LayerModelView
    {
        public static List<List<NeuronModelView>> Layer { get; set; }
        public static int NumLayers { get; set; }
        public LayerModelView(int numLayers)
        {
            Layer = new List<List<NeuronModelView>>();
            for (int i = 0; i < numLayers; i++)
            {
                Layer.Add(new List<NeuronModelView>());
            }
        }
    }
}
