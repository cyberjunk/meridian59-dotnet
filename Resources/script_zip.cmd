echo off
cls
setlocal

set ZIPPER="C:\Program Files\7-Zip\7z.exe"
set COMMONPARAMS=a -tzip -mx9 -mem=AES256

rem 7zip does not overwrite archives, must delete manually first
del caelum.zip >nul 2>&1
del particles.zip >nul 2>&1
del shader.zip >nul 2>&1
del sky.zip >nul 2>&1

del models\apple.zip >nul 2>&1
del models\barrel.zip >nul 2>&1
del models\barstool.zip >nul 2>&1
del models\brazier.zip >nul 2>&1
del models\chalice.zip >nul 2>&1
del models\dice.zip >nul 2>&1
del models\jug4.zip >nul 2>&1
del models\mushroom.zip >nul 2>&1
del models\news.zip >nul 2>&1
del models\razshrub.zip >nul 2>&1
del models\raztree1.zip >nul 2>&1
del models\table.zip >nul 2>&1
del models\tombcros.zip >nul 2>&1
del models\tombston.zip >nul 2>&1

rem FOLDERS WITH FILES
%ZIPPER% %COMMONPARAMS% -r caelum.zip -w .\caelum
%ZIPPER% %COMMONPARAMS% -r particles.zip -w .\particles
%ZIPPER% %COMMONPARAMS% -r shader.zip -w .\shader
%ZIPPER% %COMMONPARAMS% -r sky.zip -w .\sky

rem FOLDERS WITH SUBFOLDERS
%ZIPPER% %COMMONPARAMS% -r models\apple.zip -w .\models\apple
%ZIPPER% %COMMONPARAMS% -r models\barrel.zip -w .\models\barrel
%ZIPPER% %COMMONPARAMS% -r models\barstool.zip -w .\models\barstool
%ZIPPER% %COMMONPARAMS% -r models\brazier.zip -w .\models\brazier
%ZIPPER% %COMMONPARAMS% -r models\chalice.zip -w .\models\chalice
%ZIPPER% %COMMONPARAMS% -r models\dice.zip -w .\models\dice
%ZIPPER% %COMMONPARAMS% -r models\jug4.zip -w .\models\jug4
%ZIPPER% %COMMONPARAMS% -r models\mushroom.zip -w .\models\mushroom
%ZIPPER% %COMMONPARAMS% -r models\news.zip -w .\models\news
%ZIPPER% %COMMONPARAMS% -r models\razshrub.zip -w .\models\razshrub
%ZIPPER% %COMMONPARAMS% -r models\raztree1.zip -w .\models\raztree1
%ZIPPER% %COMMONPARAMS% -r models\table.zip -w .\models\table
%ZIPPER% %COMMONPARAMS% -r models\tombcros.zip -w .\models\tombcros
%ZIPPER% %COMMONPARAMS% -r models\tombston.zip -w .\models\tombston
