using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//leo waters 16/03/2023
//Bullet pool
//Provides pooling for projectiles and functions
public class BulletPoolingSystem : MonoBehaviour
{
    [System.Serializable]
    public class Bullet // stores bullet data
    {
        public GameObject Prefab;
        public AudioClip[] soundEffects;
        public AudioClip getSfx()
        {
            return soundEffects[Random.Range(0, soundEffects.Length)];
        }
    }
    public static BulletPoolingSystem Instance;//singlton
    public AudioSource src;
    public Bullet[] BulletPrefabs;
    public int PoolSize = 30;
    Dictionary<string, GameObject[]> pools;

    //get a bullet from the pool by bullet name
    public GameObject GetBullet(string Type)
    {
        foreach (var bullet in pools[Type])
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }
        Debug.Log("Bullet Pool Size Hit Limit");
        return null;
    }
    //get a audio clip of a bullet by name
    public AudioClip GetAudioClip(string name)
    {
        foreach (var bullet in BulletPrefabs)
        {
            if (bullet.Prefab.name == name)
            {
                return bullet.getSfx();
            }
        }
        Debug.Log("no bullet sfx");
        return null;
    }

    private void Start()
    {
        Instance = this;
        //create bullet pools for each bullet type
        pools = new Dictionary<string, GameObject[]>();
        foreach (var bullet in BulletPrefabs)
        {
            GameObject[] pool=new GameObject[PoolSize];
            for (int i = 0; i < PoolSize; i++)
            {
                pool[i] = Instantiate(bullet.Prefab, transform, true);
            }
            pools.Add(bullet.Prefab.name, pool);
        }
    }

    //fire a bullet from a start towards an enemy, Returns Estimated Travel Time
    public float Fire(Vector3 Start,GameObject Target,string BulletPrefabName)
    {
        GameObject bullet= GetBullet(BulletPrefabName); // get Bullet by Type
        src.PlayOneShot(GetAudioClip(BulletPrefabName));//play bullet shoot SFX
        bullet.transform.position = Start;//set bullet pos
        Projectile proj = bullet.GetComponent<Projectile>();
        proj.Setup(Target);//set bullet targeting data
        bullet.SetActive(true);

        return Vector2.Distance(Start, Target.transform.position) / proj.Speed; //retun the bullet travel time

    }
}
