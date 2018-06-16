

//Первоначальная настройка
jQuery(function () {
    jQuery.validator.addMethod("checkLookupValue", function (value, element, param) {
        var field = $('#' + param.itemId);
        if (!field) {
            return false;
        }
        if (field.val().trim()) {
            return true;
        }
        return false;
    }, "Необходимо указать значение");

    jQuery.validator.methods["date"] = function (value, element) {
         return this.optional(element) || /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(value);
     };
    
    jQuery.validator.methods["number"] = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    };
});

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

    engine.updateAjaxComboBox = function (propertyId, value) {
        (function (prop, val) {
            var ajaxComboBoxItem = $("#" + prop);
            var ajaxUrl = ajaxComboBoxItem.data("ajax-combobox-url");
            var firstEmpty = ajaxComboBoxItem.data("ajax-combobox-firstempty");
            var needFirstEmpty = (firstEmpty !== null && firstEmpty.trim());

            var ajaxValue = val;

            if (val == null) {
                ajaxValue = ajaxComboBoxItem.data("ajax-combobox-value");
            }

            var dat = { id: ajaxValue };

            engine.sendToServerData(ajaxUrl, dat, function (fData) {
                $("#" + prop).empty();

                if (needFirstEmpty) {
                    $("#" + prop).append($("<option>").val(null).text(firstEmpty));
                }

                $.map(fData.Items, function (item) {
                    var newOpt = $("<option>").val(item.Value).text(item.Text);
                    if (item.Selected == true) {
                        newOpt.prop('selected', 'selected');
                    }
                    $("#" + prop).append(newOpt);
                });
            });

        })(propertyId, value);

    };

    /*Заполнение комбобокса*/
    engine.populateAjaxComboBox = function (url, propertyId, value, firstEmptyOption) {

        var ajaxComboBoxItem = $("#" + propertyId);
        ajaxComboBoxItem.data("ajax-combobox-url", url);
        ajaxComboBoxItem.data("ajax-combobox-value", value);
        ajaxComboBoxItem.data("ajax-combobox-firstempty", firstEmptyOption);

        engine.updateAjaxComboBox(propertyId);

    };



    /*Создание ajax списка с check боксами*/
    engine.createCheckBoxList = function (url, propertyId, ulId, parentId) {
        var cblprntValue = null;
        if (parentId != null && parentId.trim()) {
            cblprntValue = $("#" + parentId).val();
            $("#" + parentId).change(function () {
                updateCheckBoxList(url, propertyId, ulId, $(this).val());
            });
        }
        updateCheckBoxList(url, propertyId, ulId, cblprntValue);
    };

    function updateCheckBoxList(url, propertyId, ulId, parentValue) {
        var dat = '{ids:' + $("#" + propertyId).val() + ',parent:"' + parentValue + '"}';

        engine.sendToServerAjax(url, dat, "application/json", function (fData) {
            var text = "";
            $.map(fData.Items, function (item, index) {
                var inputId = propertyId + "_item_" + index;
                var isChecked = "";
                if (item.Selected == true) {
                    isChecked = "checked";
                } else {
                    isChecked = "";
                }
                text += "<li><input id='" + inputId + "' type='checkbox' name='" + propertyId + "' value='" + item.Value + "' " + isChecked + "/> <label for='" + inputId + "'>" + jCrudEngine.escapeHtml(item.Text) + "</label></li>";
            });
            $("#" + ulId).html(text);
            $("#" + ulId + " input").change(function () {
                updateCheckBoxListIds(propertyId, ulId);
            });
            updateCheckBoxListIds(propertyId, ulId);
        });
    }


    function updateCheckBoxListIds(propertyId, ulId) {
        var idText = "[";
        $("#" + ulId + " input").map(function () {
            if ($(this).prop("checked") == true) {
                idText += '"' + $(this).val() + '",';
            }
        });

        if (idText.length > 1) {
            idText = idText.substring(0, idText.length - 1);
        }
        idText += "]";
        $("#" + propertyId).val(idText);
    }

    /*Создание lookup с окном выбора одного элемента*/

    engine.createSingleLookup = function (itemsUrl, itemUrl, propertyId, displayPropertyId, propValue, parentPropertyId, isRequired, documentIsReady, dialogId, isFullScreen, dialogSearchFormId, itemsId, widgetId, paginatorId, paginatorReloadFncName, dialogWidth, dialogHeight) {

        //Переносим диалоги в самый конец документа, чтобы не мешали общей валидации
        $(".singleLookupToRemove").remove(); //для начала удаляем ранее созданные
        $(document.body).append($("#" + dialogId).detach());
        $("#" + dialogId).addClass("singleLookupToRemove"); //помечаем для удаления

        //Вызываем скрипты, почему-то сами не хотят
        $("#" + dialogId + " script").each(function () {
            jQuery.globalEval($(this).html());
        });


        $("#" + displayPropertyId).on('input', function () {
            $(this).val($(this).prop("data-lookup-original"));
        });

        $("#" + dialogId).dialog({
            height: dialogHeight,
            width: dialogWidth,
            autoOpen: false,
            modal: true,
            buttons: [
                    {
                        "class": "btn btn-medium",
                        text: "ОК",
                        click: function () {
                            var propval = $("#" + itemsId + " li.lookup-selected-item").attr("data-value");
                            if (engine.hasValue(propval)) {
                                engine.loadSingleLookupItem(itemUrl, propval, propertyId, displayPropertyId, parentPropertyId);
                            } else {
                                engine.clearSingleLookupInputs(propertyId, displayPropertyId);
                            }


                            $(this).dialog("close");
                        }

                    },
                    {
                        "class": "btn btn-medium",
                        text: "Отмена",
                        click: function () {
                            $(this).dialog("close");
                        }

                    }
                ]
        });

        if (engine.hasValue(propValue)) {
            engine.loadSingleLookupItem(itemUrl, propValue, propertyId, displayPropertyId, parentPropertyId);
        }

        $("#" + widgetId + " .btn-lookup-open").click(function () {
            $("#" + itemsId).html("…");

            $("#" + dialogId).dialog("open");

            engine.reloadSingleLookupItems(itemsUrl, itemUrl, itemsId, dialogId, dialogSearchFormId, propertyId, displayPropertyId, parentPropertyId, paginatorId, paginatorReloadFncName);
            return false;
        });

        var btn = $("#" + widgetId + " .btn-lookup-clear");
        if (btn.length) {
            btn.click(function () {
                engine.clearSingleLookupInputs(propertyId, displayPropertyId);
            });
        }

        $("#" + dialogId + " .lookup-filter-btn").click(function () {
            engine.reloadSingleLookupItems(itemsUrl, itemUrl, itemsId, dialogId, dialogSearchFormId, propertyId, displayPropertyId, parentPropertyId, paginatorId, paginatorReloadFncName);
        });

        if (isFullScreen) {
            engine.setDialogFullScreen(dialogId);
        }

        if (isRequired) {
            if (!documentIsReady) {
                $("#" + displayPropertyId).rules("add", { checkLookupValue: { itemId: propertyId} });
            } //else use applyCustomValidation method
        }

    };
    /*Вызывает на загрузку пункты Lookup*/
    engine.loadSingleLookupItem = function (itemUrl, value, propertyId, displayPropertyId, parentId) {
        var dt = {
            id: value
        };

        if (engine.hasValue(parentId)) {
            dt["parent"] = $('#' + parentId).val();
        }

        engine.sendToServerData(itemUrl, dt, function (data) {
            $('#' + propertyId).val(data.Id).trigger('change');
            $('#' + displayPropertyId).val(data.Value);
            $('#' + displayPropertyId).prop("data-lookup-original", data.Value);
            $("#" + displayPropertyId).valid();
        });
    };

    engine.clearSingleLookupInputs = function (propertyId, displayPropertyId) {
        $('#' + propertyId).val(null).trigger('change');
        $('#' + displayPropertyId).val(null);
        $('#' + displayPropertyId).prop("data-lookup-original", null);
        $("#" + displayPropertyId).valid();
    };

    engine.reloadSingleLookupItems = function (itemsUrl, itemUrl, itemsAreaId, dialogId, searchFormId, propertyId, displayPropertyId, parentPropertyId, paginatorId, paginatorReloadFncName) {
        var sdt = $("#" + searchFormId).serialize();

        if (parentPropertyId != null) {
            sdt["Parent"] = $("#" + parentPropertyId).val();
        }

        engine.sendToServerData(itemsUrl, sdt, function (data) {
            $("#" + itemsAreaId + " li").empty();
            $("#" + itemsAreaId).html(data.ItemsData);
            $("#" + itemsAreaId + " li").click(function () {
                $("#" + itemsAreaId + " li.ui-state-error").removeClass("ui-state-error lookup-selected-item");
                $(this).addClass("ui-state-error lookup-selected-item");
            });

            $("#" + itemsAreaId + " li").dblclick(function () {
                $("#" + dialogId).dialog("close");
                engine.loadSingleLookupItem(itemUrl, $(this).attr("data-value"), propertyId, displayPropertyId, parentPropertyId);

            });

            engine.createPaginator(paginatorId, data.TotalCount, data.PerPage, data.CurrentPage, data.PaginatorMaxItems, paginatorReloadFncName);

        }
        );

    };


    /*Создает Lookup по параметрам*/
    engine.createLookup = function (itemsUrl, itemUrl, propertyId, displayPropertyId, propValue, parentPropertyId, isRequired, documentIsReady, minLenght) {

        var hasStoredValue = engine.hasValue(propValue);

        var hasParent = engine.hasValue(parentPropertyId);
        var parentProp = null;

        if (hasParent) {
            hasParent = true;
            parentProp = $("#" + parentPropertyId);
        }
        var prop = $('#' + propertyId);
        var displayProp = $('#' + displayPropertyId);

        if (hasStoredValue) {

            engine.sendToServerData(itemUrl, { id: propValue }, function (data) {
                displayProp.val(data.Item.Value);
                prop.val(data.Item.Id);
            });
        }

        displayProp.bind("keydown", function (event) {
            if (event.keyCode === $.ui.keyCode.TAB &&
                $(this).data("ui-autocomplete").menu.active) {
                event.preventDefault();
            }
        }).click(function () {
            $(this).autocomplete('search', $(this).val());
        }).autocomplete({
            source: function (request, response) {

                var parentId = null;

                if (hasParent) {
                    parentId = parentProp.val();
                }

                engine.sendToServerData(itemsUrl, {
                    query: request.term,
                    parent: parentId
                }, function (data) {
                    response($.map(data.Items, function (item) {
                        return {
                            id: item.Id,
                            value: item.Value
                        };
                    }));
                });
            },
            minLength: minLenght,
            select: function (event, ui) {
                if (ui.item) {
                    prop.val(ui.item.id);
                    displayProp.val(ui.item.value);
                }
            },
            search: function () {
                var term = this.value;
                if (term.length < minLenght) {
                    return false;
                }
                return true;
            }
        });

        if (isRequired) {
            if (!documentIsReady) {
                displayProp.rules("add", { checkLookupValue: { itemId: propertyId} });
            } //else use applyCustomValidation method
        }
    };

    engine.loadDataGrid = function (url, searchFormId, tableBodyId, createTdFunct, paginatorId, paginatorReloadFncName) {
        var serializedSearchForm = $('#' + searchFormId);
        var searchData = serializedSearchForm.serialize();
        engine.sendToServerData(url, searchData, function (data) {
            var htmlText = "";
            for (var i = 0; i < data.Items.length; i++) {
                htmlText += createTdFunct(data.Items[i]);
            }

            $('#' + tableBodyId).html(htmlText);

            engine.createPaginator(paginatorId, data.TotalCount, data.PerPage, data.CurrentPage, data.PaginatorMaxItems, paginatorReloadFncName);
        });
        return true;
    };


    /*Отправляет запрос на сервер об удалении определенной сущности по ее id и в случае успеха вызывает successFunct*/
    engine.postDeleteItem = function (url, entityId, successFunct) {
        engine.sendToServerData(url, { id: entityId }, successFunct);
    };

    /*Получает данные с определенного url передавая данные postData и в случае успеха вызывая функцию successFunct*/

    engine.sendToServerData = function (url, postData, successFunct) {
        engine.sendToServerAjax(url, postData, null, successFunct);
    };

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

                    engine.infrastructureCallbacks["error"](request.responseText); ;
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


    engine.createPaginator = function (paginatorId, totalCount, perPage, curPage, maxItems, paginatorReloadFncName) {
        var createPageLinkText = function (p, cp) {
            if (p == cp) {
                return "<span>" + p + "</span>";
            } else {
                return "<a href='' onclick='" + paginatorReloadFncName + "(" + p + ");return false;'>" + p + "</a>";
            }
        };

        var countPages = parseInt(totalCount / perPage, 10);
        if (totalCount % perPage) {
            countPages++;
        }

        var element = $('#' + paginatorId);
        var text = "";
        var page = 0;
        var i = 0;
        if (countPages > maxItems) {
            var middle = maxItems / 2;

            var startPage = curPage - middle;

            if (startPage < 1) {
                startPage = 1;
            }

            var endPage = curPage + middle;

            if ((endPage - startPage) < maxItems) {
                endPage += maxItems - endPage + startPage;
            }

            if (endPage > countPages) {
                endPage = countPages;
            }

            if ((endPage - startPage) < maxItems) {
                startPage -= maxItems - endPage + startPage;
            }


            if (startPage > 1) {
                text += createPageLinkText(1, curPage);
            }

            for (i = startPage; i <= endPage; i++) {
                text += createPageLinkText(i, curPage);
            }

            if (endPage < countPages) {
                text += createPageLinkText(countPages, curPage);
            }

        } else {
            for (i = 0; i < countPages; i++) {
                page = i + 1;
                text += createPageLinkText(page, curPage);
            }
        }
        element.html(text);
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


    /*Должен вызываться при динамичесом формировании html, добавляет к определенным формам правила валидации*/
    engine.applyCustomValidation = function (formId) {
        $("#" + formId + " input[data-val-lookup-required='true']").each(function () {
            $(this).rules("add", { checkLookupValue: { itemId: $(this).attr('data-val-lookup-property')} });
        });
    };


    engine.resetFormUnobtrusiveValidation = function (formElementId) {
        var formToReset = $("#" + formElementId);
        formToReset.removeData("validator");
        formToReset.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("#" + formElementId);
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

    /*Получает значения определенной ячейки дата грида*/
    engine.getDataGridRowValue = function (jGridId, rowId, columnName) {
        return $("#" + jGridId + "TableBody tr[data-val-item-id='" + rowId + "'] td[data-val-key='" + columnName + "']").text();
    };

    /*Получает тэг tr для грида с определенным id*/
    engine.getDataGridTrTag = function (jGridId, rowId) {
        return $("#" + jGridId + "TableBody tr[data-val-item-id='" + rowId + "']");
    };

    /*Сохранение данных из окна созданным через Html.EditModelDialog посредством получения ajax запросов*/
    engine.saveDataEditModelDialog = function (url, data, editFormId, editDialogId, successFunct) {
        engine.sendToServerData(url, data, function (fData) {
            if (fData.NeedReloadModel == true) {
                $("#" + editFormId).html(fData.Data);
                jCrudEngine.resetFormUnobtrusiveValidation(editDialogId);
                jCrudEngine.applyCustomValidation(editDialogId);
            } else {
                $("#" + editFormId).empty();
                $("#" + editDialogId).dialog("close");
                successFunct(fData);
            }
        });
    };

    /*Отображение окна созданным через Html.EditModelDialog посредством получения ajax запросов*/
    engine.showEditModelDialog = function (url, data, editFormId, editDialogId, successFunct) {
        engine.sendToServerData(url, data, function (fData) {
            $("#" + editFormId).html("..");
            $("#" + editDialogId).dialog("open");
            $("#" + editFormId).html(fData.Data);
            jCrudEngine.resetFormUnobtrusiveValidation(editDialogId);
            jCrudEngine.applyCustomValidation(editDialogId);
            successFunct(fData);

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




/*
use:
var html = "<input data-val='true' "+
           "data-val-required='This field is required' " +
           "name='inputFieldName' id='inputFieldId' type='text'/>;
$("form").append(html);
 
$.validator.unobtrusive.parseDynamicContent('form input:last');
Валидация динамическо добавленных элементов
Но можно и:
$("form").removeData("validator");
$("form").removeData("unobtrusiveValidation");
$.validator.unobtrusive.parse("form");
*/
(function ($) {
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    }
})($);