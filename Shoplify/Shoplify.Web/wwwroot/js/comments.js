function fetchComments(commentsSection, adId) {

    adId = adId.value;

    let url = `/Comment/GetAllByAd?id=${adId}`;

    fetch(url)
        .then(response => response.json())
        .then(comments => console.log(comments))
        .catch(err => console.log(err));
}