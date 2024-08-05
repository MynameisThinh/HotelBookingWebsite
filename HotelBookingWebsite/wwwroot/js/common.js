window.showModal = (dialogId) => document.getElementById(dialogId).showModal();
window.closeModel = (dialogId) => document.getElementById(dialogId).close();
window.closeModal = function (dialogId) {
    console.log(`Attempting to close modal with id: ${dialogId}`);
    const modal = document.getElementById(dialogId);
    if (modal) {
        modal.style.display = 'none';
        console.log(`Modal with id ${dialogId} closed successfully`);
    } else {
        console.error(`Modal with id ${dialogId} not found`);
    }
};