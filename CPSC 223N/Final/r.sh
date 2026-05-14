# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# CPSC 223N Final Exam
# Today's Date:  May 14, 2026

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile finalui.cs to create the file: finalui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:finalui.dll finalui.cs

echo Compile final.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:finalui.dll -out:final.exe final.cs

echo View the list of files in the current folder
ls -l

echo Run the Midterm program.
mono ./final.exe

echo The script has terminated.