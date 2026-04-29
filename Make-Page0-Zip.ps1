param(
    [string]$OutZip = "C:\Users\zchal\CODING\SPRING2026\CSE445\Assignment5\Page0.zip"
)

$src = "C:\Users\zchal\CODING\SPRING2026\CSE445\Assignment5"

# Folders to skip entirely
$skipFolders = @('.vs', 'obj', '.git', '.cursor', 'WeatherService', 'EncryptionLib', 'ZipUtilities', 'NaturalHazardsService', 'WcfService1', 'packages', 'TestResults')

# File extensions to skip
$skipExts = @('.suo', '.user', '.pdb', '.csproj', '.sln', '.cache', '.zip')

# Specific filenames to skip
$skipNames = @('.gitignore', 'Make-Page0-Zip.ps1')

if (Test-Path $OutZip) { Remove-Item $OutZip -Force }

Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::Open($OutZip, 'Create')

try {
    $rootLen = ($src.TrimEnd('\') + '\').Length
    Get-ChildItem $src -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($rootLen)

        # Skip junk folders
        foreach ($f in $skipFolders) {
            if ($rel.StartsWith("$f\") -or $rel.StartsWith("$f/")) { return }
        }

        # Skip junk extensions
        if ($skipExts -contains $_.Extension.ToLower()) { return }

        # Skip specific filenames
        if ($skipNames -contains $_.Name) { return }

        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $_.FullName, $rel) | Out-Null
        Write-Host "  + $rel"
    }
} finally {
    $zip.Dispose()
}

$mb = [Math]::Round(((Get-Item $OutZip).Length / 1MB), 2)
Write-Host "`nCreated: $OutZip ($mb MB)"
