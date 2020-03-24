displayUsersCountChart('usersChart');
displayAdsCountChart('adsChart');

let categoriesCtx = document.getElementById('adCategoriesChart');

let categoriesData = {
    labels: ['Home', 'Gift', 'Electronics', 'Garden', 'Hobby', 'Sport', 'Fashion'],
    datasets: [{
        
        data: [12, 11, 3, 5, 8, 9, 13],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)',
            'rgba(156, 255, 64, 0.2)'
        ],
        borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)',
            'rgba(156, 255, 64, 1)'
        ],
        borderWidth: 1
    }]
};

var categoriesChart = new Chart(categoriesCtx, {
    type: 'doughnut',
    data: categoriesData,
    options: {
        responsive: false,
        cutoutPercentage: 40
    }
});


async function displayUsersCountChart(contextId) {

    let data = await getUsersCountDataAsync();

    displayLineChart(contextId, '# of new Users', data);
}

async function displayAdsCountChart(contextId) {

    let data = await getAdsCountDataAsync();

    displayLineChart(contextId, '# of new Ads', data);
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