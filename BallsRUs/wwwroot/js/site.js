// Textareas auto adjust
var textareas = document.querySelectorAll("textarea");
var heightLimit = 200; /* Maximum height: 200px */

textareas.forEach(function (textarea) {
    textarea.addEventListener("input", function () {
        textarea.style.height = ""; /* Reset the height*/
        textarea.style.height = Math.min(textarea.scrollHeight, heightLimit) + "px";
    });
});