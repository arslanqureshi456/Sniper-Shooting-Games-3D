using UnityEngine;
using System.Collections;

namespace TacticalAI
{
    public class SpawnerScript : MonoBehaviour
    {
        public bool isGernadeScene = false;
        public Wave[] waves;
        public Transform[] spawnPoints;
        public Transform patrolPoint;
        public Transform escapePoint;
        public int currentWave = 0;
        public int enemiesLeft = 0;
        public bool spawnerActive = true;
        float timeTilNextWave = 0;
        public float timeBeforeFirstWave = 1;
        public float timeBetweenWaves = 3;
        public float spawnPointDiameter = 10.0f;
        public SniperLevels levelInfo;
        BaseScript baseScript;
        void OnEnable()//Start
        {
           
            timeTilNextWave = timeBeforeFirstWave;
        }

        void Update()
        {
            if (spawnerActive && timeTilNextWave < 0 && enemiesLeft <= 0 && currentWave < waves.Length)
            {
                StartWave();
            }

            timeTilNextWave -= Time.deltaTime;
        }

        void StartWave()
        {

            for (int i = 0; i < waves[currentWave].agents.Length; i++)
            {
                Transform spawnPointNow = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 positionNow = spawnPointNow.position;
                positionNow.x += (Random.value - 0.5f) * spawnPointDiameter;
                positionNow.z += (Random.value - 0.5f) * spawnPointDiameter;

                GameObject prefab = (GameObject)GameObject.Instantiate(waves[currentWave].agents[Random.Range(0, waves.Length)], positionNow, spawnPointNow.rotation);
                if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
                {
                    baseScript = prefab.GetComponent<BaseScript>();
                    switch (levelInfo.start)
                    {
                        case SniperLevels.StartStates.isStatic:
                            baseScript.isStatic = true;
                            break;
                        case SniperLevels.StartStates.isWalking:
                            baseScript.SetMoveTarget(patrolPoint);
                            break;
                        case SniperLevels.StartStates.isSitting:
                            baseScript.ForceSit();
                            break;
                        case SniperLevels.StartStates.isDelayedRetaliate:
                            baseScript.SetMoveWithDelayedRetaliate(patrolPoint, levelInfo.retaliateDelay);
                            break;
                        case SniperLevels.StartStates.isRunning:
                            baseScript.SetMoveTarget(patrolPoint);
                            break;
                    }
                    switch (levelInfo.firstBullet)
                    {
                        case SniperLevels.BulletReactions.alert:
                            GameManager.firstBulletDelegate += baseScript.AlertAnim;
                            break;
                        case SniperLevels.BulletReactions.retaliate:
                            GameManager.firstBulletDelegate += baseScript.Retaliate;
                            break;
                        case SniperLevels.BulletReactions.run:
                            GameManager.firstBulletDelegate += baseScript.Runaway;
                            break;
                    }
                    switch (levelInfo.secondBullet)
                    {
                        case SniperLevels.BulletReactions.alert:
                            GameManager.secondBulletDelegate += baseScript.AlertAnim;
                            break;
                        case SniperLevels.BulletReactions.retaliate:
                            GameManager.secondBulletDelegate += baseScript.Retaliate;
                            break;
                        case SniperLevels.BulletReactions.run:
                            baseScript.escapeLocation = escapePoint;
                            GameManager.secondBulletDelegate += baseScript.Runaway;
                            break;
                    }
                }
                if (isGernadeScene)
                {
                    prefab.GetComponent<GunScript>().FireOneGrenade();
                }
                prefab.SendMessage("SetWaveSpawner", this);
                enemiesLeft++;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(spawnPoints[i].position, spawnPointDiameter/2.0f);
            }
        }

        void EndWave()
        {
            currentWave++;
            timeTilNextWave = timeBetweenWaves;
        }

        public void AgentDied()
        {
            enemiesLeft--;
            if (enemiesLeft <= 0)
            {
                EndWave();
            }
        }
    }
}

namespace TacticalAI
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] agents;
    }
}