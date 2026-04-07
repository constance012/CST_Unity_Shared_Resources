@echo off

setlocal enabledelayedexpansion

REM --- Get the directory of this script ---
set "SCRIPT_DIR=%~dp0"

REM Remove trailing backslash if present
if "%SCRIPT_DIR:~-1%"=="\" set "SCRIPT_DIR=%SCRIPT_DIR:~0,-1%"

echo Please pass in your project's name, wrap in double quotes if needed, example: %~nx0 "Project Name"
set /p PROJECT_NAME=Enter the project name: 

REM === Trim surrounding quotes (if user pasted with them) ===
set "PROJECT_NAME=%PROJECT_NAME:"=%"

REM --- Path Configurations (relative to the script's directory) ---
set "TARGET_FOLDER=%SCRIPT_DIR%\..\%PROJECT_NAME%\Assets"
set "LINK_NAME=0_CstSharedResources"

for %%F in ("%SCRIPT_DIR%") do set "SCRIPT_FOLDER_NAME=%%~nxF"
set "LINK_TARGET=..\..\!SCRIPT_FOLDER_NAME!\%LINK_NAME%"

REM --- Change to the target folder ---
cd /d "%TARGET_FOLDER%"
if errorlevel 1 (
    echo [Error] Failed to cd into %TARGET_FOLDER%! The path might not exist.
    exit /b 1
)

REM --- Remove old symbolic link if it exists ---
if exist "%LINK_NAME%" (
    echo [Status] Removing old symbolic link: "%LINK_NAME%"...
    rd /s /q "%LINK_NAME%"
    del /f /q "%LINK_NAME%"
)

REM --- Create new symbolic directory link ---
echo Creating new symbolic link: %LINK_NAME% -> %LINK_TARGET%
mklink /d "%LINK_NAME%" "%LINK_TARGET%"
if errorlevel 1 (
    echo [Error] Failed to create symbolic link
) else (
    echo [Success] Symbolic link created successfully!
)

:PAUSE_AND_EXIT
echo.
pause
exit /b