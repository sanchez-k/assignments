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

    private GraphicPanel ballPanel = new GraphicPanel();

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

    // Where the ball is initially drawn, it's going to be in the center
    private static double ballCenterInitialCoordsX = formWidth / 2;
    private static double ballCenterInitialCoordsY = ballHeight / 2;

    // This helps to perfectly draw a circle
    private double ballCenterCurrCoordsX;
    private double ballCenterCurrCoordsY;
    private static double ballUpperLeftCurrCoordsX;
    private static double ballUpperLeftCurrCoordsY;

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
        title.Size = new Size(610, 50);
        title.Location = new Point((formWidth - title.Width) / 2, (titleHeight / 2) - (title.Height / 2));
        title.Text = "Ricochet Ball by Kassandra Sanchez";
        title.TextAlign = ContentAlignment.MiddleCenter;
        title.Font = new Font("Georgia", 25, FontStyle.Bold);
        title.BackColor = ColorTranslator.FromHtml("#C3B1E1");
        Controls.Add(title);

        speed.Size = new Size(370, 45);
        // im gonna crash out, how tf do you position this????
        speed.Location = new Point(enterSpeed.Location.X - speed.Width, titleHeight + ballHeight + 15);
        speed.Text = "Enter Speed (pixel/second)";
        speed.TextAlign = ContentAlignment.MiddleCenter;
        speed.Font = new Font("Georgia", 20, FontStyle.Bold);
        speed.BackColor = ColorTranslator.FromHtml("#F54927");
        Controls.Add(speed);

        direction.Size = new Size(350, 45);
        // variables wouldve helped
        direction.Location = new Point(this.ClientSize.Width - enterDirection.Width - direction.Width - 150, titleHeight + ballHeight + 15);
        direction.Text = "Enter Direction (degrees)";
        direction.TextAlign = ContentAlignment.MiddleCenter;
        direction.Font = new Font("Georgia", 20, FontStyle.Bold);
        direction.BackColor = ColorTranslator.FromHtml("#F54927");
        Controls.Add(direction);

        coords.Size = new Size(200, 45);
        coords.Location = new Point(60, 60);
        coords.Text = "Coordinates of the center of the ball";
        coords.Font = new Font("Georgia", 20, FontStyle.Bold);
        coords.BackColor = ColorTranslator.FromHtml("#F54927");
        Controls.Add(coords);

        xCoords.Size = new Size(200, 45);
        xCoords.Location = new Point(60, 60);
        xCoords.Text = "X =";
        xCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        xCoords.BackColor = ColorTranslator.FromHtml("#F54927");
        Controls.Add(xCoords);

        yCoords.Size = new Size(200, 45);
        yCoords.Location = new Point(60, 60);
        yCoords.Text = "Y =";
        yCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        yCoords.BackColor = ColorTranslator.FromHtml("#F54927");
        Controls.Add(yCoords);
        // ദി(ᴗ _ᴗ ദി)


        // Setting up the textboxes
        enterSpeed.Size = new Size(200, 45);
        enterSpeed.Location = new Point(this.ClientSize.Width - enterDirection.Width - direction.Width - enterSpeed.Width - 190, titleHeight + ballHeight + 15);
        enterSpeed.Text = "";
        enterSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterSpeed);

        enterDirection.Size = new Size(200, 45);
        enterDirection.Location = new Point(this.ClientSize.Width - enterDirection.Width - 40, titleHeight + ballHeight + 15);
        enterDirection.Text = "";
        enterDirection.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterDirection);

        enterXCoords.Size = new Size(200, 45);
        enterXCoords.Location = new Point(60, 60);
        enterXCoords.Text = "";
        enterXCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterXCoords);

        enterYCoords.Size = new Size(200, 45);
        enterYCoords.Location = new Point(60, 60);
        enterYCoords.Text = "";
        enterYCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterYCoords);
        // ദ്ദി ˉ͈̀꒳ˉ͈́ )✧


        // Setting up the buttons
        initial.Size = new Size(200, 45);
        initial.Location = new Point(40, titleHeight + ballHeight + 15);
        initial.Text = "Initial";
        initial.TextAlign = ContentAlignment.MiddleCenter;
        initial.Font = new Font("Georgia", 20, FontStyle.Bold);
        initial.BackColor = ColorTranslator.FromHtml("#CF7669");
        initial.Enabled = true;
        //initial.Click += new EventHandler(initClick);
        Controls.Add(initial);
        

        start.Size = new Size(200, 45);
        // ClientSize is the actual size of the UI, it doesnt include the title bar & borders
        // button.Height gives me the y coord, which is 40
        start.Location = new Point(40, this.ClientSize.Height - start.Height - 15);
        start.Text = "Start";
        start.TextAlign = ContentAlignment.MiddleCenter;
        start.Font = new Font("Georgia", 20, FontStyle.Bold);
        start.BackColor = ColorTranslator.FromHtml("#69CF7B");
        start.Enabled = false;
        //start.Click += new EventHandler(startClick);
        Controls.Add(start);

        quit.Size = new Size(200, 45);
        // x coords math makes sure that theres a 40 pixel margin
        quit.Location = new Point(this.ClientSize.Width - quit.Width - 40, this.ClientSize.Height - quit.Height - 15);
        quit.Text = "Quit";
        quit.TextAlign = ContentAlignment.MiddleCenter;
        quit.Font = new Font("Georgia", 20, FontStyle.Bold);
        quit.BackColor = ColorTranslator.FromHtml("#CF697B");
        quit.Enabled = true;
        //quit.Click += new EventHandler(quitClick);
        Controls.Add(quit);
        // ✧ദ്ദി( ˶^ᗜ^˶ )


        // Setting up the panels
        titlePanel.Size = new Size(formWidth, titleHeight);
        titlePanel.Location = new Point(0, 0);
        titlePanel.BackColor = ColorTranslator.FromHtml("#C3B1E1");
        Controls.Add(titlePanel);

        ballPanel.Size = new Size(formWidth, ballHeight);
        ballPanel.Location = new Point(0, titleHeight);
        ballPanel.BackColor = ColorTranslator.FromHtml("#807594");
        Controls.Add(ballPanel);

        buttonPanel.Size = new Size(formWidth, buttonHeight);
        // Adding both panel heights so it's positioned below those panels
        buttonPanel.Location = new Point(0, titleHeight + ballHeight);
        buttonPanel.BackColor = ColorTranslator.FromHtml("#453F50");
        Controls.Add(buttonPanel);
        // ദ്ദി˙ ᴗ ˙ )
    }

    // prof didnt need a graphic panel since the whole UI, in ricochet ball, was drawn
    // a graphic panel is needed if i want to confine the drawing and separate the code
    public class GraphicPanel : Panel {
        // maybe change the color???
        private Color ballColor = ColorTranslator.FromHtml("#DFFF82");

        protected override void OnPaint(PaintEventArgs artsy) {
            Graphics graph = artsy.Graphics;

            // Drawing the ball
            ballUpperLeftCurrCoordsX = ballCenterInitialCoordsX - ballRadius;
            ballUpperLeftCurrCoordsY = ballCenterInitialCoordsY - ballRadius;
            Brush colorfulBrush = new SolidBrush(ballColor);
            graph.FillEllipse(colorfulBrush,
                              (int)ballUpperLeftCurrCoordsX,
                              (int)ballUpperLeftCurrCoordsY,
                              (float)(2.0 * ballRadius),
                              (float)(2.0 * ballRadius));
            base.OnPaint(artsy);
        }
    }
}

// size, location, backcolor

// #CF69B8