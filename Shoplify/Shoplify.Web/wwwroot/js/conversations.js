document.addEventListener("DOMContentLoaded",
    () => {
        let conversationsCounter = document.getElementById("conversationsCount");
        let userId = document.getElementById("userId").value;

        fetch(`/Conversation/GetAllCount?userId=${userId}`)
            .then(response => response.json())
            .then(conversationsCount => conversationsCounter.textContent = conversationsCount)
            .catch(err => console.log(err));
    });