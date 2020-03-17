import { displayReceivedMessage } from './displayMessage.js';

const setupConnection = function () {
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("/message")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("SendMessage",
        (message) => {
            displayReceivedMessage(message, "messages");
        });

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();