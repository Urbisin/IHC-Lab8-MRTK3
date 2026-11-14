# Ejecutar en PowerShell, dentro de la carpeta del repo (donde esta este archivo).
# Clona MRTK3 (rama mrtk3) al lado de los proyectos. Lo usan Lab "Explorando" y los proyectos Lab1/Lab2.
$ErrorActionPreference = "Stop"
if (-not (Test-Path "MixedRealityToolkit-Unity")) {
    git clone --depth 1 --branch mrtk3 https://github.com/microsoft/MixedRealityToolkit-Unity.git
} else {
    Write-Host "MixedRealityToolkit-Unity ya existe, no se vuelve a clonar."
}
Write-Host ""
Write-Host "Listo. Ahora en Unity Hub: Add > Add project from disk:"
Write-Host "  1) MixedRealityToolkit-Unity\UnityProjects\MRTKDevTemplate   (Lab Explorando)"
Write-Host "  2) MRTKDemo01                                                (Lab 1)"
Write-Host "  3) LectorQR                                                  (Lab 2)"
