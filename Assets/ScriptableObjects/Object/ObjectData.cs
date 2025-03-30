using UnityEngine;

public enum Size
{
    Small,
    Medium,
    Large,
    Special,
    Unobtainable
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public Size Size;
    public int Index;
    public float Mass;
    public int difficultyLevel;
    public GameObject go;
    public GameObject ui;
}
