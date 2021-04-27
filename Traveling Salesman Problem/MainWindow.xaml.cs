using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Traveling_Salesman_Problem.MVVM.ViewModel;

namespace Traveling_Salesman_Problem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //TSP Locals.
        readonly static string filepath = @"C:\Users\Josh\C#Data\TSMP\CitiesData.Dat"; 

        readonly static Random Rand = new Random();

        readonly static int NUMOFCITIES = Rand.Next(8, 18); // Keep below 35 Total cities for map size to be most optimal for screenspace 

        //readonly static double mapScalingRate = NUMOFCITIES * (20)/*Each city takes 20sq units of room*/ / 100;

        readonly public static int UImapHeight = 880, UImapWidth = 650;

        readonly public static int MAXXVALUE = UImapWidth - 15;

        readonly public static int MAXYVALUE = UImapHeight - 15;

        readonly public static int MINXVALUE = 15;

        readonly public static int MINYVALUE = 15;

        GA ga;




        public static Map mapofCities = new Map(MAXXVALUE, MAXYVALUE, MINXVALUE, MINYVALUE, NUMOFCITIES, filepath);

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Dock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public MainWindow()
        {
            bool complete = false;
            InitializeComponent();

            mapofCities.CreateCities();
            mapofCities.LoadCities();

            ga = new GA(mapofCities);
            var e = 0; var c = 0; var countCont = 100;
            var currBF = ga.betterFit;
            while (e != countCont || complete != true)
            {
                Console.WriteLine($"Best Fit: {ga.bestFit}.\nBetter Fit: {ga.betterFit}.");
                if(e % 4 == 0)
                {
                    ga.bestFit = ga.betterFit;
                }
                if (c >= 5 && ga.betterFit == ga.bestFit)
                {
                    complete = true;
                }
                else
                {
                    if(ga.betterFit == ga.bestFit)
                    {
                        c++;
                    }
                    else
                    {
                        c = 0;
                    }
                }
                ga.CalcPathing(); //calculates dataStrings and TSP problem solution for each child.
                currBF = ga.betterFit;
                foreach (Child ch in ga.Children)
                {
                    ga.Fitness(ch);                 //get each childs fitness score
                }
                List<Child> parents = null;
                parents = ga.sortFitChildren();
                ga.Crossover(parents);
                ga.Mutation();
                e++;
            }
            complete = true;

            if (complete == true)
            {
                mapofCities.clearCities();
            }
        }
    }
}
