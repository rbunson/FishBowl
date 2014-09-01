var app = angular.module('app', []);
app.value('$', $);

app.factory('fishBowlSvc', function ($, $rootScope) {
    return {
        proxy: null,
        initialize: function (loadParticipants, loadAvatars, showAlert, loadChat) {
            var connection = $.hubConnection();
            this.proxy = connection.createHubProxy('fishBowlHub');
            connection.url = "/signalr";
            connection.start();

            this.proxy.on('loadParticipants', function (participants) {
                $rootScope.$apply(function () {
                    loadParticipants(participants);
                });
            });
            this.proxy.on('loadAvatars', function (avatars) {
                $rootScope.$apply(function () {
                    loadAvatars(avatars);
                });
            });
            this.proxy.on('showAlert', function (message) {
                $rootScope.$apply(function () {
                    showAlert(message);
                });
            });
            this.proxy.on('loadChat', function (messages) {
                $rootScope.$apply(function () {
                    loadChat(messages);
                });
            });
        },
        enter: function (name) {
            this.proxy.invoke('enter', name);
        },
        leave: function (name) {
            this.proxy.invoke('leave', name);
        },
        signOut: function (name) {
            this.proxy.invoke('signOut', name);
        },
        signIn: function (participants) {
            this.proxy.invoke('signIn', participants);
        },
        register: function (name, avatarName, autoSignIn) {
            this.proxy.invoke('register', name, avatarName, autoSignIn);
        },
        sendChat: function (message) {
            this.proxy.invoke('sendChat', message);
        },
        startFishBowl: function (maxSpeakers) {
            this.proxy.invoke('startFishBowl', maxSpeakers);
        }
    };
});