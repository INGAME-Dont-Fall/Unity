using System.Collections.Generic;
using UnityEngine;

public class UmbrellaStand : Special
{
    [SerializeField] private List<Vector3> Rotation;
    [SerializeField] private List<Vector3> Position;

    private int count = -1;

    public bool IncreaseCount()
    {
        if (count < 2)
        {
            count++;
            return true;
        }
        return false;
    }

    public Vector3 GetPosition()
    {
        return Position[count];
    }
    public Vector3 GetRotation()
    {
        return Rotation[count];
    }
}
