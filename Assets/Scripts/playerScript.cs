using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tileNamespace;
using boardNameSpace;

public class playerScript : MonoBehaviour
{
    public GameObject block;
    private int orderNum;

    public bool placeBlock(Tile t1,Tile t2){
        if(gameObject.GetComponent<BoardManager>().addTile(t1,t2)){
            gameObject.GetComponent<Spawner>().spawnTile((int)t1.getType(),t1.getCoords().Item1,t1.getCoords().Item2,block);
            gameObject.GetComponent<Spawner>().spawnTile((int)t2.getType(),t2.getCoords().Item1,t2.getCoords().Item2,block);
            return true;
        }
        return false;
    }
}
