/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  3
    Program:  Ricochet Ball
    Due:  March 8, 2026 @ 11:59pm
    Course:  CPSC223N
    Languages: C# & Bash
    Purpose: To animate a ball that ricochets off of a wall. It's speed and direction 
    of the ball is determined by the user's input.
*/

using System;
using System.Windows.Forms;

public class RicochetBallMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Ricochet Ball program.");
        RicochetBall userinterface = new RicochetBall();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the Ricochet Ball program!");
    }
}