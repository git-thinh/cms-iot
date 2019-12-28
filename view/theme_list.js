function theme_listCtrl($rootScope, $scope, API) {
    /***************************************************/
    //#region VARIABLE 

    var module = $scope.module;
    var moduleID = module.id;
    console.log(module);

    var _kit_form = '_form_theme_list';
    $rootScope.regKit(moduleID, _kit_form);

    /***************************************************/

    $().w2form({
        name: _kit_form,
        header: 'Update Theme List',
        fields: [
        {
            field: 'html_field', type: 'textarea', required: true, html: {
                caption: '&nbsp;',
                attr: ' style="position: fixed;left: 0;top: 71px;bottom: 3px;width: 100%;min-height: 50%;overflow-x: hidden;overflow-y: auto;" '
            }
        },
        ],
        toolbar: {
            items: [
                { type: 'spacer' },
                { id: 'Save', type: 'button', caption: lang.ui.save, icon: 'fa fa-save', checked: true },
                { id: 'Close', type: 'button', caption: lang.ui.close, icon: 'fa fa-close', checked: true },
            ],
            onClick: function (event) {
                if (event.target == 'Save') {
                    ////var vali = w2ui[_kit_form].validate();
                    ////if (vali != null && vali.length > 0) { 
                    ////    return;
                    ////}

                    //////var data = w2ui[_kit_form].getChanges();
                    ////m_objectData['html'] = w2ui[_kit_form].record.html;
                    ////console.log(' m_objectData = ', m_objectData);

                    ////w2confirm('Bạn chắc chắn muốn lưu thay đổi?')
                    ////.yes(function () {
                    ////    API.module_Close(moduleID, [_kit_form]);
                    ////})
                    ////.no(function () {
                    ////    API.module_Close(moduleID, [_kit_form]);
                    ////});

                    var file = 'list.htm';
                    var data = w2ui[_kit_form].record.html_field;
                    API.update_File(file, data, {
                        ok: function (rs) {
                            if (rs) {
                                localStorage[file] = data;
                                w2alert(lang.ui.update_success);
                            }
                        }
                    });
                }
                if (event.target == 'Close') API.module_Close(moduleID);
            }
        },
        onRender: function (event) {
            event.onComplete = function () {
                // update field value
                var htm = localStorage['list.htm'];
                htm = formatText(htm);
                w2ui[_kit_form].record['html_field'] = htm;
                w2ui[_kit_form].refresh();
            }
        }
    });

    /***************************************************/


    $scope.submit = function () {
        console.log('SUBMIT = ', m_objectData);
    }

    $scope.callback = function (data) {
    };

    API.module_Render(moduleID, _kit_form);
}