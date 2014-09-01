using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace FishBowl.Models
{
    public class FishBowlHub : Hub
    {
        private readonly FishBowlModel _fishBowlModel;

        public FishBowlHub() : this (FishBowlModel.Instance)
        {

        }

        public FishBowlHub(FishBowlModel fishBowlModel)
        {
            _fishBowlModel = fishBowlModel;
        }

        public void enter(string name)
        {
            _fishBowlModel.Enter(name);
        }

        public void leave(string name)
        {
            _fishBowlModel.Leave(name);
        }

        public void signOut(string name)
        {
            _fishBowlModel.SignOut(name);
        }

        public void signIn(IEnumerable <Participant> participants)
        {
            _fishBowlModel.SignIn(participants);
        }

        public void sendChat(string message)
        {
            _fishBowlModel.SendChat(message);
        }

        public void startFishBowl(int maxSpeakers)
        {
            _fishBowlModel.StartFishBowl(maxSpeakers);
        }

        public void register(string name, string avatarName, bool autoSignIn)
        {
            _fishBowlModel.Register(name, avatarName, autoSignIn);
        }

    }
}