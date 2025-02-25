function renderImagesOnCanvas(imageUrls) {
    if (!Array.isArray(imageUrls)) {
        console.error("imageUrls ist kein Array:", imageUrls);
        return;
    }

    const canvas = document.getElementById('myCanvas');
    if (!canvas) {
        console.error("Canvas mit ID 'myCanvas' nicht gefunden.");
        return;
    }

    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error("Fehler: getContext('2d') gibt null zurück.");
        return;
    }

    ctx.clearRect(0, 0, canvas.width, canvas.height);

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
                ctx.clearRect(0, 0, canvas.width, canvas.height);
            }

            if (loadedCount === imageUrls.length) {
                images.forEach(image => {
                    ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
                });
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

