//Key presses for the admin page
//to save a question
window.addKeyboardListener = (dotnetHelper) => {
    window.addEventListener('keydown', (e) => {
        if (e.ctrlKey) {
            switch (e.key.toLowerCase()) {
                case 's':
                case 'o':
                case 'i':
                case 'p':
                    e.preventDefault();
                    dotnetHelper.invokeMethodAsync('HandleKeyPress', e.ctrlKey, e.key);
                    break;
            }
        }
    });
};

window.removeKeyboardListener = () => {
    window.removeEventListener('keydown', null);
};