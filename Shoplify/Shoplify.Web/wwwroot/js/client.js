let stripe = Stripe('pk_test_14LJKTkDZ7x8eO3o5suRCWTo00KvkJghrD');
let elements = stripe.elements();
let promoteBtn = document.getElementById("promoteBtn");
let cardDetails = document.querySelector("div.CardField-input-wrapper");
console.log(cardDetails);

let style = {
    base: {
        color: "#32325d",
    }
};

let card = elements.create("card", { style: style });
card.mount("#card-element");

card.addEventListener('change', ({ error }) => {
    const displayError = document.getElementById('card-errors');
    if (error) {
        displayError.textContent = error.message;
    } else {
        displayError.textContent = '';
    }
});

let form = document.getElementById('payment-form');

form.addEventListener('submit', function (ev) {
    ev.preventDefault();
    stripe.confirmCardPayment(clientSecret, {
        payment_method: {
            card: card,
            billing_details: {
                name: 'Jenny Rosen'
            }
        }
    }).then(function (result) {
        if (result.error) {
            // Show error to your customer (e.g., insufficient funds)
            console.log(result.error.message);
        } else {
            // The payment has been processed!
            if (result.paymentIntent.status === 'succeeded') {
            }
        }
    });
});