// instapack Script Injection: automagically reference the real hot-reloading script
(function () {
    var body = document.getElementsByTagName('body')[0];

    var target = document.createElement('script');
    target.src = 'http://localhost:28080/ipack.js';
    body.appendChild(target);
})();
