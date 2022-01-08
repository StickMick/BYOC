# BYOC
## Bring Your Own Client
An online game with no client - challenge yourself by creating your own and compete with others.


Am I making it up as I go?

*Definitely*.

# TO DO
Feel free to open an issue and claim something.

* Web Server (This part actually gets a UI)
  * Blazor Server
  * Logins stored in app
  * Admin account seeded through config variables
  * User Management
    * Create User
    * View Users
    * Remove Users
    * View API Keys
    * Revoke API Keys
  * Account Management
    * Generate API Key
  * Websockets (signalR) for game interaction
    * Validate API Key

* Add more systems to RTS
  * Refine world generation
  * Refine pathfinding
  * Add buildings
  * Add resources
  * Add unit health/actions/death

### Plans
* Multiplayer RTS
* Intended as learning tool / game
* Self hosted - because I dont want to pay for servers.
* Grid based
* Configurable rate limiter - to stop players from overloading the server, probably redis
* Round based
* Dockerize it

### Player Process
* Connect to server admin through http
* Create Account
* Generate Key
* Create client that uses key with API/websockets/whatever to control their units

#### Proof of concept plans
* Create basic game library for API logic, testing with a console app/unit tests
* Create web API with authentication and server management
* Connect web API actions to library actions
* Add persistence - sql/redis/whatever
