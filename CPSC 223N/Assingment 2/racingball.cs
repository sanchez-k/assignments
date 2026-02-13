/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  2
    Program:  Racing Ball
    Due:  February 22, 2026 @ 11:59pm
    Course:  CPSC223N
    Languages: C# & Bash
*/

using System;
using System.Windows.Forms;

public class RacingBallMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Racing Ball program.");
        RacingBall userinterface = new RacingBall();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the Racing Ball program!");
    }
}