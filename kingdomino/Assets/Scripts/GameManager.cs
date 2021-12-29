using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tileNamespace;
using boardNameSpace;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Deck tileDeck;
    public GameObject obj;
    public GameObject playerPrefab;
    private GameObject[] playerList;
    BoardManager temp;
    void Start()
    {
        // deckHelper();  
        playerHelper();
    }

    public void deckHelper(){
        tileDeck = new Deck();
        List <(int,(tileType,int), (tileType, int))> newDraw = tileDeck.getNext(4);

    }
    //add transforms for where they going to display 
    //let the players choose their pick
    //have the players the selected tile on their board 
    //then place it on their own board

    public void spawnTiles(List <(int,(tileType,int), (tileType, int))> nD){
        foreach((int,(tileType,int), (tileType, int)) s in nD){
            
        }
    }


    public void playerHelper(){
        
        
        addPlayer(1);
        playerList[0].GetComponent<Spawner>().spawnTile(0,0,0,obj);
        playerList[0].GetComponent<playerScript>().placeBlock(new Tile(tileType.Fields,0,1,5),new Tile(tileType.Fields,0,2,5));
        // placeBlock(playerList[0].GetComponent<BoardManager>(),new Tile(tileType.Fields,0,1,5),new Tile(tileType.Fields,0,2,5));
        // placeBlock(players[0],new Tile(tileType.Fields,0,3,5),new Tile(tileType.Fields,0,4,5));
        // placeBlock(players[0],new Tile(tileType.Fields,1,4,5),new Tile(tileType.Fields,2,4,5));
        // placeBlock(players[0],new Tile(tileType.Fields,-1,4,5),new Tile(tileType.Fields,-1,5,5));
    }
    
    public void addPlayer(int num){
        playerList = new GameObject[num];
        for(int i = 0; i<num; i++){
            playerList[i] = Instantiate(playerPrefab, new Vector3(0,0,0),Quaternion.identity);
        }
    }

}
