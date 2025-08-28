function sortableInit(listId, handleClass, dotNetComponent) {
    const container = document.getElementById(listId);
    if (!container) return;

    const sortable = new Sortable(container, {
        draggable: '.draggable-question',
        handle: handleClass,
        animation: 150,
        ghostClass: 'sortable-ghost',
        chosenClass: 'sortable-chosen',
        dragClass: 'sortable-drag',
    });
}

function getDOMQuestionOrder(listId) {
    const container = document.getElementById(listId);
    if (!container) {
        return [];
    }
    
    const orderedIds = Array.from(container.querySelectorAll('.draggable-question'))
        .map(el => el.getAttribute('data-id'))
        
    return orderedIds;
}
