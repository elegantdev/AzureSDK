React Front end:
azurewebpubsub.js
Have function "connect" which use "@azure/web pubsub" npm package 
//Here we are getting a token to access websocket url 
let token = await serviceClient.getClientAccessToken() 
//Will use token.url to connect with websoket
const url = token.url
let wsClient = new W3CWebSocket(url, 'json.webpubsub.azure.v1')
 

App.js
In app.js we are importing connect 
 
And call connect function in componentWillMount of App class with handler functions:
 
To show notifiations comming from websocket we have used the 'react-notification-timeline' npm package.
 

 

Backend :
In coreapi project have created a api AzurePubSub to post message in websocket and this will broadcast message for all connected clients.
 
In react api we are using this api to post message.
 
 

In core of backend apis the nuget package Azure.Messaging.WebPubSub is used to connect with azure pubsub service and send message in websocket.
 







