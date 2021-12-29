using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tileNamespace;

namespace boardNameSpace{
    public class BoardManager : MonoBehaviour
    {
        private int playerNum;
        private (int,int) xValues;
        private (int,int) yValues;
        private bool xCap;
        private bool yCap;
        private List<Tile> tiles = new List<Tile>();
        private GameObject objToSpawn;
        private int boardSize;
        // public BoardManager(int pN,int bS,GameObject item){
        //     playerNum = pN;
        //     objToSpawn = item;
        //     Tile kingTile = new Tile(tileType.King,0,0,5);
        //     xValues = (0,0);
        //     yValues = (0,0);
        //     xCap = false;
        //     yCap = false;
        //     boardSize = bS - 1;
        //     tiles.Add(kingTile);
        // }

        void Awake(){
            Tile kingTile = new Tile(tileType.King,0,0,5);
            xValues = (0,0);
            yValues = (0,0);
            xCap = false;
            yCap = false;
            tiles.Add(kingTile);
            boardSize = 5;
            // Debug.Log("Added in");
        }
        
        public bool addTile(Tile t1,Tile t2){//Will return place tile add to list if everything is valid
            List <(int,int)> pCords1 = getPossibles(t1.getType());
            List <(int,int)> pCords2 = getPossibles(t2.getType());
            if((pCords1.Contains(t1.getCoords()) || pCords2.Contains(t2.getCoords())) && coordsInbounds(t1.getCoords(),t2.getCoords())){
                updateBounds(t1.getCoords());
                tiles.Add(t1);
                updateBounds(t2.getCoords());
                tiles.Add(t2);
                return true;
            }
            Debug.Log("And I oop");
            return false;
            
        }
        
        public List<(int,int)> getPossibles(tileType placingType){       //returns all possible coordinates to add a specific tile given tile type and board bound
            List<(int,int)> pos = new List<(int,int)>();
            foreach(Tile t in tiles){
                if(t.getType() == placingType || t.getType() == 0){         //Checks if king space or same space
                    if(!xCap){
                        pos.Add((t.getCoords().Item1-1, t.getCoords().Item2));
                        pos.Add((t.getCoords().Item1+1, t.getCoords().Item2));  
                    }else{
                        if(t.getCoords().Item1-1 >= xValues.Item1){
                            pos.Add((t.getCoords().Item1-1, t.getCoords().Item2));
                        }
                        if(t.getCoords().Item1+1 <= xValues.Item2){
                            pos.Add((t.getCoords().Item1+1, t.getCoords().Item2));
                        }
                    }
                    if(!yCap){
                        pos.Add((t.getCoords().Item1, t.getCoords().Item2+1));
                        pos.Add((t.getCoords().Item1, t.getCoords().Item2-1));
                    }else{
                        if(t.getCoords().Item2+1 <= yValues.Item2){
                            pos.Add((t.getCoords().Item1, t.getCoords().Item2+1));
                        }
                        if(t.getCoords().Item2-1 >= yValues.Item1){
                            pos.Add((t.getCoords().Item1, t.getCoords().Item2-1));
                        }
                    }
                    
                }
            }
            foreach((int,int)cords in getOccSpace()){
                pos.Remove(cords);
            }
            return pos;
        }
        
        private List<(int,int)> getOccSpace(){                       //helper for getPossible to just return all coordinate occupied by current tiles on board
            List<(int,int)> occupSpace = new List<(int,int)>();
            foreach(Tile t in tiles){
                occupSpace.Add(t.getCoords());
            }
            return occupSpace;
        }

        private void updateBounds((int,int) xy){                     //updates the bounds of the board every time tile is added and will flag if x or y's limit is reached
            if(xy.Item1 < xValues.Item1){
                xValues.Item1 = xy.Item1;
            }else if(xy.Item1 > xValues.Item2){
                xValues.Item2 = xy.Item1;
            }
            if(xy.Item2 < yValues.Item1){
                yValues.Item1 = xy.Item2;
            }else if(xy.Item2 > yValues.Item2){
                yValues.Item2 = xy.Item2;
            }
            if (!xCap && xValues.Item2 - xValues.Item1 >= boardSize){
                // Debug.Log("Flag Worked for x");
                xCap = true;
            }
            if (!yCap && yValues.Item2 - yValues.Item1 >= boardSize){
                yCap = true;
                // Debug.Log("Flag Worked for y");
            }
        }
        
        private bool coordsInbounds((int,int) xy1,(int,int) xy2){
            if(xValues.Item2 - xy1.Item1 > boardSize || xValues.Item2 - xy2.Item1 > boardSize){
                return false;
            }else if(xy1.Item1 - xValues.Item1 > boardSize || xy2.Item1 - xValues.Item1 > boardSize){
                return false;
            }else if(yValues.Item2 - xy1.Item2 > boardSize || yValues.Item2 - xy2.Item2 > boardSize){
                return false;
            }else if(xy1.Item2 - yValues.Item1 > boardSize || xy2.Item2 - yValues.Item1 > boardSize){
                return false;
            }
            return true;
        }
        
        // public void helperAddTiles(){
        //     addTile(new Tile(tileType.Fields,0,1,5),new Tile(tileType.Fields,0,2,5));
        //     addTile(new Tile(tileType.Fields,0,3,5),new Tile(tileType.Fields,0,4,5));
        //     addTile(new Tile(tileType.Fields,1,4,5),new Tile(tileType.Fields,2,4,5));
        //     addTile(new Tile(tileType.Fields,-1,4,5),new Tile(tileType.Fields,-1,5,5));

        //     Debug.Log(xValues);
        //     Debug.Log(yValues);
        // }

    }
}
