using System.Collections;
using System.Collections.Generic;
using boardNameSpace;
using tileNamespace;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Deck tileDeck;
    public GameObject obj;
    public GameObject playerPrefab;
    private GameObject[] playerList;
    private int playerCount;
    BoardManager temp;

    void Start()
    {
        // deckHelper();  
        playerCount = 1;
        playerHelper(playerCount);
    }

    public void playerHelper(int playerCount)
    {
        tileDeck = new Deck();
        instantiatePlayers(playerCount);

        playerList[0].GetComponent<Spawner>().spawnTile(0, 0, 0, obj);         //Spawns Middle Piece

    }



    ///////////////////////////
    // Non-Test Functions
    ///////////////////////////

    public void gameInitilaizer()
    {
        tileDeck = new Deck();
        instantiatePlayers(playerCount);
    }


    public void getNextSet()
    {
        List<Card> newDraw = tileDeck.getNext(playerCount);
    }
    //add transforms for where they going to display 
    //let the players choose their pick
    //have the players the selected tile on their board 
    //then place it on their own board


    public void instantiatePlayers(int num)
    {
        playerList = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            playerList[i] = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        //Add some sort of way to initialize block choosing order for first round
    }

}
