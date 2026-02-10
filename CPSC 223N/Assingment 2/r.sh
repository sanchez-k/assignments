# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# Assignment:  2
# Program:  Racing Ball
# Due:  February 22, 2026 @ 11:59pm
# Course:  CPSC223N
# Languages: C# & Bash

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile racingballui.cs to create the file: racingballui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:racingballui.dll racingballui.cs

echo Compile racingball.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:racingballui.dll -out:racingball.exe racingball.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 2 program.
./racingball.exe

echo The script has terminated.