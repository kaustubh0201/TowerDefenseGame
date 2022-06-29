using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MortarBallSO", menuName = "ScriptableObjects/MortarBall")]
public class MortarBallSO : ScriptableObject {
    [field: SerializeField] public float m_speed        {get; set;} = 12f;
    [field: SerializeField] public float m_damageRadius {get; set;} = 5f; 
}
