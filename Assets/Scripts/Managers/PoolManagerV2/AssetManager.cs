using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Dont change name of object you got from GetOne(), Release() is name sensetive
/// </summary>
public class AssetManager : Singleton<AssetManager>
{

    //get by name
    //release by name, no other way to do it???    
    //sorry for bad implementation.

    public ToBePoolObject[] ObjectsToInitialize;

    public Dictionary<string, PoolObject> PoolDictionary = new Dictionary<string, PoolObject>();
    [System.Serializable]
    public class PoolObject
    {
        public GameObject Sample;
        public bool ToBePooled;
        public int DesiredPoolSize = 20;
        public Pool Pool;
    }

    [System.Serializable]
    public class ToBePoolObject
    {
        public GameObject Sample;
        public int PoolSize;
    }


    void Awake()
    {
        Initialize(); 
    }


    public static GameObject getOne(string ObjectName){return Instance.GetOne(ObjectName);}
    public GameObject GetOne(string ObjectName)
    {

        PoolObject poo;
        
        if(!(PoolDictionary.TryGetValue(ObjectName,out poo)))
        {
            Debug.LogWarning("No definition for " + ObjectName + "! Giving a Cube.");
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        
        
        if (poo == null)
        {
            Debug.LogWarning("AssetManager: Definition error!!! on " + ObjectName);
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        if(poo.ToBePooled)
        {
            //try to return from pool
            Pool p;

            p = poo.Pool;

            if (p != null)
            {
                return p.GetOne();
            }

            //GameObject go = new GameObject(ObjectName + " Pool");
            //go.transform.SetParent(this.transform);
            //p = go.AddComponent<Pool>();
            //
            //p.ObjectName = poo.Sample.name;
            //p.Original = poo.Sample;
            //p.InstantiatePool(poo.DesiredPoolSize);
            poo.Pool = CreatePool(poo.Sample,poo.DesiredPoolSize,true);
            
            return poo.Pool.GetOne();
        }

        //create one
        if(poo.Sample == null)
        {
            Debug.LogWarning("There is no sample for " + ObjectName + "! returning cube");
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        GameObject ToReturn = Instantiate(poo.Sample);

        if (ToReturn == null)
        {
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        return ToReturn;
    }


    public static void release(GameObject ToRelease){Instance.Release(ToRelease);}
    public void Release(GameObject ToRelease)
    {
        string _name = ToRelease.name;

        PoolObject poo;

        if (!(PoolDictionary.TryGetValue(_name, out poo)))
        {
            Debug.LogWarning("No definition for " + _name + " Destroying.");
            Destroy(ToRelease);
            return;
        }

        Pool p = poo.Pool;

        if (p == null)
        {
            Debug.LogWarning("No pool for " + _name + " Destroying.");
            Destroy(ToRelease);
            return;
        }

        //Debug.Log("Returned " + _name);
        p.Return(ToRelease);
    }

    /// <summary>
    /// Sample is name sensetive
    /// PoolSize 0 is no pool
    /// </summary>
    /// <param name="Sample"></param>
    /// <param name="PoolSize">0 is no pool</param>
    public void CreateDefinition(GameObject Sample,int PoolSize)
    {
        PoolObject p = new PoolObject();
        p.Sample = Sample;
        if(PoolSize > 0)
        {
            p.ToBePooled = true;
            p.DesiredPoolSize = PoolSize;
            
        }else
        {
            p.ToBePooled = false;
            p.DesiredPoolSize = 0;
        }

        PoolDictionary.Add(Sample.name, p);
    }
    public Pool CreatePool(GameObject Sample, int Size, bool ToInstantiate)
    {
        //todo, fix this

        //Pool p = new Pool();
        //p.Original = PoolDictionary[name].Sample;
        //p.InstantiatePool(PoolDictionary[name].DesiredPoolSize);

        Pool p;

        GameObject go = new GameObject(Sample.name + " Pool");
        go.transform.SetParent(this.transform);
        p = go.AddComponent<Pool>();

        p.ObjectName = Sample.name;
        p.Original = Sample;
        p.InstantiatePool(Size);

        return p;
    }


    public void Initialize()
    {
        foreach (var obj in ObjectsToInitialize)
        {
            CreateDefinition(obj.Sample, obj.PoolSize);
        }



    }


}

/// <summary>
/// Extend Gameobject and Component to use this.Release();
/// </summary>
public static class PoolExtension
{
    public static void Release<T>(this T prefab) where T : Component
    {
        AssetManager.Instance.Release(prefab.gameObject);
    }

    public static void Release(this GameObject prefab)
    {
        AssetManager.Instance.Release(prefab);
    }

}

