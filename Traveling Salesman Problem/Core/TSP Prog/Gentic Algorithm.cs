using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Traveling_Salesman_Problem
{
    public class GA
    {
        public static int childPop = 20; //amnt of children per generation
        public static int parPop = childPop / 5; //amt of parents to help create each generation following the first.
        public double betterFit = 10000; //updates each generation to be compared to the bestFit (good-enough or most optimal solution check)
        public double bestFit = 0;
        Random rand = new Random();
        Map citymap = null;
        private int amntOfCities;
        List<Child> children = new List<Child>(childPop);
        public List<Child> Children
        {
            get { return children; }
            set { children = value; }
        }

        public GA(Map map)
        {
            this.citymap = map;
            amntOfCities = citymap.Cities.Count;
            calcCD();  //calculates each cities distance fields 
            createInitPop(); //creates first generation of children
        }

        private void createInitPop()
        {
            for (int i = 0; i < childPop; i++)
            {
                Child c = new Child(amntOfCities);
                children.Add(c);
            }
        }

        private void calcCD()   //calculates all distances from city to city, and then stores in their respective distances.
        {

            for(int i = 0; i < citymap.Cities.Count; i++)
            {
                var c1 = citymap.Cities[i];
                for (int j = 0; j < citymap.Cities.Count; j++)
                {
                    var c2 = citymap.Cities[j];
                    while (c2 == c1)
                    {
                        if(j != citymap.Cities.Count-1)
                        {
                            j++;
                            c2 = citymap.Cities[j];
                        }
                        else
                        {
                            return;
                        }
                    }
                    var x1 = c1.Location.X; var y1 = c1.Location.Y;
                    var x2 = c2.Location.X; var y2 = c2.Location.Y;
                    
                    var distance = Math.Sqrt(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2)); // gets distance between city 1 and 2
                    c1.distances.Add(c2.Name, distance); //stores distance and connected city in distance dictionary.
                }
            }
        }


        public void CalcPathing() //calculates the pathing of each child storing it in there data variables.
        {
            for (int i = 0; i < children.Count; i++)
            {
                Child currChild = children[i];
                double totalPathDist = 0;
                
                if (currChild.dataString[0] == 0)
                {
                    List<City> visitedCities = new List<City>();
                    City first = citymap.Cities[0];
                    visitedCities.Add(first);
                    currChild.dataString[0] = (char)first.ID;
                    currChild.cityPath.Add(first);
                    City last = first;

                    for (int z = 1; z < citymap.Cities.Count; z++)
                    {
                        City currCity = citymap.Cities[rand.Next(citymap.Cities.Count)];
                        //check new city against already visited cities using visitedCities list and the first city to ensure its not been used.
                        string hasVisited = "";
                        while (hasVisited == "visited" || hasVisited == "")
                        {
                            hasVisited = "";
                            foreach (City c in visitedCities)
                            {
                                if(currCity == first || currCity == c)
                                {
                                    currCity = citymap.Cities[rand.Next(citymap.Cities.Count)];
                                    hasVisited = "visited";
                                }
                                else if(c == visitedCities[visitedCities.Count-1] && hasVisited == "")
                                {
                                    hasVisited = "notVisited";
                                }
                                else
                                {
                                    if (hasVisited == "visited")
                                    {
                                        hasVisited = "visited";
                                    }
                                    else
                                    {
                                        hasVisited = "";
                                    }
                                }
                            }
                        }

                        double lasttoCurrDist;

                        last.distances.TryGetValue(currCity.Name, out lasttoCurrDist); //gets value from distances dictionary and passes it to *lasttoCurrDist* var.

                        totalPathDist += lasttoCurrDist;

                        //each city added should also be added to the char[] datastring of a child.
                        currChild.dataString[z] = (char)currCity.ID;
                        currChild.cityPath.Add(currCity);
                        visitedCities.Add(currCity);

                        last = currCity;
                    }
                    //link last city back to first City
                    double tempDist;
                    last.distances.TryGetValue(first.Name, out tempDist);
                    totalPathDist += tempDist;
                    //store first city as last char in childs dataString var
                    currChild.dataString[currChild.dataString.Length - 1] = (char)first.ID;
                    currChild.cityPath.Add(first);

                    currChild.Cost = totalPathDist; //assign the total distance that was calculated to each child cost variable.
                }
                else //if child was made through cross over and datapath isnt empty we calculate new cost using datapath.
                {
                    for (int j = 0; j < currChild.dataString.Length; j++)
                    {
                        for (int x = 0; x < citymap.Cities.Count; x++)
                        {
                            if (citymap.Cities[x].ID == (int)(currChild.dataString[j]))
                            {
                                currChild.cityPath.Add(citymap.Cities[x]);
                                break;
                            }
                        }
                    }
                    double newDist;
                    var last = currChild.cityPath[0];
                    for(int j = 1; j < currChild.cityPath.Count; j++)
                    {
                        last.distances.TryGetValue(currChild.cityPath[j].Name, out newDist);
                        totalPathDist += newDist;
                        last = currChild.cityPath[j];
                    }

                    currChild.Cost = totalPathDist;
                }
            }
        } 

        public List<Child> sortFitChildren() //sortsFitChildren returning best children as parents for following generation.
        {
            Dictionary<double, Child> childFScores = new Dictionary<double, Child>();
            List<Child> parents = new List<Child>(parPop);
            foreach (Child c in children)       //fills childFScores Dict with key value pairs.
            {
                
                Child testChild;
                childFScores.TryGetValue(c.FitnessScore, out testChild);

                if(testChild == null) 
                {
                    childFScores.Add(c.FitnessScore, c); //fill fscores Dict with fitness data
                }
                else
                {
                    childFScores.Add(c.FitnessScore + .10, c);
                }
            }
            var Keys = new double[childFScores.Values.Count];
            childFScores.Keys.CopyTo(Keys, 0);

            var sortedKeys = from element in Keys
                             orderby element ascending
                             select element;

            var sortedKeysList = sortedKeys.ToList();

            for(int i = 0; i < parPop; i++)
            {
                Child temp = null;
                childFScores.TryGetValue(sortedKeysList[i], out temp);

                parents.Add(temp);
            }

            return parents;
        }

        public void Crossover(List<Child> parents)
        {
            var tempChildList = new List<Child>(childPop);

            var count = 0;

            while(count < childPop)
            {
                count++;
                Child par1 = parents[rand.Next(0, parents.Count)]; Child par2 = parents[rand.Next(0, parents.Count)];

                while(par1 == par2)
                {
                    par2 = parents[rand.Next(0, parents.Count)];
                }

                double splitPercent = rand.Next(20, 80);
                Thread.Sleep(30);
                splitPercent = splitPercent / 100;
                var Descendant = new Child(par1, par2, splitPercent);
                foreach(Child c in tempChildList)
                {
                    string cDatastr = "";
                    string dDatastr = "";
                    for(int i = 0; i < c.dataString.Length; i++)
                    {
                        cDatastr += (int)c.dataString[i]+ ", ";
                        dDatastr += (int)Descendant.dataString[i] + ", ";
                    }
                    if(cDatastr == dDatastr) //if a child is a dupplicate just start fresh adding more randomness to the next generation.
                    {
                        Descendant = new Child(amntOfCities);
                        break;
                    }
                }
                tempChildList.Add(Descendant);
            }

            children = tempChildList;
        }

        public void Mutation()
        {
            foreach (Child c in children)
            {
                var decisionVar = rand.Next(1, 3);
                if (decisionVar == 1)
                {
                    int rand1 = rand.Next(1, c.dataString.Length);
                    int rand2 = rand.Next(1, c.dataString.Length);

                    while (rand1 == rand2)
                    {
                        rand2 = rand.Next(1, c.dataString.Length);
                    }

                    var tempChar = c.dataString[rand2];
                    c.dataString[rand2] = c.dataString[rand1];
                    c.dataString[rand1] = tempChar;
                }
                else
                {
                    continue; //go to next child
                }
            }
        }


        public void Fitness(Child c)
        {
            double fitnessScore = 0;  //at start of function fitness is set to 0 to begin comparisons

            if(c.Cost < betterFit)
            {
                betterFit = c.Cost;   //if curr childs cost is smaller than the current best fitness score set betterFit to childs cost.
            }
            fitnessScore = c.Cost;

            c.FitnessScore = fitnessScore;
        }
    }

    public class Child
    {

        public List<City> cityPath = new List<City>();
        public char[] dataString; //this is the main dna component of each child, this is how the childs pathing and therefor cost will be chosen.
        private double cost; //contains each childs value assosiated with the cost of its pathing through the cities of the TSP.
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        private double fitnessScore; // holds a percentage that is passed via the fitness function. Percentage represents how fit the child is.
        public double FitnessScore
        {
            get { return fitnessScore; }
            set { fitnessScore = value; }
        }

        public Child(int numofCities)
        {
            dataString = new char[numofCities + 1];
        }

        public Child(Child mother, Child father, double splitPerc) //creates a descendant given two parents.
        {
            char[] newChildData = null;
            var mDataStr = mother.dataString;
            var fDataStr = father.dataString;

            int SplitIndex;

            if(mDataStr.Length == fDataStr.Length) //checks to insure parents datastrings are equal length to begin merging them.
            {
                SplitIndex = (int)(mDataStr.Length* splitPerc);
                newChildData = new char[mDataStr.Length];
            }
            else
            {
                //throws exception if something is incorrect with mother and father datastrings, stopping the program.
                throw new Exception($"Mother and Father Datastring length is not valid \nMother:{mother.dataString.Length}\nFather:{father.dataString.Length}.");
            }

            for(int i = 0; i < SplitIndex; i++)
            {
                newChildData[i] = mDataStr[i];
            }
            for (int i = SplitIndex; i < fDataStr.Length; i++)
            {
                newChildData[i] = fDataStr[i];
            }
            var dup = "true";
            while (dup != "false")
            {
                dup = "";
                for (int i = 1; i < newChildData.Length - 1; i++) //using the for loops we cut off the first and last indexes of the data string...
                {                                                   //this is because all pathings start and finish at the same city, thus we cannot allow this merger to mess up this duplication.
                    var tempCID = newChildData[i];

                    for (int j = 1; j < newChildData.Length - 1; j++)
                    {
                        if (i == j)
                        {
                            if (j != newChildData.Length - 2)
                            {
                                j++;
                            }
                            else
                            {
                                tempCID = (char)1;
                            }
                        }
                        if (tempCID == newChildData[j])
                        {
                            dup = "true";
                            newChildData[j] = (char)0; //if there is a duplicate set it to 0;
                            var cities = mother.cityPath;

                            for (int x = 1; x < mother.cityPath.Count - 1; x++)
                            {
                                if (!newChildData.Contains((char)cities[x].ID))
                                {
                                    newChildData[j] = (char)cities[x].ID;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if(dup == "" && i == newChildData.Length - 2 && j == newChildData.Length - 2 && tempCID == (char)1)
                            {
                                dup = "false";
                            }
                        }
                    }
                }
            }
            dataString = newChildData;
        }
    }
}
