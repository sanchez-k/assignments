//****************************************************************************************************************************
//Program name: "Ninety Degree Turn 3.0".  This programs accepts the coordinates of two points from the user, draws a        *
//straight line segment connecting them, and ball travels from the beginning end point to the terminal end point.            *
//Copyright (C) 2022  Floyd Holliday                                                                                         *
//                                                                                                                           *
//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License  *
//version 3 as published by the Free Software Foundation.                                                                    *
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied         *
//warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.     *
//A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                           *
//****************************************************************************************************************************



﻿//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Author: Floyd Holliday
//Mail: holliday@fullerton.edu

//Program name: Ninety Degree Turn 3.0
//Programming language: C Sharp
//Date development of program began: 2022-Sep-14
//Date of last update: 2022-Sep-17

//Purpose:  This programs demonstrate how an animated ball can turn in a 90 degree angle and still maintain constant speed.

//Update: This file is a re-structuring of Ninety Degree Turn 2.1 for the principal reason of including panels.  Older
//versions of this Ninety Degree program did not use panels and that is evidence of poor program design.

//Files in project: ninety-degree-main.cs, straight-line-travel-user-interface.cs, straight-line-travel-algorithms.cs, r.sh

//This file's name: ninety-degree-ui.cs
//This file purpose: This module (file) defines the layout of the user interface
//Date last modified: 2022-Sep-17

//Known issues: The program is correct according to the requirements.  There is an issue with the method Update_ball_coordinates.
//That method (function) is somewhat unintelligible and needs to be re-written in a more understandable format.

//To compile straight-line-travel-user-interface.cs:
//     mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -r:straight-functions.dll -out:straight-line.dll straight-line-travel-user-interface.cs
//

//Suggestion to user: Feel free to change the values in the first five parameters and discover the effects of those changes.  
//For instance, modify the values in refresh_clock_rate, ball_linear_speed, motion_clock_rate.


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class Ninety_degree_turn_interface : Form
{  //Declare constants ordered by the specification document
   private const int ui_width = 1280;
   private const int ui_height = 900;
   private const double refresh_clock_rate = 24.8;  //Hertz = tics per second
   private const double motion_clock_rate = 57.3;   //Hertz = tics per second
   private const double ball_linear_speed = 98.3;   //pixel per second
   private Size maxframesize = new Size(ui_width,ui_height);
   private Size minframesize = new Size(ui_width,ui_height);

   //Declare the top panel and related attributes
   private Panel header = new Panel();
   private int header_height = 50;
   private Label ninety_degree_label = new Label();

   //Declare the graphic panel and related attributes.
   private Graphicpanel graph_panel = new Graphicpanel();

   //Declare the controller panel
   private Panel controler = new Panel();

   //Declare objects (controls) to be placed on the controller panel.
   private Button go_and_pause = new Button();
   private Label elapsed_time_legend = new Label();
   private Label elapsed_time_value = new Label();
   private Button quit = new Button();

   //Declare vertices of the polygonal line where the ball will travel.
   private static Point v1;
   private static Point v2;
   private static Point v3;
   private static Point v4;

   //Declare the properties of the moving ball
   enum Compass {North,West,South,East};
   Compass current_direction = Compass.West;
   private static double ball_radius = 8.65;
   private static double ball_center_x;
   private static double ball_center_y;
   private static double ball_corner_x;
   private static double ball_corner_y;
   private static double Δx;
   private static double Δy;
   private static double ball_speed_pixel_per_tic;

   //Declare properties of the clock controlling the ball.
   private int ball_clock_interval = (int)System.Math.Round(1000.0/motion_clock_rate);
   private static System.Timers.Timer ball_clock = new System.Timers.Timer();

   //Declare the refresh clock.
   private int refresh_clock_interval = (int)System.Math.Round(1000.0/refresh_clock_rate);
   private static System.Timers.Timer user_interface_refresh_clock = new System.Timers.Timer();

   //Auxiliary data
   private bool both_clocks_are_stopped = true;
   private int tic_counter = 0;

   //Begin constructor of this class
   public Ninety_degree_turn_interface()      //<====================
   {//Set properties of the form
    Width = ui_width;
    Height = ui_height;
    MaximumSize = maxframesize;
    MinimumSize = minframesize;
    BackColor = Color.Beige;
    Text = "Ninety Degree Turn";
    System.Console.WriteLine("form height = {0}, form width = {1}.", Height, Width);


    //Set up the top panel known as the header panel.
    header.Size = new Size(Width,header_height);
    header.Text = "90 Degree Turn";
    header.BackColor = Color.LightGreen;
    header.Location = new Point(0,0);
    System.Console.WriteLine("header panel height = {0}, header panel width = {1}.", header.Height, header.Width);

    //Set up the label to be placed in the top panel.
    ninety_degree_label.Font = new Font("Times New Roman",15,FontStyle.Bold);
    ninety_degree_label.Size = new Size(500,25);
    ninety_degree_label.Text = "Ninety Degree Turns programmed by the Professor";
    ninety_degree_label.TextAlign = ContentAlignment.MiddleCenter;
    //Next statement: find a point that centers the placement of ninety_degree_label in the center of header_panel.
    ninety_degree_label.Location = new Point((header.Width-ninety_degree_label.Width)/2,(header.Height-ninety_degree_label.Height)/2);
    header.Controls.Add(ninety_degree_label);

    //Set up the graphically enabled central panel
    graph_panel.Location = new Point(0,header.Height);
    graph_panel.BackColor = Color.LightPink;
    graph_panel.Width = Width;   //The graph panel has width equal to the width of the form
    graph_panel.Height = 750;
    System.Console.WriteLine("graph panel height = {0}, graph panel width = {1}.", graph_panel.Height, graph_panel.Width);

    //Set up the polygonal path in which the ball travels
    v1 = new Point(graph_panel.Width-30,30);
    v2 = new Point(30,30);
    v3 = new Point(30,graph_panel.Height-30);
    v4 = new Point(graph_panel.Width-30,graph_panel.Height-30);

    //Set the initial location of the ball.
    ball_center_x = v1.X;
    ball_center_y = v1.Y;

    //Set up the control panel containing buttons and other display devices.
    controler.Width = Width;
    controler.Height = Height-header.Height-graph_panel.Height;
    controler.BackColor = Color.Aqua;
    controler.Location = new Point(0,header.Height+graph_panel.Height);
    System.Console.WriteLine("control panel height = {0}, control panel width = {1}.", controler.Height, controler.Width);

    //Set up the go_and_pause button and attach it to the controller panel
    go_and_pause.Size = new Size(60,30);
    go_and_pause.Text = "Start";
//    go_and_pause_button.Location = new Point(control_panel.Width/15,control_panel.Height/4);
    go_and_pause.Location = new Point(50,20);
    go_and_pause.BackColor = Color.Orange;
    go_and_pause.Click += new EventHandler(Go_pause);
    go_and_pause.Enabled = true;
    go_and_pause.TextAlign = ContentAlignment.MiddleCenter;
    controler.Controls.Add(go_and_pause);

    //Configure the label that will explain elapsed time
    elapsed_time_legend.Size = new Size(80,30);
    elapsed_time_legend.BackColor = Color.LightGreen;
    //elapsed_time_legend.Location = new Point(control_panel.Width*2/10,control_panel.Height/4);
    elapsed_time_legend.Location = new Point(150,20);
    elapsed_time_legend.Font = new Font("Times New Roman",9,FontStyle.Bold);
    elapsed_time_legend.TextAlign = ContentAlignment.MiddleCenter;
    elapsed_time_legend.Text = "Elapsed\nTime (Sec)";
    controler.Controls.Add(elapsed_time_legend);

    //Configure the label that will display the time in seconds.
    elapsed_time_value.Size = new Size(90,30);
    elapsed_time_value.BackColor = Color.LightGreen;
    //elapsed_time_value.Location = new Point(control_panel.Width*3/10,control_panel.Height/4);
    elapsed_time_value.Location = new Point(230,20);
    elapsed_time_value.Font = new Font("Arial",18,FontStyle.Bold);
    elapsed_time_value.TextAlign = ContentAlignment.MiddleCenter;
    elapsed_time_value.Text = "000.00";
    controler.Controls.Add(elapsed_time_value);

    //Set up the quit button and attach it to the controller panel
    quit.Size = new Size(60,30);
    quit.Text = "Quit";
    //quit.Location = new Point(control_panel.Width*9/10,control_panel.Height/4);
    quit.Location = new Point(1180,20);
    quit.BackColor = Color.Salmon;
    quit.TextAlign = ContentAlignment.MiddleCenter;
    quit.Click += new EventHandler(Close_window);
    controler.Controls.Add(quit);

    //Prepare the refresh clock.  A button will start this clock ticking.
    user_interface_refresh_clock.Enabled = false;  //Initially this clock is stopped.
    user_interface_refresh_clock.Interval = refresh_clock_interval;
    user_interface_refresh_clock.Elapsed += new ElapsedEventHandler(Refresh_user_interface);

   //Prepare the ball clock.  A button will start this clock ticking.
    ball_clock.Enabled = false;  //Initially this clock is stopped.
    ball_clock.Interval = ball_clock_interval;
    ball_clock.Elapsed += new ElapsedEventHandler(Update_ball_coordinates);

    //Attach various elements (controls) to the form.
    Controls.Add(header);
    Controls.Add(graph_panel);
    Controls.Add(controler);

    //Change units of speed of ball
    ball_speed_pixel_per_tic = ball_linear_speed/motion_clock_rate;  //pixels per tic.

    //Set the values for Δx and Δy for initial direction "West".
    Δx = -ball_speed_pixel_per_tic;
    Δy = 0.0;

    //Make sure this UI window appears in the center of the monitor.
    CenterToScreen();

   }//End of constructor                               //<=======================






   //Handler for the ball clock.
   protected void Update_ball_coordinates(System.Object sender, ElapsedEventArgs even)
   {//This function is called each time the ball_clock makes one tic.  
    //That clock is often called the animation clock.
    tic_counter++;
    switch (current_direction)
    {  case Compass.West:
                 if(ball_center_x+Δx >= v2.X)
                      {ball_center_x += Δx;    //<= Ball moves directly West.
                       ball_center_y += Δy;    //However, Δy is 0.0 when moving West.
                      }
                 else //ball_center_x+Δx < v2.X
                      {//Ball changes direction to South
                       ball_center_x = v2.X;
                       ball_center_y = v2.Y - ball_center_x - Δx + v2.X;
                       Δx = 0.0;
                       Δy = +ball_speed_pixel_per_tic;
                       current_direction = Compass.South;
                      }//End of else
                 break;
        case Compass.South:
                 if(ball_center_y+Δy <= v3.Y)
                      {ball_center_x += Δx;   //Redundant: Δx = 0.0 when moving South.
                       ball_center_y += Δy;   //<== Ball moves directly South
                      }
                 else //ball_center_y+Δy > v3.Y
                      {//Ball changes direction to East.
                       ball_center_x = v3.X + ball_center_y + Δy - v3.Y;
                       ball_center_y = v3.Y;
                       Δx = +ball_speed_pixel_per_tic;
                       Δy = 0.0;
                       current_direction = Compass.East;
                      }//End of else
                 break;
        case Compass.East:
                 if(ball_center_x + Δx <= v4.X)
                      {ball_center_x += Δx;
                       ball_center_y += Δy;   //Redundant: Δy = 0.0 when moving East.
                      }
                 else //ball_center_x + Δx > v4.X is true.
                      {//Ball changes direction to North.
                       ball_center_x = v4.X;
                       ball_center_y = v4.Y - ball_center_x - Δx - v4.X;
                       Δx = 0.0;
                       Δy = -ball_speed_pixel_per_tic;
                       current_direction = Compass.North;
                      }
                 break;
        case Compass.North:
                 System.Console.WriteLine("There is no road going North.  This program will end.");
                 ball_center_x = v4.X;
                 ball_center_y = v4.Y;
                 ball_clock.Enabled = false;
                 user_interface_refresh_clock.Enabled = false;
                 graph_panel.Invalidate();
                 break;
        default:
                 System.Console.WriteLine("You have a serious bug in this program.  Try again");
                 break;
    }//End of switch
   }//End of method Update_ball_coordinates





   //Handler for the Go_and_pause button
   protected void Go_pause(System.Object sender, EventArgs even)
   {if(both_clocks_are_stopped)
         {//Change the message on the button
          go_and_pause.Text = "Pause";

          //Start the refresh clock running.
          user_interface_refresh_clock.Enabled = true;

          //Start the animation clock running.
          ball_clock.Enabled = true;
         }
    else
         {//Stop the refresh clock.
          user_interface_refresh_clock.Enabled = false;

          //Stop the animation clock running.
          ball_clock.Enabled = false;

          //Change the message on the button
          go_and_pause.Text = "Go";
         }//End of if
    //Toggle the variable both_clocks_are_stopped to be its negative
    both_clocks_are_stopped = !both_clocks_are_stopped;
   }//End of event handler Go_pause







   //Declare the handler method for the refresh clock
   protected void Refresh_user_interface(System.Object sender, ElapsedEventArgs even)
   {elapsed_time_value.Text = String.Format("{0:000.00}",(double)tic_counter/motion_clock_rate);
    graph_panel.Invalidate();
   }//End of event handler Refresh_user_interface





   //Declare the handler method of the Quit button.
   protected void Close_window(System.Object sender, EventArgs even)
   {System.Console.WriteLine("This program will close the UI window and stop execution.");
    Close();
   }//End of event handler Close_window




    //Define the Graphic Panel     
    public class Graphicpanel: Panel
    {
     private Pen schaffer = new Pen(Color.Purple,2);
        protected override void OnPaint(PaintEventArgs ee)
        {   Graphics graph = ee.Graphics;
            graph.DrawLine(schaffer,v1.X,v1.Y,v2.X,v2.Y);
            graph.DrawLine(schaffer,v2.X,v2.Y,v3.X,v3.Y);
            graph.DrawLine(schaffer,v3.X,v3.Y,v4.X,v4.Y);
            ball_corner_x = ball_center_x - ball_radius;
            ball_corner_y = ball_center_y - ball_radius;
            graph.FillEllipse(Brushes.Yellow,
                              (int)System.Math.Round(ball_corner_x),
                              (int)System.Math.Round(ball_corner_y),
                              (int)System.Math.Round(2.0*ball_radius),
                              (int)System.Math.Round(2.0*ball_radius));
            base.OnPaint(ee);
        }
        //End of method OnPaint
    }
    //End of class Graphicpanel

}//End of class Ninety_degree_turn_interface

//======End of file=============================================================