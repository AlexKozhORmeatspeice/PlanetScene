using UnityEngine;

public interface IScreenObject
{
    string Name { get; }
    Vector3 Position { get; }
    Vector3 Size { get; }
}
