let elements = document.getElementsByClassName('element-with-date');

for (let element of elements) {
    let utcDate = element.getElementsByClassName('utc-date')[0].value;
    
    let localDate = getLocalDataTimeString(new Date(utcDate + ' UTC'));

    element.getElementsByClassName('local-date')[0].textContent = localDate;
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