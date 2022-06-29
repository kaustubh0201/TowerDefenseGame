using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CannonBallSO", menuName = "ScriptableObjects/CannonBall")]
public class CannonBallSO : ScriptableObject {
    [field: SerializeField] public float m_speed {get; set;} = 1f;
    [field: SerializeField] public float m_radius {get; set;} = 1f;
}
