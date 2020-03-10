


[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$databaseServer = "", 
    [string]$databaseName = "",
    [Parameter(Mandatory=$True)]
    [string]$databaseUser = "",
    [Parameter(Mandatory=$true)] 
    [string]$databasePassword,
    [string]$logFileFolder = "$PSScriptRoot"
    
) 
[string]$logFilePath = $logFileFolder + "\dbInstallLog.txt"
[string]$sqlScriptRoot = "$PSScriptRoot"
[boolean]$newDatabase = $false
 
# Function that executes all scripts in the provided string array, which should contain full file paths to the scripts.
Function RunScripts([string[]]$scripts)
{
    # if function called with empty array, exit function
    if($scripts.Count -eq 0) {
        return
    }
    # Execute each script 
    $scripts | ForEach-Object {
        $timestamp = Get-Date -Format s
        Write-Host [$timestamp] Executing $_ ...

        $query = Get-Content $_ -Raw 
        Invoke-Mysqlcmd -ServerInstance $databaseServer -Database $databaseName -Username $databaseUser -Password $databasePassword -Query $query -QueryTimeout 0 -ErrorAction Stop # -OutputSqlErrors $true
    }
}

try 
{
    # Create the log file, if necessary
    if(!(Test-Path $logFilePath)) {
        New-Item -Path $logFilePath -ItemType file
    }
    # Start logging
    Start-Transcript -Path $logFilePath -Force

    'Importing Module'
    Import-Module ./Mysql.Powershell.dll

    'Executing Query' 
    $sd = Invoke-Mysqlcmd -ServerInstance $databaseServer -Username $databaseUser -Password $databasePassword -Query "SHOW databases;"
    $sd.Database | ForEach-Object {
        "db $_ ..."
	}
}
finally
{
    Stop-Transcript
}