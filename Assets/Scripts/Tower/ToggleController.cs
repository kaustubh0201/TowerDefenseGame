using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TowerPlacement m_placement;

    private bool m_isFollowingObject;

    private bool m_isThereMoney = true;

    // Start is called before the first frame update
    void Start()
    {
        m_isFollowingObject = false;

        for (int i = 0; i < toggleGroup.transform.childCount; i++){
            int j = i;
            // Debug.Log(j);
            Toggle t = toggleGroup.gameObject.transform.GetChild(j).GetComponent<Toggle>();
            t.onValueChanged.AddListener(delegate {
                ToggleValueChanged(t, j);
            });
        }
    }

    public void setIsThereMoney(bool b){
        m_isThereMoney = b;
    }

    public void ToggleValueChanged(Toggle change, int towerIndex){
        
        if(m_isThereMoney){
            
            // coroutine call
            if(!m_isFollowingObject){
                m_placement.startFollowingObject(towerIndex);
                m_isFollowingObject = true;
            } else {
                m_placement.stopFollowingObject();
                m_placement.startFollowingObject(towerIndex);
            }
        }

        // Debug.Log(towerIndex);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            
            toggleGroup.SetAllTogglesOff();


            if(m_isFollowingObject){
                m_placement.stopFollowingObject();
                m_isFollowingObject = false;
            }
        }
    }
}
