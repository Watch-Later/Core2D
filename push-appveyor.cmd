@echo off

if %PLATFORM% == "x86" set NotAnyCPU=true
if %PLATFORM% == "x64" set NotAnyCPU=true

if %NotAnyCPU% == "true" ( 
7z a "Core2D.Avalonia.Skia-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D.Avalonia.Skia\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D.Avalonia.Skia-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"
)

7z a "Core2D-%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"

7z a "Core2D.Avalonia-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D.Avalonia\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D.Avalonia-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"

7z a "Core2D.Avalonia.Cairo-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D.Avalonia.Cairo\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D.Avalonia.Cairo-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"

7z a "Core2D.Avalonia.Direct2D-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D.Avalonia.Direct2D\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D.Avalonia.Direct2D-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"

7z a "Core2D.Wpf-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip" "%APPVEYOR_BUILD_FOLDER%\src\Core2D.Wpf\bin\%PLATFORM%\%CONFIGURATION%\*"
appveyor PushArtifact "Core2D.Wpf-%PLATFORM%.%CONFIGURATION%-%APPVEYOR_BUILD_VERSION%.zip"
