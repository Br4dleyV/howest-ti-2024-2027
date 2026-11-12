function renderOrganizations(organizations) {
    organizations.forEach(organization => {
        const selectElement = document.querySelector('#organization');
        selectElement.innerHTML += `<option value="${organization.id}">${organization.name}</option>`;
    })
}

function renderAccounts(accounts) {
    accounts.forEach(account => {
        const selectElement = document.querySelector('#accounts');
        selectElement.innerHTML += `<option value="${account.id}">${account.name}</option>`;
    })
}

export { renderOrganizations, renderAccounts };