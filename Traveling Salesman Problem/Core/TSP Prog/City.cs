using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Traveling_Salesman_Problem
{
    [Serializable()]
    public class City : ISerializable
    {
        readonly Random rand = new Random();
        public int XValue, YValue;

        public int MaxX, MaxY, MinX, MinY;

        public string Name { get; set; }

        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [XmlIgnore]
        public Dictionary<string/*city name*/, double/*distance to city*/> distances = new Dictionary<string, double>();
        
        public Point cityLocation;
        public Point Location
        {
            get
            {
                return cityLocation;
            }
            set
            {
                if(XValue != 0 && YValue != 0)
                {
                    cityLocation = new Point(XValue, YValue);
                    return;
                }
                if (value.GetType() == typeof(Point)) //if obj passed is a point 
                {
                    Point obj = value;
                    if (obj.X <= this.MaxX && obj.X > MinX && obj.Y <= this.MaxY && obj.Y > MinY)  //if city MAX and MINS are correct create city locals.
                    {
                        cityLocation = value;
                    }
                    else
                    {
                        Console.WriteLine("Cannot Set Location of City {0} because the X and Y values passed were not between 0 and the MAX X and Y values.", obj);
                    }
                }
                else
                {
                    Console.WriteLine("The object passed is not the same type as Location.");
                }
            } 
        }

        public City() { }

        public City(string name, int maxXval, int maxYval, int minXval, int minYval)
        {
            Name = name;
            XValue = rand.Next(maxXval);
            YValue = rand.Next(maxYval);
            MaxX = maxXval;
            MaxY = maxYval;
            MinX = minXval;
            MinY = minYval;
            Location = new Point(XValue, YValue);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("XValue", XValue);
            info.AddValue("YValue", YValue);
            info.AddValue("MaxX", MaxX);
            info.AddValue("MaxY", MaxY);
            info.AddValue("MinX", MinX);
            info.AddValue("MinY", MinY);
            info.AddValue("cityLoction", cityLocation);
        }

        public City(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            XValue = (int)info.GetValue("XValue", typeof(int));
            YValue = (int)info.GetValue("YValue", typeof(int));
            MaxX = (int)info.GetValue("MaxX", typeof(int));
            MaxY = (int)info.GetValue("MaxY", typeof(int));
            MinX = (int)info.GetValue("MinX", typeof(int));
            MinY = (int)info.GetValue("MinY", typeof(int));
            cityLocation = (Point)info.GetValue("cityLoction", typeof(Point));
        }
    }
}
