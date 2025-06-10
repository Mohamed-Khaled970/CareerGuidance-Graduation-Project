@echo off
setlocal

:: Check if the correct number of arguments is passed
if "%~1"=="" (
    echo Usage: create_file.bat ^<file_name^> ^<author^>
    exit /b 1
)
if "%~2"=="" (
    echo Usage: create_file.bat ^<file_name^> ^<author^>
    exit /b 1
)

:: Set variables
set "FILE_NAME=%~1"
set "AUTHOR=%~2"
set "DATE_CREATED=%date% %time%"
set "VERSION_INFO=V1.0"
set "DESCRIPTION=Description goes here"
set "DEPENDENCIES=Dependencies go here"
set "CONTRIBUTORS=Contributors go here"
set "LAST_MODIFIED_DATE=%DATE_CREATED%"
set "EXT=%~x1"

:: Create the file and add the appropriate header based on the file extension
if "%EXT%"==".c" (
    (
     echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else if "%EXT%"==".cpp" (
    (
    echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else if "%EXT%"==".py" (
    (
    echo # File Name: %FILE_NAME%
    echo # Author: %AUTHOR%
    echo # Date of Creation: %DATE_CREATED%
    echo # Version Information: %VERSION_INFO%
    echo # Dependencies: %DEPENDENCIES%
    echo # Contributors: %CONTRIBUTORS%
    echo # Last Modified Date: %LAST_MODIFIED_DATE%
    echo # Description: %DESCRIPTION%
    ) > "%FILE_NAME%"
) else if "%EXT%"==".html" (
    (
    echo ^<!--
    echo  File Name: %FILE_NAME%
    echo  Author: %AUTHOR%
    echo  Date of Creation: %DATE_CREATED%
    echo  Version Information: %VERSION_INFO%
    echo  Dependencies: %DEPENDENCIES%
    echo  Contributors: %CONTRIBUTORS%
    echo  Last Modified Date: %LAST_MODIFIED_DATE%
    echo  Description: %DESCRIPTION%
    echo --^>
    ) > "%FILE_NAME%"
) else if "%EXT%"==".jsx" (
    (
    echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else if "%EXT%"==".cs" (
    (
    echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else if "%EXT%"==".css" (
    (
    echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else if "%EXT%"==".js" (
    (
    echo /*
    echo  * File Name: %FILE_NAME%
    echo  * Author: %AUTHOR%
    echo  * Date of Creation: %DATE_CREATED%
    echo  * Version Information: %VERSION_INFO%
    echo  * Dependencies: %DEPENDENCIES%
    echo  * Contributors: %CONTRIBUTORS%
    echo  * Last Modified Date: %LAST_MODIFIED_DATE%
    echo  * Description: %DESCRIPTION%
    echo  */
    ) > "%FILE_NAME%"
) else (
    echo Unsupported file type: %EXT%
    exit /b 1
)

echo File '%FILE_NAME%' created with header comments.

endlocal
exit /b 0
