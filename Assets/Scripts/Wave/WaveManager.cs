using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private WaveData[] m_waves;
    public WaveData[] Waves {
        set {
            m_waves = value;
        }
    } 
    
    private uint m_curWave = 0;
    private uint m_currentTotalEnemies;

    public uint getCurrentWave(){
        return m_curWave;
    }
    
    [SerializeField] private EnemySpawn[] m_spawnPoints;

    public IEnumerator startNextWave(){
        WaveData wd = m_waves[m_curWave];
        m_currentTotalEnemies = wd.NumEnemies;

        for (uint i = 0; i != wd.NumEnemies; i++){
            foreach (EnemySpawn sPoint in m_spawnPoints){
                EnemyMovement obj = sPoint.spawnEnemy(wd.Enemy);
                obj.gameObject.GetComponent<EnemyAI>().deathEvent += decrementEnemies;
            }

            yield return new WaitForSeconds(wd.WaitTime);
        }

        m_curWave += 1;
    }

    public IEnumerator waitTillEnemiesDie(){
        while(getLeftEnemies() != 0)
            yield return null;
    }

    public uint getLeftEnemies(){
        return m_currentTotalEnemies;
    }

    private void decrementEnemies(GameObject g){
        m_currentTotalEnemies -= 1;
    }
}