displayUsersCountChart('usersChart');
displayAdsCountChart('adsChart');
displayAdsCountByCategoriesChart('adCategoriesChart');

async function displayUsersCountChart(contextId) {

    let data = await getUsersCountDataAsync();

    displayLineChart(contextId, '# of new Users', data);
}

async function displayAdsCountChart(contextId) {

    let data = await getAdsCountDataAsync();

    displayLineChart(contextId, '# of new Ads', data);
}

async function displayAdsCountByCategoriesChart(contextId) {

    let data = await getAdsCountByCategoriesDataAsync();

    displayPieChart(contextId, null, data);
}

async function getUsersCountDataAsync() {
    let response = await fetch(`/Administration/User/RegisteredThisWeek`);

    let data = await response.json();

    return data;
}

async function getAdsCountDataAsync() {
    let response = await fetch(`/Administration/Advertisement/CreatedThisWeek`);

    let data = await response.json();

    return data;
}

async function getAdsCountByCategoriesDataAsync() {
    let response = await fetch(`/Administration/Advertisement/CountByCategories`);

    let data = await response.json();

    return data;
}

function displayLineChart(contextId, label, data) {
    let context = document.getElementById(contextId);

    const backgroundColors = [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)',
        'rgba(156, 255, 64, 0.2)'
    ];

    const borderColors = [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)',
        'rgba(156, 255, 64, 1)'
    ];

    const options = {
        scales: {
            yAxes: [
                {
                    ticks: {
                        beginAtZero: true
                    }
                }
            ]
        }
    };

    let chart = new Chart(context, {
        type: 'line',
        data: {
            labels: Object.keys(data),
            datasets: [{
                label,
                data: Object.values(data),
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options
    });
}

function displayPieChart(contextId, label, data) {
    let context = document.getElementById(contextId);

    const backgroundColors = [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)',
        'rgba(156, 255, 64, 0.2)',
        'rgba(255, 159, 64, 0.2)',
        'rgba(123, 178, 64, 0.2)',
        'rgba(44, 255, 122, 0.2)',
        'rgba(154, 22, 64, 0.2)',
        'rgba(44, 159, 164, 0.2)',
        'rgba(255, 56, 164, 0.2)',
        'rgba(44, 22, 255, 0.2)'
    ];

    const borderColors = [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)',
        'rgba(156, 255, 64, 1)',
        'rgba(255, 159, 64, 1)',
        'rgba(123, 178, 64, 1)',
        'rgba(44, 255, 122, 1)',
        'rgba(154, 22, 64, 1)',
        'rgba(44, 159, 164, 1)',
        'rgba(255, 56, 164, 1)',
        'rgba(44, 22, 255, 1)'
    ];

    const options = {
        responsive: false,
        cutoutPercentage: 40
    };

    let chart = new Chart(context, {
        type: 'doughnut',
        data: {
            labels: Object.keys(data),
            datasets: [{
                label,
                data: Object.values(data),
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options
    });
}