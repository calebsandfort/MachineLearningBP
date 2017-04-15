﻿var MachineLearningBP;
if (!MachineLearningBP) MachineLearningBP = {
    Admin: {},
    Console: {},
    Optimize: {},
    Nba: {},
    Mlb: {},
    Util: {},
    Enums: {
        GeneticOptimizeTargets: {
            NbaPoints: 0
        }
    }
};

(function ($) {

    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);
    });

    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }
})(jQuery);

$(document).ready(function () {
    MachineLearningBP.Util.initForm("accessTokenForm", MachineLearningBP.Admin.submitAccessTokenForm);
    
});

$(document).ready(function () {
    var consoleHub = $.connection.consoleHub; //get a reference to the hub

    consoleHub.client.writeLine = function (line) { //register for incoming messages
        MachineLearningBP.Console.writeLine(line);
    };
});

MachineLearningBP.Util.initForm = function (id, submitFunc) {
    var _$form = $('#' + id);

    _$form.validate({
        ignore: ":hidden:Not(.includeHidden), .ignoreValidation"
    });

    _$form.find('button[type=submit]')
        .click(function (e) {
            e.preventDefault();

            if (!_$form.valid()) {
                return;
            }

            var input = _$form.serializeFormToObject();

            submitFunc(input);
        });
}

MachineLearningBP.Util.showModalForm = function (id, clearForm) {
    var _$modal = $('#' + id);
    var _$form = _$modal.find("form");

    _$modal.modal("show");

    if (_$form.size() > 0) {
        setTimeout(function () {
            if (typeof (clearForm) == "undefined" || clearForm) {
                _$form.find('input').val("");
            }

            _$form.find('input:first').focus();
        }, 500);
    }
}

MachineLearningBP.Util.hideModalForm = function (id) {
    var _$modal = $('#' + id);
    _$modal.modal("hide");
}

MachineLearningBP.Admin.submitAccessTokenForm = function (input) {
    MachineLearningBP.Util.hideModalForm("accessTokenModal");

    abp.services.app.sheetUtility.completeGetAccessToken(input)
        .done(function () { });
}

MachineLearningBP.Admin.getAccessTokenUrl = function () {
    abp.services.app.sheetUtility.getAccessTokenUrl().done(function (result) {
        window.open(result);
        MachineLearningBP.Util.showModalForm("accessTokenModal")
    });
}

MachineLearningBP.Optimize.showGeneticOptimizeModal = function (target) {
    $.ajax({
        type: "POST",
        url: abp.appPath + 'ViewRenderer/GeneticOptimizeModal',
        data: JSON.stringify({
            target: target
        }),
        success: function (r) {
            $("#geneticOptimizeModalWrapper").html(r);
            MachineLearningBP.Util.initForm("geneticOptimizeForm", MachineLearningBP.Optimize.geneticOptimize);
            MachineLearningBP.Util.showModalForm("geneticOptimizeModal", false);
        },
        contentType: "application/json"
    });
}

MachineLearningBP.Optimize.geneticOptimize = function (input) {
    MachineLearningBP.Util.hideModalForm("geneticOptimizeModal");

    switch (parseInt(input.target)) {
        case MachineLearningBP.Enums.GeneticOptimizeTargets.NbaPoints:
            abp.services.app.nbaPointsExample.geneticOptimize(input).done(function () { });
            break;
    }
}

MachineLearningBP.Console.clear = function () {
    $("#consoleWell").html("");
}

MachineLearningBP.Console.writeLine = function (line) {
    $("#consoleWell").prepend("<div>" + line + "</div>");
}

MachineLearningBP.Nba.populateGames = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaGame.populateSamples().done(function (result) {
        
    });
}

MachineLearningBP.Nba.fillGames = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaGame.fillSamples().done(function (result) {

    });
}

MachineLearningBP.Nba.setSeasonsRollingWindowStart = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaSeason.setSeasonsRollingWindowStart().done(function (result) {

    });
}

MachineLearningBP.Nba.populatePointsExamples = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaPointsExample.populateExamples().done(function (result) {

    });
}

MachineLearningBP.Nba.kNearestNeighborsDoStuff = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaPointsExample.kNearestNeighborsDoStuff().done(function (result) {

    });
}

MachineLearningBP.Nba.getPointsKnnBestParametersCsv = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.nbaPointsExample.findOptimalParameters().done(function () {
    });
}

MachineLearningBP.Mlb.populateGames = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.mlbGame.populateSamples().done(function (result) {

    });
}

MachineLearningBP.Mlb.fillGames = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.mlbGame.fillSamples().done(function (result) {

    });
}

MachineLearningBP.Mlb.setSeasonsRollingWindowStart = function () {
    MachineLearningBP.Console.clear();

    abp.services.app.mlbSeason.setSeasonsRollingWindowStart().done(function (result) {

    });
}

MachineLearningBP.getPath = function () {
    return window.location.protocol + "//" + window.location.host + abp.appPath;
}