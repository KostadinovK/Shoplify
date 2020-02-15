function displaySubCategories(element) {
    let url = `/SubCategory/GetByCategoryId`;

    let subCategoriesSelectMenu = document.getElementById("subCategorySelectMenu");
    let subCategoryDiv = document.getElementById('subCategoryDiv');

    let selectedCategoryId = element.value;
    url += `?categoryId=${selectedCategoryId}`;

    fetch(url)
        .then(response => response.json())
        .then((subCategories) => {

            while (subCategoriesSelectMenu.firstChild) {
                subCategoriesSelectMenu.removeChild(subCategoriesSelectMenu.firstChild);
            }

            let initial = document.createElement("option");

            initial.value = '0';
            initial.text = 'Select SubCategory';
            initial.disabled = true;
            initial.selected = true;

            subCategoriesSelectMenu.appendChild(initial);

            for (let subCategory of subCategories) {

                let subCategoryElement = document.createElement("option");

                subCategoryElement.value = subCategory.id;
                subCategoryElement.text = subCategory.name;

                subCategoriesSelectMenu.appendChild(subCategoryElement);
            }

            subCategoryDiv.innerHTML = '';
            subCategoryDiv.appendChild(subCategoriesSelectMenu);

            subCategoriesSelectMenu.classList.add("nice-select");
            subCategoriesSelectMenu.classList.add("current");
            subCategoriesSelectMenu.style.color = "#777777";

            subCategoriesSelectMenu.style.display = 'block';
        })
        .catch(err => console.log(err));
}