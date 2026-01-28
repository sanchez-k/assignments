// must rewrite using statements as they only apply to the file they are written in
using System;
// deals with graphics like color and such
using System.Drawing;
using System.Windows.Forms;

// it's an inheritance class because of the colon
// the class inherits everything Form has which comes from System.Windows.Forms
public class Metric : Form
{
    // displays read-only text
    private Label title = new Label();
    private Label subtitle = new Label();
    private Label enterSection = new Label();
    private Label midText = new Label();
    // lets the user to input text
    private TextBox inches = new TextBox();
    // creates buttons that the user can click on
    private Button compute = new Button();
    private Button clear = new Button();
    private Button exit = new Button();
    private double inputtedInches = 0.0;

    public Metric() {
        /* names the title part of the UI, you can also write
              this.Text = "";
        */
        Text = "Imperial to Metric Converter";
        // .Text is a property that tells the label/button what words to display on the screen
        title.Text = "Imperial to Metric Converter";
        subtitle.Text = "By Kassandra Sanchez";
        enterSection.Text = "Enter inches:";
        // pls let me modify this later and not create 2 other labels
        midText.Text = "The metric equivalent will be displayed here.";
        inches.Text = "";
        compute.Text = "Compute";
        clear.Text = "Clear";
        exit.Text = "Exit";

        // wipppppppppppppppppppppppppppppppppppppppppppppp
        Size = new Size(500, 500);
        // .Size is a property that tells the label/button how many pixels it is by wxh
        title.Size = new Size();
        subtitle.Size = new Size(200, 60);
        enterSection.Size = new Size(200, 60);
        midText.Size = new Size(200, 60);
        inches.Size = new Size(200, 60);
        compute.Size = new Size(200, 60);
        clear.Size = new Size(200, 60);
        exit.Size = new Size(200, 60);

        // .Location is where the stuff is positioned by x, y
        title.Location = new Point(15, 20);
        subtitle.Location = new Point(15, 20);
        enterSection.Location = new Point(15, 20);
        midText.Location = new Point(15, 20);
        inches.Location = new Point(15, 20);
        compute.Location = new Point(15, 20);
        clear.Location = new Point(15, 20);
        exit.Location = new Point(15, 20);
        // wipppppppppppppppppppppppppppppppppppppppppppp

        // .Enabled determines whether clicking on it executes the algo or if you can type in it
        // so if the button is set to false, clicking on it does nothing. true makes it run the algo
        enterSection.Enabled = false;
        inches.Enabled = true;
        compute.Enabled = true;
        clear.Enabled = true;
        exit.Enabled = true;
        // might need to do some other stuff too

        //add

        // hooks the button up to some code
        // so when compute is clicked it then runs the func called computeClick
        // the EventHandler points to the func like computeClick
        compute.Click += new EventHandler(computeClick);
        clear.Click += new EventHandler(clearClick);
        exit.Click += new EventHandler(exitClick);
    }

    // protected means that only other class members and any class that inherits the class are allowed to use this func
    /* An event handler is a method that responds to an event. It contains the code to be executed when the event is raised.
       Event handlers are usually void methods with two parameters:
           The sender (object that raised the event). (the button that was clicked)
           Event data (extract data about the event, derived from EventArgs).
    */
    protected void computeClick(Object sender, EventArgs events) {
        inputtedInches = 0.0;
        // first you need to read what the user inputted from the UI
        // The inputted data is stored in .Text since a textbox is just:
        // “a rectangle on the screen that accepts keyboard input”
        if (inches.Text == "") {
            inputtedInches = 0.0;
        } else {
            // converts the string to a double
            inputtedInches += Double.Parse(inches.Text);
            midText.Text += MetricConversion(inputtedInches);
            System.Console.WriteLine("The conversion is {0} meters.", midText.Text);
        }
    }
}