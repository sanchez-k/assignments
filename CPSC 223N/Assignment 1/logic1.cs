/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Course:  CPSC 223N
    Assignment:  1
    Due:  February 8, 2026
    Program:  Imperial to Metric Converter
*/

using System;

// a new class because reasons
public class MetricLogic {
    public double metricConversion(double num) {
        double meters = num * 0.0254;
        return meters;
    }
}