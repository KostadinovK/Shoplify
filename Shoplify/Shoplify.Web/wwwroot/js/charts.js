let usersCtx = document.getElementById('usersChart');
let adsCtx = document.getElementById('adsChart');
let categoriesCtx = document.getElementById('adCategoriesChart');

let usersData = {
    labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
    datasets: [{
        label: '# of new Users',
        data: [3, 10, 11, 5, 8, 2, 5],
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

let adsData = {
    labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
    datasets: [{
        label: '# of new Ads',
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

let options = {
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

let usersChart = new Chart(usersCtx, {
    type: 'line',
    data: usersData,
    options: options
});

let adsChart = new Chart(adsCtx, {
    type: 'line',
    data: adsData,
    options: options
});

var categoriesChart = new Chart(categoriesCtx, {
    type: 'doughnut',
    data: categoriesData,
    options: {
        responsive: false,
        cutoutPercentage: 40
    }
});