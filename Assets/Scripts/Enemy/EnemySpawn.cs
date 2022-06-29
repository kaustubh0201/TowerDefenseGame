using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform m_spawnPoint;
    

    public EnemyMovement spawnEnemy(EnemySO enemy){
        Vector3 spawnPosition = m_spawnPoint.position + Vector3.up * enemy.height;
        EnemyMovement obj = Instantiate(enemy.prefab, spawnPosition, Quaternion.identity).GetComponent<EnemyMovement>();
        obj.Path = m_spawnPoint.GetComponent<BezierPath>();
        obj.startFollow();

        return obj;
    }
}
