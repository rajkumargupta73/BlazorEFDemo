using Microsoft.JSInterop;

namespace BlazorEFDemo.Services
{
    public class JsInteropService
    {
        private readonly IJSRuntime _js;
        public JsInteropService(IJSRuntime js) => _js = js;

        public async Task ShowToastAsync(string msg, string type = "success")
            => await _js.InvokeVoidAsync("showToast", msg, type);

        public async Task<bool> CopyToClipboardAsync(string text)
            => await _js.InvokeAsync<bool>("copyToClipboard", text);

        public async Task<bool> ConfirmAsync(string msg)
            => await _js.InvokeAsync<bool>("confirmDelete", msg);

        public async Task FocusAsync(string id)
            => await _js.InvokeVoidAsync("focusElement", id);

        public async Task ScrollToTopAsync()
            => await _js.InvokeVoidAsync("scrollToTop");

        public async Task LocalStorageSetAsync(string k, string v)
            => await _js.InvokeVoidAsync("localStorageSet", k, v);

        public async Task<string?> LocalStorageGetAsync(string k)
            => await _js.InvokeAsync<string?>("localStorageGet", k);

        public async Task PrintAsync()
            => await _js.InvokeVoidAsync("printPage");

        public async Task DownloadCsvAsync(string filename, string csv)
            => await _js.InvokeVoidAsync("downloadCsv", filename, csv);
    }
}
