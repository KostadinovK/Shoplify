﻿const displayReceivedMessage = function (message, messageContainerId) {
    let messageContainer = document.getElementById(messageContainerId);

    let utcTimeSendOn = new Date(`${message.sendOn} UTC`);
    let localTimeSendOn = getLocalDataTimeString(utcTimeSendOn);

    messageContainer.innerHTML += 
        `
<div class="media w-50 mb-3">
    <div class="media-body ml-3">
        <p class="small text-muted">${message.senderName}</p>
        <div class="bg-light rounded py-2 px-3 mb-2">
            <p class="text-small mb-0 text-muted">${message.text}</p>
        </div>
        <p class="small text-muted">${localTimeSendOn}</p>
    </div>
</div>        `;
} 

function getLocalDataTimeString(date) {

    const months = {
        Jan: '01',
        Feb: '02',
        Mar: '03',
        Apr: '04',
        May: '05',
        Jun: '06',
        Jul: '07',
        Aug: '08',
        Sep: '09',
        Oct: '10',
        Nov: '11',
        Dec: '12'
    };

    let dateLocalTime = date.toString();

    let month = months[dateLocalTime.split(' ')[1]];
    let day = dateLocalTime.split(' ')[2];
    let year = dateLocalTime.split(' ')[3];
    let time = dateLocalTime.split(' ')[4];

    let hours = time.split(':')[0];
    let minutes = time.split(':')[1];

    return `${day}/${month}/${year} ${hours}:${minutes}`;
}

export { displayReceivedMessage };