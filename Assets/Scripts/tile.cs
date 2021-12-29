using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace tileNamespace{
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


    public class Tile {
        private int xCoord;
        private int yCoord;
        private int value;
        private tileType type;

        public Tile(tileType tileT, int x, int y, int v){
            type = tileT;
            xCoord = x;
            yCoord = y;
        }
        
        public int getValue(){
            return value;
        } 
        public tileType getType(){
            return type;
        }
        public (int,int) getCoords(){
            return (xCoord,yCoord);
        }
    }

    public class Deck{
        private List<(int,(tileType,int),(tileType,int))> deckCards = new List<(int,(tileType,int),(tileType,int))>();
        public Deck(){
            deckCards.Add((1,(tileType.Fields,0),(tileType.Fields,0)));
            deckCards.Add((2,(tileType.Fields,0),(tileType.Fields,0)));
            deckCards.Add((3,(tileType.Forest,0),(tileType.Forest,0)));
            deckCards.Add((4,(tileType.Forest,0),(tileType.Forest,0)));
            deckCards.Add((5,(tileType.Forest,0),(tileType.Forest,0)));
            deckCards.Add((6,(tileType.Forest,0),(tileType.Forest,0)));
            deckCards.Add((7,(tileType.Ocean,0),(tileType.Ocean,0)));
            deckCards.Add((8,(tileType.Ocean,0),(tileType.Ocean,0)));
            deckCards.Add((9,(tileType.Ocean,0),(tileType.Ocean,0)));
            deckCards.Add((10,(tileType.GrassLands,0),(tileType.GrassLands,0)));
            deckCards.Add((11,(tileType.GrassLands,0),(tileType.GrassLands,0)));
            deckCards.Add((12,(tileType.Swamp,0),(tileType.Swamp,0)));
            deckCards.Add((13,(tileType.Fields,0),(tileType.Forest,0)));
            deckCards.Add((14,(tileType.Fields,0),(tileType.Ocean,0)));
            deckCards.Add((15,(tileType.Fields,0),(tileType.GrassLands,0)));
            deckCards.Add((16,(tileType.Fields,0),(tileType.Swamp,0)));
            deckCards.Add((17,(tileType.Forest,0),(tileType.Ocean,0)));
            deckCards.Add((18,(tileType.Forest,0),(tileType.GrassLands,0)));
            deckCards.Add((19,(tileType.Fields,1),(tileType.Forest,0)));
            deckCards.Add((20,(tileType.Fields,1),(tileType.Ocean,0)));
            deckCards.Add((21,(tileType.Fields,1),(tileType.GrassLands,0)));
            deckCards.Add((22,(tileType.Fields,1),(tileType.Swamp,0)));
            deckCards.Add((23,(tileType.Fields,1),(tileType.Mines,0)));
            deckCards.Add((24,(tileType.Forest,1),(tileType.Fields,0)));
            deckCards.Add((25,(tileType.Forest,1),(tileType.Fields,0)));
            deckCards.Add((26,(tileType.Forest,1),(tileType.Fields,0)));
            deckCards.Add((27,(tileType.Forest,1),(tileType.Fields,0)));
            deckCards.Add((28,(tileType.Forest,1),(tileType.Ocean,0)));
            deckCards.Add((29,(tileType.Forest,1),(tileType.GrassLands,0)));
            deckCards.Add((30,(tileType.Ocean,1),(tileType.Fields,0)));
            deckCards.Add((31,(tileType.Ocean,1),(tileType.Fields,0)));
            deckCards.Add((32,(tileType.Ocean,1),(tileType.Forest,0)));
            deckCards.Add((33,(tileType.Ocean,1),(tileType.Forest,0)));
            deckCards.Add((34,(tileType.Ocean,1),(tileType.Forest,0)));
            deckCards.Add((35,(tileType.Ocean,1),(tileType.Forest,0)));
            deckCards.Add((36,(tileType.Fields,0),(tileType.GrassLands,1)));
            deckCards.Add((37,(tileType.Ocean,0),(tileType.GrassLands,1)));
            deckCards.Add((38,(tileType.Fields,0),(tileType.Swamp,1)));
            deckCards.Add((39,(tileType.GrassLands,0),(tileType.Swamp,1)));
            deckCards.Add((40,(tileType.Mines,1),(tileType.Fields,0)));
            deckCards.Add((41,(tileType.Fields,0),(tileType.GrassLands,2)));
            deckCards.Add((42,(tileType.Ocean,0),(tileType.GrassLands,2)));
            deckCards.Add((43,(tileType.Fields,0),(tileType.Swamp,2)));
            deckCards.Add((44,(tileType.GrassLands,0),(tileType.Swamp,2)));
            deckCards.Add((45,(tileType.Mines,2),(tileType.Fields,0)));
            deckCards.Add((46,(tileType.Swamp,0),(tileType.Mines,2)));
            deckCards.Add((47,(tileType.Swamp,0),(tileType.Mines,2)));
            deckCards.Add((48,(tileType.Fields,0),(tileType.Mines,3)));
        }
        
        public List<(int, (tileType,int),(tileType,int))> getNext(int drawCount){
            List<(int,(tileType,int),(tileType,int))> drawCards = new List<(int,(tileType,int),(tileType,int))>();
            (int,(tileType,int),(tileType,int)) temp;
            for(int i = 0; i < drawCount; i++){
                temp = deckCards[Random.Range(0,deckCards.Count)];
                drawCards.Add(temp);
                deckCards.Remove(temp);
            }
            drawCards.Sort();
            foreach((int,(tileType,int),(tileType,int)) da in drawCards){
                Debug.Log(da.Item1);
            }
            for(int i = 0; i < drawCount; i++){
                drawCards[i] = (i,drawCards[i].Item2,drawCards[i].Item3);
            }
            return drawCards;
        }
        public int getCount(){
            return deckCards.Count;
        }
        public int getCertainCount(int t){
            int count = 0;
            foreach((int,(tileType,int),(tileType,int)) i in deckCards){
                if((int)i.Item2.Item1 == t){
                    count++;
                    if(i.Item2.Item2 == 1){
                        Debug.Log(i.Item1);
                    }
                    
                }
                if((int)i.Item3.Item1 == t){
                    count++;
                    if(i.Item3.Item2 == 1){
                        Debug.Log(i.Item1);
                    }
                }
            }
            return count;
        }
        public int getCertainCountV(int t,int v){
            int count = 0;
            foreach((int,(tileType,int),(tileType,int)) i in deckCards){
                if((int)i.Item2.Item1 == t && i.Item2.Item2 == v){
                    count++;
                }
                    
                if((int)i.Item3.Item1 == t && i.Item3.Item2 == v){
                    count++;
                }
                    
            }
            return count;
        }

    }
    

}
