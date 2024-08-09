using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRecord
{
    public int mapX, mapZ, radius;
    public int[,] details;

    public TrailRecord(int x, int z, int r, int[,] d)
    {
        mapX = x;
        mapZ = z;
        radius = r;
        details = d;
    }
}

