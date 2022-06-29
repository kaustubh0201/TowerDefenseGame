using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour {
    
    [SerializeField] private Camera m_worldCam;
    [SerializeField] private WeaponSO[] m_towers;
    [SerializeField] private ToggleController m_toggleController;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private Material incorrectMaterial;

    private LayerMask m_layerMaskPlane;
    private LayerMask m_layerMaskCannon;
    private GameObject m_tower;
    private IEnumerator m_isFollowingObject;
    private GameObject m_currentTower;
    private WeaponSO m_towerSO;

    void Start(){
        m_layerMaskPlane = LayerMask.GetMask("placementplane");
        m_layerMaskCannon = LayerMask.GetMask("cannonLayer");
        m_isFollowingObject = null;
    }

    void Update() {
    }

    public void startFollowingObject(int towerIndex){
        
        if(m_isFollowingObject == null){
            m_currentTower = m_towers[towerIndex].m_prefabObject;
            m_towerSO = m_towers[towerIndex];
            m_tower = Instantiate(m_towerSO.m_fakePrefabObject, new Vector3(0, 0, 0), Quaternion.identity);
            //m_tower.transform.GetChild(3).gameObject.layer = 0;
            //m_tower.GetComponent<MeshRenderer>().enabled = false;
            m_tower.GetComponent<CannonPlacement>().makeMeVisible(false);
            m_isFollowingObject = followingObject();
            StartCoroutine(m_isFollowingObject);
        }
    }

    private IEnumerator followingObject(){
        float sY = m_towerSO.m_aabb.y / 2;
        bool canPlace = false;

        while(true){
            Vector3 mousePosition = Input.mousePosition;
            Ray r = m_worldCam.ScreenPointToRay(mousePosition);
            RaycastHit h;
            if(Physics.Raycast(r, out h, maxDistance: Mathf.Infinity, layerMask: m_layerMaskPlane)){
                m_tower.transform.position = h.point + new Vector3(0, sY, 0);
                //m_tower.GetComponent<MeshRenderer>().enabled = true;
                m_tower.GetComponent<CannonPlacement>().makeMeVisible(true);
                

                // checking if object is hitting the layer or not
                if(isObjectOnTopPlane(m_towerSO, h)){
                    m_tower.GetComponent<CannonPlacement>().changeMaterial(correctMaterial);
                    canPlace = true;
                    
                    // checking if the object is hitting another object or not
                    // Debug.Log(m_tower);
                    if(m_tower.GetComponent<CannonPlacement>().isThereNearbyCannons()){
                        m_tower.GetComponent<CannonPlacement>().changeMaterial(incorrectMaterial);
                        canPlace = false;
                    }else{
                        m_tower.GetComponent<CannonPlacement>().changeMaterial(correctMaterial);
                        canPlace = true;
                    }

                }else{
                    m_tower.GetComponent<CannonPlacement>().changeMaterial(incorrectMaterial);
                    canPlace = false;
                }                

            }else{
                // m_tower.GetComponent<MeshRenderer>().enabled = false;
                m_tower.GetComponent<CannonPlacement>().makeMeVisible(false);
                canPlace = false;
            }


            if(Input.GetMouseButtonDown(0)){

                if(canPlace){
                    FindObjectOfType<Manager>().changeCurrency(m_towerSO.CostPrice);
                    Instantiate(m_currentTower, h.point + new Vector3(0, sY, 0), Quaternion.identity);
                }

                m_tower.GetComponent<CannonPlacement>().makeMeVisible(false);

                break;                
            }

            yield return null;
        }
    }

    public void stopFollowingObject(){
        if(m_isFollowingObject != null){
            StopCoroutine(m_isFollowingObject);
            m_isFollowingObject = null;
            Destroy(m_tower);
        }        
    }

    public Vector3[] ObjectOnTopDetails(WeaponSO towerSO, RaycastHit h, float height){
        Vector3 localMetric = towerSO.m_aabb / 2;

        float sX = localMetric.x;
        float sY = localMetric.y;
        float sZ = localMetric.z;

        Vector3 point = h.point + h.collider.gameObject.transform.up * (sY + height);

        Vector3 topLeft = new Vector3(-sX, 0f, -sZ) + point;
        Vector3 topRight = new Vector3(sX, 0f, -sZ) + point;
        Vector3 bottomLeft = new Vector3(-sX, 0f, sZ) + point;
        Vector3 bottomRight = new Vector3(sX, 0f, sZ) + point;

        Vector3[] v = {topLeft, topRight, bottomLeft, bottomRight};

        return v;    
        
    }

    public bool isObjectOnTopPlane(WeaponSO towerSO, RaycastHit h){

        Vector3[] v = ObjectOnTopDetails(towerSO, h, 0f);
        
        List<GameObject> g = hittingSameObjects(towerSO.m_fakePrefabObject.transform.up, v[0], v[1], v[2], v[3], m_layerMaskPlane);

        if(g.Count == 4)
            return true;
        else
            return false;

    }


    public List<GameObject> hittingSameObjects(Vector3 up, Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight, LayerMask layerMask){
        
        List<GameObject> g = new List<GameObject>();

        Ray r1 = new Ray(topLeft, -up);
        RaycastHit h1;
        //Debug.DrawRay(topLeft, -up, Color.red, 100f);
        bool b1 = Physics.Raycast(r1, out h1, maxDistance: Mathf.Infinity, layerMask: layerMask);
        //Debug.Log(b1);
        if(b1)
            g.Add(h1.collider.gameObject);      

        Ray r2 = new Ray(topLeft, -up);
        RaycastHit h2;
        //Debug.DrawRay(topLeft, -up, Color.red, 100f);
        bool b2 = Physics.Raycast(r2, out h2, maxDistance: Mathf.Infinity, layerMask: layerMask);
        if(b2)
            g.Add(h2.collider.gameObject);

        //Debug.Log(b1);

        Ray r3 = new Ray(bottomLeft, -up);
        RaycastHit h3;
        //Debug.DrawRay(bottomLeft, -up, Color.red, 100f);
        bool b3 = Physics.Raycast(r3, out h3, maxDistance: Mathf.Infinity, layerMask: layerMask);
        if(b3)
            g.Add(h3.collider.gameObject);
            

        // Debug.Log(b1);

        Ray r4 = new Ray(bottomRight, -up);
        RaycastHit h4;
        // Debug.DrawRay(bottomRight, -up, Color.red, 100f);
        bool b4 = Physics.Raycast(r4, out h4, maxDistance: Mathf.Infinity, layerMask: layerMask);
        if(b4)
            g.Add(h4.collider.gameObject);

        // Debug.Log(b1);

        return g; 
    }
}
