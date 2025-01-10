using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FarAttackElemental",menuName = "ScriptableObjects/FarAttackElemental")]
public class ArbaletScriptaleObject : ScriptableObject
{
    public string nameElemental = "fire";
    public GameObject AddObject;
    public float moveSpeed = 22f;
    public GameObject particalOnHitPrefabVFX;
    public float projectileRange = 10f;
    public int damageAttack = 1;
}
