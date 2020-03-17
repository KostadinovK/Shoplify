const displayReceivedMessage = function (message, messageContainerId) {

    let messageContainer = document.getElementById(messageContainerId);
    console.log(message);
    messageContainer.innerHTML += 
        `
<div class="media w-50 mb-3">
    <div class="media-body ml-3">
        <p class="small text-muted">${message.senderName}</p>
        <div class="bg-light rounded py-2 px-3 mb-2">
            <p class="text-small mb-0 text-muted">${message.text}</p>
        </div>
        <p class="small text-muted">${message.sendOn}</p>
    </div>
</div>        `;
} 

export { displayReceivedMessage };