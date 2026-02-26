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
    private const int ballHeight = 700;
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

    private int objMargin = 40;
    private int tinyMargin = 10;
    private bool bothClocksStopped = true;
    private bool showBall = false;

    public RicochetBall() {
        // Setting up the UI
        Text = "Ricochet Ball";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        System.Console.WriteLine("The size of the user interface is {0}x{1} pixels.",formWidth,formHeight);
        Size = new Size(formWidth, formHeight);
        CenterToScreen();

        // order matters, since the other stuff depends on the position of the buttons, they go first
        // Setting up the buttons
        initial.Size = new Size(200, 45);
        initial.Location = new Point(40, titleHeight + ballHeight + 15);
        initial.Text = "Reset";
        initial.TextAlign = ContentAlignment.MiddleCenter;
        initial.Font = new Font("Georgia", 20, FontStyle.Bold);
        initial.BackColor = ColorTranslator.FromHtml("#CF7669");
        initial.Enabled = true;
        initial.Click += new EventHandler(initClick);
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
        start.Click += new EventHandler(startClick);
        Controls.Add(start);
        AcceptButton = start;

        quit.Size = new Size(200, 45);
        // x coords math makes sure that theres a 40 pixel margin
        // start.Top uses start's Y value
        quit.Location = new Point(this.ClientSize.Width - quit.Width - 40, start.Top);
        quit.Text = "Quit";
        quit.TextAlign = ContentAlignment.MiddleCenter;
        quit.Font = new Font("Georgia", 20, FontStyle.Bold);
        quit.BackColor = ColorTranslator.FromHtml("#CF697B");
        quit.Enabled = true;
        quit.Click += new EventHandler(quitClick);
        Controls.Add(quit);
        CancelButton = quit;
        // ✧ദ്ദി( ˶^ᗜ^˶ )


        // hardcoding the size man, smh
        title.Size = new Size(610, 50);
        title.Location = new Point((formWidth - title.Width) / 2, (titleHeight / 2) - (title.Height / 2));
        title.Text = "Ricochet Ball by Kassandra Sanchez";
        title.TextAlign = ContentAlignment.MiddleCenter;
        title.Font = new Font("Georgia", 25, FontStyle.Bold);
        title.BackColor = ColorTranslator.FromHtml("#C3B1E1");
        Controls.Add(title);
        // ദ്ദി ˉ͈̀꒳ˉ͈́ )✧


        // Now the rest of the labels & textboxes
        enterDirection.Size = new Size(200, 45);
        enterDirection.Location = new Point(quit.Left, initial.Top);
        enterDirection.Text = "";
        enterDirection.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterDirection);

        direction.Size = new Size(350, 45);
        direction.Location = new Point(enterDirection.Left - direction.Width - tinyMargin, initial.Top);
        direction.Text = "Enter Direction (degrees)";
        direction.TextAlign = ContentAlignment.MiddleCenter;
        direction.Font = new Font("Georgia", 20, FontStyle.Bold);
        direction.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(direction);


        enterSpeed.Size = new Size(200, 45);
        enterSpeed.Location = new Point(this.direction.Left - enterSpeed.Width - objMargin, initial.Top);
        enterSpeed.Text = "";
        enterSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterSpeed);

        speed.Size = new Size(370, 45);
        speed.Location = new Point(enterSpeed.Left - speed.Width - tinyMargin, initial.Top);
        speed.Text = "Enter Speed (pixel/second)";
        speed.TextAlign = ContentAlignment.MiddleCenter;
        speed.Font = new Font("Georgia", 20, FontStyle.Bold);
        speed.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(speed);


        enterYCoords.Size = new Size(200, 45);
        // quit.Left uses quit's X value, the math also makes sure there's a 40 pixel margin
        enterYCoords.Location = new Point(quit.Left - enterYCoords.Width - objMargin, quit.Top);
        enterYCoords.Text = "";
        enterYCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterYCoords);

        yCoords.Size = new Size(50, 45);
        yCoords.Location = new Point(enterYCoords.Left - yCoords.Width - tinyMargin, quit.Top);
        yCoords.Text = "Y = ";
        yCoords.TextAlign = ContentAlignment.MiddleCenter;
        yCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        yCoords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(yCoords);


        enterXCoords.Size = new Size(200, 45);
        enterXCoords.Location = new Point(yCoords.Left - enterXCoords.Width - objMargin, quit.Top);
        enterXCoords.Text = "";
        enterXCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        Controls.Add(enterXCoords);

        xCoords.Size = new Size(50, 45);
        xCoords.Location = new Point(enterXCoords.Left - xCoords.Width - tinyMargin, quit.Top);
        xCoords.Text = "X = ";
        xCoords.TextAlign = ContentAlignment.MiddleCenter;
        xCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        xCoords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(xCoords);


        coords.Size = new Size(500, 45);
        // StartEdge + (EndEdge - StartEdge - ObjectSize) / 2
        coords.Location = new Point(enterYCoords.Right + (xCoords.Left - enterYCoords.Right - coords.Width) / 2, initial.Bottom + (quit.Top - initial.Bottom - coords.Height) / 2);
        coords.Text = "Coordinates of the center of the ball";
        coords.TextAlign = ContentAlignment.MiddleCenter;
        coords.Font = new Font("Georgia", 20, FontStyle.Bold);
        coords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(coords);
        // ദി(ᴗ _ᴗ ദി)


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


        //Prepare the refresh clock.  A button will start this clock ticking.
        uiRefreshClock.Enabled = false;
        uiRefreshClock.Interval = uiRefreshRate;
        //uiRefreshClock.Elapsed += new ElapsedEventHandler(refreshUI);

        //Prepare the ball clock.  A button will start this clock ticking.
        ballClock.Enabled = false;  //Initially this clock is stopped.
        ballClock.Interval = ballClockRate;
        //ballClock.Elapsed += new ElapsedEventHandler(updateBallCoords);
    }

    protected void startClick(Object sender, EventArgs events) {
        // make it so start gets disbled if a textbox was messed with after hitting Initial
        // rn the function has to check if its valid, which is repeating code
        // a helper function wouldve been nice
        double num = 0.0;

        // Prefilling in the textboxes if empty
        if (enterSpeed.Text == "") {
            enterSpeed.Text = "100";
        }
        if (enterDirection.Text == "") {
            enterDirection.Text = "25";
        }
        if (enterXCoords.Text == "") {
            enterXCoords.Text = "100";
        }
        if (enterYCoords.Text == "") {
            enterYCoords.Text = "100";
        }

        if (Double.TryParse(enterSpeed.Text, out num) == false ||
            Double.TryParse(enterDirection.Text, out num) == false ||
            Double.TryParse(enterXCoords.Text, out num) == false ||
            Double.TryParse(enterYCoords.Text, out num) == false) {
            System.Console.WriteLine("Please only put numbers in the textboxes.");
            start.Enabled = false;
            //ballPanel.displayBall(showBall);
            return;
        }

        //ballCenterInitialCoordsX = (Double)enterXCoords.Text;
        Double.TryParse(enterXCoords.Text, out ballCenterInitialCoordsX);
        Double.TryParse(enterYCoords.Text, out ballCenterInitialCoordsY);
        ballPanel.Invalidate();

        if (bothClocksStopped) {
            start.Text = "Pause";
            uiRefreshClock.Enabled = true;
            ballClock.Enabled = true;

            // Ensuring that these can't be messed with while the ball is moving
            enterDirection.Enabled = false;
            enterSpeed.Enabled = false;
            enterXCoords.Enabled = false;
            enterYCoords.Enabled = false;
            initial.Enabled = false;
        } else {
            uiRefreshClock.Enabled = false;
            ballClock.Enabled = false;
            start.Text = "Start";

            // re-enabling these back, maybe disable the start button too?
            enterDirection.Enabled = true;
            enterSpeed.Enabled = true;
            enterXCoords.Enabled = true;
            enterYCoords.Enabled = true;
            initial.Enabled = true;
        }
        bothClocksStopped = !bothClocksStopped;
    }

    protected void initClick(Object sender, EventArgs events) {
        showBall = true;
        enterSpeed.Text = "";
        enterDirection.Text = "";
        enterXCoords.Text = ballCenterInitialCoordsX.ToString();
        enterYCoords.Text = ballCenterInitialCoordsY.ToString();
        ballPanel.displayBall(showBall);
    }

    protected void quitClick(Object sender, EventArgs events) {
        Close();
    }

    // prof didnt need a graphic panel since the whole UI, in ricochet ball, was drawn
    // a graphic panel is needed if i want to confine the drawing and separate the code
    public class GraphicPanel : Panel {
        // maybe change the color???
        private Color ballColor = ColorTranslator.FromHtml("#DFFF82");
        private bool ballShown = false;

        public void displayBall(bool ans) {
            ballShown = ans;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs artsy) {
            Graphics graph = artsy.Graphics;

            // Drawing the ball
            if (ballShown == true) {
                ballUpperLeftCurrCoordsX = ballCenterInitialCoordsX - ballRadius;
                ballUpperLeftCurrCoordsY = ballCenterInitialCoordsY - ballRadius;
                Brush colorfulBrush = new SolidBrush(ballColor);
                graph.FillEllipse(colorfulBrush,
                                (int)ballUpperLeftCurrCoordsX,
                                (int)ballUpperLeftCurrCoordsY,
                                (float)(2.0 * ballRadius),
                                (float)(2.0 * ballRadius));
            }
            base.OnPaint(artsy);
        }
    }
}

// backcolor

// fix location of coords


// #CF69B8
// 69CFA1