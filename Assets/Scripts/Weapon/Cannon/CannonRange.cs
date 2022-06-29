using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CannonRange : MonoBehaviour {
    
    // HashSet of potential targets
    private HashSet<GameObject> m_potTargets; 
    
    // We will register all enemies that enter the range
    void OnTriggerEnter(Collider other){
        
        // checking the tag to ensure that object entered is an enemy 
        if(other.gameObject.tag != "Enemy") return;
        
        // add the delegate
        other.GetComponent<EnemyAI>().deathEvent += onEnemyDeath;
        
        // add the colliding object 
        m_potTargets.Add(other.gameObject);
    }

    // We will remove the target from the list of potential target
    void OnTriggerExit(Collider other){
        // check if it is an enemy
        if(other.gameObject.tag != "Enemy") return;
        
        // remove the delegate
        other.GetComponent<EnemyAI>().deathEvent -= onEnemyDeath;

        // remove the leaving object 
        m_potTargets.Remove(other.gameObject);
    }

    void Awake() {
        m_potTargets = new HashSet<GameObject>();
    }
    
    // to check if the target is still in range
    public bool isTargetInRange(GameObject g) {
        return m_potTargets.Contains(g);
    }
    
    // function to be called by the enemy when it dies
    private void onEnemyDeath(GameObject g){
        m_potTargets.Remove(g);
    }
    
    // to find the next target
    public GameObject getNextTarget(){

        // check if there are potential targets
        if(m_potTargets.Count == 0){
            return null;
        }
        
        // find the one with minimum distance
        /// uses linear search
        GameObject min = null;
        float dist = float.MaxValue;
        float temp;
        foreach (GameObject gb in m_potTargets){
            temp = Vector3.Distance(gb.transform.position, transform.position);  
            if(temp < dist){
                dist = temp;
                min = gb;
            } 
        }
        
        // if the new target is not in range
        if(!m_potTargets.Contains(min)){

            // find another target
            if(m_potTargets.Count > 0){
                min = getNextTarget();
            }

            // no new targets can be found
            else {
                min = null;
            }
        }

        return min;
    }
}
