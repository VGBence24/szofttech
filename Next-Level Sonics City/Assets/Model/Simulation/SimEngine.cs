using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System;
using Model;
using Tiles;
using Buildings;
using Statistics;


namespace Simulation
{
    public class SimEngine : MonoBehaviour
    {
        private float _money;
        private DateTime _date;
        private City _city;
        private List<Car> _carsOnRoad;
        private int _timeSpeed;
        private List<Person> _people;
        private StatEngine _statEngine;

        private static Thread _t;
        private static bool _isSimulating = false;
        private static bool _isRunning = false; //dont modify


        public static void ThreadProc()
        {
            
            lock (_isRunning)
            {
				lock (_isSimulating) 
                {
                    if (_isRunning) { return; }
                    _isRunning = true;
                    _isSimulating = true; //in case if user forget to set
                }

            }

            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            while (_isSimulating)
            {

                //TICK
                if (!EditorApplication.isPaused) { Tick(); }

                //TICKING DELAY
                long sleepTime = 1000 / 10 - (DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime);
                if (sleepTime > 0) { Thread.Sleep((int)sleepTime); }
                startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            _isRunning = false;
        }
        // Start is called before the first frame update
        void Start()
        {
            Init();
            StartSimulation();
        }
        void OnApplicationQuit()
        {
            _isSimulating = false;
        }

        private static void Init()
        {
            
        }
        private static void Tick()
        {
            //Do the things that should done during a tick
        }
        private static void StopSimulation()
        {
            _isSimulating = false;
        }
        private static void StartSimulation()
        {
            if (_t == null) { _t = new Thread(new ThreadStart(ThreadProc)); }
            _t.Start();
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        public bool MarkZone(List<Tile> tiles, ZoneBuilding z)
		{
            return true;
            //TODO
		}
        public bool RemoveZone(List<Tile> tiles)
		{
            throw new NotImplementedException();
            //TODO
		}
        public bool BuildService(Tile tile, ServiceBuilding s)
		{
            throw new NotImplementedException();
            //TODO
        }
        public bool Destroy(Tile tile)
		{
           throw new NotImplementedException();
           //TODO
        }
        public bool DestoryForce(Tile tile)
		{
           throw new NotImplementedException();
           //TODO
        }
        private bool BuildByPeople(Tile t,ZoneBuilding z)
		{
           throw new NotImplementedException();
           //TODO
        }
        /*
        private bool LevelUpZone(IBuilding b)
		{
            TODO
		}*/
        public int GetPriceMarkZone(List<Tile> tile,ZoneBuilding z)
		{
            throw new NotImplementedException();
            //TODO
		}
        public int GetPriceRemoveZone(List<Tile> tiles)
		{
            throw new NotImplementedException();
            //TODO
        }
        public int GetPriceBuildService(Tile tile,ServiceBuilding sb)
		{
            throw new NotImplementedException();
            //TODO
        }
        public int GetPriceDestroy(Tile tile)
		{
            throw new NotImplementedException();
            //TODO
        }
        /*
        public int GetPriceLevelUpZone(IBuilding)
		{
            throw new NotImplementedException();
            //TODO
        }*/

        public void SetTax(float f)
		{
            throw new NotImplementedException();
            //TODO
        }
        private bool MoveIn(int i)
		{
            throw new NotImplementedException();
            //TODO
        }
        private bool MoveOut(int i)
		{
            throw new NotImplementedException();
            //TODO
        }
        private void Die(Person person)
		{
            throw new NotImplementedException();
            //TODO
        }
        public int GetMoney()
		{
            throw new NotImplementedException();
            //TODO
        }
        public DateTime Getdate()
		{
            throw new NotImplementedException();
            //TODO
        }
        /*
        public List<IBuilding> GetBuildingsOnFire()
		{
            throw new NotImplementedException();
            //TODO
        }*/
        public List<Car> GetCarsOnRoad()
		{
            throw new NotImplementedException();
            //TODO
        }
        public int GetTimeSpeed()
		{
            throw new NotImplementedException();
            //TODO
        }
        public int SetTimeSpeed()
		{
            throw new NotImplementedException();
            //TODO
        }
    }
}
