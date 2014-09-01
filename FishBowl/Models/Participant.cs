namespace FishBowl.Models
{
    public class Participant
    {
        public bool Waiting { get; set; }
        public bool Speaking { get; set; }
        public bool SignedIn { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ImageExtension { get
        {
            return ImageName.Substring(ImageName.LastIndexOf(".", System.StringComparison.Ordinal));
        } }
        public string ImageUrl { get { return "/content/images/registered/" + ImageName; } }
    }
}