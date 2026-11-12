---
name: handle
description: "Handle an attached handwritten hackathon registration form end-to-end and submit valid data to the hackathon registration API."
agent: "agent"
---

# Handle Hackathon Registration Form

Use this workflow when the user asks to handle a handwritten registration image.

## Steps

1. Inspect the attached image.
2. Decide if it is a hackathon registration form.
3. Extract these fields exactly as written:
   - group name
   - goal
   - members with first name and last name
4. Validate before any API call:
   - group name is not empty
   - goal is not empty
   - member count is between 2 and 5
   - each member has both first name and last name
5. Do not invent missing data. If required fields are missing, stop and report what is missing.
6. For valid data, use the `hackathon-registration-api` skill and submit the registration.

## Output Format

Return a concise report in this order:

1. `formDetected`: `yes` or `no`
2. `extractedData`: JSON object
3. `apiCall`: `skipped` or `POST /groups` with response id
4. `confirmation`: one short sentence
