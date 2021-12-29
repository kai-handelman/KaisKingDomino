using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Material[] mat;
    public void spawnTile(int type,int x, int y, GameObject obj){
        obj.GetComponent<Renderer>().material = mat[type];
        Instantiate(obj, new Vector3(x,y,0),Quaternion.identity);
    }
}
