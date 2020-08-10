<#
    .SYNOPSIS
    Dynamically create a parameters.json file from an arm template

    .DESCRIPTION
    Dynamically create a parameters.json file with values from an arm template and environment variables matching the arm template parameters

    .PARAMETER TemplateFilePath
    File path to the ARM template

    .PARAMETER ParametersFilePath
    File path to store the generated Parameters

    .EXAMPLE
    New-ParametersFile.ps1 -TemplateFilePath "C:\template.json" -ParametersFilePath "C:\template.parameters.json"
#>
[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$TemplateFilePath,
    [Parameter(Mandatory = $true)]
    [String]$ParametersFilePath
)

try {

    $TemplateParameters = (Get-Content -Path $TemplateFilePath -Raw | ConvertFrom-Json).Parameters
    $ParametersFile = [PSCustomObject]@{
        "`$schema"     = "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#"
        contentVersion = "1.0.0.0"
        parameters     = @{ }
    }
    $ParameterObjects = $TemplateParameters.PSObject.Members | Where-Object MemberType -eq NoteProperty

    foreach ($ParameterObject in $ParameterObjects) {
        $ParameterType = $ParameterObject.Value.Type
        $ParameterName = $ParameterObject.Name
        $ParameterValue = (Get-Item -Path "env:$ParameterName" -ErrorAction SilentlyContinue).Value

        if (!$ParameterValue) {
            Write-Verbose -Message "Environment variable for $ParameterName was not found, attempting default value"
            $ParameterValue = $TemplateParameters.$ParameterName.defaultValue

            if ($ParameterValue) {
                Write-Verbose -Message "Using default value for $ParameterName"

                if ($ParameterType -eq "object") {
                    $ParameterValue = $ParameterValue | ConvertTo-Json -Depth 10
                }
            }
        }
        else {
            Write-Verbose -Message "Using environment variable value for $ParameterName"
        }

        # --- Throw if pipeline variable does not exist and if the type is not a secure string and array
        if (!$ParameterValue -and ($ParameterType -ne "securestring") -and ($ParameterType -ne "array")) {
            Write-Verbose -Message "Default value for $ParameterName was not found. Process will terminate"
            throw "Could not find environment variable or default value for template parameter $ParameterName"
        }

        switch ($ParameterType) {
            'array' {
                # If Default value is an empty array
                if (!$ParameterValue) {
                    $ParameterValue = @()
                }
                elseif (($ParameterValue | ConvertFrom-Json | Get-Member)[0].TypeName -eq "System.String") {
                    $ParameterValue = [String[]]($ParameterValue | ConvertFrom-Json)
                }
                else {
                    $HashTable = @{ }
                    (ConvertFrom-Json $ParameterValue).psobject.properties | ForEach-Object { $HashTable[$_.Name] = $_.Value }
                    $ParameterValue = @($HashTable.SyncRoot)
                }

                break
            }
            'bool' {
                # In the case of default values do a type comparison
                if ($ParameterValue -is [Boolean]) {
                    break
                }
                if ($ParameterValue.ToLower() -eq "true") {
                    $ParameterValue = $true
                    break
                }
                if ($ParameterValue.ToLower() -eq "false") {
                    $ParameterValue = $false
                    break
                }
                throw "Not a valid boolean input for $ParameterName"
            }
            'int' {
                $ParameterValue = [Int]$ParameterValue
                break
            }
            'object' {
                $ParameterValue = $ParameterValue | ConvertFrom-Json
                break
            }
        }

        $ParametersFile.parameters.Add($ParameterName, @{ value = $ParameterValue })
    }

    $null = Set-Content -Path $ParametersFilePath -Value ([Regex]::Unescape(($ParametersFile | ConvertTo-Json -Depth 10))) -Force

}
catch {
    throw "$_"
}
