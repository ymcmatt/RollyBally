# RollyBally
Multiplayer Game using Airconsole

RollyBally (4-player competitive game)
Role: Game Programmer
Team Size: 5 (2 programmers, 2 artists, 1 sound desginer)
Project Duration: 1 week

Programming Challenges:
   -  AirConsole Platform
   -  real-time networking game

In this project we are trying to create a 4-player competitive game. Each player can join a game with a code provided by Airconsole to play on its platform. Your goal is to use the bounce pad to fly to magic potion on floating island, and become the hunter to hunt others down.

Because this project is a one-week fast prototyping game, there are still many potential problems and space for improvement.
1. The biggest one is networking issue. For a real-time action game, lagging is critical and Airconsole playform has rather bad lag, which makes the gameplay pretty bad. If we want to make the lagging better, we should use Photon for networking or build our own server.
2. Because our game involves heavily on using jumping pad to fly to islands, the jumping physics is critical. Right now we just have a upper force on rigidbody to make the player jump, but for a more realistic physics, we should add a directional force at the start of jumping and create physics to prevent player from making unrealistic turn in midair. 
