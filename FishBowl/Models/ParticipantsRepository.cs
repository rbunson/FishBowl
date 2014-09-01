using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace FishBowl.Models
{
    public class ParticipantsRepository
    {
        private readonly string _imagePath;
        private readonly string _signedInPath;
        private readonly string _registeredPath;
        private readonly string _avatarPath;

        public ParticipantsRepository(string imagePath)
        {
            _imagePath = imagePath;
            _signedInPath = _imagePath + @"\signedin";
            _registeredPath = _imagePath + @"\registered";
            _avatarPath = _imagePath + @"\avatars";
        }
        public ParticipantsRepository() : this ((HttpContext.Current == null)
                             ? System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + @"content\images"
                             : HttpContext.Current.Request.PhysicalApplicationPath + @"content\images")
        {
        }

        public List<Avatar> GetAvatars()
        {
            return Directory.GetFiles(_avatarPath).Select(file =>
                new Avatar
                {
                    Name = file.ExtractName(),
                    ImageName = file.ExtractFileName()
                }).ToList();
        }


        public List<Participant> GetRegisteredParticipants()
        {
            return Directory.GetFiles(_registeredPath).Select(file =>
                new Participant
                {
                    Name = file.ExtractName(),
                    ImageName = file.ExtractFileName()
                }).ToList();
        }

        public void Register(string name, string avatarName)
        {
            var avatar = GetAvatars().FirstOrDefault(m => m.Name == avatarName);

            if (avatar == null)
            {
                return;
            }

            var avatarFullPath = Path.Combine(_avatarPath, avatar.ImageName);
            var registedFullPath = Path.Combine(_registeredPath, name + avatar.ImageExtension);

            File.Copy(avatarFullPath, registedFullPath, true);

            File.SetAttributes(registedFullPath, FileAttributes.Normal);
        }

        public void UnRegister(string name)
        {
            var participant = GetRegisteredParticipants().FirstOrDefault(m => m.Name == name);

            if (participant == null)
            {
                return;
            }

            var registedFullPath = Path.Combine(_registeredPath, participant.ImageName);

            File.Delete(registedFullPath);

        }


    }

    public static class StringExtensions
    {

        public static string ExtractFileName(this string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public static string ExtractName(this string fullPath)
        {
            return RemoveExtension(ExtractFileName(fullPath));
        }

        private static string RemoveExtension(string file)
        {
            return file.Substring(0, file.Length - 4);

        }

    
    }

}