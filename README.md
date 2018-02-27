# Realtime Chat Sentiment
This is a simple chat application intended for use in hackathons and workshops. iOS and Android clients can connect to a central SignalR Core hub (Web API project) hosted in Azure App Service. The hub receives client messages as JSON and relays them to all connected clients. Chat text is analyzed in realtime by Azure cognitive services and averaged on each client using a SkiaSharp-based circular fill. 

## Architecture
![sentiment1](https://user-images.githubusercontent.com/780735/36743774-b4fe528a-1bb0-11e8-96f1-958a4b461801.png)

## Tech
- Azure App Service
- SignalR (.NET Core)
- Azure Text Analytics
- Xamarin.Forms
- SkiaSharp

## User Interface and HUD
The UI and its substructure (BaseContentPage, BaseViewModel) is based on Hunt by Rob DeRosa: http://www.github.com/rob-derosa/Hunt

## Future Improvements
Plan to use Azure Functions to trigger text analytics + enable persistence of chat sessions across clients using Cosmos DB
![sentiment2](https://user-images.githubusercontent.com/780735/36743921-1732c0f8-1bb1-11e8-8966-15c09ac7b455.png)
