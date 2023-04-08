using System.Collections;
using System.Collections.Generic;
using boardNameSpace;
using tileNamespace;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject block;
    private int orderNum;
    private BoardManager boardManager;
    private string playerName;

    void Awake()
    {
        boardManager = new BoardManager();
    }

    public bool addTile(Tile t1, Tile t2)
    {
        // Check if there is even a space
        // If yes, while loop:
        // Specify the tile orientation and position
        boardManager.addTile(t1, t2);
        // Return 

        // If no, return as a pass
        return false;
    }

    // public bool placeBlock(Tile t1,Tile t2){
    //     if(gameObject.GetComponent<BoardManager>().addTile(t1,t2)){
    //         gameObject.GetComponent<Spawner>().spawnTile((int)t1.getType(),t1.getCoords().Item1,t1.getCoords().Item2,block);
    //         gameObject.GetComponent<Spawner>().spawnTile((int)t2.getType(),t2.getCoords().Item1,t2.getCoords().Item2,block);
    //         return true;
    //     }
    //     return false;
    // }
}
