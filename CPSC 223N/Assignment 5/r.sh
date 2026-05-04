# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# Assignment:  5
# Program:  Ellipse
# Due:  May 6, 2026 @ 11:59pm
# Course:  CPSC223N-1
# Languages: C# & Bash
# Purpose: To animate one ball that travels in the shape of an ellipse.

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile ellipseui.cs to create the file: ellipseui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:ellipseui.dll ellipseui.cs

echo Compile ellipse.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:ellipseui.dll -out:ellipse.exe ellipse.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 5 program.
mono ./ellipse.exe

echo The script has terminated.