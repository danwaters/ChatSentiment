# Realtime Chat Sentiment
This is a simple chat application intended for use in hackathons and workshops. iOS and Android clients can connect to a central SignalR Core hub (Web API project) hosted in Azure App Service. The hub receives client messages as JSON and relays them to all connected clients. Chat text is analyzed in realtime by Azure cognitive services and averaged on each client using a SkiaSharp-based circular fill. 

## User Interface and HUD
The UI and its substructure (BaseContentPage, BaseViewModel) is based on Hunt by Rob DeRosa: http://www.github.com/rob-derosa/Hunt

## Architecture

