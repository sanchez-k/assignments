/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    CPSC 223N Final Exam
    Today's Date:  May 14, 2026
*/

using System;
using System.Windows.Forms;

public class RicochetBallMain {
    static void Main(string[] args) {
        System.Console.WriteLine("Welcome to the Main method of the CPSC 223N Final Exam program.");
        RicochetBall userinterface = new RicochetBall();
        Application.Run(userinterface);
        System.Console.WriteLine("Thank you for using the CPSC 223N Final Exam program!");
    }
}