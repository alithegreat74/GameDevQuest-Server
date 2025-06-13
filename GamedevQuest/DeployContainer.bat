@echo off
set /p VERSION=Enter version number 
docker build -t gamedevquest .
docker tag gamedevquest alithegreat74/gamedevquest:%VERSION%
docker push alithegreat74/gamedevquest:%VERSION%
pause
