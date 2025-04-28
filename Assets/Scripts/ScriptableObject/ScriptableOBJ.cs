using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName ="News Script", menuName = "Enemies", order = 1)]
public class ScriptableOBJ : ScriptableObject
{ 
    
    public int HP;
    
    public int Attacka;
    
    public GameObject prefab;
    
    public int Coins;
}
