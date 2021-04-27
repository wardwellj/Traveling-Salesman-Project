using System;
using System.Collections.Generic;
using System.Text;
using Traveling_Salesman_Problem.Core;
using Traveling_Salesman_Problem.Core.MVVM_Control_Prog;

namespace Traveling_Salesman_Problem.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand MapViewCmd { get; set; }
        public RelayCommand CityInfoViewCmd { get; set; }
        public RelayCommand GenInfoViewCmd { get; set; }
        public RelayCommand ChildMapViewCmd { get; set; }



        public MapViewModel MapVM { get; set; }

        public CityInfoViewModel CityInfoVM { get; set; }

        public GenInfoViewModel GenInfoVM { get; set; }

        public ChildMapViewModel ChildMapVM { get; set; }

        private object currView;

        public object CurrentView
        {
            get { return currView; }
            set { 
                currView = value;
                OnPropChanged();
            }
        }


        public MainViewModel()
        {
            MapVM = new MapViewModel();
            CityInfoVM = new CityInfoViewModel();
            GenInfoVM = new GenInfoViewModel();
            ChildMapVM = new ChildMapViewModel();


            CurrentView = MapVM;

            MapViewCmd = new RelayCommand(o => {

                CurrentView = MapVM;
            });

            CityInfoViewCmd = new RelayCommand(o => {

                CurrentView = CityInfoVM;
            });

            GenInfoViewCmd = new RelayCommand(o => {

                CurrentView = GenInfoVM;
            });

            ChildMapViewCmd = new RelayCommand(o => {

                CurrentView = ChildMapVM;
            });
        }
    }
}
