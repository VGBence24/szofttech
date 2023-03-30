using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public abstract class Tile
    {
        public uint DesignID { get; protected set; }
        public Vector3 Coordinates { get; protected set; }


        public void Build()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            //TODO
            throw new NotImplementedException();
        }

        public int GetMaintainanceCost()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}