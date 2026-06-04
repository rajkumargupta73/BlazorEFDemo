function showMessage(message) {
    alert(message);
}


function addNumbers(a, b) {
    return a + b;
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text);
} 

async function getMessage() {

    let result =
        await DotNet.invokeMethodAsync(
            'BlazorEFDemo',
            'GetMessage');
     
    alert(result);
}


function saveData(key, value) {
    localStorage.setItem(key, value);
}

function getData(key) {
    return localStorage.getItem(key);
}