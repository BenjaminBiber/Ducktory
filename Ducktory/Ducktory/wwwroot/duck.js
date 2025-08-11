function renderImagesOnCanvas(imageUrls, isBehind, filterString) {
    if (!Array.isArray(imageUrls)) {
        console.error("imageUrls ist kein Array:", imageUrls);
        return;
    }

    let canvas = document.getElementById(isBehind ? 'behindCanvas' : 'myCanvas');
    if (!canvas) {
        console.error("Canvas nicht gefunden.");
        return;
    }

    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error("Fehler: getContext('2d') gibt null zurück.");
        return;
    }

    // --- Double Buffer Setup ---
    // Unsichtbares Offscreen-Canvas als Kopie des aktuellen Stands
    const bufferCanvas = document.createElement('canvas');
    const bufferCtx = bufferCanvas.getContext('2d');

    bufferCanvas.width = canvas.width;
    bufferCanvas.height = canvas.height;
    bufferCtx.drawImage(canvas, 0, 0); // bisherigen Inhalt sichern

    // Erst den Buffer zeichnen (damit direkt was angezeigt wird)
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(bufferCanvas, 0, 0);

    // Bilder laden und neu zeichnen
    let loadedCount = 0;
    const images = [];

    imageUrls.forEach((url, index) => {
        const img = new Image();
        img.crossOrigin = 'anonymous';
        img.onload = () => {
            loadedCount++;
            images[index] = img;

            if (index === 0) {
                canvas.width = img.width;
                canvas.height = img.height;

                // Buffer anpassen an neue Größe und wieder aufzeichnen
                bufferCanvas.width = img.width;
                bufferCanvas.height = img.height;
                bufferCtx.drawImage(canvas, 0, 0);
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                ctx.drawImage(bufferCanvas, 0, 0);
            }

            if (loadedCount === imageUrls.length) {
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                ctx.drawImage(bufferCanvas, 0, 0); // alter Stand als Untergrund

                // Jetzt mit Filter neue Bilder drüberlegen
                images.forEach(image => {
                    ctx.filter = filterString || 'none';
                    ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
                });

                ctx.filter = 'none'; // Filter zurücksetzen
            }
        };
        img.onerror = () => console.error(`Fehler beim Laden des Bildes: ${url}`);
        img.src = url;
    });
}



window.RemoveImage = () => {
    const canvas = document.getElementById("myCanvas");
    if (!canvas) {
        console.error("Canvas-Element nicht gefunden!");
        return;
    }
    const ctx = canvas.getContext("2d");
    const img = new Image();
    img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    };
};

window.DownloadImage = () => {
    const canvas = document.getElementById("myCanvas");

    const dataURL = canvas.toDataURL("image/png", 1.0);

    const link = document.createElement("a");
    link.href = dataURL;
    link.download = "Ente.png";

    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
};

window.CopyCanvasToClipboard = async () => {
    const canvas = document.getElementById("myCanvas");
    if (!canvas) {
        console.error("Canvas mit ID 'myCanvas' nicht gefunden.");
        return;
    }

    canvas.toBlob(async (blob) => {
        try {
            const item = new ClipboardItem({ "image/png": blob });
            await navigator.clipboard.write([item]);
            console.log("Canvas-Bild wurde in die Zwischenablage kopiert!");
        } catch (err) {
            console.error("Fehler beim Kopieren in die Zwischenablage:", err);
        }
    }, "image/png");
};

async function getMergedPngBlob(imageUrls, _isBehind /* ignoriert */) {
    if (!Array.isArray(imageUrls)) throw new Error("imageUrls muss ein Array sein.");

    // OffscreenCanvas bevorzugen (falls vorhanden)
    let canvas, ctx, isOffscreen = false;
    if (typeof OffscreenCanvas !== 'undefined') {
        canvas = new OffscreenCanvas(1, 1);
        ctx = canvas.getContext('2d');
        isOffscreen = true;
    } else {
        // Fallback: in-memory <canvas>, aber ohne es ins DOM zu hängen
        canvas = document.createElement('canvas');
        ctx = canvas.getContext('2d');
    }
    if (!ctx) throw new Error("2D-Context nicht verfügbar.");

    // Bilder laden (CORS beachten!)
    const loadImage = (url) => new Promise((resolve, reject) => {
        const img = new Image();
        img.crossOrigin = 'anonymous';
        img.onload = () => resolve(img);
        img.onerror = () => reject(new Error("Fehler beim Laden: " + url));
        img.src = url;
    });

    const images = await Promise.all(imageUrls.map(loadImage));

    // Größe an erstes Bild anpassen
    const w = images[0].naturalWidth || images[0].width;
    const h = images[0].naturalHeight || images[0].height;

    if (isOffscreen) {
        canvas.width = w; canvas.height = h;
    } else {
        canvas.width = w; canvas.height = h;
    }
    ctx.clearRect(0, 0, w, h);

    // In Reihenfolge übereinander zeichnen
    for (const img of images) {
        ctx.drawImage(img, 0, 0, w, h);
    }

    // Blob erzeugen (OffscreenCanvas hat convertToBlob)
    let blob;
    if (isOffscreen && typeof canvas.convertToBlob === 'function') {
        blob = await canvas.convertToBlob({ type: "image/png" });
    } else {
        blob = await new Promise((resolve) => canvas.toBlob(resolve, "image/png"));
    }
    if (!blob) throw new Error("Blob-Erzeugung fehlgeschlagen.");

    return blob; // für Blazor: IJSStreamReference
}