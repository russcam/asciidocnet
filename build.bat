@echo off

REM build 
REM build build [skiptests]
REM build release [version] [skiptests]
REM build version [version] [skiptests]

.paket\paket.bootstrapper.exe 
IF EXIST paket.lock (.paket\paket.exe restore)
IF NOT EXIST paket.lock (.paket\paket.exe install)

SET TARGET="build"
SET VERSION=
SET SKIPTESTS=0

IF /I "%1"=="skiptests" (
	set SKIPTESTS="1"
	SHIFT
)

IF NOT [%1]==[] (set TARGET="%1")

IF /I "%1"=="version" (
	IF NOT [%2]==[] (set VERSION="%2")
	IF /I "%3"=="skiptests" (set SKIPTESTS=1)
	IF /I "%2"=="skiptests" (set SKIPTESTS=1)
)
IF /I "%1"=="release" (
	IF NOT [%2]==[] (set VERSION="%2")
	IF /I "%3"=="skiptests" (set SKIPTESTS=1)
	IF /I "%2"=="skiptests" (set SKIPTESTS=1)
)

ECHO starting build using target=%TARGET% version=%VERSION% skiptests=%SKIPTESTS%
"packages\build\FAKE\tools\Fake.exe" "build\\scripts\\Targets.fsx" "target=%TARGET%" "version=%VERSION%" "skiptests=%SKIPTESTS%"