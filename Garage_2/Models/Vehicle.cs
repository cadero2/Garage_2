using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Garage_2.Models
{
    public class Vehicle
    {
        [Key] public int ID { get; set; }
        public Type Type { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime Date { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Wheels { get; set; }
        public int[] ParkingLot { get; set; }
        public int ParkingSpace1 { get; set; }
        public int ParkingSpace2 { get; set; }
        public int ParkingSpace3 { get; set; }

        public static void SortList(List<Vehicle> list, String sortingParameter)
        {

            if (list != null)
            {
                if (sortingParameter != null)
                {
                    //SORT BY ID
                    if (sortingParameter == "ID")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).ID > list.ElementAt(m + 1).ID )
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY TYPE
                    if (sortingParameter=="Type")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).Type.CompareTo(list.ElementAt(m + 1).Type) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY REGNR
                    else if (sortingParameter == "RegistrationNumber")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).RegistrationNumber.CompareTo(list.ElementAt(m + 1).RegistrationNumber) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY Parking Space
                    else if (sortingParameter == "ParkingSpace")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).ParkingLot[0] > list.ElementAt(m + 1).ParkingLot[0])
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY DATE
                    else if (sortingParameter == "Date")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).Date.CompareTo(list.ElementAt(m + 1).Date) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY COLOR
                    else if (sortingParameter == "Color")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).Color.CompareTo(list.ElementAt(m + 1).Color) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY BRAND
                    else if (sortingParameter == "Brand")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).Brand.CompareTo(list.ElementAt(m + 1).Brand) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                    //SORT BY MODEL
                    else if (sortingParameter == "Model")
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            for (int m = 0; m < list.Count - 1; m++)
                            {
                                if (list.ElementAt(m).Model.CompareTo(list.ElementAt(m + 1).Model) > 0)
                                {
                                    Vehicle temp = list.ElementAt(m + 1);
                                    list[m + 1] = list[m];
                                    list[m] = temp;
                                }
                            }
                        }
                    }
                }
            }
            
        }

        public static int TotalFeeFromVehicleList(IEnumerable<Vehicle> list)
        {
            if (list != null)
            {
                int fee = 0;
                DateTime refPoint = DateTime.Now;
                foreach (Vehicle vehicle in list)
                {
                    TimeSpan time = refPoint - vehicle.Date;
                    int vehicleFee = (time.Days * 1440) + (time.Hours * 60) + time.Minutes;
                    fee += vehicleFee;
                }
                return fee;
            }
            return -1;
        }

        public static int VehicleTotalWheelsInListCount(IEnumerable<Vehicle> list)
        {
            if (list != null)
            {
                int count = 0;
                foreach (Vehicle vehicle in list)
                {
                    count += vehicle.Wheels;
                }
                return count;
            }
            return -1;
        }

        public static int VehicleTypeCountInList(IEnumerable<Vehicle> list, Type type)
        {
            if (list!=null)
            {
                int count = 0;
                foreach (Vehicle vehicle in list)
                {
                    if (vehicle.Type.Equals(type))
                    {
                        count++;
                    }
                }
                return count;
            }
            return -1;
        }

    }

    public class VehicleDBContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
    }

    public enum Type
    {
        Airplane, Boat, Bus, Car, Motorcycle
    }
    
}