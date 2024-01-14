@ECHO OFF

ECHO -------------------------------------
ECHO Building client packages
CALL yarn --cwd client/react-bulk-components build
IF %errorlevel% NEQ 0 EXIT /B %errorlevel%

ECHO -------------------------------------
ECHO Copy files
XCOPY /y/i/E client\react-bulk-components\dist\ src\Advanced.CMS.BulkEdit\ClientResources\views\ || Exit /B 1
IF %errorlevel% NEQ 0 EXIT /B %errorlevel%

EXIT /B %ERRORLEVEL%
