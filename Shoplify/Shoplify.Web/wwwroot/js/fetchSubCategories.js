function getSubCategories(element) {
    let url = `/SubCategory/GetByCategoryId`;

    let subCategoriesSelectMenu = document.getElementById("subCategorySelectMenu");
    let subCategoryDiv = document.getElementById('subCategoryDiv');

    let selectedCategoryId = element.value;
    url += `?categoryId=${selectedCategoryId}`;

    fetch(url)
        .then(response => response.json())
        .then(subCategories => displaySubCategories(subCategories, subCategoriesSelectMenu, subCategoryDiv))
        .catch(err => console.log(err));
}

function displaySubCategories(subCategories, selectMenu, subCategoryDiv) {
    while (selectMenu.firstChild) {
        selectMenu.removeChild(selectMenu.firstChild);
    }

    let initial = document.createElement("option");

    initial.value = '0';
    initial.text = 'Select SubCategory';
    initial.disabled = true;
    initial.selected = true;

    selectMenu.appendChild(initial);

    for (let subCategory of subCategories) {

        let subCategoryElement = document.createElement("option");
        subCategoryElement.value = subCategory.id;
        subCategoryElement.text = subCategory.name;

        subCategoryElement.style.color = "#777777";

        selectMenu.appendChild(subCategoryElement);
    }

    subCategoryDiv.innerHTML = '';
    subCategoryDiv.appendChild(selectMenu);

    selectMenu.classList.add("nice-select");
    selectMenu.classList.add("current");
    selectMenu.style.color = "#777777";

    selectMenu.style.display = 'block';
}