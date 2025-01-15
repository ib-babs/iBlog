import TagSanitizer from './EditorTagSanitizer.js';
window.addEventListener("load", function () {
    const textArea = document.querySelector("textarea"),
        boldBtn = document.querySelector("#bold-btn"),
        italcBtn = document.querySelector("#italics-btn"),
        linkBtn = document.querySelector("#link-btn"),
        imageBtn = document.querySelector("#image-btn"),
        videoBtn = document.querySelector("#video-btn"),
        previewBtn = document.querySelector("#previewBtn"),
        previewPage = document.querySelector("#preview-page"),
        previewPageContainer = document.querySelector("#preview-page-container");


    boldBtn.addEventListener("click", () => {
        textArea.value += "*your text here*";
    });
    italcBtn.addEventListener("click", () => {
        textArea.value += "`your text here`";
    });
    linkBtn.addEventListener("click", () => (textArea.value += "@website-link, text@"));
    imageBtn.addEventListener("click", () => (textArea.value += "[image-link]"));
    videoBtn.addEventListener("click", () => (textArea.value += "+video-link, video-title+"));


    textArea.addEventListener("keyup", () => {
        previewPage.innerText = textArea.value
        TagSanitizer("preview-page")
    })
    previewBtn.addEventListener("click", () => {
        if (previewPageContainer.hasAttribute("hidden")) {
            previewPageContainer.removeAttribute("hidden")
            previewBtn.innerText = "Hide Preview"
        }
        else {
            previewPageContainer.setAttribute("hidden", "hidden")
            previewBtn.innerText = "Preview Blog"
        }
    })
});
