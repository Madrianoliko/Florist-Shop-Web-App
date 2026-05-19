// Funkcje pomocnicze do kadrowania zdjęć — używane przez BouquetTemplateForm i RentalItemForm.
// Wymagają Cropper.js załadowanego wcześniej (App.razor).

let _kwCropper = null;

/**
 * Inicjuje Cropper.js na elemencie <img id="imgId"> z podanym src (data URL).
 * aspectRatio: liczba (np. 3/4) lub NaN/pominięty = wolny format.
 */
window.initCropper = function (imgId, src, aspectRatio) {
    const img = document.getElementById(imgId);
    if (!img) return;

    if (_kwCropper) {
        _kwCropper.destroy();
        _kwCropper = null;
    }

    img.src = src;

    img.onload = function () {
        _kwCropper = new Cropper(img, {
            aspectRatio: (aspectRatio !== undefined && aspectRatio !== null) ? aspectRatio : NaN,
            viewMode: 2,
            dragMode: 'move',
            autoCropArea: 0.85,
            restore: false,
            guides: true,
            center: true,
            highlight: false,
            cropBoxMovable: true,
            cropBoxResizable: true,
            toggleDragModeOnDblclick: false,
        });
    };
};

/**
 * Zwraca skropowane zdjęcie jako data URL (JPEG 88%).
 * Maksymalna rozdzielczość 1600×1600 — wystarczy dla katalogu kwiaciarni.
 */
window.getCroppedDataUrl = function () {
    if (!_kwCropper) return null;
    // 1000×1000 max — wystarczy dla katalogu kwiaciarni, mniejszy base64 = mniej obciążenia SignalR.
    // 900×1200 = proporcje 3:4, wystarczająca jakość dla katalogu kwiaciarni
    return _kwCropper.getCroppedCanvas({
        width: 900,
        height: 1200,
        imageSmoothingEnabled: true,
        imageSmoothingQuality: 'high',
    }).toDataURL('image/jpeg', 0.85);
};

/**
 * Niszczy instancję croppera (wywołaj przy zamknięciu modala).
 */
window.destroyCropper = function () {
    if (_kwCropper) {
        _kwCropper.destroy();
        _kwCropper = null;
    }
};
