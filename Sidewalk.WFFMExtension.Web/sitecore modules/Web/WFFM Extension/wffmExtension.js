

function IsNormalPageMode() {
    if (typeof Sitecore == "undefined") {
        return true;
    }
    return false;
}

$scw(window).load(function () {
    init();
});



function init() {


    $scw('.js-onchange').bind('change', function () {

        var el = $scw(this);
        var id = el.attr('id');
        var selectedValue = el.val();
        if (selectedValue === '') {
            // if selectedValue is empty, we assume it's because we are on a checkbox or radio and so we took a span
            // if it's not the case, we will send an empty selectedValue;
            el = $scw(this).find("input");
            id = el.attr('id');
        }

        if (el.attr("type") === "checkbox") {
            var value = "";
            var allCheckbox = el.closest("div").find("input");
            for (var i = 0; i < allCheckbox.length; i++) {
                if ($('#' + allCheckbox[i].id).is(":checked")) {
                    value += allCheckbox[i].value + ",";
                }
            }
            if (value !== '') {
                value = value.slice(0, -1);
            }
            callWs(id, value);
        } else if (el.attr("type") === "radio") {
            callWs(id, el.val());
        }
        else {
            callWs(id, selectedValue);
        }
    });
}

function callWs(id, selectedVal) {
    var isNormalPageMode = IsNormalPageMode();
    $scw.ajax({
        url: "/Webservices/EvaluateConditions.asmx/GetActions",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({ id: id, selectedValue: selectedVal, pageMode: isNormalPageMode }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var actions = data.d;
            var arrayLength = actions.length;
            var currentAction;
            var hideControl;
            var classSelector;
            var el;
            var sectionToHideShow;

            for (var i = 0; i < arrayLength; i++) {
                currentAction = actions[i];
                hideControl = currentAction.HideControl;
                classSelector = 'name\.' + currentAction.CssClassSelector;

                // control is field
                if (currentAction.ControlType == 0) {
                    el = $scw(document.getElementsByClassName(classSelector));
                    var validatorsField = el.find("span");
                    if (hideControl) {
                        for (var j = 0; j < validatorsField.length; j++) {
                            window.ValidatorEnable(validatorsField[j], false);
                            validatorsField[j].style.visibility = 'hidden';
                        }
                    } else {
                        for (var j = 0; j < validatorsField.length; j++) {
                            window.ValidatorEnable(validatorsField[j], true);
                            validatorsField[j].style.visibility = 'visible';
                        }
                    }
                    el.toggle(!hideControl);
                } else {
                    var section = $scw(document.getElementsByClassName(classSelector));
                    sectionToHideShow = section.parent().parent();
                    var validatorsSection = section.find("span");
                    if (hideControl) {
                        for (var z = 0; z < validatorsSection.length; z++) {
                            window.ValidatorEnable(validatorsSection[z], false);
                            validatorsField[j].style.visibility = 'hidden';
                        }
                    } else {
                        for (var z = 0; z < validatorsSection.length; z++) {
                            window.ValidatorEnable(validatorsSection[z], true);
                            validatorsField[j].style.visibility = 'visible';
                        }
                    }
                    if (hideControl) {
                        sectionToHideShow.hide();
                        sectionToHideShow.parent().hide();
                    } else {
                        sectionToHideShow.show();
                        sectionToHideShow.parent().show();
                    }
                }
            }

        }
    });//getJSON
}



