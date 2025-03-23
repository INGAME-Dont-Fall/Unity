using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Daruma : Special
{
    private List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            objects.Add(child.gameObject);
        }
        objects.Reverse();
    }

    public bool RemoveBlock()
    {
        if(objects.Count > 1)
        {
            Destroy(objects[0]);
            objects.RemoveAt(0);
            return true;
        }
        return false;
    }
}
