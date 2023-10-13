using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceField : MonoBehaviour
{
    public int currentRiceField=0;
    public ParticleSystem spawnEffect;
    public Material[] riceMaterials;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
       ReviveFarm(); 
    }

    public void GrowFarm()
    {
        var terrains = GetComponentsInChildren<Terrain>(true);
        terrains[currentRiceField].gameObject.SetActive(false);
        currentRiceField++;
        spawnEffect.Play();
        terrains[currentRiceField].gameObject.SetActive(true);
    }

    public void ResetFarm()
    {
        
        var terrains = GetComponentsInChildren<Terrain>(true);
        if (terrains[0].gameObject.activeSelf == false)
        {
            spawnEffect.Play();
            terrains[0].gameObject.SetActive(true);
            terrains[1].gameObject.SetActive(false);
            terrains[2].gameObject.SetActive(false);
            currentRiceField=0;
        }
    }

    public void DeadFarm()
    {
        foreach (var material in riceMaterials)
        {
            material.SetColor("_BaseColor", new Color(0.8584906f,0.4986559f,0.368503f,1f));
        }   
    }

    public void ReviveFarm()
    {
        foreach (var material in riceMaterials)
        {
            material.SetColor("_BaseColor", new Color(1f,1f,1f,1f));
        }       
    }

    public void RemoveFarm()
    {
        var terrains = GetComponentsInChildren<Terrain>(true);
        spawnEffect.Play();
        terrains[0].gameObject.SetActive(false);
        terrains[1].gameObject.SetActive(false);
        terrains[2].gameObject.SetActive(false);
    }

}
