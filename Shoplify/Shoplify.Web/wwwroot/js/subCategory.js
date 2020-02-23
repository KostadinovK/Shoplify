function showSubCategories(rootElement) {
    let list = rootElement.getElementsByClassName("collapse")[0];
    list.classList.add("show");
}

function hideSubCategories(rootElement) {
    let list = rootElement.getElementsByClassName("collapse")[0];
    list.classList.remove("show");
}