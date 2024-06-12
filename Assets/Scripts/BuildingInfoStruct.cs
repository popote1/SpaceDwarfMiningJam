using System;
using UnityEngine;

[Serializable]
public struct BuildingInfoStruct
{
    public int Mass;
    public int Gaz;
    public int Petrol;

    public GameObject _prefabBuilding;
    public BuildingGhost _buildingGhost;
}
