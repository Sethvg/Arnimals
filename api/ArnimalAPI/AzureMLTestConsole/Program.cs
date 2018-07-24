using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureMLLibrary.Prediction;

namespace AzureMLTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://www.cowtownceramics.com/images/Corgi%203%20in.jpg";
            string test = CSPrediction.MakePredictionRequestByImageUrl(url).Result;
            Console.WriteLine(test);
        }
    }
}
