/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  5
    Program:  Ellipse
    Due:  May 6, 2026 @ 11:59pm
    Course:  CPSC223N-1
    Languages: C# & Bash
    Purpose: To animate one ball that travels in the shape of an ellipse.
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class EllipseProgram : Form {
    // Declaring the size of the UI
    private const int formWidth = 1820;
    private const int formHeight = 1000;
    // How big the title panel will be
    private const int titleHeight = 80;
    // How big the ball panel will be
    private const int ballHeight = 650;
    private const int buttonHeight = formHeight - titleHeight - ballHeight;


    // Declaring the data for the labels, textboxes, buttons, and panels
    private Label title = new Label();
    private Label initSpeed = new Label();
    private Label currSpeed = new Label();
    private Label coords = new Label();

    private TextBox enterInitSpeed = new TextBox();
    private TextBox enterCurrSpeed = new TextBox();
    private TextBox enterCoords = new TextBox();

    private Button go = new Button();
    private Button exit = new Button();

    private Panel titlePanel = new Panel();
    private Panel buttonPanel = new Panel();

    private GraphicPanel ballPanel = new GraphicPanel();


    ////////////////////////////////////////////////////////////////////////////////////
    

    // Declaring data about the mouse
    // Size of the ball
    private const double radius = 12.65;

    // where the ball starts each time
    private static double centerInitCoordsX = (formWidth/2 + 600);
    private static double centerInitCoordsY = (ballHeight/2);

    // Speed variables
    // How far should the ball move each time the timer tics
    private double pixelPerTic;

    // How much pixels the ball should move per frame
    private double deltaX;
    private double deltaY;

    // This helps to perfectly draw a circle
    private static double centerCurrCoordsX = centerInitCoordsX;
    private static double centerCurrCoordsY = centerInitCoordsY;
    private static double upperLeftCurrCoordsX;
    private static double upperLeftCurrCoordsY;


    // Declaring data about the clocks
    private static System.Timers.Timer ballClock = new System.Timers.Timer();
    // How often the ball's coords gets updated
    private const double ballClockRate = 57.3;

    // Another timer object
    private static System.Timers.Timer uiRefreshClock = new System.Timers.Timer();
    // This updates how often the ball gets redrawn
    private const double uiRefreshRate = 60.8;

    private static System.Windows.Forms.Timer textClock = new System.Windows.Forms.Timer();

    private int objMargin = 30;
    private int tinyMargin = 10;
    private bool bothClocksStopped = true;
    private static double userMouseSpeed = 0.0;
    private double centerEllipseX = formWidth / 2;
    private double centerEllipseY = ballHeight / 2;
    private static double coordT = 0; 
    private static double curve = 0;
    private static double deltaT = 0;

    public EllipseProgram() {
        // Setting up the UI
        Text = "Traveling on an Ellipse";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        System.Console.WriteLine("The size of the user interface is {0}x{1} pixels.",formWidth,formHeight);
        Size = new Size(formWidth, formHeight);
        CenterToScreen();

        // Setting up the buttons
        go.Size = new Size(200, 45);
        go.Location = new Point(20, this.ClientSize.Height - go.Height - 15);
        go.Text = "Go";
        go.TextAlign = ContentAlignment.MiddleCenter;
        go.Font = new Font("Georgia", 20, FontStyle.Bold);
        go.BackColor = ColorTranslator.FromHtml("#69CF7B");
        go.Enabled = false;
        go.Click += new EventHandler(goClick);
        Controls.Add(go);
        AcceptButton = go;

        exit.Size = new Size(200, 45);
        exit.Location = new Point(this.ClientSize.Width - exit.Width - 20, go.Top);
        exit.Text = "Quit";
        exit.TextAlign = ContentAlignment.MiddleCenter;
        exit.Font = new Font("Georgia", 20, FontStyle.Bold);
        exit.BackColor = ColorTranslator.FromHtml("#CF697B");
        exit.Enabled = true;
        exit.Click += new EventHandler(exitClick);
        Controls.Add(exit);
        CancelButton = exit;
        // ✧ദ്ദി( ˶^ᗜ^˶ )


        title.Size = new Size(780, 50);
        title.Location = new Point((formWidth - title.Width) / 2, (titleHeight / 2) - (title.Height / 2));
        title.Text = "Traveling on an Ellipse by Kassandra Sanchez";
        title.TextAlign = ContentAlignment.MiddleCenter;
        title.Font = new Font("Georgia", 25, FontStyle.Bold);
        title.BackColor = ColorTranslator.FromHtml("#C3B1E1");
        Controls.Add(title);
        // ദ്ദി ˉ͈̀꒳ˉ͈́ )✧


        // Now the rest of the labels & textboxes
        initSpeed.Size = new Size(360, 45);
        initSpeed.Location = new Point(go.Left, titleHeight + ballHeight + 15);
        initSpeed.Text = "Enter initial curve pix/sec";
        initSpeed.TextAlign = ContentAlignment.MiddleCenter;
        initSpeed.Font = new Font("Georgia", 20, FontStyle.Bold);
        initSpeed.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(initSpeed);

        enterInitSpeed.Size = new Size(200, 45);
        enterInitSpeed.Location = new Point(initSpeed.Left + initSpeed.Width / 2 - enterInitSpeed.Width / 2, initSpeed.Bottom + tinyMargin);
        enterInitSpeed.Text = "";
        enterInitSpeed.TextAlign = HorizontalAlignment.Center;
        enterInitSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterInitSpeed.BackColor = Color.White;
        Controls.Add(enterInitSpeed);


        coords.Size = new Size(250, 45);
        coords.Location = new Point(initSpeed.Right + objMargin, initSpeed.Top);
        coords.Text = "Current location";
        coords.TextAlign = ContentAlignment.MiddleCenter;
        coords.Font = new Font("Georgia", 20, FontStyle.Bold);
        coords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(coords);

        enterCoords.Size = new Size(200, 45);
        enterCoords.Location = new Point(coords.Left + coords.Width / 2 - enterCoords.Width / 2, coords.Bottom + tinyMargin);
        enterCoords.Text = $"({centerInitCoordsX:F0}, {centerInitCoordsY:F0})";
        enterCoords.TextAlign = HorizontalAlignment.Center;
        enterCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterCoords.ReadOnly = true;
        enterCoords.BackColor = Color.White;
        Controls.Add(enterCoords);


        currSpeed.Size = new Size(310, 45);
        currSpeed.Location = new Point(coords.Right + objMargin, initSpeed.Top);
        currSpeed.Text = "Current curve pix/sec";
        currSpeed.TextAlign = ContentAlignment.MiddleCenter;
        currSpeed.Font = new Font("Georgia", 20, FontStyle.Bold);
        currSpeed.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(currSpeed);

        enterCurrSpeed.Size = new Size(200, 45);
        enterCurrSpeed.Location = new Point(currSpeed.Left + currSpeed.Width / 2 - enterCurrSpeed.Width / 2, currSpeed.Bottom + tinyMargin);
        enterCurrSpeed.Text = "";
        enterCurrSpeed.TextAlign = HorizontalAlignment.Center;
        enterCurrSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterCurrSpeed.ReadOnly = true;
        enterCurrSpeed.BackColor = Color.White;
        Controls.Add(enterCurrSpeed);
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


        //Prepare the refresh clock.  A button will go this clock ticking.
        uiRefreshClock.Enabled = false;
        uiRefreshClock.Interval = (int)System.Math.Round(1000.0 / uiRefreshRate);
        uiRefreshClock.Elapsed += new ElapsedEventHandler(refreshUI);


        textClock.Interval = 100;
        textClock.Tick += new System.EventHandler(timerText);

        // Prepare the ball clock.  A button will go this clock ticking.
        ballClock.Enabled = false;
        ballClock.Interval = (int)System.Math.Round(1000.0 / ballClockRate);
        ballClock.Elapsed += new ElapsedEventHandler(updateBallCoords);

        enterInitSpeed.TextChanged += textFilled;
    }

    protected void goClick(Object sender, EventArgs events) {
        if (bothClocksStopped) {
            go.Text = "Pause";
            uiRefreshClock.Enabled = true;
            ballClock.Enabled = true;
            textClock.Start();

            // Ensuring that these can't be messed with while the ball is moving
            enterInitSpeed.Enabled = false;
            enterCurrSpeed.Enabled = false;
            enterCoords.Enabled = false;
        } else {
            uiRefreshClock.Enabled = false;
            ballClock.Enabled = false;
            textClock.Stop();
            go.Text = "Go";

            // re-enabling these back
            enterInitSpeed.Enabled = true;
            enterCurrSpeed.Enabled = true;
            enterCoords.Enabled = true;
        }
        bothClocksStopped = !bothClocksStopped;
    }

    private void textFilled(object sender, EventArgs events) {
        enterCoords.Text = $"({centerInitCoordsX:F0}, {centerInitCoordsY:F0})";
        centerCurrCoordsX = centerInitCoordsX;
        centerCurrCoordsY = centerInitCoordsY;
        ballPanel.Invalidate();
        go.Text = "Go";

        if (Double.TryParse(enterInitSpeed.Text, out userMouseSpeed) == false ||
            userMouseSpeed <= 0) {
            go.Enabled = false;
            return;
        } else {
            enterCurrSpeed.Text = userMouseSpeed.ToString();
            go.Enabled = true;
            pixelPerTic = userMouseSpeed/ballClockRate;
            coordT = 0;
        }
    }

    protected void exitClick(Object sender, EventArgs events) {
        Close();
    }

    protected void updateBallCoords(System.Object sender, ElapsedEventArgs events) {
        deltaX = -600 * Math.Sin(coordT);
        deltaY =  250 * Math.Cos(coordT);

        curve = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        deltaT = pixelPerTic / curve;
        coordT -= deltaT;

        centerCurrCoordsX = centerEllipseX + 600 * Math.Cos(coordT);
        centerCurrCoordsY = centerEllipseY + 250 * Math.Sin(coordT);
    }

    protected void refreshUI(System.Object sender, ElapsedEventArgs even) {
        ballPanel.Invalidate();
    }

    protected void timerText(System.Object sender, EventArgs even) {
        enterCoords.Text = $"({centerCurrCoordsX:F0}, {centerCurrCoordsY:F0})";
    }

    public class GraphicPanel : Panel {
        private Pen ellipseTrack = new Pen(ColorTranslator.FromHtml("#FCFCFC"), 2);
        private SolidBrush redBrush;

        public GraphicPanel() {
            redBrush = new SolidBrush(ColorTranslator.FromHtml("#10288C"));
            //899475
        }

        protected override void OnPaint(PaintEventArgs artsy) {
            Graphics graph = artsy.Graphics;

            // these will draw the ellipse of OUR race track
            // pen, x, y, width, height
            /*
              If width = height → it’s a circle
              If width > height → it’s wide / stretched sideways
              If width < height → it’s tall / stretched vertically
            */
            // hardcoding
            graph.DrawEllipse(ellipseTrack, (this.ClientSize.Width/2 - 600), (ballHeight/2 - 250), 1200, 500);
            /*
            // a = 600, b = 250
            The equation of an ellipse is a pair: x(t) = a*cos(t)  and y(t) = b*sin(t)
            Here ‘a’ is a major radius and ‘b’ is a minor radius.  For example, a =420.0 and b = 350.0.
            DrawEllipse(blackpen,500,400,840,700)
            The 840 is 2 times the major radius and the 700 is 2 times the minor radius.
            */

            upperLeftCurrCoordsX = centerCurrCoordsX - radius;
            upperLeftCurrCoordsY = centerCurrCoordsY - radius;
            graph.FillEllipse(redBrush,
                            (int)upperLeftCurrCoordsX,
                            (int)upperLeftCurrCoordsY,
                            (float)(2.0 * radius),
                            (float)(2.0 * radius));

            base.OnPaint(artsy);
        }
    }
}