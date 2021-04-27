using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Traveling_Salesman_Problem.MVVM.View
{
    /// <summary>
    /// Interaction logic for ChildMapView.xaml
    /// </summary>
    public partial class ChildMapView : UserControl
    {
        static string filename = @"C:\Users\Josh\C#Data\TSMP\CitiesData.Dat";
        Canvas myCanvas;
        TextBlock cityInfo;
        Map map = new Map(filename);


        public ChildMapView()
        {
            InitializeComponent();

            myCanvas = mapCanvas;
            cityInfo = cInfoBox;

        }
        //click on city to see city details 

        private void city_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(Ellipse))
            {

                var cEllipse = sender as Ellipse;

                foreach(UIElement elem in myCanvas.Children)
                {
                    if(elem.GetType() == typeof(Ellipse))
                    {
                        if((elem as Ellipse).Fill == Brushes.DarkViolet && (elem as Ellipse) != cEllipse)
                        {
                            ((Ellipse)elem).Fill = Brushes.Red;
                        }
                    }
                }

                cEllipse.Fill = Brushes.DarkViolet;

                string info = cEllipse.Uid;

                String text = info;

                cityInfo.Text = text;
            }
        }

        public void CreateMap(MapUI ui)
        {
            myCanvas = ui.PlotCities(map);

            foreach (var child in myCanvas.Children)     //adds mouse down function to all cities.
            {
                if (child.GetType() == typeof(Ellipse))
                {
                    (child as UIElement).MouseLeftButtonDown += city_MouseLeftButtonDown;
                }
            }
        }
    }
}
