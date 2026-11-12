<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Audit Report</title>
</head>

<body>
    <h1>Audit Report (confidential)</h1>
    <h2>Hello, {{ $audit->email }}, here are the audit results:</h2>
    <ul>
        <li>Success: {{ $audit->success ? 'YES' : 'NO' }}</li>
        <li>Report: {{ $audit->report }}</li>
    </ul>
</body>

</html>