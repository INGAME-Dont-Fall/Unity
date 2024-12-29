using System;
using UnityEngine;

public enum Size
{ 
    Small,
    Medium,
    Large,
    Special,
    Assist
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public Size size;
    public int index;
    public float mass;
    public GameObject go;
    public GameObject ui;
}
