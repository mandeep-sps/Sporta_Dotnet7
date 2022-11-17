// URLs
let get_questions = '/Candidate/GetDriveCategoryQuestions',
    get_user_email = '/ApplicationUser/GetEmail',
    save_result = '/Candidate/SaveTestCategory';


//test question option variables
let question_name = '#question-name',
    option_A_lable = '#option-a-label',
    option_B_lable = '#option-b-label',
    option_C_lable = '#option-c-label',
    option_D_lable = '#option-d-label',

    option_A_input = '#option-a-input',
    option_B_input = '#option-b-input',
    option_C_input = '#option-c-input',
    option_D_input = '#option-d-input',

    btn_next = '#btn-next',
    btn_previous = '#btn-previous';


//object variables
let timer,
    time_left,
    allotted_time,
    test_timer_countdown = '#test-timer-countdown',
    user_email_label = '#emailTxt',
    currentQuestion = 0,
    user_answers = [],
    questions_set,
    is_focused_lost = false,
    is_submitted = false,
    is_callback = false,
    category_id,
    drive_id,
    result_id,
    focused_lost_count = 0;

var env = getEnv();

$(document).ready(function () {
    SportaUtil.MessageBoxWarning('Do not close the full screen mode else your test will be submitted automatically!', 'Warning!', InitializeTest)
});


/// This function is for check Screen is in fullscreen or not
(function () {
    var
        fullScreenApi = {
            supportsFullScreen: false,
            isFullScreen: function () { return false; },
            requestFullScreen: function () { },
            cancelFullScreen: function () { },
            fullScreenEventName: '',
            prefix: ''
        },
        browserPrefixes = 'webkit moz o ms khtml'.split(' ');

    // check for native support
    if (typeof document.cancelFullScreen != 'undefined') {
        fullScreenApi.supportsFullScreen = true;
    } else {
        // check for fullscreen support by vendor prefix
        for (var i = 0, il = browserPrefixes.length; i < il; i++) {
            fullScreenApi.prefix = browserPrefixes[i];
            if (typeof document[fullScreenApi.prefix + 'CancelFullScreen'] != 'undefined') {
                fullScreenApi.supportsFullScreen = true;
                break;
            }
        }
    }

    // update methods to do something useful
    if (fullScreenApi.supportsFullScreen) {
        fullScreenApi.fullScreenEventName = fullScreenApi.prefix + 'fullscreenchange';
        fullScreenApi.isFullScreen = function () {
            switch (this.prefix) {
                case '':
                    return document.fullScreen;
                case 'webkit':
                    return document.webkitIsFullScreen;
                default:
                    return document[this.prefix + 'FullScreen'];
            }
        }
        fullScreenApi.requestFullScreen = function (el) {
            return (this.prefix === '') ? el.requestFullScreen() : el[this.prefix + 'RequestFullScreen']();
        }
        fullScreenApi.cancelFullScreen = function (el) {
            return (this.prefix === '') ? document.cancelFullScreen() : document[this.prefix + 'CancelFullScreen']();
        }
    }
    // jQuery plugin
    if (typeof jQuery != 'undefined') {
        jQuery.fn.requestFullScreen = function () {
            return this.each(function () {
                if (fullScreenApi.supportsFullScreen) {
                    fullScreenApi.requestFullScreen(this);
                }
            });
        };
    }
    // export api
    window.fullScreenApi = fullScreenApi;
})();

$(document.body).on('fullscreenchange', changeFullScreen);


//Exception handling through JS
$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    if (env != 'Development') {
        $(document.body).html(jqxhr.responseText);
    }
});

function InitializeTest() {
    fullScreen();

    InitializeWindowEvents();
    category_id = $('#category-id').val();
    drive_id = $('#drive-id').val();
    result_id = $('#result-id').val();

    $.get(get_user_email, function (response) {
        $(user_email_label).html(response);
    });

    //Get Questions from db here here
    $.get(get_questions, { 'driveId': drive_id, 'categoryId': category_id }, function (response) {
        questions_set = response.data.questions
        allotted_time = response.data.allotedTime
        $(btn_next).focus();
        var count = 1;
        var Progress_bar_html = '';
        for (var i = 0; i < questions_set.length; i++) {

            Progress_bar_html += `
            
            <div class="step">
               <div  class="bullet" id="bullet-${i}">${count}</div> 
            </div>
        `;

            count++;
        }

        $("#stepProgressBar").html(Progress_bar_html);
        initializeTest();



    });
}


function fullScreen() {
    var element = document.body;
    const rfs = element.requestFullscreen || element.webkitRequestFullScreen || element.mozRequestFullScreen || element.msRequestFullscreen;
    rfs.call(element);
}



function changeFullScreen(event) {
    var fs = window.fullScreenApi.isFullScreen();
    if (!fs) {
        terminateTest();
    }
}


function InitializeWindowEvents() {
    $(window).blur(function (e) {
        terminateTest(e);
    }).on('contextmenu', function () {
        return false;
    }).on('keydown', function (e) {
        e.preventDefault();
        return false;
    }).on('dblclick', function (e) {
        e.preventDefault();
        return false;
    }).on('selectstart ', function (e) {
        e.preventDefault();
        return false;
    }).on('mousedown', function (e) {
        e.preventDefault();
        return false;
    });
}


function terminateTest(e) {
    if (is_focused_lost) {
        //submitTest(true)
        e.stopPropagation();
    } else {
        SportaUtil.MessageBoxWarning('Don\'t lose the focus from the test screen. <br/> Next time your test will be submitted!', 'Focus Lost!', fullScreen)
        focused_lost_count++;
        e.stopPropagation();
    }
}


function initializeTest() {
    DisplayQuestion(currentQuestion);
    startCountdown();
}


function startCountdown() {
    let time = allotted_time * 60;
    timer = setInterval(function () {
        let minutes = Math.floor(time / 60);
        let seconds = time % 60;
        let timer_format = minutes + ':' + (seconds ? seconds : '00')
        if (seconds <= 9)
            seconds = '0' + seconds;
        if (minutes < 10)
            timer_format = "0" + minutes + ':' + seconds;

        if (minutes <= 0) {
            timer_format = '00:' + seconds;
            $(test_timer_countdown).removeClass('text-white').addClass('text-danger')
        }
        if (seconds == 0) {
            if (minutes == 0)
                timeOut();
            else
                minutes--;
        }
        $(test_timer_countdown).html(timer_format);
        time--;
        time_left = time;
    }, 1000);
}


function DisplayQuestion(questionIndex) {
    let question = questions_set[questionIndex];
    var questionForshuffle = [question.optionA, question.optionB, question.optionC, question.optionD];
    $("#bullet-" + questionIndex).removeClass('bg-success border-success bg-danger border-danger').addClass('border-primary');

    $(question_name).html('<h5><pre class="text-white text-wrap">' + "Q." + (questionIndex + 1) + '&nbsp;  ' + question.question1 + '</pre></h5>')

    //For radio label
    //$(option_A_lable).html(`<pre class="text-white mt-1"> ${question.optionA} </pre>`)
    for (var i = 0; i < questionForshuffle.length; i++) {
        var j = i + Math.floor(Math.random() * (questionForshuffle.length - i));
        var temp = questionForshuffle[j];
        questionForshuffle[j] = questionForshuffle[i];
        questionForshuffle[i] = temp;

    }

    $(option_A_lable).html(questionForshuffle[0])
    $(option_B_lable).html(questionForshuffle[1])
    $(option_C_lable).html(questionForshuffle[2])
    $(option_D_lable).html(questionForshuffle[3])

    //For radio input
    $(option_A_input).val(questionForshuffle[0])
    $(option_B_input).val(questionForshuffle[1])
    $(option_C_input).val(questionForshuffle[2])
    $(option_D_input).val(questionForshuffle[3])


    var isAttemped = user_answers.find(x => { return x.id == questions_set[currentQuestion].id })
    if (isAttemped) {
        $('input[name="options"][value="' + isAttemped.selected + '"]').prop('checked', true);
    }
}


function timeOut() {
    clearInterval(timer);
    SportaUtil.MessageBoxInfo('Time out!. Click Ok to submit the test.', 'Time Over', function () {
        submitTest(true);
    })
}


function NextQuestion() {
    $("#bullet-" + currentQuestion).removeClass('border-primary');
    var selected_option = $('input[name="options"]:checked').val();
    $(btn_previous).show();
    user_answers = user_answers.filter(x => { return x.id != questions_set[currentQuestion].id });

    if (selected_option) {

        var correctAnswer = {
            id: questions_set[currentQuestion].id,
            question1: questions_set[currentQuestion].question1,
            optionB: questions_set[currentQuestion].optionA,
            optionC: questions_set[currentQuestion].optionB,
            optionD: questions_set[currentQuestion].optionC,
            optionD: questions_set[currentQuestion].optionD,
            selected: selected_option,
            isCorrect: selected_option == questions_set[currentQuestion].answer

        }
        user_answers.push(correctAnswer);
        $('input[name="options"]').prop('checked', false);
        $("#bullet-" + currentQuestion).removeClass('not-selected').addClass('selected');
    }
    else
        $("#bullet-" + currentQuestion).addClass('not-selected').removeClass('selected');

    if (currentQuestion < questions_set.length - 1) {
        currentQuestion++;
        if (currentQuestion == questions_set.length - 1) {
            $(btn_next).html('Submit')
            $(btn_next).removeClass('btn-theme').addClass('btn-success')
        }
        else {
            $(btn_next).html('Next')
            $(btn_next).removeClass('btn-success').addClass('btn-theme')
        }
        DisplayQuestion(currentQuestion);

    }
    else {
        submitTest()
    }
}



function PreviousQuestion() {
    $("#bullet-" + currentQuestion).removeClass('border-primary');

    var selected_option = $('input[name="options"]:checked').val();
    user_answers = user_answers.filter(x => { return x.id != questions_set[currentQuestion].id });

    if (selected_option) {
        var correctAnswer = {
            id: questions_set[currentQuestion].id,
            question1: questions_set[currentQuestion].question1,
            optionB: questions_set[currentQuestion].optionA,
            optionC: questions_set[currentQuestion].optionB,
            optionD: questions_set[currentQuestion].optionC,
            optionD: questions_set[currentQuestion].optionD,
            selected: selected_option,
            isCorrect: selected_option == questions_set[currentQuestion].answer
        }
        user_answers.push(correctAnswer);
        $('input[name="options"]').prop('checked', false);
        $("#bullet-" + currentQuestion).removeClass('not-selected').addClass('selected');
    }
    else
        $("#bullet-" + currentQuestion).addClass('not-selected').removeClass('selected');

    $(btn_next).html('Next')
    $(btn_next).removeClass('btn-success').addClass('btn-theme')
    if (currentQuestion >= 0) {
        currentQuestion--;
        DisplayQuestion(currentQuestion);

        if (currentQuestion == 0)
            $(btn_previous).hide();
    }
}


function submitTest(isTimeout) {
    is_submitted = true;
    if (!isTimeout) {
        var confirmation_message = user_answers.length == questions_set.length ? 'Your test will be submitted and can not be retake.<br/> Do you want to submit now?' :
            'You have some unattemped question/s. Do you still wish to proceed with submission? it can not be retake.'

        SportaUtil.ConfirmDialogue(confirmation_message, null, function () {
            saveResult();
        });
    }
    else
        saveResult();
}


function saveResult() {
    if (!is_callback) {
        is_callback = true;
        $.ajax({
            url: save_result,
            data: {
                request: {
                    DriveId: drive_id,
                    ResultId: result_id,
                    CategoryId: category_id,
                    TotalQuestions: questions_set.length,
                    Attemped: user_answers.length,
                    TimeTaken: timeTaken(time_left),
                    FocusLost: focused_lost_count,
                    Score: ($.grep(user_answers, function (item) { return item.isCorrect })).length
                }
            },
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {
                    window.opener.closeTestWindow()
                    window.close();
                }
            },
        });
    }
}




function logout() {
    SportaUtil.ConfirmDialogue("Do you want to logout? This will submit this test can not be undone.", null, function () {
        location.href = '/ApplicationUser/logout';
    });
}


function timeTaken(timeLeft) {
    return ((allotted_time * 60) - 1) - timeLeft
}
