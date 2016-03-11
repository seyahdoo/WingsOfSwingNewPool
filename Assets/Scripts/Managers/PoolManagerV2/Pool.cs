using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

    private Transform _transform;

    public string ObjectName;
    
    private Stack<GameObject> _stack = new Stack<GameObject>();
    
    void Awake()
    {
        //Cache transform
        _transform = transform;
    }

    private GameObject _original;    

    /// <summary>
    /// The original object to pool
    /// </summary>
    public GameObject Original
    {
        get
        {
            return _original;
        }
        set
        {
            _original = value;
            ObjectName = value.name;
        }
    }


    /// <summary>
    /// Gives One Object from pool.
    /// Usage: GameObject FromPool = Pool.GetOne();
    /// </summary>
    public GameObject GetOne()
    {
        
        GameObject ToReturn;
        
        if (_stack.Count > 0)
        {
            //if there is an object in pool
            ToReturn = _stack.Pop();

            OnPoolObjectOut(ToReturn);
            
            return ToReturn;
        }

        if(_original == null)
        {
            Debug.LogError("Object Pool is empty. And we cant Instantiate new poll member because Original is missing. Returning A cube.");
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        
        Debug.LogWarning("Object Pool For " + _original.name + "is empty!"+
            " Creating a new object and adding to the pool");
        
        //else then make a new object
        ToReturn = Instantiate(_original);
        
        //this "if" is for my compiler :] thanks for all the warnings you give me, i can write code more elegantly.
        //Really thanks... Much thanks..
        if(ToReturn != null)
        {
            OnPoolObjectCreateForInsufficientPool(ToReturn);
            
            
            return ToReturn;
        }

            
        Debug.LogError("::ERROR WE CANT INSTANTIATE A NEW OBJECT IN POOL."+
                        " WE ARE PRETTY FUCKED UP RIGHT NOW:::");
        //aaw nevermind i can give him a cube. that will solve it
        return GameObject.CreatePrimitive(PrimitiveType.Cube); 
    }

    /// <summary>
    /// Returns used object to pool
    /// </summary>
    /// <param name="ToReturn">Used Object</param>
    public void Return(GameObject ToReturn)
    {
        //We assume (ToReturn.name == Original.name) is true

        //Trigger On Pool Object Returned. We want it to be deactivated and ready to be pooled
        OnPoolObjectReturned(ToReturn);

        //And Add to pool as usual
        _stack.Push(ToReturn);
    }

    
    /// <summary>
    /// Headstarts a pool and grows it.
    /// </summary>
    /// <param name="Size">Number of objects to instantiate</param>
    public void InstantiatePool(int Size)
    {
        //Debug.Log("Instantiating pool for , " + ObjectName + "," + Size + "Times");

        //What if there is no original object here
        if(_original == null)
        {
            Debug.LogError("Cannot Instantiate Pool, Original is empty");
            return;
        }

        GameObject ToAdd;

        for (int i = 0; i < Size; i++)
        {
            //make a new Object From Original
            ToAdd = Instantiate(_original);

            //more event thingies
            OnPoolObjectCreate(ToAdd);
            
            //Add to Pool
            _stack.Push(ToAdd);
        }

    }
    

    private void OnPoolObjectOut(GameObject go)
    {
        go.SetActive(true);
        go.transform.SetParent(null);
    }

    private void OnPoolObjectReturned(GameObject go)
    {

        go.transform.SetParent(_transform);
        go.SetActive(false);
    }

    private void OnPoolObjectCreate(GameObject go)
    {
        go.name = ObjectName;

        go.transform.SetParent(_transform);

        //deactivate object for storage
        go.SetActive(false);
    }

    private void OnPoolObjectCreateForInsufficientPool(GameObject go)
    {
        go.transform.SetParent(null);
        go.name = ObjectName;
    }

}
