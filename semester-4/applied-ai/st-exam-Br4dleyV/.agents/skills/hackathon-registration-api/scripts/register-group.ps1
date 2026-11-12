param(
    [Parameter(Mandatory = $true)]
    [string]$GroupName,

    [Parameter(Mandatory = $true)]
    [string]$Goal,

    [Parameter(Mandatory = $true)]
    [string]$MembersJson
)

if ([string]::IsNullOrWhiteSpace($GroupName)) {
    throw "GroupName is required."
}

if ([string]::IsNullOrWhiteSpace($Goal)) {
    throw "Goal is required."
}

$members = ($MembersJson | ConvertFrom-Json) | ForEach-Object {
    @{
        firstName = $_.firstName
        lastName = $_.lastName
    }
}

if (($members | Measure-Object).Count -lt 2 -or ($members | Measure-Object).Count -gt 5) {
    throw "Members must contain between 2 and 5 entries."
}

$body = @{
    name = $GroupName
    goal = $Goal
    members = $members
} | ConvertTo-Json -Depth 10

$request = @{
    Method = "Post"
    Uri = "http://localhost:40520/groups"
    ContentType = "application/json"
    Body = $body
}

try {
    $response = Invoke-RestMethod @request
}
catch {
    $errorBody = ""
    if ($_.Exception.Response -and $_.Exception.Response.GetResponseStream()) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $errorBody = $reader.ReadToEnd()
    }

    if (-not [string]::IsNullOrWhiteSpace($errorBody)) {
        Write-Error "Registration failed. API response: $errorBody"
    }
    else {
        Write-Error "Registration failed. $($_.Exception.Message)"
    }

    throw
}

Write-Output ($response | ConvertTo-Json -Depth 10)
