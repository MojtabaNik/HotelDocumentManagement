$(window).load(function(){
    //Welcome Message (not for login page)
    function notify(message, type){
        $.growl({
            message: message
        },{
            type: type,
            allow_dismiss: false,
            label: 'حذف',
            className: 'btn-xs btn-inverse',
            placement: {
                from: 'top',
                align: 'left'
            },
            delay: 2500,
            animate: {
                    enter: 'animated fadeIn',
                    exit: 'animated fadeOut'
            },
            offset: {
                x: 20,
                y: 85
            }
        });
    };
    
    //if (!$('.login-content')[0]) {
    //    notify('مجتبی نیکونژاد به صفحه خود خوش آمدید.', 'inverse');
    //} 
});