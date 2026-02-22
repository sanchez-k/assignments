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
    // f that, 60 fps!!!11!!1!!
    // So it controls how often the UI redraws and how smooth the animation looks
    private const double refreshClockRate = 60.8;  //Hertz = tics per second
    // Controls how many times per second the ball's position updates, which is 57.3 time per sec
    private const double motionClockRate = 57.3;   //Hertz = tics per second
    // The speed of the ball, so how often it moves
    private const double ballLinearSpeed = 0.0;   //pixel per second

    // declares how big the ball will be
    private static double ballRadius = 12.65;
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
        // this aligns the text to the center
        speed.TextAlign = ContentAlignment.MiddleCenter;
        speedSection.Text = "";
        go.Text = "Go";
        exit.Text = "Exit";

        // Setting the fonts
        title.Font = new Font("Times New Roman", 25, FontStyle.Bold);
        go.Font = new Font("Times New Roman", 20, FontStyle.Bold);
        exit.Font = new Font("Times New Roman", 20, FontStyle.Bold);
        speed.Font = new Font("Times New Roman", 20, FontStyle.Bold);
        speedSection.Font = new Font("Times New Roman", 20, FontStyle.Regular);

        // Setting the backcolor of the text/panels
        title.BackColor = ColorTranslator.FromHtml("#5E798B");
        go.BackColor = ColorTranslator.FromHtml("#A2CF8C");
        exit.BackColor = ColorTranslator.FromHtml("#CFB38C");
        speed.BackColor = ColorTranslator.FromHtml("#CF8CCD");
        titlePanel.BackColor = ColorTranslator.FromHtml("#5E798B");
        ballPanel.BackColor = ColorTranslator.FromHtml("#7596AD");
        buttonPanel.BackColor = ColorTranslator.FromHtml("#8CB4CF");

        // Setting the size
        title.Size = new Size(510, 40);
        go.Size = new Size(160, 60);
        exit.Size = new Size(160, 60);
        speed.Size = new Size(180, 60);
        speedSection.Size = new Size(180, 60);
        titlePanel.Size = new Size(1820, 80);
        ballPanel.Size = new Size(1820, 760);
        buttonPanel.Size = new Size(1820, 160);

        // Setting the location
        /* to center it using math use this
           (container width - obj width) / 2
        */
        title.Location = new Point(655, 20);
        go.Location = new Point(60, 880);
        exit.Location = new Point(1600, 880);
        speed.Location = new Point(270, 880);
        speedSection.Location = new Point(460, 890);
        titlePanel.Location = new Point(0, 0);
        ballPanel.Location = new Point(0, 80);
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
        Controls.Add(go);
        Controls.Add(exit);
        Controls.Add(speed);
        Controls.Add(speedSection);
        Controls.Add(titlePanel);
        Controls.Add(ballPanel);
        Controls.Add(buttonPanel);

        ////////////////////////////////////////////////////////////////////////////////////
        
        // Setting up the new stuff minus the ballPanel

        // This sets up the path where the ball will travel
        int margin = 60;
        v1 = new Point(ballPanel.Width - margin, margin);
        v2 = new Point(margin, margin);
        v3 = new Point(margin, ballPanel.Height - margin);
        v4 = new Point(ballPanel.Width - margin, ballPanel.Height - margin);

        // the balls starting location
        ballCenterX = v1.X;
        ballCenterY = v1.Y;

        //Prepare the refresh clock.  A button will start this clock ticking.
        uiRefreshClock.Enabled = false;  //Initially this clock is stopped.
        uiRefreshClock.Interval = refreshClockInterval;
        // idk, prob not using it
        uiRefreshClock.Elapsed += new ElapsedEventHandler(refreshUI);

        //Prepare the ball clock.  A button will start this clock ticking.
        ballClock.Enabled = false;  //Initially this clock is stopped.
        ballClock.Interval = ballClockInterval;
        // idk if i need this
        ballClock.Elapsed += new ElapsedEventHandler(updateBallCoords);

        //Change units of speed of ball
        // og pixelPerTick = ballLinearSpeed/motionClockRate;
        pixelPerTick = ballLinearSpeed/motionClockRate;  //pixels per tic.

        //Set the values for Δx and Δy for initial direction "West".
        horizontalMovement = - pixelPerTick;
        verticalMovement = 0.0;
    }

    protected void goClick(Object sender, EventArgs events) {
        double num = 0;
        int intNum = 0;
        int done = 0;
        if (speedSection.Text == "") {
            System.Console.WriteLine("Please enter a speed value first.");
            return;
        } else if (Double.TryParse(speedSection.Text, out num) == true) {
            if (num <= 0) {
                System.Console.WriteLine("Please enter a positive number.");
                return;
            } else if (int.TryParse(speedSection.Text, out intNum) == true) {
                System.Console.WriteLine("The number must include a decimal point.");
                return;
            }
        } else {
            System.Console.WriteLine("Please enter a float number.");
            return;
        }

        double sped2 = Convert.ToDouble(speedSection.Text);
        pixelPerTick = sped2/motionClockRate;
        horizontalMovement = - pixelPerTick;
        done++;
        // theres a bug where if you switch pixel speed
        // once it goes down itll veer to the left and then
        // it fixes itself once it's going east
        // but it'll also go backwards too soooooooo

        if (bothClocksStopped) {
            go.Text = "Pause";
            uiRefreshClock.Enabled = true;
            ballClock.Enabled = true;
        } else {
            uiRefreshClock.Enabled = false;
            ballClock.Enabled = false;
            go.Text = "Go";
        }

        // acts like a toggle and flips it from on and off
        bothClocksStopped = !bothClocksStopped;
    }

    protected void exitClick(Object sender, EventArgs events) {
        // maybe make it wait a sec, nah no time for that
        Close();
    }

    // updateBallCoords
    protected void updateBallCoords(System.Object sender, ElapsedEventArgs events) {
        ticCount++;

        switch (currDirection) {
            case Compass.West:
                horizontalMovement = -pixelPerTick;
                verticalMovement = 0;
                break;

            case Compass.South:
                horizontalMovement = 0;
                verticalMovement = +pixelPerTick;
                break;

            case Compass.East:
                horizontalMovement = +pixelPerTick;
                verticalMovement = 0;
                break;

            case Compass.North:
                horizontalMovement = 0;
                verticalMovement = -pixelPerTick;
                break;
        }

        switch(currDirection) {
            case Compass.West:
                ballPanel.changeColor(ColorTranslator.FromHtml("#f000ff"));
                if (ballCenterX + horizontalMovement >= v2.X) {
                    ballCenterX += horizontalMovement;
                    ballCenterY += verticalMovement;
                } else {
                    ballCenterX = v2.X;
                    ballCenterY = v2.Y;
                    horizontalMovement = 0.0;
                    verticalMovement = +pixelPerTick;
                    currDirection = Compass.South;
                }
                break;

            case Compass.South:
                ballPanel.changeColor(ColorTranslator.FromHtml("#74ee15"));
                if (ballCenterY + verticalMovement <= v3.Y) {
                    ballCenterX += horizontalMovement;
                    ballCenterY += verticalMovement;
                } else {
                    ballCenterX = v3.X;
                    ballCenterY = v3.Y;
                    horizontalMovement = +pixelPerTick;
                    verticalMovement = 0.0;
                    currDirection = Compass.East;
                }
                break;

            case Compass.East:
                ballPanel.changeColor(ColorTranslator.FromHtml("#ED864C"));
                if (ballCenterX + horizontalMovement <= v4.X) {
                    ballCenterX += horizontalMovement;
                    ballCenterY += verticalMovement;
                } else {
                    ballCenterX = v4.X;
                    ballCenterY = v4.Y;
                    horizontalMovement = 0.0;
                    verticalMovement = -pixelPerTick;
                    currDirection = Compass.North;
                }
                break;

            case Compass.North:
                ballPanel.changeColor(ColorTranslator.FromHtml("#4deeea"));
                if (ballCenterY + verticalMovement >= v1.Y) {
                    ballCenterX += horizontalMovement;
                    ballCenterY += verticalMovement;
                } else {
                    ballCenterX = v1.X;
                    ballCenterY = v1.Y;
                    horizontalMovement = -pixelPerTick;
                    verticalMovement = 0.0;
                    currDirection = Compass.West;
                }
                return;

            default:
                System.Console.WriteLine("Something went wrong! Try again.");
                break;
        }
    }

    protected void refreshUI(System.Object sender, ElapsedEventArgs even) {
        ballPanel.Invalidate();
    }

    // this actually draws the stuff, we're inheriting from the Panel class
    // but there'll be some custom drawing behavior
    public class GraphicPanel : Panel {
        // Pen is used to draw lines, the line thickness will be 2 pixels
        private Pen coloring = new Pen(ColorTranslator.FromHtml("#FCFCFC"), 2);
        private Color ballColor = ColorTranslator.FromHtml("#f000ff");

        // this will run each time the panel is redrawn so when the refresh clock triggers a redraw
        // we're overriding the default drawing behavior
        protected override void OnPaint(PaintEventArgs artsy) {
            // Graphics is the canvas and artsy.Graphics lets you draw on it
            Graphics graph = artsy.Graphics;

            // these will draw the rectangle OUR race track
            graph.DrawLine(coloring, v1.X, v1.Y, v2.X, v2.Y);
            graph.DrawLine(coloring, v2.X, v2.Y, v3.X, v3.Y);
            graph.DrawLine(coloring, v3.X, v3.Y, v4.X, v4.Y);
            graph.DrawLine(coloring, v4.X, v4.Y, v1.X, v1.Y);

            // coords to help draw the circle
            ballCornerX = ballCenterX - ballRadius;
            ballCornerY = ballCenterY - ballRadius;

            // a unique brush so the color can change
            Brush colorfulBrush = new SolidBrush(ballColor);
            // this draws the circle
            graph.FillEllipse(colorfulBrush,
                              (int)System.Math.Round(ballCornerX),
                              (int)System.Math.Round(ballCornerY),
                              (int)System.Math.Round(2.0 * ballRadius),
                              (int)System.Math.Round(2.0 * ballRadius));

            // this calls the original Panel and ensures the panel finishes its painting
            base.OnPaint(artsy);
        }

        public void changeColor(Color newColour) {
            ballColor = newColour;
        }
    }

}