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

// Lets you use types from the System namespace like Console.Writeline()
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class RacingBall : Form {
    // Since it's const, the UI will always be 1820x1000
    private const int formWidth = 1820;
    private const int formHeight = 1000;

    private Label title = new Label();
    private Label speed = new Label();

    private TextBox speedSection = new TextBox();

    private Button go = new Button();
    private Button exit = new Button();

    private Panel titlePanel = new Panel();
    private Panel ballPanel = new Panel();
    private Panel buttonPanel = new Panel();

    public RacingBall() {
        // UI configuration 
        // This is another way to prevent the user to resize the window
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        // This doesn't change the size of the UI
        System.Console.WriteLine("The size of the user interface is {0}x{1} pixels.",formWidth,formHeight);
        // Set the initial size of this form
        Size = new Size(formWidth,formHeight);
        CenterToScreen();

        // Setting the text for most of the private data types
        Text = "Racing Ball";
        title.Text = "Racing Ball by Kassandra Sanchez";
        speed.Text = "Enter speed (pixel/sec)";
        speedSection.Text = "";
        go.Text = "Go";
        exit.Text = "Exit";

        // Setting the fonts
        title.Font = new Font("Times New Roman", 25, FontStyle.Bold);

        // Setting the backcolor of the text/panels
        title.BackColor = ColorTranslator.FromHtml("#D3D3D3");
        titlePanel.BackColor = ColorTranslator.FromHtml("#D3D3D3");
        // maybe use this color #36454F and make the ball change to neon colors???
        ballPanel.BackColor = ColorTranslator.FromHtml("#E5E4E2");
        buttonPanel.BackColor = ColorTranslator.FromHtml("#C0C0C0");

        // Setting the size
        title.Size = new Size(510, 40);
        titlePanel.Size = new Size(1820, 80);
        ballPanel.Size = new Size(1820, 760);
        // was 160, changed to 180 then back again
        buttonPanel.Size = new Size(1820, 160);

        // Setting the location
        /* to center it using math use this
           (container width - obj width) / 2
        */
        title.Location = new Point(655, 20);
        titlePanel.Location = new Point(0, 0);
        ballPanel.Location = new Point(0, 80);
        // was 800 when paired with 160
        buttonPanel.Location = new Point(0, 820);

        // Button configuration
        AcceptButton = go;
        CancelButton = exit;
        speedSection.Enabled = true;
        go.Enabled = true;
        exit.Enabled = true;
        go.Click += new EventHandler(goClick);
        exit.Click += new EventHandler(exitClick);

        // Displaying the variables to the UI
        Controls.Add(title);
        Controls.Add(titlePanel);
        Controls.Add(ballPanel);
        Controls.Add(buttonPanel);
    }

    protected void goClick(Object sender, EventArgs events) {
        // needs to go from go to pause, so you can stop the balls movement
        return;
    }

    protected void exitClick(Object sender, EventArgs events) {
        // maybe make it wait a sec
        Close();
    }
}