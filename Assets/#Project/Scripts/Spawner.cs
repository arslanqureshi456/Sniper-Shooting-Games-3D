using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TacticalAI;

public class Spawner : MonoBehaviour
{
    public Transform[] _spawnTransforms;
    public Transform[] _destTransforms;

    //public GameObject[] npcPrefabs;
    public GameObject[] npcPrefabs;
    public float startSpawnDelay = 3f;
    public float spawnDelay = 10f;
    public int maxActiveNpcs = 3;
    public int npcsToSpawn = 5;
    [HideInInspector] public int spawnedNpcAmt;
    [HideInInspector] public bool pauseSpawning;
    [HideInInspector] public int _destTransformIndex = 0;

    private float spawnTime;
    private int _spawnTransformIndex = 0;
    private GameObject npcInstance = null;
    private List<HealthScript> npcs = new List<HealthScript>();

    private IEnumerator Start()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            switch (AutoQualityChooser.finalResult)
            {
                case 0:
                    maxActiveNpcs = 1;//2
                    break;
                case 1:
                    maxActiveNpcs = 2;
                    break;
                case 2:
                    maxActiveNpcs = 3;
                    break;
            }
        }

        pauseSpawning = true;
        yield return new WaitForSeconds(startSpawnDelay);
        pauseSpawning = false;
        spawnTime = -1024f;
    }

    private void Update()
    {
        if (!pauseSpawning)
        {
            if (npcs.Count < maxActiveNpcs && spawnedNpcAmt < npcsToSpawn)
            {
                if (spawnTime + spawnDelay < Time.time)
                    Spawn();
            }
        }
    }

    private void Spawn()
    {
        //int index = 0;
        //if (GameManager.Instance.levelIndexAnalytics < 6)
        //{
        //    index = GameManager.Instance.levelIndexAnalytics - 1;
        //}
        //else
        //{
        //    index = Random.Range(0, npcPrefabs.Length);
        //}
        if (npcPrefabs.Length > 0)//(npcPrefabs[index])
        {
            npcInstance = Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)]/*[index]*/, _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;

            // Set destination point here
            if (_destTransforms.Length > 0)
            {
                npcInstance.GetComponent<BaseScript>().keyTransform = _destTransforms[_destTransformIndex];
                _destTransformIndex++;
                if (_destTransformIndex == _destTransforms.Length)
                    _destTransformIndex = 0;
            }

            HealthScript AIComponent = npcInstance.GetComponent<HealthScript>();
            AIComponent.spawner = this;
            npcs.Add(AIComponent);
            spawnTime = Time.time;
            spawnedNpcAmt++;
            _spawnTransformIndex++;
            if (_spawnTransformIndex == _spawnTransforms.Length)
                _spawnTransformIndex = 0;
        }
    }

    public void UnregisterSpawnedNPC(HealthScript _npcAI)
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i] == _npcAI)
            {
                npcs.RemoveAt(i);
                break;
            }
        }
    }
}