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
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class RicochetBall : Form {
    // Declaring the size of the UI
    private const int formWidth = 1820;
    private const int formHeight = 1000;
    // How big the title panel will be
    private const int titleHeight = 80;
    // How big the ball panel will be
    private const int ballHeight = 760;
    // Doing this makes it easier to read and recalculate, if the UI's size gets changed
    // Also no hard coding
    private const int buttonHeight = formHeight - titleHeight - ballHeight;

    // Declaring the data for the labels/ textboxes, buttons, and panels
    private Label title = new Label();
    private Label speed = new Label();
    private Label direction = new Label();
    private Label coords = new Label();
    private Label xCoords = new Label();
    private Label yCoords = new Label();

    private TextBox enterSpeed = new TextBox();
    private TextBox enterDirection = new TextBox();
    private TextBox enterXCoords = new TextBox();
    private TextBox enterYCoords = new TextBox();

    private Button initial = new Button();
    private Button start = new Button();
    private Button quit = new Button();

    private Panel titlePanel = new Panel();
    private Panel buttonPanel = new Panel();

    private GraphicPanel ballPanel = new Graphic();

    ////////////////////////////////////////////////////////////////////////////////////
    
    // Declaring data about the ball
    // Size of the ball
    private const double ballRadius = 12.65;

    // Speed variables
    // The speed the user gives (pixels per second)
    private double speedPerSec;
    // How far should the ball move each time the timer tics
    private double speedPerTic;

    // Direction variables, so how many pixels it should move to the right/left & up/down
    private double ballDirectionX;
    private double ballDirectionY;

    // How much pixels the ball should move per frame
    private double ballDeltaX;
    private double ballDeltaY;

    // Where the ball is initially drawn, the (double) converts the int to a double
    private const double ballCenterInitialCoordsX = (double)formWidth * 0.65;
    private const double ballCenterInitialCoordsY = (double)ballHeight / 2.0 + titleHeight;

    // This helps to perfectly draw a circle
    private double ballCenterCurrCoordsX;
    private double ballCenterCurrCoordsY;
    private double ballUpperLeftCurrCoordsX;
    private double ballUpperLeftCurrCoordsY;


    // Declaring data about the clocks
    // Creates a timer object
    private static System.Timers.Timer ballClock = new System.Timers.Timer();
    // How often the ball's coords gets updated
    // So it updates 60.5 times per second which is around 0.0165 seconds, itll add the deltas
    private const double ballClockRate = 60.5;  //Units are Hz

    // Another timer object
    private static System.Timers.Timer uiRefreshClock = new System.Timers.Timer();
    // This updates how often the ball gets redrawn
    // Nums for both clocks should be diff because it can create a timing artifact
    private const double uiRefreshRate = 60.3;  //Units are Hz = #refreshes per second

    public RicochetBall() {
        // Setting up the UI
        Text = "Ricochet Ball";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        System.Console.WriteLine("The size of the user interface is {0}x{1} pixels.",formWidth,formHeight);
        Size = new Size(formWidth, formHeight);
        CenterToScreen();

        // Setting the data for the labels
        // hardcoding the size man
        title.Size = new Size(510, 40);
        title.Location = new Point((formWidth - title.Width) / 2, (titleHeight / 2) - (title.Height / 2));
        title.Text = "Ricochet Ball by Kassandra Sanchez";
        title.Font = new Font("Georgia", 25, FontStyle.Bold);
        title.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(title);

        speed.Size = new Size(510, 40);
        speed.Location = new Point(60, 60);
        speed.Text = "Enter Speed (pixel/second)";
        speed.Font = new Font("Georgia", 20, FontStyle.Bold);
        speed.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(speed);

        direction.Size = new Size(510, 40);
        direction.Location = new Point(60, 60);
        direction.Text = "Enter Direction (degrees)";
        direction.Font = new Font("Georgia", 20, FontStyle.Bold);
        direction.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(direction);

        coords.Size = new Size(510, 40);
        coords.Location = new Point(60, 60);
        coords.Text = "Coordinates of the center of the ball";
        coords.Font = new Font("Georgia", 20, FontStyle.Bold);
        coords.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(coords);

        xCoords.Size = new Size(510, 40);
        xCoords.Location = new Point(60, 60);
        xCoords.Text = "X =";
        xCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        xCoords.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(xCoords);

        yCoords.Size = new Size(510, 40);
        yCoords.Location = new Point(60, 60);
        yCoords.Text = "Y =";
        yCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        yCoords.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(yCoords);
        // ദി(ᴗ _ᴗ ദി)


        // Setting up the textboxes
        enterSpeed.Size = new Size(510, 40);
        enterSpeed.Location = new Point(60, 60);
        enterSpeed.Text = "";
        enterSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterSpeed.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(enterSpeed);

        enterDirection.Size = new Size(510, 40);
        enterDirection.Location = new Point(60, 60);
        enterDirection.Text = "";
        enterDirection.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterDirection.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(enterDirection);

        enterXCoords.Size = new Size(510, 40);
        enterXCoords.Location = new Point(60, 60);
        enterXCoords.Text = "";
        enterXCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterXCoords.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(enterXCoords);

        enterYCoords.Size = new Size(510, 40);
        enterYCoords.Location = new Point(60, 60);
        enterYCoords.Text = "";
        enterYCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterYCoords.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(enterYCoords);
        // ദ്ദി ˉ͈̀꒳ˉ͈́ )✧


        // Setting up the buttons
        initial.Size = new Size(510, 40);
        initial.Location = new Point(60, 60);
        initial.Text = "Initial";
        initial.Font = new Font("Georgia", 20, FontStyle.Bold);
        initial.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(initial);

        start.Size = new Size(510, 40);
        start.Location = new Point(60, 60);
        start.Text = "Start";
        start.Font = new Font("Georgia", 20, FontStyle.Bold);
        start.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(start);

        Quit.Size = new Size(510, 40);
        Quit.Location = new Point(60, 60);
        Quit.Text = "Quit";
        Quit.Font = new Font("Georgia", 20, FontStyle.Bold);
        Quit.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(Quit);
        // ✧ദ്ദി( ˶^ᗜ^˶ )


        // Setting up the panels
        titlePanel.Size = new Size(formWidth, titleHeight);
        titlePanel.Location = new Point(0, 0);
        titlePanel.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(titlePanel);

        ballPanel.Size = new Size(formWidth, ballHeight);
        ballPanel.Location = new Point(0, titleHeight);
        ballPanel.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(ballPanel);

        buttonPanel.Size = new Size(formWidth, buttonHeight);
        // Adding both panel heights so it's positioned below those panels
        buttonPanel.Location = new Point(0, titleHeight + ballHeight);
        buttonPanel.BackColor = ColorTranslator.FromHtml("#FCFC");
        Controls.Add(buttonPanel);
        // ദ്ദി˙ ᴗ ˙ )
    }
}

// size, location, backcolor