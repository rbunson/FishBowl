function FishBowlCtrl($scope, fishBowlSvc) {
    $scope.nonSpeakingParticipants1thru12 = [];
    $scope.nonSpeakingParticipants13thru24 = [];
    $scope.participantsToSignIn = [];
    $scope.nonSignedInParticipants = [];
    $scope.signedInParticipants = [];
    $scope.speakingParticipants = [];
    $scope.nonSpeakingParticipants = [];
    $scope.registeredParticipants = [];
    $scope.avatars = [];
    $scope.waitingToSpeak = null;
    $scope.registeredName = '';
    $scope.registeredAvatar = '';
    $scope.chatMessage = '';
    $scope.chatMessages = [];
    $scope.started = false;
    $scope.showValidationMessage = false;
    $scope.registeredNameRequired = '';
    $scope.registeredAvatarRequired = '';
    $scope.registrationNameUnique = '';

    var modes = { fishBowl: "fishBowl", signIn: "signIn", register: "register" };
    var mode = modes.fishBowl;
        
    var maxSpeaking = 4;
    
    $scope.fishBowl = function () {
        $scope.registeredName = '';
        $scope.registeredAvatar = '';
        mode = modes.fishBowl;
    };

    $scope.signIn = function () {
        mode = modes.signIn;
    };

    $scope.register = function () {
        $scope.showValidationMessage = false;
        mode = modes.register;
    };

    $scope.showFishBowl = function () {
        return $scope.started && mode == modes.fishBowl;
    };

    $scope.showSignIn = function () {
        return $scope.started && mode == modes.signIn;
    };

    $scope.showRegister = function () {
        return $scope.started && mode == modes.register;
    };

    $scope.showChair = function () {
        return $scope.waitingToSpeak == null;
    };
    
    $scope.noMoreSeats = function () {
        return $scope.speakingParticipants.length == maxSpeaking && $scope.waitingToSpeak != null;
    };

    var loadParticipants = function (registeredParticipants) {
        var maxPeopleInRow = 12;
        $scope.registeredParticipants = registeredParticipants;
        $scope.waitingToSpeak = _.find(registeredParticipants, function (participant) { return participant.Waiting; });
        $scope.nonSpeakingParticipants = _.filter(registeredParticipants, function (participant) { return participant.SignedIn && !(participant.Speaking || participant.Waiting); });
        
        $scope.nonSpeakingParticipants1thru12 = _.first($scope.nonSpeakingParticipants, Math.max($scope.nonSpeakingParticipants.length-1, maxPeopleInRow));
        $scope.nonSpeakingParticipants13thru24 = $scope.nonSpeakingParticipants.length > maxPeopleInRow ? _.rest($scope.nonSpeakingParticipants, maxPeopleInRow) : [];
        
        $scope.speakingParticipants = _.filter(registeredParticipants, function (participant) { return participant.Speaking; });
        $scope.signedInParticipants = _.filter(registeredParticipants, function (participant) { return participant.SignedIn; });
        $scope.nonSignedInParticipants = _.filter(registeredParticipants, function (participant) { return !participant.SignedIn; });
    };

    var loadAvatars = function(avatars) {
        $scope.avatars = avatars;
    };

    $scope.enter = function (myName) {
        fishBowlSvc.enter(myName);
    };

    $scope.leave = function (myName) {
        fishBowlSvc.leave(myName);
    };

    $scope.signOut = function (myName) {
        fishBowlSvc.signOut(myName);
    };

    $scope.registerParticipant = function () {
        $scope.showValidationMessage = true;
        $scope.validateRegistration();
        if (!$scope.showRegistrationValidationMessage()) {
            fishBowlSvc.register($scope.registeredName, $scope.registeredAvatar, true);
            $scope.fishBowl();
        } 
    };

    $scope.startFishBowl = function () {
        fishBowlSvc.startFishBowl(maxSpeaking);
        
        $scope.started = true;
    };

    $scope.signInToFishBowl = function () {
        fishBowlSvc.signIn($scope.nonSignedInParticipants);
        $scope.fishBowl();
    };

    $scope.sendChat = function () {
        fishBowlSvc.sendChat($scope.chatMessage);
        $scope.chatMessage = '';
    };

    $scope.startChat = function (name) {
        angular.element('#chat').focus();
        $scope.chatMessage = name + ": ";
    };

    var showAlert = function (message) {
        angular.element('.top-right').notify({
            message: { text: message }
        }).show(); 
    };

    var loadChat = function (messages) {
        $scope.chatMessages = messages;
    };

    $scope.registrationValid = function () {
        return $scope.showRegistrationValidationMessage();
    };

    $scope.showRegistrationValidationMessage = function () {
        return $scope.registeredNameRequired.length > 0 || $scope.registeredAvatarRequired.length > 0 || $scope.registrationNameUnique.length > 0;
    };

    $scope.validateRegistration = function () {
        $scope.registeredNameRequired = $scope.registeredName.length > 0 ? '' : 'Name is required';
        $scope.registeredAvatarRequired = $scope.registeredAvatar.length > 0 ? '' : 'Avatar is required';

        var isDuplicateParticipant;
        try {
            var duplicateParticipant = _.find($scope.registeredParticipants, function (participant) { return $scope.registeredName == participant.Name; });
            isDuplicateParticipant = duplicateParticipant != null;
        } catch (e) {
            isDuplicateParticipant = false;
        }
        $scope.registrationNameUnique = !isDuplicateParticipant ? '' : 'Name must be unique.  Select a new name';
    };   
    
    fishBowlSvc.initialize(loadParticipants, loadAvatars, showAlert, loadChat);
}