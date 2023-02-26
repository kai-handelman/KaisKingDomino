using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace tileNamespace
{
    public enum tileType
    {
        King,   //0
        Fields, //1
        GrassLands, //2
        Ocean, //3
        Swamp,  //4
        Forest,  //5
        Mines   //6
    }

    [System.Serializable]
    public class Card
    {   //For Use before a block is placed
        public int cardNum { get; set; }
        public (string type, int value) anchor { get; set; }
        public (string type, int value) secondary { get; set; }

    }

    public class Tile
    {
        private int xCoord;
        private int yCoord;
        private int value;
        private tileType type;

        public Tile(tileType tileT, int x, int y, int v)
        {
            type = tileT;
            xCoord = x;
            yCoord = y;
        }

        public int getValue()
        {
            return value;
        }
        public tileType getType()
        {
            return type;
        }
        public (int, int) getCoords()
        {
            return (xCoord, yCoord);
        }
    }

    public class Deck
    {
        const string fileName = "Assets/Scripts/deck.json";
        private List<Card> deckOfCards;

        //Put this into a json so it not horrendus
        public Deck()
        {
            string jsonString = System.IO.File.ReadAllText(fileName);
            deckOfCards = JsonConvert.DeserializeObject<List<Card>>(jsonString);
            Debug.Log(deckOfCards.Count);
        }


        //Input how cards are drawn per turn (For the # of Players)
        public List<Card> getNext(int drawCount)
        {
            List<Card> drawCards = new List<Card>();
            Card temp;
            for (int i = 0; i < drawCount; i++)
            {                                                 //Randomly selects cards from deck to a the current turn's deck
                temp = deckOfCards[Random.Range(0, deckOfCards.Count)];
                drawCards.Add(temp);
                deckOfCards.Remove(temp);
            }

            drawCards.Sort();                                //Sorts the temp deck out to least to greatest
            foreach (Card da in drawCards)
            {
                Debug.Log(da.cardNum);
            }

            return drawCards;
        }
        public int getCount()
        {                                                                  //See how many cards are left in the deck
            return deckOfCards.Count;
        }


        //Count Point Values and "head count"
        //    |
        //    V
        /**
        // public int getCertainCount(int t){
        //     int count = 0;
        //     foreach((int,(tileType,int),(tileType,int)) i in deckOfCards){
        //         if((int)i.Item2.Item1 == t){
        //             count++;
        //             if(i.Item2.Item2 == 1){
        //                 Debug.Log(i.Item1);
        //             }
        //             
        //         }
        //         if((int)i.Item3.Item1 == t){
        //             count++;
        //             if(i.Item3.Item2 == 1){
        //                 Debug.Log(i.Item1);
        //             }
        //         }
        //     }
        //     return count;
        // }
        // public int getCertainCountV(int t,int v){
        //     int count = 0;
        //     foreach((int,(tileType,int),(tileType,int)) i in deckOfCards){
        //         if((int)i.Item2.Item1 == t && i.Item2.Item2 == v){
        //             count++;
        //         }
        //             
        //         if((int)i.Item3.Item1 == t && i.Item3.Item2 == v){
        //             count++;
        //         }
        //             
        //     }
        //     return count;
        // }**/

    }


}
