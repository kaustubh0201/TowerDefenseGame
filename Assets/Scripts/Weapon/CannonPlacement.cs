using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPlacement : MonoBehaviour
{

    private HashSet<GameObject> m_nearbyCannons = new HashSet<GameObject>();


    void OnTriggerEnter(Collider other){

        Debug.Log(other.gameObject.name);

        if(other.gameObject.tag != "Weapon")
            return;

        m_nearbyCannons.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other){

        if(other.gameObject.tag != "Weapon")
            return;

        m_nearbyCannons.Remove(other.gameObject);
    }

    public bool isThereNearbyCannons(){
        if(m_nearbyCannons.Count == 0)
            return false;
        else 
            return true;
    }

    public void makeMeVisible(bool b){
        
        MeshRenderer[] mRen = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer m in mRen){
            m.enabled = b;
        }
    }

    public void changeMaterial(Material mat){

        MeshRenderer[] mRem = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer m in mRem){
            m.material = mat;
        }
    }
}
