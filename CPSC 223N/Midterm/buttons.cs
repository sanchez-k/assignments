/*
    Author: Kassandra Sanchez
    Cwid:  884962788
    Course:  CPSC223N
    Purpose:  Midterm Test
    Today's Date:  March 10, 2026
    Email:  k.sanchez@csu.fullerton.edu
    Languages: C# & Bash
*/

using System;
using System.Windows.Forms;

public class RicochetBallMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Midterm Test program.");
        RicochetBall userinterface = new RicochetBall();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the Midterm Test program!");
    }
}