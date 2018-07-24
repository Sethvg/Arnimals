using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMLLibrary
{
    public class AuthenticationManager
    {
        public static string GetPredictionKey()
        {
            // TODO: Don't save the key in the file
            return "f0a2b7038c3b497a9804878a87ea4460";
        }

        public static string GetTrainingKey()
        {
            // TODO: Don't save the key in the file
            return "4a1c89faa74d479cb97ee40e93bc140d";
        }
    }
}
