function TriggerModal(event) {
    
    event.preventDefault();

    var button = event.target;
    var message = $(event.target).attr('data-message');
    var modalHtml = '<div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">' +
        '<div class="modal-dialog modal-dialog-centered" role = "document">' +
        '<div class="modal-content">' +
        '<div class="modal-body">'+ message +'</div>' +
        '<div class="modal-footer">' +
        '<button id="close" type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>' +
        '<button id="delete" type="button" class="btn btn-primary">Ok</button>' +
        '</div>' +
        '</div>' +
        '</div >' +
        '</div>';

    $('form').append(modalHtml);

    $('#exampleModalCenter').modal('show');

    $('#close').click(() => { $('#exampleModalCenter').modal('hide');});

    $('#delete').click(() => {
        $(button).removeAttr('onclick');
        $(button).click();
    });
}