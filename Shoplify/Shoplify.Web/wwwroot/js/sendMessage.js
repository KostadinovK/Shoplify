let sendMessageForm = document.getElementById('sendMessageForm');

sendMessageForm.addEventListener('keypress', (event) => { sendMessageHandler(event, sendMessageForm) });

function sendMessageHandler(event, form) {
    let keyPressed = event.which;

    if (keyPressed !== 13) {
        return;
    }

    event.preventDefault();

    let url = form.getAttribute("action");

    $.ajax({
        type: 'POST',
        url: url,
        data: $("#sendMessageForm").serialize(),
        success: displayMessage
    });
}

function displayMessage() {
    let todayDate = new Date();
    let hour = todayDate.getHours();
    let day = String(todayDate.getDate()).padStart(2, '0');
    let month = String(todayDate.getMonth() + 1).padStart(2, '0');
    let year = todayDate.getFullYear();

    let partOfDay = todayDate.getHours() >= 12 ? 'PM' : 'AM';
    hour = hour % 12;
    hour = hour ? hour : 12;

    let time = hour + ":" + (todayDate.getMinutes() < 10 ? '0' : '') + todayDate.getMinutes();

    todayDate = month + '/' + day + '/' + year + ' ' + time + ' ' + partOfDay;

    const messageMaxLength = 1;

    let messageText = document.getElementById("messageContent").value;

    let messagesContainer = document.getElementById('messages');

    if (messageText.length > messageMaxLength) {

        messagesContainer.innerHTML += 
            `
                <div class="media w-50 ml-auto mb-3">
                    <div class="media-body">
                        <p class="small text-muted">Me</p>
                        <div class="btn-pink rounded py-2 px-3 mb-2">
                            <p class="text-small mb-0 text-white">${messageText}</p>
                        </div>
                        <p class="small text-muted">${todayDate}</p>
                    </div>
                </div>
            `;
    }

    document.getElementById("messageContent").value = '';
}