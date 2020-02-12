document.body.onload = function() {
    setTimeout(function() {
            var preloader = document.getElementById('preloader-id');
            if (!preloader.classList.contains('done')) {
                preloader.classList.add('done');
            }
        },
        2500);
}