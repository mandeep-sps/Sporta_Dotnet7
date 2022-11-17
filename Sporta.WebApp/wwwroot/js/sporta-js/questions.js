//CRUD URLs
let addQuestionUrl = "/Question/AddQuestion",
    getQuestionUrl = "/Question/GetQuestion",
    updateQuestionUrl = "/Question/UpdateQuestion",
    deleteQuestionUrl = "/Question/RemoveQuestion",
    navbar_question = "navbar-question";

//Forms 
let question_form = "#question-form",
    addExcelFile = "#addExcelFile";

//table
let question_list = "#question-list",
    cacheCategoryData,
    pills_questions_content = '#pills-questions-content',
    pill_category_content = '#pills-category-content';

//Global
let selectedFile, flag = 0,
    dbQuestions,
    _category_id = 0,
    _questions_data;

var Question1 = null;
var editor = null;
$(document).ready(function () {

    editor = CKEDITOR.replace('Question1');
    editor.on('change', function (evt) {
        $('#txt-question').val(evt.editor.getData());
    });
    toogleNavbar(navbar_question);
    loadCategories();
    $('.sidebar').slideUp(), $('#CategorySearchFormDiv').hide(), $('#question-btn-div').hide(), $('#category-btn-div').show();

    $('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {

        if ($(e.target).attr('href') === pills_questions_content) {
            loadDropDownData();
            $('.sidebar').slideUp(), $('.category-form-div').hide(), $('#CategorySearchFormDiv').show(), $('#question-btn-div').show(), $('#category-btn-div').hide();
        } else {
            loadCategories();
            $('.sidebar').slideUp(), $('#CategorySearchFormDiv').hide(), $('#question-btn-div').hide(), $('#category-btn-div').show();
        }
    });

    $('[data-toggle="tooltip"]').tooltip();
});

// Loading Questions on Category On Change
$('select#category-search').on('change', function (e) {
    _category_id = $(this).val();
    closeSideBar();
    loadQuestions();
});

//Binding Questions with Grid
function loadQuestions() {
    var getAllQuestions = _category_id ? "/Question/GetQuestionsByCategory?categoryId=" + _category_id : "/Question/GetQuestions";

    $.get(getAllQuestions, function (response) {
        _questions_data = response.data;
        var categoryName = response.data[0]?.category?.toUpperCase();
        $('#categoryNameSpan').html(categoryName ? categoryName : "");
        var tableColumns = [
            { data: "question1", render:questionRemoveRegex},
            { data: "optionA", render: getShortString },
            { data: 'optionB', render: getShortString },
            { data: 'optionC', render: getShortString },
            { data: 'optionD', render: getShortString },
            { data: 'answer', render: getShortString },
            { data: null, render: usersActionButtons }
        ]
        SportaDataTable.CreateDataTable(question_list, response.data, tableColumns)
        $("#question-list").show();
    });
}

function getShortString(data, type, row) {
    return type === 'display' && data.length > 15 ?
        `<span data-toggle="tooltip" title='${data}'> ${data.substr(0, 15)}… </span>` : data;
}

//Remove Html Tags From Question
function questionRemoveRegex(data, type, row) {
    var regX = /(<([^>]+)>)/ig;
    var html = data;
    var findIsStringIshtml = isHTML(html);
    if (findIsStringIshtml) {
        var textQuestion = html.replace(regX, "");
        return type === 'display' && textQuestion.length > 15 ?
            `<span data-toggle="tooltip" title='${textQuestion}'> ${textQuestion.substr(0, 15)}… </span>` : textQuestion;
    }
    else {
        return type === 'display' && html.length > 15 ?
            `<span data-toggle="tooltip" title='${html}'> ${html.substr(0, 15)}… </span>` : html;
    }
}

//Find Is String Contains Html Or Not
function isHTML(str) {
    var a = document.createElement('div');
    a.innerHTML = str;

    for (var c = a.childNodes, i = c.length; i--;) {
        if (c[i].nodeType == 1) return true;
    }

    return false;
}


function viewQuestions(categoryId) {
    _category_id = categoryId;
    $('.sidebar').slideUp(), $('.category-form-div').hide(), $('#CategorySearchFormDiv').show(), $('#question-btn-div').show(), $('#category-btn-div').hide();
    $('#pills-questions-tab').tab('show');
}
//buttons added to grid

function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" data-placement="top" title="Edit Question" onclick="openQuestionForm('${data.id}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" data-placement="top" title="Delete Question" onclick="deleteQuestion('${data.id}','${data.categoryId}')"><i class="far fa-trash-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" data-placement="top" title="View Details" onclick="viewDetails('${data.id}')"><i class="fas fa-clipboard-list"></i></button>`
        + `</div>`;

    return _buttons;
}


function loadDropDownData() {
    $.ajax({
        url: '/Question/GetCategoriesDropdown/',
        type: 'POST',
        success: function (response) {
            populateDropdown('#category-search', response.data);
            populateDropdown('#category-list', response.data);
            populateDropdown('#category-list-csv', response.data);
        },
        error: function (resposne) {
        },
        complete: function () {
            if (_category_id) {
                $('#category-search').val(_category_id).change();               
            }
        }
    });
}


function viewDetails(id) {
    var data = _questions_data.filter(x => x.id == id)[0];
    var message = '<div class="text-left">'
        + '<pre>Question:<br> ' + data.question1 + '</pre><hr/>'
        + '<ol type="A">'
        + '<li> <pre>' + data.optionA + '</pre></li>'
        + '<li> <pre>' + data.optionB + '</pre></li>'
        + '<li> <pre>' + data.optionC + '</pre></li>'
        + '<li> <pre>' + data.optionD + '</pre></li>'
        + '</ol>'
        + '<p class="text-success">Correct Answer: ' + bindAnswersKey(data) + '</p>'
        + '</div>';

    SportaUtil.MessageBox(message,
        {
            
            type: 'info',
            heading: 'Details',
            modalSize: 'modal-xl'
        }
    )
}


function populateDropdown(element, data) {
    $(element).empty();
    $(element).selectpicker();
    $.each(data, function (index, item) {
        $(element).append("<option value=" + item.id + ">" + item.name + "</option>");
    });
    $(element).selectpicker('refresh')

}


function closeForm() {
    SportaForms.ResetForm(question_form);
    closeSideBar();
    $('#myVideo')[0].pause();
}


function openQuestionForm(id) {
    SportaForms.ResetForm(question_form);
    if (id) {
        $.get(getQuestionUrl, 'id=' + id, function (response) {
            var question = response.data;
            editor.setData(question.question1);
            $("#txt-id").val(question.id);
            $("#txt-question").val(question.question1);
            $("#txt-OPtionA").val(question.optionA);
            $("#txt-OPtionB").val(question.optionB);
            $("#txt-OPtionC").val(question.optionC);
            $("#txt-OPtionD").val(question.optionD);
            $("#txt-Answer").selectpicker('val', bindAnswersKey(question));
            $("#txt-QuestionLevel").selectpicker('val', question.questionLevel);
            $("#category-list").selectpicker('val', question.categoryId)
            openBulkForm(false)
            SportaForms.InitializeFormStyle(question_form);
            $("#txt-question").focus();
        });
    }
    else {
        editor.setData(null);
        openBulkForm(false)
        SportaForms.InitializeFormStyle(question_form);
        $('#txt-id').val(null);
        $("#txt-question").focus();
        if (_category_id) {
            $('#category-list').val(_category_id).change();           
        }
    }
}


function openBulkForm(isSuccess = true) {
    openSidebar();
    if (isSuccess) {
        $('.sidebar .sidebar-body .single-upload').hide();
        $('.sidebar .sidebar-body .bulk-upload').show();
        $('.sidebar #form-title').text('Bulk Upload Form');
        SportaForms.ResetForm(addExcelFile);
        SportaForms.InitializeFormStyle(addExcelFile);
        if (_category_id) {           
            $('#category-list-csv').val(_category_id).change();
        }
    } else {
        $('.sidebar .sidebar-body .single-upload').show();
        $('.sidebar .sidebar-body .bulk-upload').hide();
        $('.sidebar #form-title').text('Question Form');
    }
}


function addUpdateQuestion(data) {
    var url = data.Id == 0 ? addQuestionUrl : updateQuestionUrl;
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(response.message);
                closeForm();
                $('#category-search').selectpicker('val', response.data);
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }
        },
        error: function (resposne) {

        }
    });
}

$(question_form).unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(question_form, validateForm);
    if (validateForm()) {
        let answer = saveAnswersKey($("#txt-Answer").val())
        let questionLevel = $('#txt-QuestionLevel').val();
        var requestModel = {
            Id: $("#txt-id").val(),
            Question1: $("#txt-question").val(),
            OptionA: $("#txt-OPtionA").val(),
            OptionB: $("#txt-OPtionB").val(),
            OptionC: $("#txt-OPtionC").val(),
            OptionD: $("#txt-OPtionD").val(),
            Answer: answer,
            QuestionLevel: questionLevel,
            CategoryId: $("#category-list").val()
        }
        addUpdateQuestion(requestModel);
    }
});


function saveAnswersKey(answerKey) {
    var answer;
    switch (answerKey) {
        case 'A':
            answer = $("#txt-OPtionA").val();
            break;
        case 'B':
            answer = $("#txt-OPtionB").val();
            break;
        case 'C':
            answer = $("#txt-OPtionC").val();
            break;
        case 'D':
            answer = $("#txt-OPtionD").val();
            break;
    }
    return answer;
}


function bindAnswersKey(questions) {
    var answerKey;
    switch (true) {
        case questions.optionA == questions.answer:
            answerKey = 'A';
            break;
        case questions.optionB === questions.answer:
            answerKey = 'B';
            break;
        case questions.optionC === questions.answer:
            answerKey = 'C';
            break;
        case questions.optionD === questions.answer:
            answerKey = 'D'
            break;
    }
    return answerKey;
}


$("#excelfile").on('change', function (e) {
    selectedFile = event.target.files[0]
});


$('.close').on('click', function () {
    $('#myVideo')[0].pause();
});


$('#addExcelFile').unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    let validFile = true;
    localStorage.clear();
    SportaForms.EnableLiveValidation(addExcelFile, validateBulkForm);
    var categoryId = $("#category-list-csv").val();
    if (validateBulkForm()) {
        if (validateFile($("#excelfile").val().toLowerCase())) {
            $.ajax({
                url: "/Question/GetQuestionNameByCategory?categoryId=" + categoryId,
                type: 'Get',
                success: function (response) {

                    dbQuestions = response.data;
                },
                complete: function () {
                    var excelData = [];
                    if (selectedFile) {
                        let fileReader = new FileReader();
                        fileReader.readAsBinaryString(selectedFile)
                        fileReader.onload = (event) => {
                            let data = event.target.result;
                            let workBook = XLSX.read(data, { type: "binary" });
                            workBook.SheetNames.forEach(sheet => {
                                let row = XLSX.utils.sheet_to_row_object_array(workBook.Sheets[sheet]);
                                if (row.length) {
                                    var question = row.every(e => e.hasOwnProperty('Question')), option_A = row.every(e => e.hasOwnProperty('OptionA'));
                                    var option_B = row.every(e => e.hasOwnProperty('OptionB')), option_C = row.every(e => e.hasOwnProperty('OptionC'));
                                    var option_D = row.every(e => e.hasOwnProperty('OptionD')), answer = row.every(e => e.hasOwnProperty('Answer'));
                                    var QuestionLevel = row.every(e => e.hasOwnProperty('QuestionLevel'));
                                    if (question && option_A && option_B && option_C && option_D && answer && QuestionLevel) {
                                        row = $.grep(row, function (item) {
                                            return !(dbQuestions.filter(function (e) { return e.toLowerCase() == item.Question.toLowerCase() }).length > 0)
                                        });
                                        for (var i = 0; i < row.length; i++) {
                                            var quiz_question = {
                                                categoryId: $("#category-list-csv").val(), //Add dropdown value
                                                Question1: row[i].Question,
                                                OptionA: row[i].OptionA,
                                                OptionB: row[i].OptionB,
                                                OptionC: row[i].OptionC,
                                                OptionD: row[i].OptionD,
                                                Answer: row[i].Answer,
                                                QuestionLevel: row[i].QuestionLevel

                                            }
                                            excelData.push(quiz_question)
                                        }
                                    }
                                    else {
                                        SportaUtil.MessageBoxDanger("Invalid Excel Format OR Some Fields are missing.<br> Please check and try again.");
                                        validFile = false;
                                    }
                                }
                                else {
                                    SportaUtil.MessageBoxDanger("File is empty!");
                                    validFile = false;
                                }
                            })
                            if (validFile) {
                                saveQuestions(excelData, categoryId);
                            }
                        }
                    }
                },
                error: function (resposne) { }
            });
        } else {
            SportaUtil.MessageBoxDanger("Please Upload Excel File Only!");

        }
    }
    else {
        SportaUtil.MessageBoxDanger("All fields are mandatory");
    }
})


function saveQuestions(data, categoryId) {
    if (data.length) {
        $.ajax({
            url: "/Question/AddBulkQuestion",
            type: 'POST',
            data: { 'request': data, 'categoryId': categoryId },

            success: function (response) {
                if (response.isSuccess) {
                    SportaUtil.MessageBoxSuccess(response.message + `<br>${data.length} ${data.length == 1 ? 'question has' : 'questions have'} been added!`);
                    closeForm();
                    loadQuestions();
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message)
                }

            },
            error: function (resposne) {

            }
        });
    }
    else {
        SportaUtil.MessageBoxDanger("All Questions Exists Already");
    }

}


function validateForm() {
    SportaForms.ClearValidataionErrors(question_form);

    var blankChecks = ["#txt-question", "#txt-OPtionB", "#txt-OPtionA", "#txt-OPtionC", "#txt-OPtionD", "#txt-Answer", '#category-list','#txt-QuestionLevel'];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(question_form);
}


function validateBulkForm() {
    SportaForms.ClearValidataionErrors(addExcelFile);

    var blankChecks = ["#txt-question", "#excelfile", "#category-list-csv"];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(addExcelFile);
}


function deleteQuestion(id, categoryId) {

    SportaUtil.ConfirmDialogue(`Are you sure  to delete?`, null, function () {
        $.get(deleteQuestionUrl, { 'Id': id, }, function (response) {
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message)
            loadQuestions();
        });
    });
}


// /* Category Js starts here /*

let catForm_id = '#category-form',
    category_id = '#txt-category-id',
    category_name = '#txt-cat-name',
    category_submit = '#category-submit',
    getAllCategoryUrl = '/Question/GetAllCategories',
    btn_create_category = '#category-btn-div',
    getCategoryUrl = '/Question/GetCategory',
    addCategoryUrl = '/Question/AddCategory',
    updateCategoryUrl = '/Question/UpdateCategory',
    category_card = '#category-card',
    removeCategoryUrl = '/Question/RemoveCategory';


function loadCategories() {
    $.get(getAllCategoryUrl, function (response) {
        $(category_card).html(response)
    });
}


function openCategoryForm(isSuccess = true, id) {

    SportaForms.ResetForm(catForm_id);
    if (isSuccess) {
        $(catForm_id).parent('.category-form-div').show();
        $(btn_create_category).hide();

        if (id) {
            $.get(getCategoryUrl, { 'id': id }, function (response) {
                var data = response.data;
                $(category_id).val(data.id);
                $(category_name).val(data.categoryName);
                $(category_submit).text('Update');
            });

        } else {
            $(category_id).val(0);
            $(category_submit).text('Save');
        }
        SportaForms.InitializeFormStyle(catForm_id)
    } else {
        $(category_id).val(0);
        $(catForm_id).parent('.category-form-div').hide();
        $(btn_create_category).show();
    }

}


$(catForm_id).unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(catForm_id, validateCategory);
    if (validateCategory()) {
        var requestModel = {
            Id: $(category_id).val(),
            CategoryName: $(category_name).val()
        }

        addUpdateCategory(requestModel);
    }
});


$("#category-form-pop").unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation("#category-form-pop", validateCategoryForm);
    if (validateCategoryForm()) {
        var CategoryName = $("#txt-cname").val();
        $.ajax({
            url: '/Question/AddCategory',
            data: { "CategoryName": CategoryName },
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {
                    SportaUtil.MessageBoxSuccess(`${response.message}`);
                    $(".buttons").show();
                    $("#category-form-pop").hide();
                    SportaForms.ResetForm('#category-form-pop');
                    loadDropDownData();
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message)
                }
            },
            error: function (resposne) {

            }
        });
    }
});


function validateCategoryForm() {
    SportaForms.ClearValidataionErrors("#category-form-pop");
    var blankChecks = ["#txt-cname"];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus("#category-form-pop");
}


function validateCategory() {
    SportaForms.ClearValidataionErrors(catForm_id);
    var blankChecks = [category_name];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(catForm_id);
}


function addUpdateCategory(data) {
    var url = data.Id == 0 ? addCategoryUrl : updateCategoryUrl;
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(`${response.message}`);

                openCategoryForm(false);
                loadCategories();
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }
        },
        error: function (resposne) {

        }
    });
}


function categoryIsExist(name) {

    if (cacheCategoryData) {
        var aa = cacheCategoryData.filter(x => x.categoryName == name);
    }
}


function openCategoryPop() {

    $(".buttons").hide();
    $("#category-form-pop").show();
}


function closeCatForm() {

    $(".buttons").show();
    $("#category-form-pop").hide();
    SportaForms.ResetForm('#category-form-pop');

}


function deleteCategory(id) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete?`, null, function () {
        $.get(removeCategoryUrl, { 'id': id, }, function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(response.message);
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }
            loadCategories();
        });
    });
}


function validateFile(file) {
    const re = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
    return re.test(file);
}