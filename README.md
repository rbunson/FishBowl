Fish Bowl
===============

From Wikipedia, http://en.wikipedia.org/wiki/Fishbowl_(conversation), "A fishbowl conversation is a form of dialog that can be used when discussing topics within large groups. Fishbowl conversations are usually used in participatory events like Open Space Technology and Unconferences. The advantage of Fishbowl is that it allows the entire group to participate in a conversation. Several people can join the discussion"

This application supports virtual fishbowls where there is that large group of participants, but they are communicating over an audio line.  The fishbowl is set up with 5 virtual chairs.  Participants sit in the 4 main chairs and they are the only people allowed to speak (along with the facillitator).  A person from the pool of non-speakers declares her intention to speak by occupying the empty fifth chair.  When that occurs, one of the original speakers would voluntarily leave.

This application uses Microsoft's SignalR, http://signalr.net/, to push the current state of the fishbowl to all the connected clients so that when somebody joins or leaves the fishbowl everyone is immediately notified.  There is also a basic chat capability.

The main technology components are
- ASP.NET
- SignalR
- AngularJS
- Bootstrap

The application uses responsive design techniques tied into Bootstrap and the UI renders on desktops, tablets, and to some extent, phones.
