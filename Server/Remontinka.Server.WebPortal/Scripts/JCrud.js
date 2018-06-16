
var jCrudEngine = (function () {

    var engine = new function () {
    };

    /*Здесь будут содержаться функции */
    engine.infrastructureCallbacks = [];

    /*Регистрация колбеков инфраструктуры приложения*/
    engine.registerCallbacks = function (unauthorize, error) {
        engine.infrastructureCallbacks["unauthorize"] = unauthorize;
        engine.infrastructureCallbacks["error"] = error;
    };

    /*Регистрация коблеков для индикации ajax запросов*/
    engine.registerAjaxCallbacks = function (beginSendAjax, endSendAjax) {
        engine.infrastructureCallbacks["beginSendAjax"] = beginSendAjax;
        engine.infrastructureCallbacks["endSendAjax"] = endSendAjax;
    };

    /**
     Сохраняет текущий фильтр.
     */
    engine.saveCurrentFilter = function(grid,nameTextBox,filtersComboBox, saveUrl) {
        //alert(grid.cpFilterExpression);
        var nm = nameTextBox.GetValue();
        if (!engine.hasValue(nm) || nm.length === 0) {
            alert("Нельзя сохранить пустое имя");
            return;
        }
        engine.sendToServerData(saveUrl,
            { filterData: grid.cpFilterExpression, filterName: nm },
            function() {
                filtersComboBox.PerformCallback();
            });
    };

    /*
     Получает фильтр для грида по Id.
     */
    engine.applyGridFilter = function (grid, combobox, getUrl) {
        var vlId = combobox.GetValue();
        if (!engine.hasValue(vlId) || vlId.length === 0) {
            return;
        }
        engine.sendToServerData(getUrl, { userFilterId: vlId },
            function (filterData) {
                grid.ApplyFilter(filterData.Data);
            });
    }
    engine.updateWarehouseDocStatus = function (grid, docId, updateUrl) {
        engine.sendToServerData(updateUrl, { warehouseDocID: docId },
            function (s) {
                grid.Refresh();
            });
    }

    /*Получает данные с определенного url передавая данные postData и в случае успеха вызывая функцию successFunct*/

    engine.sendToServerData = function (url, postData, successFunct) {
        engine.sendToServerAjax(url, postData, null, successFunct);
    };

    /**
     Производит разбор ответа с ошибками.
     */
    engine.processErrorResponse = function(data)
    {
        var resObj = jQuery.parseJSON(data);
        if (resObj) {
            if (resObj.Result == 'Authorize') {
                engine.infrastructureCallbacks["unauthorize"](resObj.AuthUrl);
                return;
            } else if (resObj.Result == 'Error') {
                engine.infrastructureCallbacks["error"](resObj.Description);
                return;
            }
            
        } 
        engine.infrastructureCallbacks["error"](data);
        
    }

    engine.sendToServerAjax = function (url, postData, contentType, successFunct) {
        var settings = {
            url: url,
            method: "POST",
            data: postData,
            success: function (data) {
                if (data.Result == 'Success') {
                    successFunct(data);
                }
                else if (data.Result == 'Error') {
                    engine.infrastructureCallbacks["error"](data.Description);
                }
                else if (data.Result == 'Authorize') {
                    engine.infrastructureCallbacks["unauthorize"](data.AuthUrl);
                }
            },
            error: function (request, status, error) {
                if (request.status && request.status == 403 && request.responseText) {
                    var resObj = jQuery.parseJSON(request.responseText);
                    if (resObj && resObj.Result == 'Authorize') {
                        engine.infrastructureCallbacks["unauthorize"](resObj.AuthUrl);
                    }
                } else {

                    engine.infrastructureCallbacks["error"](request.responseText);;
                }

            },
            complete: function () {
                if (engine.infrastructureCallbacks["endSendAjax"]) {
                    engine.infrastructureCallbacks["endSendAjax"]();
                }
            }
        };

        if (contentType != null) {
            settings.contentType = contentType;
        }

        if (engine.infrastructureCallbacks["beginSendAjax"]) {
            engine.infrastructureCallbacks["beginSendAjax"]();
        }

        $.ajax(settings);
    };



    /*Возвращает результат проверки наличия хранимого значения*/
    engine.hasValue = function (propValue) {
        if (propValue != null && propValue.trim()) {
            return true;
        }
        return false;
    };

    /*Устанавливает настройки определенного диалогового окна на полный укран*/
    engine.setDialogFullScreen = function (dialogId) {
        var outLine = 50;
        $("#" + dialogId).dialog("option", {
            resizable: false,
            draggable: false,
            height: $(window).height() - outLine,
            width: $(window).width() - outLine
        });


        $(window).bind("resize", function (e) {
            $("#" + dialogId).dialog("option", {
                height: $(window).height() - outLine,
                width: $(window).width() - outLine
            }).trigger('dialogresize');
        });
    };


    

    // List of HTML entities for escaping.
    engine.htmlEscapes = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#x27;',
        '/': '&#x2F;'
    };

    // Regex containing the keys listed immediately above.
    engine.htmlEscaper = /[&<>"'\/]/g;


    // Escape a string for HTML interpolation.
    engine.escapeHtml = function (string) {
        return ('' + string).replace(engine.htmlEscaper, function (match) {
            return engine.htmlEscapes[match];
        });
    };

    engine.autocompleteSplit = function (val) {
        return val.split(/,\s*/);
    };

    engine.autocompleteExtractLast = function (term) {
        return engine.autocompleteSplit(term).pop();
    };

    return engine;
})();



