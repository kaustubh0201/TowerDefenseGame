using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [SerializeField] private int health;

    private int m_health;

    void Start(){
        m_health = health;
    }

    public int Health{
        get {
            return m_health;
        }
    }

    public void reduceHealth(uint amt){
        m_health -= (int)amt;

        if(m_health <= 0){
            GetComponent<EnemyMovement>().setDeathState(1);
            Destroy(this.gameObject, 0.2f);
        }
    }
    
    //public delegate void onDeathDelegate(GameObject go);
    //public event onDeathDelegate deathEvent; 
    public Action<GameObject> deathEvent;
    void OnDestroy(){
        if(deathEvent != null){
            deathEvent(this.gameObject);
        }
    }
}
