using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="News Script", menuName = "Enemies")]
public class ScriptableOBJ : ScriptableObject
{
    [SerializeField]
    private int HP;
    [SerializeField] 
    private int MP;
}
