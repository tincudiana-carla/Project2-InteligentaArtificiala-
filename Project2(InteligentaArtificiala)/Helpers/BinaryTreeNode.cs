using Project2_InteligentaArtificiala_.Models;
using System.Collections.Generic;

namespace Project2_InteligentaArtificiala_.Helpers
{
    public class BinaryTreeNode
    {
        public List<NeuronModelView> Neurons { get; set; }

        public BinaryTreeNode(int numNeurons)
        {
            Neurons = new List<NeuronModelView>();

            for (int i = 0; i < numNeurons; i++)
            {
                var neuron = new NeuronModelView
                {
                    currentNeuronId = i,
                };
                Neurons.Add(neuron);
            }
        }
    }
}

