// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const Divmessage = document.querySelector("#hide-message");
window.addEventListener("load", function () {
    if (Divmessage != null) {
        setTimeout(function () {
            Divmessage.style.display = "none";
        }, 2000);
    }
    //Show or hide panel
    function showHidePanel(btnClass, panelClass, hidePanelBtn) {
        for (let i = 0; i < $(btnClass).length; i++) {
            $(btnClass[i]).on("click", function () {
                $(panelClass[i]).show();
            })
        }
        $(hidePanelBtn).on("click", function () {
            panelClass.hide();
        })
    }
    var profilePanel = $(".profile-panel"),
        deletePanel = $(".delete-panel"),
        showPanelBtn = $(".show-profile-edit-form"),
        showDeletePanelBtn = $(".show-delete-panel");

    profilePanel.hide();
    deletePanel.hide();

    showHidePanel(showPanelBtn, profilePanel, $(".hide-profile-panel"));
    showHidePanel(showDeletePanelBtn, deletePanel, $(".hide-delete-panel"));
});
