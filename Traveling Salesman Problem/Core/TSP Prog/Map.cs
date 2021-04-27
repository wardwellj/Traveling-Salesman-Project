using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls.Primitives;
using System.Dynamic;
using System.DirectoryServices.ActiveDirectory;

namespace Traveling_Salesman_Problem
{
    public class Map
    {
        string filename;
        int NumOfCities = 0;
        int CityNum = 1;

        int MaxX = 0;
        int MaxY = 0;
        int MinX = 0;
        int MinY = 0;

        private List<City> cities;

        public List<City> Cities
        {
            get
            {
                return cities;
            }
            set
            {
                if (value.GetType() == typeof(List<City>))
                {
                    cities = value;
                }
            }
        }

        public Map(string fileName)
        {
            this.filename = fileName;
        }

        public Map(int mxX, int mxY, int mnX, int mnY, int amtofCities, string fileName)
        {
            NumOfCities = amtofCities;
            MaxX = mxX;
            MaxY = mxY;
            MinY = mnY;
            MinX = mnX;
            cities = new List<City>();
            this.filename = fileName;
        }

        public City CreateCity()
        {
            string cityName = $"City : {CityNum}";
            int id = CityNum;
            CityNum++;
            City c = new City(cityName, this.MaxX, this.MaxY, this.MinX, this.MinY);
            c.ID = id;

            return c;
        }

        public void CreateCities()
        {
            for (int i = CityNum; i <= NumOfCities; i++)
            {
                bool toClose = false;
                int count = cities.Count;


                City c = CreateCity();
                if (count == 0)
                {
                    cities.Add(c);
                }
                else
                {
                    while (toClose == false)
                    {
                        foreach (City city in cities)
                        {
                            toClose = WithinMargin(city, c);
                            if (toClose == true)
                                break;
                        
                        }
                        break;
                    }
                }
                if (count == 0)
                {
                    continue;
                }
                else if (toClose == false)
                {
                    cities.Add(c);
                }
                else
                {
                    Top:
                    CityNum--;
                    c = CreateCity();
                    toClose = false;
                    while (toClose == false)
                    {
                        foreach (City city in cities)
                        {
                            toClose = WithinMargin(city, c);
                            if (toClose == true)
                                break;
                        }
                        break;
                    }
                    if (toClose == false)
                    {
                        cities.Add(c);
                    }
                    else 
                    {
                        goto Top;
                    }
                }

            }



            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));
            //XmlSerializer serializer2 = new XmlSerializer(typeof(string));

            //string numofcitiesStr = $"Trial Number 2\nThe Number of Cities : {NumOfCities}";


            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //serializer2.Serialize(fs, numofcitiesStr);
                serializer.Serialize(fs, Cities);
            };

            cities = null;
        }

        public void LoadCities()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
  
                cities = (List<City>)serializer.Deserialize(fs);
            }
        }

        public void clearCities()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Close();
            }
        }


        private bool WithinMargin(City oldcity, City newCity)
        {
              bool toClose = false;

            if (newCity.Location.X < oldcity.Location.X + 20 && newCity.Location.X > oldcity.Location.X - 20)
            {
                toClose = true;
            }
            else if (newCity.Location.Y < oldcity.Location.Y + 20 && newCity.Location.Y > oldcity.Location.Y - 20)
            {
                toClose = true;
            }
            else if (newCity.Location == oldcity.Location)
            {
                toClose = true;
            }
            else
            {
                return toClose;
            }

            return toClose;
        }

    }
}
