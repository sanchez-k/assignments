# Author: Kassandra Sanchez
# Cwid:  884962788
# Course:  CPSC223N-1
# Purpose:  Midterm Test
# Today's Date:  March 10, 2026
# Email:  k.sanchez@csu.fullerton.edu
# Languages: C# & Bash

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile buttonsui.cs to create the file: buttonsui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:buttonsui.dll buttonsui.cs

echo Compile buttons.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:buttonsui.dll -out:buttons.exe buttons.cs

echo View the list of files in the current folder
ls -l

echo Run the Midterm program.
mono ./buttons.exe

echo The script has terminated.
