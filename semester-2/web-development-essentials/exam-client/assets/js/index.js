import * as API from './api.js';
import { renderErrorMessage } from './util/helper.js';
import * as FormRenderer from './form-component/renderer.js';
import * as FormHandler from './form-component/handler.js';

function init() {
    API.fetchData('GET', '/organizations')
        .then(data => {
            FormRenderer.renderOrganizations(data.data);
        })
        .catch(error => {
            renderErrorMessage(error.message);
        });

    API.fetchData('GET', '/accounts')
        .then(data => {
            FormRenderer.renderAccounts(data.data);
        })
        .catch(error => {
            renderErrorMessage(error.message);
        });

    document.querySelector('form').addEventListener('submit', FormHandler.launchAudit);
}

init();