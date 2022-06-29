using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MortarBallRange : MonoBehaviour
{
    // hashset of potential targets
    private HashSet<GameObject> m_potTargets;

    // registering enemies when they enter the range
    void OnTriggerEnter(Collider other){

        // checking if the object entered is an enemy
        if(other.gameObject.tag != "Enemy") 
            return;

        // add the delegate 
        other.GetComponent<EnemyAI>().deathEvent += onEnemyDeath;

        // adding the colliding object
        m_potTargets.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other){

        // checking if it is an enemy
        if(other.gameObject.tag != "Enemy")
            return;

        // removing the delegate 
        other.GetComponent<EnemyAI>().deathEvent -= onEnemyDeath;

        // removing the object when it gets out of range
        m_potTargets.Remove(other.gameObject);
    }

    void Awake(){
        m_potTargets = new HashSet<GameObject>();
    }

    // to check if the target is in range
    public bool isTargetInRange(GameObject g){
        return m_potTargets.Contains(g);
    }

    // function to be called by the when it dies
    private void onEnemyDeath(GameObject g){
        m_potTargets.Remove(g);
    }

    public HashSet<GameObject> getPotTargets(){
        return m_potTargets;
    }
}
