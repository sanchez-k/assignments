#!/bin/bash

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile metric1.cs to create the file: metric.dll
mcs -target:library -r:System.Drawing -r:System.Windows.Forms -out:metric.dll metric1.cs

echo Compile driver1.cs and link the one previously created dll file to create an executable file.
mcs -r:System -r:System.Windows.Forms -r:metric.dll -out:metric.exe driver1.cs

echo Run the Metric program.
./metric.exe &

echo The script has terminated.