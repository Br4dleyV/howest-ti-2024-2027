$request = @{
    Method = "Get"
    Uri = "http://localhost:40520/groups"
}

$response = Invoke-RestMethod @request

Write-Output ($response | ConvertTo-Json -Depth 10)
