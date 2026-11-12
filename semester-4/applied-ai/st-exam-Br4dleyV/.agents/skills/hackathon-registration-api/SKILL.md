---
name: hackathon-registration-api
description: "Call the local hackathon registration API on localhost:40520 to create and verify group registrations from extracted handwritten form data. Use for POST /groups and validation checks."
argument-hint: "Register extracted hackathon group data"
---

# Hackathon Registration API Skill

Use this skill after form data has been extracted and validated.

## Preconditions

1. The data includes `name`, `goal`, and `members`.
2. Members contain 2 to 5 entries.
3. Each member contains `firstName` and `lastName`.

If preconditions fail, do not call the API.

## Procedure

1. Check API reachability by running:

```powershell
& ".agents/skills/hackathon-registration-api/scripts/list-groups.ps1"
```

2. Register the group by running:

```powershell
& ".agents/skills/hackathon-registration-api/scripts/register-group.ps1" -GroupName "<group name>" -Goal "<goal>" -MembersJson '<members json array>'
```

`MembersJson` must be a JSON array like:

```json
[
  { "firstName": "Ada", "lastName": "Lovelace" },
  { "firstName": "Alan", "lastName": "Turing" }
]
```

3. If the registration succeeds, optionally verify by listing groups again.

## Reporting Rules

1. Include the payload used for `POST /groups`.
2. Include API result id when available.
3. Keep the final user-facing confirmation concise.
