window.showToast = function (message, type) {
    type = type || 'success';
    var el = document.getElementById('liveToast');
    var msg = document.getElementById('toastMessage');
    if (!el || !msg) return;
    msg.textContent = message;
    el.className = 'toast align-items-center text-bg-' + type + ' border-0';
    new bootstrap.Toast(el, { delay: 3000 }).show();
};
window.copyToClipboard = async function (text) {
    try { await navigator.clipboard.writeText(text); return true; }
    catch (e) { return false; }
};
window.confirmDelete = function (msg) { return confirm(msg); };
window.focusElement = function (id) { var el = document.getElementById(id); if (el) el.focus(); };
window.scrollToTop = function () { window.scrollTo({ top: 0, behavior: 'smooth' }); };
window.localStorageSet = function (k, v) { localStorage.setItem(k, v); };
window.localStorageGet = function (k) { return localStorage.getItem(k); };
window.localStorageRemove = function (k) { localStorage.removeItem(k); };
window.printPage = function () { window.print(); };
window.downloadCsv = function (filename, content) {
    var blob = new Blob([content], { type: 'text/csv;charset=utf-8;' });
    var url = URL.createObjectURL(blob);
    var a = document.createElement('a');
    a.setAttribute('href', url);
    a.setAttribute('download', filename);
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
};
