using Microsoft.AspNetCore.Mvc;
using Project2_InteligentaArtificiala_.Helpers;
using Project2_InteligentaArtificiala_.Models;
using System;

namespace Project2_InteligentaArtificiala_.Controllers
{
    public class NeuronController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult CalculateGIN(int layerIndex, int neuronIndex, string operation)
        //{
        //    var neuron = LayerModelView.Layer[layerIndex][neuronIndex];
        //    double operationResult = 0.0;

        //    switch (operation)
        //    {
        //        case "SUM":
        //            operationResult = CalculatingGIN.SUM(LayerModelView.Layer[layerIndex]);
        //            break;
        //        case "PROD":
        //            operationResult = CalculatingGIN.PROD(LayerModelView.Layer[layerIndex]);
        //            break;
        //        case "MAX":
        //            operationResult = CalculatingGIN.MAX(LayerModelView.Layer[layerIndex]);
        //            break;
        //        case "MIN":
        //            operationResult = CalculatingGIN.MIN(LayerModelView.Layer[layerIndex]);
        //            break;
        //        default:
        //            // Handle the default case if necessary
        //            break;
        //    }

        //    ViewBag.GINResult = operationResult;
        //    return View("ConfigureNetwork");
        //}




        [HttpPost]
        public IActionResult CalculateActivation(string function, double a, double g, double theta)
        {
            double gin = NeuronModelView.GIN;
            double result;
            switch (function)
            {
                case "Step":
                    result = CalculatingActivation.Step(gin);
                    NeuronModelView.function = function;
                    break;

                case "Sigmoid":
                    result = CalculatingActivation.Sigmoid(gin, g, theta);
                    NeuronModelView.function = function;
                    break;

                case "Sign":
                    result = CalculatingActivation.Sign(gin);
                    NeuronModelView.function = function;
                    break;

                case "Tanh":
                    string decimalResult = CalculatingActivation.Tanh(gin, g, theta);
                    NeuronModelView.Activation = double.Parse(decimalResult);
                    result = NeuronModelView.Activation;
                    NeuronModelView.function = function;
                    break;

                case "LinearRamp":
                    result = CalculatingActivation.LinearRamp(gin, a);
                    NeuronModelView.function = function;
                    break;

                default:
                    result = 0;
                    break;
            }

            ViewBag.Activation = result;
            ViewBag.Function = NeuronModelView.function;
            NeuronModelView.Activation = result;
            NeuronModelView.a = a;
            NeuronModelView.g = g;
            NeuronModelView.theta = theta;
            NeuronModelView.GIN = gin;
            return View("Index");
        }
        [HttpPost]
        public IActionResult CalculateOutputResult()
        {
            double activation = NeuronModelView.Activation;
            double result;
            string activationFunction = NeuronModelView.function;
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
            return View("Index");
        }

        [HttpPost]
        public IActionResult UpdateAGTheta(double a, double g, double theta)
        {
            NeuronModelView.a = a;
            NeuronModelView.g = g;
            NeuronModelView.theta = theta;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ShowFloatOutputResult()
        {
            NeuronModelView.OutputResult = NeuronModelView.Activation;
            ViewBag.OutputResult = NeuronModelView.OutputResult;
            return View("Index");
        }

    }
}
