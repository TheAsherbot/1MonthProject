using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingSO : ScriptableObject
{


    public _BaseBuilding prefab;
    public List<Grid.BuildingLayer> buildingLeyerListFromBottomToTop;
    public new string name;


}
