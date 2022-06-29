using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/Weapon")]
public class WeaponSO : ScriptableObject {
    [field: SerializeField] public Vector3      m_aabb {get; set;}
    [field: SerializeField] public GameObject   m_prefabObject {get; set;}
    [field: SerializeField] public GameObject   m_fakePrefabObject {get; set;}

    [SerializeField] private int m_costPrice;
    public int CostPrice {
        get{
            return -m_costPrice;
        }   
    }
}

