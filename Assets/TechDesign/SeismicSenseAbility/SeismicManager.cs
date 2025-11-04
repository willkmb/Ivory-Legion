using SeismicSense;
using System.Collections.Generic;
using UnityEngine;

public class SeismicManager : MonoBehaviour
{
    public static SeismicManager instance;

    public List<GameObject> freeReturnPulses;
    public List<GameObject> inactiveReturnPulses;
    private void Awake()
    {
        instance ??= this;
    }
    private void Start()
    {
        SpawnObjectPool();
    }
    private void SpawnObjectPool()
    {
        for (int i = 0; i < 100; i++) // Spawns x amount of obj
        {
            GameObject instantiate = Instantiate(SeismicSenseScript.instance.returnPulse, transform.position, Quaternion.identity); // Spawns in returnPulse(s)
            instantiate.transform.SetParent(this.transform); // Sets parent to an obj "PoolParent"
            freeReturnPulses.Add(instantiate); // Spawns inreturn particle
            instantiate.SetActive(false); //Sets to ianctive so no code runs
        }
    }
    public void CallPoolObj(GameObject parentObj)
    {
        GameObject returnPulse = freeReturnPulses[0];
        returnPulse.SetActive(true);
        returnPulse.transform.SetParent(parentObj.transform);
        returnPulse.transform.position = parentObj.transform.position;
        freeReturnPulses.Remove(returnPulse);
        inactiveReturnPulses.Add(returnPulse);
    }
}
