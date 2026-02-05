/*
    Author: Kassandra Sanchez
    Email:  k.sanchez@csu.fullerton.edu
    Cwid:  884962788
    Course:  CPSC 223N
    Assignment:  1
    Due:  February 8, 2026
    Program:  Imperial to Metric Converter
*/

// must rewrite using statements as they only apply to the file they are written in
using System;
// deals with graphics like color and such
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions; //check ifalpha

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
    private Panel headerPanel = new Panel();
    private Panel displayPanel = new Panel();
    private Panel buttonsPanel = new Panel();
    private Size maximumMetricInterfaceSize = new Size(500,450);
    private Size minimumMetricInterfaceSize = new Size(500,450);

    public Metric() {
        // sets size of the UI
        MaximumSize = maximumMetricInterfaceSize;
        MinimumSize = minimumMetricInterfaceSize;
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

        // Can ommit size since max/min size were set
        Size = new Size(500, 450);
        // .Size is a property that tells the label/button how many pixels it is by wxh
        // DONT TOUCH
        title.Size = new Size(430, 40);
        // DONT TOUCH
        subtitle.Size = new Size(200, 30);
        enterSection.Size = new Size(110, 30);
        midText.Size = new Size(370, 30);
        inches.Size = new Size(200, 30);
        // DONT TOUCH
        compute.Size = new Size(90, 50);
        // 90,50
        clear.Size = new Size(90, 50);
        exit.Size = new Size(90, 50);
        headerPanel.Size = new Size(500, 115);
        displayPanel.Size = new Size(500, 185);
        buttonsPanel.Size = new Size(500, 250);

        // .Location is where the stuff is positioned by x, y
        title.Location = new Point(35, 20);
        subtitle.Location = new Point(150, 65);
        enterSection.Location = new Point(30, 150);
        midText.Location = new Point(30, 230);
        inches.Location = new Point(150, 150);
        // fine
        // x, 340
        compute.Location = new Point(40, 340);
        clear.Location = new Point(200, 340);
        exit.Location = new Point(360, 340);
        headerPanel.Location = new Point(0, 0);
        displayPanel.Location = new Point(0, 115);
        buttonsPanel.Location = new Point(0, 185);
        // wipppppppppppppppppppppppppppppppppppppppppppp

        // .Enabled determines whether clicking on it executes the algo or if you can type in it
        // so if the button is set to false, clicking on it does nothing. true makes it run the algo
        // Setting to false also makes it look grayed out
        inches.Enabled = true;
        compute.Enabled = true;
        clear.Enabled = true;
        exit.Enabled = true;
        // might need to do some other stuff too

        // Add displays the stuff to the GUI
        Controls.Add(title);
        Controls.Add(subtitle);
        Controls.Add(enterSection);
        Controls.Add(midText);
        Controls.Add(inches);
        Controls.Add(compute);
        Controls.Add(clear);
        Controls.Add(exit);
        Controls.Add(headerPanel);
        Controls.Add(displayPanel);
        Controls.Add(buttonsPanel);

        title.Font = new Font("Times New Roman", 25, FontStyle.Bold);
        subtitle.Font = new Font("Times New Roman", 15, FontStyle.Regular);
        enterSection.Font = new Font("Times New Roman", 15, FontStyle.Regular);
        midText.Font = new Font("Times New Roman", 15, FontStyle.Regular);
        inches.Font = new Font("Times New Roman", 15, FontStyle.Regular);
        // DONT TOUCH
        compute.Font = new Font("Times New Roman",15,FontStyle.Regular);
        clear.Font = new Font("Times New Roman",15,FontStyle.Regular);
        exit.Font = new Font("Times New Roman",15,FontStyle.Regular);

        // Set colors
        // Set colors using a hexcode
        //this.BackColor = Color.Magenta;
        //this.TransparencyKey = Color.Magenta;
        title.BackColor = ColorTranslator.FromHtml("#cdc1ff");
        subtitle.BackColor = ColorTranslator.FromHtml("#cdc1ff");
        enterSection.BackColor = ColorTranslator.FromHtml("#e5d9f2");
        midText.BackColor = ColorTranslator.FromHtml("#e5d9f2");
        compute.BackColor = ColorTranslator.FromHtml("#F3E4F5");
        clear.BackColor = ColorTranslator.FromHtml("#FFE5F8");
        exit.BackColor = ColorTranslator.FromHtml("#e2cbeeff");
        // e6eef5, e3e9f8, d5dff6
        headerPanel.BackColor = ColorTranslator.FromHtml("#cdc1ff");
        displayPanel.BackColor = ColorTranslator.FromHtml("#e5d9f2");
        buttonsPanel.BackColor = ColorTranslator.FromHtml("#f5efff");
        //compute.ForeColor = Color.White;

        // Creates a border around the button
        compute.FlatStyle = FlatStyle.Flat;
        compute.FlatAppearance.BorderSize = 3;
        // if not using hex code
        // compute.FlatAppearance.BorderColor = Color.White;
        compute.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#a7abde");
        // clear
        clear.FlatStyle = FlatStyle.Flat;
        clear.FlatAppearance.BorderSize = 3;
        clear.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#a7abde");
        // exit
        exit.FlatStyle = FlatStyle.Flat;
        exit.FlatAppearance.BorderSize = 3;
        exit.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#a7abde");




        //Associate the Compute button with the Enter key of the keyboard
        AcceptButton = compute;
        // Same thing but when the Esc key gets entered
        CancelButton = exit;
        

        // hooks the button up to some code
        // so when compute is clicked it then runs the func called computeClick
        // the EventHandler points to the func like computeClick
        compute.Click += new EventHandler(computeClick);
        clear.Click += new EventHandler(clearClick);
        exit.Click += new EventHandler(exitClick);
        CenterToScreen();
    }

    // protected means that only other class members and any class that inherits the class are allowed to use this func
    /* An event handler is a method that responds to an event. It contains the code to be executed when the event is raised.
       Event handlers are usually void methods with two parameters:
           The sender (object that raised the event). (the button that was clicked)
           Event data (extract data about the event, derived from EventArgs).
    */
    // compute click should be done
    protected void computeClick(Object sender, EventArgs events) {
        // first you need to read what the user inputted from the UI
        // The inputted data is stored in .Text since a textbox is just:
        // “a rectangle on the screen that accepts keyboard input”

        // need to check if its valid or not
        // [0-9]* means that the text must ONLY contain digits
        // it also means that the string can be empty because of *

        // fails because it wont accept periods or negs
        // Regex.IsMatch(inches.Text, "^[0-9]*$")
        // https://www.regexpal.com/
        // ^-?[0-9]*(\.?)$
        // plain dot gives error "."
        // "num." gives error
        // ".8" doesnt give an error
        // (?!.\*$)(-?)([^-.]*\.[^-.]*)([0-9])([^\s])
        // ^(-?)([^-.]*\.[^-.]*)([0-9])$
        inputtedInches = 0.0;
        
        if (inches.Text == "") {
            inputtedInches = 0.0;
            // og ^(?!.*\*$)(-?)([^-.]*\.[^-.]*)([0-9])$
        } else if (Regex.IsMatch(inches.Text, "^(?!.*\\*$)(-?)([^-.]*\\.[^-.]*)([0-9])$") == true) {
            // check if theres whitespace and give an error
            if (Regex.IsMatch(inches.Text, @"\s") == true) {
                midText.Text = "Invalid input. Please try again.";
                System.Console.WriteLine("Please only write numbers that include a decimal point.");
                return;
            }
        } else {
            midText.Text = "Invalid input. Please try again.";
            System.Console.WriteLine("Please only write numbers that include a decimal point.");
            return;
        }


        // converts the string to a double
        inputtedInches += Double.Parse(inches.Text);
        // create a diff class because reasons???? idk but its calling the class and the function
        midText.Text = "The metric value is: ";
        MetricLogic logic = new MetricLogic();
        // dont need to convert to a string since C# automatically calls ToString() when using the +
        midText.Text += logic.metricConversion(inputtedInches) + " meters.";
        string meter = logic.metricConversion(inputtedInches).ToString();
        // FIX THIS
        System.Console.WriteLine("The conversion is {0} meters.", meter);
    } // add try and catch so any errors get printed to the terminal

    // it might be done
    protected void clearClick(Object sender, EventArgs evt) {
        inches.Text = "";
        midText.Text = "The metric equivalent will be displayed here.";
    }

    // also might be done
    protected void exitClick(Object sender, EventArgs evt) {
        // closes the windows instances
        Close();
    }
}