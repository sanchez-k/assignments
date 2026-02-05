/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Course:  CPSC 223N
    Assignment:  1
    Due:  February 8, 2026
    Program:  Imperial to Metric Converter
*/
// blah blah

// very essential, contains the building blocks of programming like int, char, etc
using System;
// it'a  GUI class library for making Windows desktop applications. lets you create buttons, text boxes, and etc.
// Makes the program able to have a UI
using System.Windows.Forms;

// every program runs inside a class
public class Metricmain {
    /* Putting static means that the func belongs to the class itself, so you don't need to write
          Metricmain obj = new Metricmain();
       You can instead write this:
          Metricmain.Main("Hi");
    */
    // lets you have args before running the program
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Metric program.");
        // creates a GUI obj in the class Metric, defined in a diff file, called userinterface
        Metric userinterface = new Metric();
        // part of GUI library, opens the UI for the user
        Application.Run(userinterface);
        // exits from UI and prints these statement to the console
        System.Console.WriteLine("Press enter to close the program.");
    }
}