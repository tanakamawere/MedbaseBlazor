window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text)
            .catch(function (error) {
                alert(error);
            });
    }
};

//Key presses for the admin page
//to save a question
window.addKeyDownEvent = (dotnetHelper) => {
    document.addEventListener("keydown", function (event) {
        if (event.ctrlKey && event.key === 's') {
            event.preventDefault(); // Prevent the default "Save Page" behavior
            dotnetHelper.invokeMethodAsync("OnCtrlSPressed");
        }
    });
};
