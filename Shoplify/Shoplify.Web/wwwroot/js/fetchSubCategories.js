function displaySubCategories(element) {
    let url = `/SubCategory/GetByCategoryId`;

    let subCategoriesSelectMenu = document.getElementById("subCategorySelectMenu");

    let selectedCategoryId = element.value;
    url += `?categoryId=${selectedCategoryId}`;

    fetch(url)
        .then(response => response.json())
        .then(subCategories => console.log(subCategories))
        .catch(err => console.log(err));
}