using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingSO : ScriptableObject
{
    
    
    public _BaseBuilding prefab;
    public List<int> buildingWidthListFromTopToBottom;
    public new string name;


}
