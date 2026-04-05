# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# Assignment:  4
# Program:  Cat and Mouse
# Due:  March 30, 2026 @ 11:59pm
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

echo Compile catmouseui.cs to create the file: catmouseui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:catmouseui.dll catmouseui.cs

echo Compile catmouse.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:catmouseui.dll -out:catmouse.exe catmouse.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 4 program.
mono ./catmouse.exe

echo The script has terminated.