var MachineLearningBP;
if (!MachineLearningBP) MachineLearningBP = {
    Console: {},
    Nba: {},
    Mlb: {}
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
    var consoleHub = $.connection.consoleHub; //get a reference to the hub

    consoleHub.client.writeLine = function (line) { //register for incoming messages
        MachineLearningBP.Console.writeLine(line);
    };
});

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