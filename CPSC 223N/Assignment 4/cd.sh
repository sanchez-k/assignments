# Author: Kassandra Sanchez
# Email:  k.sanchez@csu.fullerton.edu
# Cwid:  884962788
# Assignment:  4
# Program:  Collision Detection
# Due:  April 12, 2026 @ 11:59pm
# Course:  CPSC223N-1
# Languages: C# & Bash
# Purpose: To animate two balls that ricochet independently. The speed & direction of the balls is determined
# by the user's input and the program stops when the balls collide against each other.

#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile collisiondetectionui.cs to create the file: collisiondetectionui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:collisiondetectionui.dll collisiondetectionui.cs

echo Compile collisiondetection.cs and link the previously created dll file to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:collisiondetectionui.dll -out:collisiondetection.exe collisiondetection.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 4 program.
mono ./collisiondetection.exe

echo The script has terminated.