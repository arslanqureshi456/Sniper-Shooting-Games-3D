using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TacticalAI;
using UnityEngine.Analytics;

public class CS_Spawner : MonoBehaviour
{
    public Transform[] _spawnTransforms;
    public Transform[] _destTransforms;

    public GameObject[] npcPrefabs, bossPrefabs;
    public float startSpawnDelay = 3f;
    public float spawnDelay = 10f;
	public int maxActiveNpcs = 3;
	public int npcsToSpawn = 5;
    [HideInInspector] public int spawnedNpcAmt;
    [HideInInspector] public bool pauseSpawning;
    [HideInInspector] public int _destTransformIndex = 0;

    private float spawnTime;
    private int _spawnTransformIndex = 0;
    private int _lastSpawnTransformIndex = 0;
	private GameObject npcInstance = null;
    private List<HealthScript> npcs = new List<HealthScript>();
    private List<CS_Enemy> npcs_CS = new List<CS_Enemy>();

    private IEnumerator Start()
    {
        pauseSpawning = true;
        yield return new WaitForSeconds(0.1f);
        pauseSpawning = false;
        spawnTime = -1024f;
    }

    private void Update()
    {
        if (!pauseSpawning)
        {
            if (npcs_CS.Count < maxActiveNpcs && spawnedNpcAmt < npcsToSpawn)
            {
                if (spawnTime + spawnDelay < Time.time)
                    Spawn();
            }
        }

        //if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        //{
        //    if (!pauseSpawning)
        //    {
        //        if (npcs.Count < maxActiveNpcs && spawnedNpcAmt < npcsToSpawn)
        //        {
        //            if (spawnTime + spawnDelay < Time.time)
        //                Spawn();
        //        }
        //    }
        //}
        //else
        //{
        //    if (!pauseSpawning)
        //    {
        //        if (npcs_CS.Count < maxActiveNpcs && spawnedNpcAmt < npcsToSpawn)
        //        {
        //            if (spawnTime + spawnDelay < Time.time)
        //                Spawn();
        //        }
        //    }
        //}
    }

    private void Spawn()
    {
        if (npcPrefabs.Length > 0)
        {
            if (GameManager.Instance.levelIndex.Equals(5) && bossPrefabs.Length > 0)
            {
                if(spawnedNpcAmt.Equals(npcsToSpawn - 1))
                {
                    npcInstance = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;
                }
                else
                    npcInstance = Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;
            }
            else if (GameManager.Instance.levelIndex.Equals(10) && bossPrefabs.Length > 0)
            {
                if (spawnedNpcAmt.Equals(2))
                {
                    npcInstance = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;
                }
                else
                    npcInstance = Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;
            }
            else
                npcInstance = Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;

            // Set destination point here
            if (npcInstance.GetComponent<CS_Enemy>())
            {
                if (_destTransforms.Length > 0)
                {
                    npcInstance.GetComponent<CS_Enemy>().destinationPoint = _destTransforms[_destTransformIndex];
                    _destTransformIndex++;

                    if (_destTransformIndex == _destTransforms.Length)
                        _destTransformIndex = _lastSpawnTransformIndex;
                }

                CS_Enemy AIComponent = npcInstance.GetComponent<CS_Enemy>();
                AIComponent.spawner = this;
                npcs_CS.Add(AIComponent);
                spawnTime = Time.time;
                spawnedNpcAmt++;
                _spawnTransformIndex++;
                if (_spawnTransformIndex == _spawnTransforms.Length)
                    _spawnTransformIndex = 0;
            }
            //else if (npcInstance.GetComponent<BaseScript>())
            //{
            //    if (_destTransforms.Length > 0)
            //    {
            //        npcInstance.GetComponent<BaseScript>().keyTransform = _destTransforms[_destTransformIndex];
            //        _destTransformIndex++;
            //        if (_destTransformIndex == _destTransforms.Length)
            //            _destTransformIndex = 0;
            //    }

            //    HealthScript AIComponent = npcInstance.GetComponent<HealthScript>();
            //    AIComponent.spawner = this;
            //    npcs.Add(AIComponent);
            //    spawnTime = Time.time;
            //    spawnedNpcAmt++;
            //    _spawnTransformIndex++;

            //    if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)// Assault
            //    {
            //        if (GameManager.Instance.levelIndex.Equals(5))
            //        {
            //            if (bossPrefabs.Length > 0)
            //            {
            //                if (spawnedNpcAmt == npcsToSpawn - 1)
            //                {
            //                    npcInstance = Instantiate(npcPrefabs[bossPrefabs.Length], _spawnTransforms[_spawnTransformIndex].position, _spawnTransforms[_spawnTransformIndex].rotation) as GameObject;
            //                }
            //            }
            //        }
            //    }

            //    if (_spawnTransformIndex == _spawnTransforms.Length)
            //        _spawnTransformIndex = 0;
            //}
        }
    }

    public void UnregisterSpawnedNPC(GameObject _npcAI)
    {
        //if(_npcAI.GetComponent<HealthScript>())
        //{
        //    for (int i = 0; i < npcs.Count; i++)
        //    {
        //        if (npcs[i] == _npcAI.GetComponent<HealthScript>())
        //        {
        //            npcs.RemoveAt(i);
        //            break;
        //        }
        //    }
        //}
        /*else*/ if(_npcAI.GetComponent<CS_Enemy>())
        {
            for (int i = 0; i < npcs_CS.Count; i++)
            {
                if (npcs_CS[i] == _npcAI.GetComponent<CS_Enemy>())
                {
                    for(int j = 0; j < _destTransforms.Length; j++)
                    {
                        if(npcs_CS[i].destinationPoint == _destTransforms[j])
                        {
                            _lastSpawnTransformIndex = j;
                        }
                    }
                    
                    _destTransformIndex = _lastSpawnTransformIndex;
                    npcs_CS.RemoveAt(i);
                    break;
                }
            }

            if (npcs_CS.Count.Equals(0))
            {
                PlayerPrefs.SetInt("TotalWaves", PlayerPrefs.GetInt("TotalWaves") + 1);
                Analytics.CustomEvent("CSWave", new Dictionary<string, object>
                {
                { "level_index", PlayerPrefs.GetInt("CoverStrikeCurrentLevel") },
                { "Count", PlayerPrefs.GetInt("TotalWaves") }
                });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "CSWave");
#endif
                //Analytics.CustomEvent("CSWave", new Dictionary<string, object>
                //{
                //{ "V" + Application.version, PlayerPrefs.GetInt("CoverStrikeCurrentLevel") },
                //{ "Count", PlayerPrefs.GetInt("TotalWaves") }
                //});
            }

            if (npcs_CS.Count.Equals(0) && Player.Instance.waveObjectcurrentIndex != 0 &&
                spawnedNpcAmt.Equals(npcsToSpawn))
                StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(1.0f);
        Player.Instance.StartNextWave();
    }

    private void OnDisable()
    {
        StopCoroutine("StartNextWave");
    }
}
