namespace ArnimalService.Models
{
    public class Animal
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public string Type { get; set; }

        public long HitCount { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public float Weight { get; set; }
        public float Height { get; set; }

        public string PathToTrainingImages { get; set; }
    }
}
