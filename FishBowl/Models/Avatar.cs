namespace FishBowl.Models
{
    public class Avatar
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ImageExtension { get
        {
            return ImageName.Substring(ImageName.LastIndexOf(".", System.StringComparison.Ordinal));
        } }
        public string ImageUrl { get { return "/content/images/avatars/" + ImageName; } }
    }
}