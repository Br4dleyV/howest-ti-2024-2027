import * as API from '../api.js';
import { renderErrorMessage, renderResults } from '../util/helper.js';

function launchAudit(e) {
    e.preventDefault();

    const email = document.querySelector('#email').value;
    const organizationId = document.querySelector('#organization').value;
    const accountIds = Array.from(document.querySelector('#accounts').selectedOptions)
        .map(option => option.value);

    if (!email || !organizationId || accountIds.length === 0) {
        renderErrorMessage('Please fill in all fields and select at least one account.');
        return;
    }

    const payload = {
        "emailAuditor": email,
        "organizationId": organizationId,
        "accountIds": accountIds
    };

    API.fetchData('POST', '/launch-audit', payload)
        .then(response => {
            renderResults(response.report);
        })
        .catch(error => {
            renderErrorMessage(error.message || 'An error occurred while launching the audit.');
        });
}

export { launchAudit }; 