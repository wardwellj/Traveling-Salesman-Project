using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Traveling_Salesman_Problem.Core;
using Traveling_Salesman_Problem.MVVM.View;
using Traveling_Salesman_Problem.MVVM.ViewModel;

namespace Traveling_Salesman_Problem
{
    public class MapUI
    {
        GA ga;

        //private bool cityInfoActive = false; //used to see if a city is active

        public MapUI(GA GA)
        {
            this.ga = GA;
        }

        public Canvas PlotCities(Child child)
        {
            Canvas myCanvas = new Canvas();
                var cityList = child.cityPath;
                foreach (var c in cityList)
                {
                    City mycity = c;

                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = 10;
                    ellipse.Width = 10;
                    ellipse.Stroke = Brushes.Black;
                    ellipse.Fill = Brushes.BlueViolet;
                    ellipse.Margin = new Thickness(mycity.Location.X, mycity.Location.Y, 0, 0);
                    ellipse.Uid = $"Name: {c.Name}\nX: {mycity.Location.X} Y: {mycity.Location.Y}";

                    myCanvas.Children.Add(ellipse);
                }

            return myCanvas;
        }

        public Canvas PlotCities(Map cityMap)
        {
            Canvas myCanvas = new Canvas();

                var cityList = cityMap.Cities;
                foreach (var c in cityList)
                {
                    City mycity = c;

                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = 10;
                    ellipse.Width = 10;
                    ellipse.Stroke = Brushes.Black;
                    ellipse.Fill = Brushes.BlueViolet;
                    ellipse.Margin = new Thickness(mycity.Location.X, mycity.Location.Y, 0, 0);
                    ellipse.Uid = $"Name: {c.Name}\nX: {mycity.Location.X} Y: {mycity.Location.Y}";

                    myCanvas.Children.Add(ellipse);
                }

            return myCanvas;
        }

        //plots a single city on the map
        void PlotCity(City c, Canvas myCanvas)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 10;
            ellipse.Width = 10;
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = Brushes.BlueViolet;
            ellipse.Margin = new Thickness(c.Location.X, c.Location.Y, 0, 0);
            ellipse.Uid = c.Name;

            myCanvas.Children.Add(ellipse);

        }
    }
}
