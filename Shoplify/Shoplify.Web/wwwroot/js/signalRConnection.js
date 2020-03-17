setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/message")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("SendMessage",
        (message) => {
            let allMessages = document.getElementById("messages");
            allMessages.insertAdjacentHTML('beforeend', '<div class="row">' +
                '<div class="offset-1 col-10">' +
                '<p>' +
                '<div class="row">' +
                '<div class="col-2">' +
                message.sender +
                '</div>' +
                '<div class="offset-6 col-3">' +
                message.sentOn +
                '</div> </div>' +
                '<div class="card bg-info col-11">' +
                '<div class="card-body text-white">' +
                message.content +
                '</div > </div >' +
                '</div > </div >' +
                '</p>'
            );
        });

    connection.start()
        .catch(err => console.error(err.toString()));

};
setupConnection();