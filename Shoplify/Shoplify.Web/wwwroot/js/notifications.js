document.addEventListener("DOMContentLoaded",
    () => {
        let notificationCounter = document.getElementById("notificationsCount");
        let userId = document.getElementById("userId").value;

        fetch(`/Notification/GetAllCount?userId=${userId}`)
            .then(response => response.json())
            .then(notificationsCount => notificationCounter.textContent = notificationsCount)
            .catch(err => console.log(err));
    });