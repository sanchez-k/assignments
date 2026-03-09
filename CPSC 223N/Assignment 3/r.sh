# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# Assignment:  3
# Program:  Ricochet Ball
# Due:  March 8, 2026 @ 11:59pm
# Course:  CPSC223N-1
# Languages: C# & Bash
# Purpose: To animate a ball that ricochets off of a wall. Its speed and
# direction of the ball is determined by the user's input.

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile ricochetballui.cs to create the file: ricochetballui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:ricochetballui.dll ricochetballui.cs

echo Compile ricochetball.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:ricochetballui.dll -out:ricochetball.exe ricochetball.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./ricochetball.exe

echo The script has terminated.