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
    private Panel buttonPanel = new Panel();

    ////////////////////////////////////////////////////////////////////////////////////

    // new stuff, most of it comes from the 90 Degree program
    private GraphicPanel ballPanel = new GraphicPanel();
    // the points where the ball will travel
    private static Point v1;
    private static Point v2;
    private static Point v3;
    private static Point v4;

    // 24.8 Hertz means the screen will refresh 24.8 times per second (~25 FPS)
    // So it controls how often the UI redraws and how smooth the animation looks
    private const double refreshClockRate = 24.8;  //Hertz = tics per second
    // Controls how many times per second the ball's position updates, which is 57.3 time per sec
    private const double motionClockRate = 57.3;   //Hertz = tics per second
    // The speed of the ball, so how often it moves
    private const double ballLinearSpeed = 98.3;   //pixel per second

    // declares how big the ball will be
    private static double ballRadius = 8.65;
    // half of the current center coordinates of the ball
    private static double ballCenterX;
    // same thing above, just the other half
    private static double ballCenterY;
    // position of the corner to draw the circle on the UI??
    private static double ballCornerX;
    private static double ballCornerY;
    // how much the ball moves in X and Y per clock tick
    private static double horizontalMovement;
    private static double verticalMovement;
    // how many pixels the ball should move in one tick, regardless of direction
    private static double pixelPerTick;

    // Clock stuff
    // Keeps track of where the ball is moving
    // enum lets you create a new data type, inside the {} are the values that Compass has
    enum Compass {North, West, South, East};
    // Creating a variable of type Compass, it's set to west meaning that's where it'll move
    Compass currDirection = Compass.West;
    // how often the ball updates its postion in milliseconds
    private int ballClockInterval = (int)System.Math.Round(1000.0/motionClockRate);
    // the actual timer object that will tick at whatever interval i put
    private static System.Timers.Timer ballClock = new System.Timers.Timer();

    // the second clock
    //Declare the refresh clock.
    // calculates how often the screen should refresh, in milliseconds
    private int refreshClockInterval = (int)System.Math.Round(1000.0/refreshClockRate);
    // another timer that redraws the UI, makes it seem like the ball is moving
    private static System.Timers.Timer uiRefreshClock = new System.Timers.Timer();

    // helpers
    private bool bothClocksStopped = true;
    private int ticCount = 0;

    public RacingBall() {
        // UI size configuration 
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

        ////////////////////////////////////////////////////////////////////////////////////
        
        // Setting up the new stuff minus the ballPanel

        // This sets up the path where the ball will travel
        v1 = new Point(ballPanel.Width-30,30);
        v2 = new Point(30,30);
        v3 = new Point(30,ballPanel.Height-30);
        v4 = new Point(ballPanel.Width-30,ballPanel.Height-30);

        // the balls starting location
        ballCenterX = v1.X;
        ballCenterY = v1.Y;

        //Prepare the refresh clock.  A button will start this clock ticking.
        uiRefreshClock.Enabled = false;  //Initially this clock is stopped.
        uiRefrshuiRefreshClockClock.Interval = refreshClockInterval;
        // idk
        uiRefreshClock.Elapsed += new ElapsedEventHandler(refreshUI);

        //Prepare the ball clock.  A button will start this clock ticking.
        ballClock.Enabled = false;  //Initially this clock is stopped.
        ballClock.Interval = ballClockInterval;
        // idk if i need this
        ballClock.Elapsed += new ElapsedEventHandler(updateBallCoords);

        //Change units of speed of ball
        pixelPerTick = ballLinearSpeed/motionClockRate;  //pixels per tic.

        //Set the values for Δx and Δy for initial direction "West".
        horizontalMovement = - pixelPerTick;
        verticalMovement = 0.0;
    }

    protected void goClick(Object sender, EventArgs events) {
        // needs to go from go to pause, so you can stop the balls movement
        return;
    }

    protected void exitClick(Object sender, EventArgs events) {
        // maybe make it wait a sec
        Close();
    }

    //
    // updateBallCoords

    // refreshUI

    public class GraphicPanel : Panel {
        //
    }

}