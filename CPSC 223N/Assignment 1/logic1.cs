/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  1
    Program:  Imperial to Metric Converter
    Due:  February 8, 2026 @ 11:59pm
    Course:  CPSC223N
    Languages: C# & Bash
*/

using System;

// a new class because reasons
public class MetricLogic {
    public double metricConversion(double num) {
        double meters = num * 0.0254;
        return meters;
    }
}