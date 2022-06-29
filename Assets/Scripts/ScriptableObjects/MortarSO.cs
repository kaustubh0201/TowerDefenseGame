using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MortarSO", menuName = "ScriptableObjects/Mortar")]
public class MortarSO : WeaponSO {
    [field: SerializeField] public float m_rangeRadius  {get; set;} = 10f;
    [field: SerializeField] public uint m_fireDelay     {get; set;} = 2000;
    [field: SerializeField] public uint m_damage        {get; set;} = 20;
    [field: SerializeField] public float m_angVelocity  {get; set;} = 5f;
} 
