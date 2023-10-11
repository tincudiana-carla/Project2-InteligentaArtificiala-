using Microsoft.AspNetCore.Mvc;
using Project2_InteligentaArtificiala_.Helpers;
using Project2_InteligentaArtificiala_.Models;
using System.Globalization;

namespace Project2_InteligentaArtificiala_.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConfigureNetwork(int numLayers, List<int> neuronsPerLayer)
        {
            LayerModelView.Layer = new List<List<NeuronModelView>>();

            for (int i = 0; i < numLayers; i++)
            {
                int numNeurons = neuronsPerLayer[i];
                LayerModelView.Layer.Add(new List<NeuronModelView>());

                for (int j = 0; j < numNeurons; j++)
                {
                    LayerModelView.Layer[i].Add(new NeuronModelView { currentNeuronId = j });
                }
            }

            return View("ConfigureNetwork");
        }

        [HttpPost]
        public IActionResult CalculateGIN(int layerIndex, int neuronIndex, string operation)
        {
            var neuronsFromTheLayer = LayerModelView.Layer[layerIndex][neuronIndex];
            double operationResult = 0.0;
            if(neuronIndex != 0 || layerIndex != 0)
            {
                NeuronModelView.currentNeuronID = neuronIndex;
                NeuronModelView.currentLayerID = layerIndex;
            }
            switch (operation)
            {
                case "SUM":
                    if(neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.SUM(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                        NeuronModelView.operationFunction = "SUM";
                        ViewBag.GINResult = operationResult;
                    }
                    else { operationResult = CalculatingGIN.SUM(LayerModelView.Layer[layerIndex]);
                        NeuronModelView.operationFunction = "SUM";
                        ViewBag.GINResult = operationResult;
                    }
                    break;
                case "PROD":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        string stringOperationResult = CalculatingGIN.PROD(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                        operationResult = double.Parse(stringOperationResult);
                        NeuronModelView.operationFunction = "PROD";
                        ViewBag.GINResult = stringOperationResult;
                    }
                    else {
                        string stringOperationResult = CalculatingGIN.PROD(LayerModelView.Layer[layerIndex]);
                        operationResult = double.Parse(stringOperationResult);
                        NeuronModelView.operationFunction = "PROD";
                        ViewBag.GINResult = operationResult;
                    }
                    string result = operationResult.ToString("F20");

                    break;
                case "MAX":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.MAX(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                        NeuronModelView.operationFunction = "MAX";
                        ViewBag.GINResult = operationResult;
                    }
                    else { operationResult = CalculatingGIN.MAX(LayerModelView.Layer[layerIndex]);
                        NeuronModelView.operationFunction = "MAX";
                        ViewBag.GINResult = operationResult;
                    }
                    break;
                case "MIN":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.MIN(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                        NeuronModelView.operationFunction = "MIN";
                        ViewBag.GINResult = operationResult;
                    }
                    else { operationResult = CalculatingGIN.MIN(LayerModelView.Layer[layerIndex]);
                        NeuronModelView.operationFunction = "MIN";
                        ViewBag.GINResult = operationResult;
                    }
                    
                    break;
                default:
                    
                    break;
            }

            
            NeuronModelView.GIN = operationResult;
            NeuronModelView.currentNeuronID = neuronIndex;
            NeuronModelView.currentLayerID = layerIndex;
            Update();
            return View("ConfigureNetwork");

        }
        [HttpPost]
        public IActionResult CalculateActivation(int layerIndex, int neuronIndex, string function, double g, double theta)
        {
            var neuron = LayerModelView.Layer[layerIndex][neuronIndex];
            layerIndex = NeuronModelView.currentLayerID;
            neuronIndex = NeuronModelView.currentNeuronID;
            double activationResult = 0.0;

            switch (function)
            {
                case "Step":
                    activationResult = CalculatingActivation.Step(NeuronModelView.GIN);
                    NeuronModelView.function = function;
                    break;
                case "Sigmoid":
                    activationResult = CalculatingActivation.Sigmoid(NeuronModelView.GIN, g, theta);
                    NeuronModelView.function = function;
                    break;
                case "Sign":
                    activationResult = CalculatingActivation.Sign(NeuronModelView.GIN);
                    NeuronModelView.function = function;
                    break;
                case "Tanh":
                    activationResult = double.Parse(CalculatingActivation.Tanh(NeuronModelView.GIN, g, theta));
                    NeuronModelView.function = function;
                    break;
                case "LinearRamp":
                    activationResult = CalculatingActivation.LinearRamp(NeuronModelView.GIN, g);
                     NeuronModelView.function = function;
                    break;
                default:
                    break;
            }
            NeuronModelView.Activation = activationResult;
            NeuronModelView.function = function;
            ViewBag.Activation = activationResult;
            Update();
            return View("ConfigureNetwork");
        }
        [HttpPost]
        public IActionResult CalculateOutputResult()
        {
            double activation = NeuronModelView.Activation;
            double result;
            string activationFunction = NeuronModelView.function;
            int layerIndex = NeuronModelView.currentLayerID;
            int neuronIndex = NeuronModelView.currentNeuronID;
            switch (activationFunction)
            {
                case "Step":
                    result = CalculatingOutputResult.Step(activation);
                    break;
                case "Sigmoid":
                    result = CalculatingOutputResult.Sigmoid(activation);
                    break;
                case "Sign":
                    result = CalculatingOutputResult.Sign(activation);
                    break;
                case "Tanh":
                    result = CalculatingOutputResult.Tanh(activation);
                    break;
                case "LinearRamp":
                    result = CalculatingOutputResult.LinearRamp(activation);
                    break;
                default:
                    result = 0;
                    break;
            }

            NeuronModelView.OutputResult = result;
            ViewBag.OutputResult = result;
            Update();
            return View("ConfigureNetwork");
        }


        public IActionResult Update()
        {
            var form = HttpContext.Request.Form;

            for (int layerIndex = 0; layerIndex < LayerModelView.Layer.Count; layerIndex++)
            {
                var neuronList = LayerModelView.Layer[layerIndex];
                int neuronIndex = 0;

                foreach (var neuron in neuronList)
                {
                    string xKey = $"neurons[{layerIndex}][{neuronIndex}].x";
                    string wKey = $"neurons[{layerIndex}][{neuronIndex}].w";

                    double updatedXValue = neuron.x;
                    double updatedWValue = neuron.w;

                    if (form.TryGetValue(xKey, out var xValues) && form.TryGetValue(wKey, out var wValues))
                    {
                        string xString = xValues.ToString().Replace(',', '.');
                        string wString = wValues.ToString().Replace(',', '.');

                        if (double.TryParse(xString, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedXValue))
                        {
                            updatedXValue = parsedXValue;
                        }

                        if (double.TryParse(wString, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedWValue))
                        {
                            updatedWValue = parsedWValue;
                        }
                    }

                    neuron.x = Math.Round(updatedXValue, 13);
                    neuron.w = Math.Round(updatedWValue, 6); 
                    neuronIndex++;
                }
            }
            return View("ConfigureNetwork");
        }



        [HttpPost]
        public IActionResult UpdateAGTheta(double a, double g, double theta)
        {
            NeuronModelView.a = a;
            NeuronModelView.g = g;
            NeuronModelView.theta = theta;
              Update();
            return View("ConfigureNetwork");
        }
        [HttpPost]
        public IActionResult ShowFloatOutputResult()
        {
            NeuronModelView.OutputResult = NeuronModelView.Activation;
            int layerIndex = NeuronModelView.currentLayerID;
            Update();
            ViewBag.OutputResult = NeuronModelView.OutputResult;
            return View("ConfigureNetwork");
        }
        [HttpPost]
        public IActionResult SetNeuronX(int layerIndex, int neuronIndex)
        {
            LayerModelView.Layer[layerIndex][neuronIndex].x = NeuronModelView.OutputResult;
            Update(); 
            return View("ConfigureNetwork"); 
        }

        public IActionResult GetTheFunctions(int layerIndex)
        {
            ViewBag.Functions = NeuronModelView.function;
            ViewBag.operationFunctions = NeuronModelView.operationFunction;
            return View("ConfigureNetwork");
        }


    }
}
