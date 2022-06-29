using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/Level")]
public class LevelSO : ScriptableObject {
    [SerializeField] private int m_coins = 500;

    public int Coins {
        get {
            return m_coins;
        } 
        set {
            m_coins = value;
        }
    }

    public void changeCoins(int amt){
        if( (m_coins + amt) < 0){ 
            m_coins = 0; 
            return; 
        }


        m_coins += amt;    
    }
}
