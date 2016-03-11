using UnityEngine;
using System.Collections;

public class PoolTester2 : MonoBehaviour {

    //public Pool p;
    AssetManager am;

	void Awake()
    {
        am = AssetManager.Instance;
        StartCoroutine(LateAwake());
        
    }

    IEnumerator LateAwake()
    {
        yield return new WaitForSeconds(1.0f);

        //p.InstantiatePool(10);
        GameObject go = am.GetOne("SamplePrefab");
        go.transform.position = Vector3.up;

        //go.Release();

        //p.Return(go);

        //go = p.GetOne();
        //go.transform.position = Vector3.zero;
        yield break;
    }



}
