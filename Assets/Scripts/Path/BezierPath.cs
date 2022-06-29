using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour {
    private LineRenderer m_lr;

    private Vector3[] positions;
    public Vector3[] Positions {
        get {
            return positions;
        }
    }

    private int count; 
    public int Count {
        get {
            return count;
        }
    }

    [SerializeField] private Transform[] m_waypoints;

    void Awake(){ 
        m_lr = this.GetComponent<LineRenderer>();

        if(m_lr == null){
            Debug.LogError("LineRenderer not found.");
            return;
        }
        
        if(m_waypoints.Length <= 2){
            Debug.LogError("Can't generate a path, need atleast 2 points");
            return;
        }
        

        count = m_waypoints.Length;
        positions = new Vector3[count];
        for(int i = 0; i < count; i++){
            positions[i] = m_waypoints[i].position; 
        }

        //m_lr.enabled = true;
        m_lr.enabled = false;
        m_lr.positionCount = count; 
        m_lr.SetPositions(positions);

    }
}
