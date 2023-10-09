using Microsoft.AspNetCore.Mvc;
using Project2_InteligentaArtificiala_.Helpers;
using Project2_InteligentaArtificiala_.Models;
using System;
using System.Collections.Generic;
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
                    }
                    else { operationResult = CalculatingGIN.SUM(LayerModelView.Layer[layerIndex]); }
                    break;
                case "PROD":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.PROD(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                    }
                    else { operationResult = CalculatingGIN.PROD(LayerModelView.Layer[layerIndex]); }
                    break;
                case "MAX":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.MAX(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                    }
                    else { operationResult = CalculatingGIN.MAX(LayerModelView.Layer[layerIndex]); }
                    break;
                case "MIN":
                    if (neuronIndex == 0 && layerIndex == 0)
                    {
                        operationResult = CalculatingGIN.MIN(LayerModelView.Layer[NeuronModelView.currentLayerID]);
                    }
                    else { operationResult = CalculatingGIN.MIN(LayerModelView.Layer[layerIndex]); }
                    
                    break;
                default:
                    
                    break;
            }

            ViewBag.GINResult = operationResult;
            NeuronModelView.GIN = operationResult;
            NeuronModelView.currentNeuronID = neuronIndex;
            NeuronModelView.currentLayerID = layerIndex;
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
                    break;
                case "Sigmoid":
                    activationResult = CalculatingActivation.Sigmoid(NeuronModelView.GIN, g, theta);
                    break;
                case "Sign":
                    activationResult = CalculatingActivation.Sign(NeuronModelView.GIN);
                    break;
                case "Tanh":
                    activationResult = double.Parse(CalculatingActivation.Tanh(NeuronModelView.GIN, g, theta));
                    break;
                case "LinearRamp":
                    activationResult = CalculatingActivation.LinearRamp(NeuronModelView.GIN, g);
                    break;
                default:
                    break;
            }
            NeuronModelView.Activation = activationResult;
            NeuronModelView.function = function;
            ViewBag.Activation = activationResult;
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
            if(layerIndex != LayerModelView.Layer.Count-1) 
            {
                var nextLayerNeurons = LayerModelView.Layer[layerIndex + 1];
                foreach(var neuron in nextLayerNeurons)
                {
                    neuron.x = result;
                }
            }
            Update();
            ViewBag.OutputResult = result;
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

                    // Initialize values to the current neuron values before parsing
                    double updatedXValue = neuron.x;
                    double updatedWValue = neuron.w;

                    if (form.TryGetValue(xKey, out var xValues) && form.TryGetValue(wKey, out var wValues))
                    {
                        // Convert StringValues to string and replace commas with dots
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


                    // Update neuron values without rounding
                    neuron.x = updatedXValue;
                    neuron.w = updatedWValue;

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
            return View("ConfigureNetwork");
        }
        [HttpPost]
        public IActionResult ShowFloatOutputResult()
        {
            NeuronModelView.OutputResult = NeuronModelView.Activation;
            int layerIndex = NeuronModelView.currentLayerID;
            if (layerIndex != LayerModelView.Layer.Count - 1)
            {
                var nextLayerNeurons = LayerModelView.Layer[layerIndex + 1];
                foreach (var neuron in nextLayerNeurons)
                {
                    neuron.x =  NeuronModelView.OutputResult;
                }
            }
            Update();
            ViewBag.OutputResult = NeuronModelView.OutputResult;
            return View("ConfigureNetwork");
        }

    }
}
