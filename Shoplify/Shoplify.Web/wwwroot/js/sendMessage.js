$('#send-message-form').on('submit keypress', function (e) {
    var key = e.which;
    if (key === 13) {
        e.preventDefault();

        let form = $(this);
        let url = form.attr('action');

        $.ajax({
            type: 'POST',
            url: url,
            data: form.serialize(),
            success: function (data) {
                var today = new Date();
                var hours = today.getHours();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                var yyyy = today.getFullYear();
                var ampm = today.getHours() >= 12 ? 'PM' : 'AM';
                hours = hours % 12;
                hours = hours ? hours : 12;
                var time = hours + ":" + (today.getMinutes() < 10 ? '0' : '') + today.getMinutes();

                today = mm + '/' + dd + '/' + yyyy + ' ' + time + ' ' + ampm;

                const messageLength = 2;
                var message = $('#message-content').val();

                if (message.length > messageLength) {
                    $('#messages').append(
                        '<div class="row mb-3">' +
                        '<div class="offset-1 col-10">' +
                        '<p>' +
                        '<div class="row">' +
                        '<div class="offset-1 col-2">Me' +
                        '</div>' +
                        '<div class="offset-6 col-3">' +
                        today +
                        '</div>' +
                        '<div class="offset-1 col-11 card bg-warning">' +
                        '<div class="card-body">' +
                        $('#message-content').val() +
                        '</div>' +
                        '</div>' +
                        '</p>' +
                        '</div>' +
                        '</div>');

                    $('#message-content').val('');
                }
            }
        });
    }
});