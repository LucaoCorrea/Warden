//$(document).ready(function () {
//    getDatatable('#table-contatos');
//    getDatatable('#table-usuarios');

//    $('.btn-total-contatos').click(function () {
//        var usuarioId = $(this).attr('usuario-id');

//        $.ajax({
//            type: 'GET',
//            url: '/User/ListContactsByUserId/' + userId,
//            success: function (result) {
//                $("#listaContatosUsuario").html(result);
//                $('#modalContatosUsuario').modal();
//                getDatatable('#table-contatos-usuario');
//            }
//        });
//    });
//})


let table = new DataTable('#table-contacts'); //optei por não traduzir

$('.close-alert').click(function () {
        $(this).closest('.alert').hide(); // Fecha apenas o alerta correspondente
});

