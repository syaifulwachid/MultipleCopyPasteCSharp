@echo off
echo ========================================
echo Multiple Copy Paste Manager
echo Dibuat oleh: Syaiful Wachid
echo Senior Project Designer: Fiberhome Indonesia
echo Hotkeys: F1=Copy, F2=Paste, F4=Search, F5=Stats, F6=Settings, F7=About
echo ========================================
echo.

echo Restoring NuGet packages...
dotnet restore

echo.
echo Building project with custom icon...
dotnet build --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful! Icon sudah diterapkan pada:
    echo - File EXE hasil build
    echo - Header window aplikasi  
    echo - Tray icon
    echo.
    echo Running application...
    start "" "bin\Release\net6.0-windows\MultipleCopyPaste.exe"
) else (
    echo Build failed!
    pause
    exit /b 1
)

pause 