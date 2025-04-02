using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IPlanetInfo : IScreenObject
{
    List<PointInfo> PointInfos { get; }

    string SectornName { get; }
    string Description { get; }
}

public class PlanetInfo : MonoBehaviour , IPlanetInfo
{
    [Header("Info")]
    [SerializeField] private string planetName;
    [SerializeField] private string sector;
    [SerializeField] private string description;

    [Header("Points of interest")]
    [SerializeField] private List<PointInfo> points;
    
    public string Name => planetName;

    public string Description => description;

    public string SectornName => sector;

    public Vector3 Position => gameObject.transform.position;

    public Vector3 Size => gameObject.transform.localScale;

    List<PointInfo> IPlanetInfo.PointInfos => points;

}
