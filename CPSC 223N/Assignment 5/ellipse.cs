/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  5
    Program:  Ellipse
    Due:  April 27, 2026 @ 11:59pm
    Course:  CPSC223N-1
    LanguagesTo animate one ball that travels in the shape of an ellipse.
*/

using System;
using System.Windows.Forms;

public class EllipseProgramMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Ellipse program.");
        EllipseProgram userinterface = new EllipseProgram();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the Ellipse program!");
    }
}