#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile logic1.cs to create the file: logic1.dll
mcs -target:library -out:logic1.dll logic1.cs

echo Compile metric1.cs to create the file: metric1.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -r:logic1.dll -out:metric1.dll metric1.cs

echo Compile driver1.cs and link the two previously created dll files to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:metric1.dll -r:logic1.dll -out:driver1.exe driver1.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 1 program.
./driver1.exe

echo The script has terminated.