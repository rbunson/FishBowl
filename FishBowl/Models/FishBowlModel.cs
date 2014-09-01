using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace FishBowl.Models
{
    public class FishBowlModel
    {
        private readonly ParticipantsRepository _participantsRepository;
        private List<ChatMessage> _chatMessages = new List<ChatMessage>();
        private readonly static FishBowlModel _instance =
            new FishBowlModel(GlobalHost.ConnectionManager.GetHubContext<FishBowlHub>().Clients, new ParticipantsRepository());

        private readonly object _participantLock = new object();
        private readonly List<Participant> _participants = new List<Participant>();
        private int _maxSpeakers = 4;

        private FishBowlModel(IHubConnectionContext clients, ParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
            Clients = clients;
            InitializeParticipants();
        }

        private void InitializeParticipants()
        {
            _participants.AddRange(_participantsRepository.GetRegisteredParticipants());
            _chatMessages = new List<ChatMessage>();
        }

        private Participant GetParticipant(string name)
        {
            return _participants.FirstOrDefault(m => m.Name == name);
        }

        public void Enter(string name)
        {
            var myParticipant = GetParticipant(name);

            if (myParticipant != null)
            {
                if (_participants.Count(m => m.Speaking) < _maxSpeakers)
                {
                    myParticipant.Speaking = true;
                }
                else
                {
                    myParticipant.Waiting = true;
                }
            }
            Broadcast(string.Format("{0} has entered the Fish Bowl", name));
        }

        public void Leave(string name)
        {
            var myParticipant = GetParticipant(name);

            if (myParticipant != null)
            {
                myParticipant.Speaking = false;
            }

            var waitingParticipant = _participants.FirstOrDefault(m => m.Waiting);
            if (waitingParticipant != null)
            {
                waitingParticipant.Waiting = false;
                waitingParticipant.Speaking = true;
            }
            Broadcast(string.Format("{0} has left the Fish Bowl", name));
        }

        public void Initialize()
        {
            Broadcast(string.Empty);
        }

        public void StartFishBowl(int maxSpeakers)
        {
            _maxSpeakers = maxSpeakers;
            Clients.All.loadAvatars(_participantsRepository.GetAvatars());
            Broadcast(string.Empty);
        }

        private void Broadcast(string message)
        {
            Clients.All.loadParticipants(_participants);
            Clients.All.loadChat(_chatMessages);

            if (!string.IsNullOrEmpty(message))
            {
                Clients.All.showAlert(message);
            }
            
        }
        public static FishBowlModel Instance
        {
            get
            {
                return _instance;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public void SignIn(IEnumerable<Participant> participantsToSignIn)
        {
            var signedIn = string.Empty;
            foreach (var participantToSignIn in participantsToSignIn.Where(m => m.SignedIn))
            {
                var myParticipant = _participants.FirstOrDefault(m => m.Name == participantToSignIn.Name && !m.SignedIn);
                if (myParticipant != null)
                {
                    myParticipant.SignedIn = true;
                    myParticipant.Speaking = false;
                    myParticipant.Waiting = false;
                    if (signedIn.Length > 0)
                    {
                        signedIn += ", " ;
                    }
                    signedIn += participantToSignIn.Name;
                }
            }
            if (signedIn.Length > 0)
            {
                Broadcast(string.Format("{0} {1} signed in", signedIn, signedIn.IndexOf(",", System.StringComparison.Ordinal) < 0 ? "has" : "have"));
            }
        }

        public void Register(string name, string avatarName, bool autoSignIn)
        {
            _participantsRepository.Register(name, avatarName);

            var newParticipant = _participantsRepository.GetRegisteredParticipants().FirstOrDefault(m => m.Name == name);
            if (newParticipant != null)
            {
                newParticipant.SignedIn = autoSignIn;
            }
            _participants.Add(newParticipant);
            Broadcast(string.Format("{0} has registered", name));
        }

        public void SendChat(string message)
        {
            _chatMessages.Add(new ChatMessage(message));
            Clients.All.loadChat(_chatMessages);
            Clients.All.showAlert("New chat message");
        }

        public void SignOut(string name)
        {
            var myParticipant = GetParticipant(name);

            if (myParticipant != null)
            {
                myParticipant.SignedIn = false;
            }

            Broadcast(string.Format("{0} has signed out", name));
        }
    }

    public class ChatMessage
    {
        public ChatMessage(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
        public bool Read { get; set; }
    }
}