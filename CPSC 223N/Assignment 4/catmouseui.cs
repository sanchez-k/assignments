/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Assignment:  4
    Program:  Cat and Mouse
    Due:  March 30, 2026 @ 11:59pm
    Course:  CPSC223N
    Languages: C# & Bash
    Purpose: To animate a ball that moves around a racetrack, with its movement speed determined by the user's input.
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class CatMouse : Form {
    // Declaring the size of the UI
    private const int formWidth = 1820;
    private const int formHeight = 1000;
    // How big the title panel will be
    private const int titleHeight = 80;
    // How big the ball panel will be
    private const int ballHeight = 650;
    // Doing this makes it easier to read and recalculate, if the UI's size gets changed
    // Also no hard coding
    private const int buttonHeight = formHeight - titleHeight - ballHeight;

    // Declaring the data for the labels/ textboxes, buttons, and panels
    private Label title = new Label();
    private Label mouseSpeed = new Label();
    private Label catSpeed = new Label();
    private Label mouseCoords = new Label();
    private Label catCoords = new Label();
    private Label distance = new Label();
    private Label mouseDirection = new Label();

    private TextBox enterMouseSpeed = new TextBox();
    private TextBox enterCatSpeed = new TextBox();
    private TextBox enterMouseCoords = new TextBox();
    private TextBox enterCatCoords = new TextBox();
    private TextBox enterDistance = new TextBox();
    private TextBox enterMouseDirection = new TextBox();

    private Button start = new Button();
    private Button quit = new Button();

    private Panel titlePanel = new Panel();
    private Panel buttonPanel = new Panel();

    private GraphicPanel ballPanel = new GraphicPanel();

    ////////////////////////////////////////////////////////////////////////////////////
    
    // Declaring data about the mouse
    // Size of the ball
    private const double mouseRadius = 12.65;

    // where the ball starts each time
    private static double mouseCenterInitialCoordsX = formWidth - formWidth / 4;
    private static double mouseCenterInitialCoordsY = ballHeight / 2;

    // Speed variables
    // How far should the ball move each time the timer tics
    private double pixelPerTic;

    // How much pixels the ball should move per frame
    private double mouseDeltaX;
    private double mouseDeltaY;

    // This helps to perfectly draw a circle
    private static double mouseCenterCurrCoordsX = mouseCenterInitialCoordsX;
    private static double mouseCenterCurrCoordsY = mouseCenterInitialCoordsY;
    private static double mouseUpperLeftCurrCoordsX;
    private static double mouseUpperLeftCurrCoordsY;

    // Declaring data about the blue ball
    private const double catRadius = 16.65;

    private static double catCenterInitialCoordsX = formWidth / 4;
    private static double catCenterInitialCoordsY = ballHeight / 2;

    private double catPixelPerTic;

    private double catDeltaX;
    private double catDeltaY;

    private static double catCenterCurrCoordsX = catCenterInitialCoordsX;
    private static double catCenterCurrCoordsY = catCenterInitialCoordsY;
    private static double catUpperLeftCurrCoordsX;
    private static double catUpperLeftCurrCoordsY;


    // Declaring data about the clocks
    // Creates a timer object
    private static System.Timers.Timer ballClock = new System.Timers.Timer();
    // How often the ball's coords gets updated
    // So it updates 60.5 times per second which is around 0.0165 seconds, itll add the deltas
    private const double ballClockRate = 57.3;  //Units are Hz

    // Another timer object
    private static System.Timers.Timer uiRefreshClock = new System.Timers.Timer();
    // This updates how often the ball gets redrawn
    // Nums for both clocks should be diff because it can create a timing artifact
    private const double uiRefreshRate = 60.8;  //Units are Hz = #refreshes per second

    private int objMargin = 40;
    private int tinyMargin = 10;
    private bool bothClocksStopped = true;
    private static double userMouseSpeed = 0.0;
    private static double userCatSpeed = 0.0;
    private static int num = 0;
    private static bool ballsColliding = false;
    private int hits = 0;
    // all one line
    //Math.Pow(catDeltaX - mouseDeltaX, 2) + Math.Pow(catDeltaY - mouseDeltaY, 2)
    private static double distanceFormula = Math.Sqrt(Math.Pow(catCenterCurrCoordsX - mouseCenterCurrCoordsX, 2) + Math.Pow(catCenterCurrCoordsY - mouseCenterCurrCoordsY, 2));

    public CatMouse() {
        // Setting up the UI
        Text = "Cat and Mouse";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        System.Console.WriteLine("The size of the user interface is {0}x{1} pixels.",formWidth,formHeight);
        Size = new Size(formWidth, formHeight);
        CenterToScreen();

        // Setting up the buttons
        start.Size = new Size(200, 45);
        start.Location = new Point(40, titleHeight + ballHeight + 15);
        start.Text = "Start";
        start.TextAlign = ContentAlignment.MiddleCenter;
        start.Font = new Font("Georgia", 20, FontStyle.Bold);
        start.BackColor = ColorTranslator.FromHtml("#69CF7B");
        start.Enabled = false;
        start.Click += new EventHandler(startClick);
        Controls.Add(start);
        AcceptButton = start;

        quit.Size = new Size(200, 45);
        // start.Top uses start's Y value
        quit.Location = new Point(this.ClientSize.Width - quit.Width - 40, this.ClientSize.Height - quit.Height - 15);
        quit.Text = "Quit";
        quit.TextAlign = ContentAlignment.MiddleCenter;
        quit.Font = new Font("Georgia", 20, FontStyle.Bold);
        quit.BackColor = ColorTranslator.FromHtml("#CF697B");
        quit.Enabled = true;
        quit.Click += new EventHandler(quitClick);
        Controls.Add(quit);
        CancelButton = quit;
        // ✧ദ്ദി( ˶^ᗜ^˶ )


        title.Size = new Size(610, 50);
        title.Location = new Point((formWidth - title.Width) / 2, (titleHeight / 2) - (title.Height / 2));
        title.Text = "Cat and Mouse by Kassandra Sanchez";
        title.TextAlign = ContentAlignment.MiddleCenter;
        title.Font = new Font("Georgia", 25, FontStyle.Bold);
        title.BackColor = ColorTranslator.FromHtml("#C3B1E1");
        Controls.Add(title);
        // ദ്ദി ˉ͈̀꒳ˉ͈́ )✧


        // Now the rest of the labels & textboxes
        catSpeed.Size = new Size(290, 45);
        catSpeed.Location = new Point(start.Right + objMargin, start.Top - 5);
        catSpeed.Text = "Enter Cat Speed (p/s)";
        catSpeed.TextAlign = ContentAlignment.MiddleCenter;
        catSpeed.Font = new Font("Georgia", 20, FontStyle.Bold);
        catSpeed.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(catSpeed);

        mouseSpeed.Size = new Size(330, 45);
        mouseSpeed.Location = new Point(catSpeed.Right + objMargin, catSpeed.Top);
        mouseSpeed.Text = "Enter Mouse Speed (p/s)";
        mouseSpeed.TextAlign = ContentAlignment.MiddleCenter;
        mouseSpeed.Font = new Font("Georgia", 20, FontStyle.Bold);
        mouseSpeed.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(mouseSpeed);


        enterCatSpeed.Size = new Size(200, 45);
        enterCatSpeed.Location = new Point(catSpeed.Left + catSpeed.Width / 2 - enterCatSpeed.Width / 2, catSpeed.Bottom + tinyMargin);
        enterCatSpeed.Text = "";
        enterCatSpeed.TextAlign = HorizontalAlignment.Center;
        enterCatSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterCatSpeed.BackColor = Color.White;
        Controls.Add(enterCatSpeed);

        enterMouseSpeed.Size = new Size(200, 45);
        enterMouseSpeed.Location = new Point(mouseSpeed.Left + mouseSpeed.Width / 2 - enterMouseSpeed.Width / 2, mouseSpeed.Bottom + tinyMargin);
        enterMouseSpeed.Text = "";
        enterMouseSpeed.TextAlign = HorizontalAlignment.Center;
        enterMouseSpeed.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterMouseSpeed.BackColor = Color.White;
        Controls.Add(enterMouseSpeed);

        enterCatCoords.Size = new Size(200, 45);
        enterCatCoords.Location = new Point(enterCatSpeed.Left, quit.Top + 5);
        enterCatCoords.Text = $"({catCenterInitialCoordsX}, {catCenterInitialCoordsY})";
        enterCatCoords.TextAlign = HorizontalAlignment.Center;
        enterCatCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterCatCoords.ReadOnly = true;
        enterCatCoords.BackColor = Color.White;
        Controls.Add(enterCatCoords);

        enterMouseCoords.Size = new Size(200, 45);
        enterMouseCoords.Location = new Point(enterMouseSpeed.Left, enterCatCoords.Top);
        enterMouseCoords.Text = $"({mouseCenterInitialCoordsX}, {mouseCenterInitialCoordsY})";
        enterMouseCoords.TextAlign = HorizontalAlignment.Center;
        enterMouseCoords.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterMouseCoords.ReadOnly = true;
        enterMouseCoords.BackColor = Color.White;
        Controls.Add(enterMouseCoords);

        catCoords.Size = new Size(220, 45);
        catCoords.Location = new Point(catSpeed.Left + catSpeed.Width / 2 - catCoords.Width / 2, enterCatCoords.Top - catCoords.Height - tinyMargin);
        catCoords.Text = "Cat Location";
        catCoords.TextAlign = ContentAlignment.MiddleCenter;
        catCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        catCoords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(catCoords);

        mouseCoords.Size = new Size(240, 45);
        mouseCoords.Location = new Point(mouseSpeed.Left + mouseSpeed.Width / 2 - mouseCoords.Width / 2, catCoords.Top);
        mouseCoords.Text = "Mouse Location";
        mouseCoords.TextAlign = ContentAlignment.MiddleCenter;
        mouseCoords.Font = new Font("Georgia", 20, FontStyle.Bold);
        mouseCoords.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(mouseCoords);



        
        mouseDirection.Size = new Size(390, 45);
        mouseDirection.Location = new Point(mouseSpeed.Right + objMargin, mouseSpeed.Top);
        mouseDirection.Text = "Enter mouse initial direction";
        mouseDirection.TextAlign = ContentAlignment.MiddleCenter;
        mouseDirection.Font = new Font("Georgia", 20, FontStyle.Bold);
        mouseDirection.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(mouseDirection);

        enterMouseDirection.Size = new Size(200, 45);
        enterMouseDirection.Location = new Point(mouseDirection.Left + mouseDirection.Width / 2 - enterMouseDirection.Width / 2, mouseSpeed.Bottom + tinyMargin);
        enterMouseDirection.Text = "";
        enterMouseDirection.TextAlign = HorizontalAlignment.Center;
        enterMouseDirection.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterMouseDirection.BackColor = Color.White;
        Controls.Add(enterMouseDirection);


        enterDistance.Size = new Size(200, 45);
        enterDistance.Location = new Point(enterMouseDirection.Left, enterMouseCoords.Top);
        enterDistance.Text = $"{distanceFormula:F2}";;
        enterDistance.TextAlign = HorizontalAlignment.Center;
        enterDistance.Font = new Font("Georgia", 20, FontStyle.Regular);
        enterDistance.ReadOnly = true;
        enterDistance.BackColor = Color.White;
        Controls.Add(enterDistance);

        distance.Size = new Size(350, 45);
        distance.Location = new Point(enterDistance.Left + enterDistance.Width / 2 - distance.Width / 2, mouseCoords.Top);
        distance.Text = "Distance between players";
        distance.TextAlign = ContentAlignment.MiddleCenter;
        distance.Font = new Font("Georgia", 20, FontStyle.Bold);
        distance.BackColor = ColorTranslator.FromHtml("#CF8969");
        Controls.Add(distance);
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
        uiRefreshClock.Interval = (int)System.Math.Round(1000.0 / uiRefreshRate);;
        uiRefreshClock.Elapsed += new ElapsedEventHandler(refreshUI);

        //Prepare the ball clock.  A button will start this clock ticking.
        ballClock.Enabled = false;
        // you need the 1000 to make it around 17.09 ms which is almost 60 fps
        ballClock.Interval = (int)System.Math.Round(1000.0 / ballClockRate);
        ballClock.Elapsed += new ElapsedEventHandler(updateBallCoords);

        // this hooks up any text changes to the function textFilled, kinda like a button
        // but you write stuff in it instead
        enterMouseSpeed.TextChanged += textFilled;
        enterCatSpeed.TextChanged += textFilled;
        enterMouseDirection.TextChanged += textFilled;
    }

    protected void startClick(Object sender, EventArgs events) {
        if (bothClocksStopped) {
            start.Text = "Pause";
            uiRefreshClock.Enabled = true;
            ballClock.Enabled = true;

            // Ensuring that these can't be messed with while the ball is moving
            enterMouseSpeed.Enabled = false;
            enterCatSpeed.Enabled = false;
            enterMouseCoords.Enabled = false;
            enterCatCoords.Enabled = false;
            enterMouseDirection.Enabled = false;
            enterDistance.Enabled = false;


            pixelPerTic = userMouseSpeed/ballClockRate;
            catPixelPerTic = userCatSpeed/ballClockRate;
            if (mouseCenterCurrCoordsX == mouseCenterInitialCoordsX &&
                mouseCenterCurrCoordsY == mouseCenterInitialCoordsY &&
                catCenterCurrCoordsX == catCenterInitialCoordsX &&
                catCenterCurrCoordsY == catCenterInitialCoordsY) {
                randoNum(pixelPerTic, catPixelPerTic);
            }
            




        } else {
            uiRefreshClock.Enabled = false;
            ballClock.Enabled = false;
            start.Text = "Resume";

            // re-enabling these back
            enterMouseSpeed.Enabled = true;
            enterCatSpeed.Enabled = true;
            enterMouseCoords.Enabled = true;
            enterCatCoords.Enabled = true;
            enterMouseDirection.Enabled = true;
            enterDistance.Enabled = true;
        }
        bothClocksStopped = !bothClocksStopped;
    }

    private void textFilled(object sender, EventArgs events) {
        double moussee = 0;
        enterMouseCoords.Text = $"({mouseCenterInitialCoordsX}, {mouseCenterInitialCoordsY})";
        enterCatCoords.Text = $"({catCenterInitialCoordsX}, {catCenterInitialCoordsY})";
        mouseCenterCurrCoordsX = mouseCenterInitialCoordsX;
        mouseCenterCurrCoordsY = mouseCenterInitialCoordsY;
        catCenterCurrCoordsX = catCenterInitialCoordsX;
        catCenterCurrCoordsY = catCenterInitialCoordsY;
        distanceFormula = Math.Sqrt(Math.Pow(catCenterCurrCoordsX - mouseCenterCurrCoordsX, 2) + Math.Pow(catCenterCurrCoordsY - mouseCenterCurrCoordsY, 2));
        enterDistance.Text = $"{distanceFormula:F2}";
        start.Text = "Start";
        num = 0;
        ballPanel.ballCollision(num);

        if (Double.TryParse(enterMouseSpeed.Text, out userMouseSpeed) == false ||
            Double.TryParse(enterCatSpeed.Text, out userCatSpeed) == false ||
            Double.TryParse(enterMouseDirection.Text, out moussee) == false ||
            userMouseSpeed < 0 || userCatSpeed < 0) {
            start.Enabled = false;
            return;
        } else {
            start.Enabled = true;
        }
    }

    protected void quitClick(Object sender, EventArgs events) {
        Close();
    }

    protected void updateBallCoords(System.Object sender, ElapsedEventArgs events) {
        mouseCenterCurrCoordsX += mouseDeltaX;
        mouseCenterCurrCoordsY -= mouseDeltaY;
        catCenterCurrCoordsX += catDeltaX;
        catCenterCurrCoordsY += catDeltaY;
        enterMouseCoords.Text = $"({mouseCenterCurrCoordsX:F0}, {mouseCenterCurrCoordsY:F0})";
        enterCatCoords.Text = $"({catCenterCurrCoordsX:F0}, {catCenterCurrCoordsY:F0})";

        distanceFormula = Math.Sqrt(Math.Pow(catCenterCurrCoordsX - mouseCenterCurrCoordsX, 2) + Math.Pow(catCenterCurrCoordsY - mouseCenterCurrCoordsY, 2));
        enterDistance.Text = $"{distanceFormula:F2}";

        // Red ball
        // ricochet if it hits the right wall
        if ((int)System.Math.Round(mouseCenterCurrCoordsX + mouseRadius) >= this.ClientSize.Width) {
            // this makes deltaX a negative
            mouseDeltaX = -mouseDeltaX;
        }

        // ricochet if it hits the left wall
        if ((int)System.Math.Round(mouseCenterCurrCoordsX - mouseRadius) <= 0) {
            // this flips the sign, so it turns it into a positive
            mouseDeltaX = -mouseDeltaX;
        }
        
        // ricochet if it hits the top wall
        if ((int)System.Math.Round(mouseCenterCurrCoordsY - mouseRadius) <= 0) {
            // this flips the sign, so it turns it into a negative
            mouseDeltaY = -mouseDeltaY;
        }

        // ricochet if it hits the bottom wall
        if ((int)System.Math.Round(mouseCenterCurrCoordsY + mouseRadius) >= ballHeight) {
            // this flips the sign, so it turns it into a positive
            mouseDeltaY = -mouseDeltaY;
        }

        // Blue ball
        if ((int)System.Math.Round(catCenterCurrCoordsX + mouseRadius) >= this.ClientSize.Width) {
            catDeltaX = -catDeltaX;
        }

        if ((int)System.Math.Round(catCenterCurrCoordsX - mouseRadius) <= 0) {
            catDeltaX = -catDeltaX;
        }
        
        if ((int)System.Math.Round(catCenterCurrCoordsY - mouseRadius) <= 0) {
            catDeltaY = -catDeltaY;
        }

        if ((int)System.Math.Round(catCenterCurrCoordsY + mouseRadius) >= ballHeight) {
            catDeltaY = -catDeltaY;
        }
        // a function wouldve been nice

        // if the balls collide
        double distanceX = mouseCenterCurrCoordsX - catCenterCurrCoordsX;
        double distanceY = mouseCenterCurrCoordsY - catCenterCurrCoordsY;

        double distanceSquared = distanceX * distanceX + distanceY * distanceY;
        double collisionDistance = 2 * mouseRadius;
        if (distanceSquared <= collisionDistance * collisionDistance) {
            if (!ballsColliding) {
                ballsColliding = true;
                num++;
                ballPanel.ballCollision(num);
            }
        } else {
            ballsColliding = false;
        }
    }

    protected void refreshUI(System.Object sender, ElapsedEventArgs even) {
        ballPanel.Invalidate();
    }

    public class GraphicPanel : Panel {
        private SolidBrush redBrush;
        private SolidBrush blueBrush;

        public GraphicPanel() {
            redBrush = new SolidBrush(Color.Red);
            blueBrush = new SolidBrush(Color.Blue);
        }

        protected override void OnPaint(PaintEventArgs artsy) {
            Graphics graph = artsy.Graphics;

            // Drawing the red ball  
            mouseUpperLeftCurrCoordsX = mouseCenterCurrCoordsX - mouseRadius;
            mouseUpperLeftCurrCoordsY = mouseCenterCurrCoordsY - mouseRadius;
            graph.FillEllipse(redBrush,
                            (int)mouseUpperLeftCurrCoordsX,
                            (int)mouseUpperLeftCurrCoordsY,
                            (float)(2.0 * mouseRadius),
                            (float)(2.0 * mouseRadius));

            // drawing the blue ball
            catUpperLeftCurrCoordsX = catCenterCurrCoordsX - catRadius;
            catUpperLeftCurrCoordsY = catCenterCurrCoordsY - catRadius;
            graph.FillEllipse(blueBrush,
                            (int)catUpperLeftCurrCoordsX,
                            (int)catUpperLeftCurrCoordsY,
                            (float)(2.0 * catRadius),
                            (float)(2.0 * catRadius));

            base.OnPaint(artsy);
        }

        public void ballCollision(int hit) {
            if (hit % 2 != 0) {
                redBrush.Color = Color.Green;
                blueBrush.Color = Color.Orange;
            } else {
                redBrush.Color = Color.Red;
                blueBrush.Color = Color.Blue;
            }
            this.Invalidate();
        }
    }

    protected void randoNum(double redPix, double bluePix) {
        Random rando = new Random();
        double redNum = rando.Next(20, 361);
        double blueNum = rando.Next(20,361);

        mouseDeltaX = pixelPerTic * Math.Cos(redNum * Math.PI/180.0);
        mouseDeltaY = pixelPerTic * Math.Sin(redNum * Math.PI/180.0);
        catDeltaX = catPixelPerTic * Math.Cos(blueNum * Math.PI/180.0);
        catDeltaY = catPixelPerTic * Math.Sin(blueNum * Math.PI/180.0);
    }
}

// enter direction