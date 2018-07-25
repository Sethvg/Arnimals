using System;

namespace ArnimalService.Models
{
    public class AnimalMetadata
    {
        public long Id { get; set; }

        public Guid MLGuid { get; set; }

        public bool Trained { get; set; }

        public string PathToTrainingImages { get; set; }
    }
}
