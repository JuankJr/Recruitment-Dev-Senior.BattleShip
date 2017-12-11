var gamejs = gamejs || {};

gamejs.loadDefaultBoar = function (board, gameboard, Generateboard, user, jugar)
{
    $(board + ' > tbody', gameboard).empty();
    for (var i = 0; i < 8; i++) {
        var tr = $("<tr>");
        for (var j = 0; j < 8; j++) {
            var td = $("<td data-column='" + j + "' data-row='" + i + "' class='water'></td>");
            if (Generateboard != undefined) {
                switch (Generateboard._board[i][j]) {
                    case "B":
                        td.addClass('ship');
                        break;
                    default:
                }
            }
            tr.append(td);
        }
        $(board + ' > tbody:last-child', gameboard).append(tr);
    }
    

}

gamejs.ServerMethod = function (Email, User, gameboard, form)
{
    var gameConnection = $.connection.gameHub;
    $("#oppBoard >tbody", gameboard).empty();
    $("#myBoard >tbody", gameboard).empty();
  
    gamejs.loadDefaultBoar("#oppBoard", gameboard);

    gameConnection.client.showMessageGame = function (mensaje) {
        alert(mensaje);
    }

    gameConnection.client.usuarioOnline = function (mensaje) {
        alert(mensaje);
        gameboard.hide();
        form.show();
    }

    gameConnection.client.userTurn = function (mensaje) {
        alert(mensaje);
        gameboard.hide();
        form.show();
    }
    gameConnection.client.loadBoardGame = function ()
    {
        $("#oppBoard >tbody", gameboard).empty();
        $("#myBoard >tbody", gameboard).empty();
        gamejs.ServerMethod(Email, User, gameboard, form);
    }

    gameConnection.client.opponentShoot = function (shoot) {
        switch (shoot.response) {
            case "D":
                $("*[data-column='" + shoot.column + "'][data-row='" + shoot.row + "']", $("#myBoard", gameboard)).addClass('failShoot')
                break;
            case "X":
                $("*[data-column='" + shoot.column + "'][data-row='" + shoot.row + "']", $("#myBoard", gameboard)).addClass('xploit')
                break;
            default:
                break;
        }
    }

    gameConnection.client.GanoElJugador = function (mensaje)
    {
        alert(mensaje)

    }


    gameConnection.client.userMove = function (response) {
        switch (response.response) {
            case "D":
                $("*[data-column='" + response.column + "'][data-row='" + response.row + "']", $("#oppBoard", gameboard)).addClass('failShoot')
                break;
            case "X":
                $("*[data-column='" + response.column + "'][data-row='" + response.row + "']", $("#oppBoard", gameboard)).addClass('xploit')
                break;
            default:
                break;
        }
    }
    gameConnection.client.LoadBoard = function (response) {
            gamejs.loadDefaultBoar("#myBoard", gameboard, response, User);
    }
    gameConnection.client.LoadAllBoard = function (response) {
        gamejs.loadDefaultBoar("#myBoard", gameboard, response, User);
        
    }
    $.connection.hub.start().done(function () {
        gameConnection.server.login(User, Email);
        $("#oppBoard td", gameboard).click(function (event) {
            var tdelement = $(this);
            gameConnection.server.jugar(Email, tdelement.data('column'), tdelement.data('row'));
        });
        $("#btnRestart", gameboard).click(function (event) {
            gameConnection.server.restartGame(Email);
            $.each($("#oppBoard td", gameboard), function (i, e) {
                var td = $(e);
                td.removeClass();
                td.addClass('water');
            });
        });
        
    });
    

   
  
    
 
}

$(document).ready(function () {
    var gameboard = $("#GameBoard", document);
    gameboard.hide();
     $("#btnRegisterUser", document).click(function (event) {
         

         var form = $("#PlayerInformation", document);
         var UserName = $("#username", form).val();
         var Email = $("#email", form).val();

         gamejs.ServerMethod(Email, UserName, gameboard, form);
          form.hide();
          gameboard.show();

     });
});
