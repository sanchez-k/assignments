/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  4
    Program:  Collision Detection
    Due:  April 12, 2026 @ 11:59pm
    Course:  CPSC223N-1
    Languages: C# & Bash
    Purpose: To animate two balls that ricochet independently. The speed & direction of the balls is determined
    by the user's input and the program stops when the balls collide against each other.
*/

using System;
using System.Windows.Forms;

public class CollisionDetectionMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the Collision Detection program.");
        CollisionDetection userinterface = new CollisionDetection();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the Collision Detection program!");
    }
}