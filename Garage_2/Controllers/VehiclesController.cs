using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage_2.Models;

namespace Garage_2.Controllers
{
    public class VehiclesController : Controller
    {
        private VehicleDBContext db = new VehicleDBContext();
        private static bool[] parkingLot = new bool[30];

        //Default constructor
        public VehiclesController()
        {
            RecreateParkingLot();
        }

        /*public VehicleDBContext GetDBContext()
        {
            return db;
        } */
        
        // GET: Vehicles
        public ActionResult Index(string sortingParameter = null)
        {
            List<Vehicle> list = db.Vehicles.ToList();
            if (sortingParameter!=null)
            {
                Vehicle.SortList(list, sortingParameter);
            }
            return View("Index", list);
        }
        
        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,RegistrationNumber,Color,Brand,Model,Wheels")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Date = DateTime.Now;
                ; // Method call to assign parking space
                if (assignParkingSpace(vehicle))
                {
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    //If there are no parking spaces, the vehicle can't be parked and can't be added; of course, should be prevented to try in the first place.
                }
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,RegistrationNumber,Color,Brand,Model,Wheels")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            //Create copy of vehicle to check out.
            Vehicle vehicle2 = new Vehicle();
            vehicle2.ID = vehicle.ID;
            vehicle2.Type = vehicle.Type;
            vehicle2.RegistrationNumber = vehicle.RegistrationNumber;
            vehicle2.Date = vehicle.Date;
            vehicle2.Color = vehicle.Color;
            vehicle2.Brand = vehicle.Brand;
            vehicle2.Model = vehicle.Model;
            vehicle2.Wheels = vehicle.Wheels;
            vehicle2.ParkingLot = vehicle.ParkingLot;
            vehicle2.ParkingSpace1 = vehicle.ParkingSpace1;
            vehicle2.ParkingSpace2 = vehicle.ParkingSpace2;
            vehicle2.ParkingSpace3 = vehicle.ParkingSpace3;

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            //Clear parking spaces
            parkingLot[vehicle2.ParkingSpace1] = false;
            if (vehicle2.ParkingSpace2 > -1)
            {
                parkingLot[vehicle2.ParkingSpace2] = false;
            }
            if (vehicle2.ParkingSpace3 > -1)
            {
                parkingLot[vehicle2.ParkingSpace3] = false;
            }
            /*int[] spots = vehicle2.ParkingLot;
            if (spots!=null)
            {
                foreach (int n in spots)
                {
                    this.parkingLot[n] = false;
                }
            } */

            //return RedirectToAction("Receipt", vehicle2);
            return View("Receipt", vehicle2);
        }

        [HttpPost]
        public ActionResult Search(string searchId)
        {
            // filter snomething
            List<Vehicle> list = db.Vehicles.ToList();
            List<Vehicle> searchResult = new List<Vehicle>();
            foreach (Vehicle v in list)
            {
                if ( (v.RegistrationNumber + "").Contains(searchId) )
                {
                    searchResult.Add(v);
                }
                else if ((v.Type + "").Contains(searchId))
                {
                    searchResult.Add(v);
                }
                else if ((v.Color + "").Contains(searchId))
                {
                    searchResult.Add(v);
                }
                else if ((v.Brand + "").Contains(searchId))
                {
                    searchResult.Add(v);
                }
                else if ((v.Model + "").Contains(searchId))
                {
                    searchResult.Add(v);
                }
            }
            Console.WriteLine(searchId);
            return View("Search", searchResult);
        }
        
        public ActionResult Statistics()
        {
            return View(db.Vehicles.ToList());
        }

        public int GetVehicleCount()
        {
            if (db!=null)
            {
                return db.Vehicles.Count();
            }
            return -1;
        }

        public int GetVehicleCountV2()
        {
            if (db != null)
            {
                List<Vehicle> garage = db.Vehicles.ToList();
                if (garage!=null)
                {
                    int occSpaces = 0;
                    int noMotorcycles = 0;
                    foreach (Vehicle vehicle in garage)
                    {
                        if (vehicle.Type.Equals(Garage_2.Models.Type.Airplane))
                        {
                            occSpaces += 3;
                        }
                        else if (vehicle.Type.Equals(Garage_2.Models.Type.Boat))
                        {
                            occSpaces += 3;
                        }
                        else if (vehicle.Type.Equals(Garage_2.Models.Type.Bus))
                        {
                            occSpaces += 2;
                        }
                        else if (vehicle.Type.Equals(Garage_2.Models.Type.Motorcycle))
                        {
                            noMotorcycles++;
                        }
                        else
                        {
                            occSpaces += 1;
                        }
                    }
                    occSpaces += (noMotorcycles / 3);
                    if (noMotorcycles%3 > 0)
                    {
                        occSpaces += 1;
                    }
                    return occSpaces;
                }
            }
            return -1;
        }

        private bool assignParkingSpace(Vehicle vehicle)
        {
            if (vehicle!=null)
            {
                if (!vehicle.Type.Equals(Models.Type.Motorcycle))
                {
                    if (vehicle.Type.Equals(Models.Type.Airplane) || vehicle.Type.Equals(Models.Type.Boat))
                    {
                        return findParkingSpace(vehicle, 3);
                    }
                    else if (vehicle.Type.Equals(Models.Type.Bus))
                    {
                        return findParkingSpace(vehicle, 2);
                    }
                    else
                    {
                        return findParkingSpace(vehicle, 1);
                    }
                }
                else
                {
                    // Find parking space for motorcycle
                    return findParkingSpaceForMotorcycle(vehicle);
                }
            }
            return false;
        }

        private bool findParkingSpace(Vehicle vehicle, int size)
        {
            if (size >= 3)
            {
                for (int n=0; n + 2 < parkingLot.Length; n++)
                {
                    bool space1 = parkingLot[n];
                    bool space2 = parkingLot[n+1];
                    bool space3 = parkingLot[n+2];
                    if (!space1 && !space2 && !space3)
                    {
                        //int[] occSpaces = new int[3];
                        vehicle.ParkingSpace1 = n;
                        vehicle.ParkingSpace2 = n+1;
                        vehicle.ParkingSpace3 = n+2;
                        //occSpaces[0] = n;
                        //occSpaces[1] = n+1;
                        //occSpaces[2] = n+2;
                        //vehicle.ParkingLot = occSpaces;
                        parkingLot[n] = true;
                        parkingLot[n + 1] = true;
                        parkingLot[n + 2] = true;
                        return true;
                    }
                }
            }
            else if (size==2)
            {
                for (int n = 0; n + 1 < parkingLot.Length; n++)
                {
                    bool space1 = parkingLot[n];
                    bool space2 = parkingLot[n + 1];
                    if (!space1 && !space2)
                    {
                        //int[] occSpaces = new int[2];
                        vehicle.ParkingSpace1 = n;
                        vehicle.ParkingSpace2 = n + 1;
                        vehicle.ParkingSpace3 = -1;
                        //occSpaces[0] = n;
                        //occSpaces[1] = n + 1;
                        //vehicle.ParkingLot = occSpaces;
                        parkingLot[n] = true;
                        parkingLot[n + 1] = true;
                        return true;
                    }
                }
            }
            else if (size==1)
            {
                for (int n = 0; n < parkingLot.Length; n++)
                {
                    bool space1 = parkingLot[n];
                    if (!space1)
                    {
                        //int[] occSpaces = new int[1];
                        vehicle.ParkingSpace1 = n;
                        vehicle.ParkingSpace2 = -1;
                        vehicle.ParkingSpace3 = -1;
                        //occSpaces[0] = n;
                        //vehicle.ParkingLot = occSpaces;
                        parkingLot[n] = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool findParkingSpaceForMotorcycle(Vehicle vehicle)
        {
            int[] noOfMotorcyclesInParkingSpace = new int[parkingLot.Length]; //30
            foreach (Vehicle v in db.Vehicles.ToList())
            {
                if (v.Type.Equals(Models.Type.Motorcycle))
                {
                    noOfMotorcyclesInParkingSpace[v.ParkingSpace1]++; //Altered from v.ParkingLot[0]
                }
            }
            
            //Find space with 2 motorcycles
            for (int n=0; n < noOfMotorcyclesInParkingSpace.Length; n++)
            {
                if (noOfMotorcyclesInParkingSpace[n]==2)
                {
                    //Assign motorcycle to this space and return to calling method.
                    //int[] newSpot = new int[1];
                    //newSpot[0] = n;
                    //vehicle.ParkingLot = newSpot;
                    vehicle.ParkingSpace1 = n;
                    vehicle.ParkingSpace2 = -1;
                    vehicle.ParkingSpace3 = -1;
                    return true;
                }
            }

            //If no space with 2 motorcycles exist, find space with 1 motorcycle
            for (int n = 0; n < noOfMotorcyclesInParkingSpace.Length; n++)
            {
                if (noOfMotorcyclesInParkingSpace[n] == 1)
                {
                    //Assign motorcycle to this space and return to calling method.
                    //int[] newSpot = new int[1];
                    //newSpot[0] = n;
                    //vehicle.ParkingLot = newSpot;
                    vehicle.ParkingSpace1 = n;
                    vehicle.ParkingSpace2 = -1;
                    vehicle.ParkingSpace3 = -1;
                    return true;
                }
            }

            //If no space with motorcycles exist, assign to any empty space
            for (int n = 0; n < parkingLot.Length; n++)
            {
                if (!parkingLot[n])
                {
                    //Assign motorcycle to this space and return to calling method.
                    //int[] newSpot = new int[1];
                    //newSpot[0] = n;
                    //vehicle.ParkingLot = newSpot;
                    vehicle.ParkingSpace1 = n;
                    vehicle.ParkingSpace2 = -1;
                    vehicle.ParkingSpace3 = -1;
                    parkingLot[n] = true;
                    return true;
                }
            }

            return false; //If no spaces at all, returns false!
        }

        private void RecreateParkingLot()
        {
            //Retrieve database list
            List<Vehicle> list = db.Vehicles.ToList();
            //Reset every parking space
            for (int n = 0; n < parkingLot.Length; n++)
            {
                parkingLot[n] = false;
            }
            //State which parking spaces are occupied
            foreach ( Vehicle vehicle in list )
            {
                if (vehicle.ParkingSpace1 != -1)
                {
                    parkingLot[vehicle.ParkingSpace1] = true;
                }
                if (vehicle.ParkingSpace2 != -1)
                {
                    parkingLot[vehicle.ParkingSpace2] = true;
                }
                if (vehicle.ParkingSpace3 != -1)
                {
                    parkingLot[vehicle.ParkingSpace3] = true;
                }
            }
        }
        
        public static String GetParkingLotOutput(int row)
        {
            String s = "";
            int startIndex = -1;
            switch (row)
            {
                case 1:
                    startIndex = 0;
                    break;
                case 2:
                    startIndex = 10;
                    break;
                case 3:
                    startIndex = 20;
                    break;
            }
            for (int n = startIndex; n < row*10; n++)
            {
                if (parkingLot[n])
                {
                    s += "[#]";
                }
                else
                {
                    s += "[" + n + "]";
                }
            }
            return s;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
