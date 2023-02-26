using System.Collections;
using System.Collections.Generic;
using tileNamespace;
using UnityEngine;

namespace boardNameSpace
{
    public class BoardManager : MonoBehaviour
    {
        private int playerNum;
        private (int, int) xValues;
        private (int, int) yValues;
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

        void Awake()
        {
            Tile kingTile = new Tile(tileType.King, 0, 0, 5);
            xValues = (0, 0);
            yValues = (0, 0);
            xCap = false;
            yCap = false;
            tiles.Add(kingTile);
            boardSize = 5;
        }

        public bool addTile(Tile t1, Tile t2)
        {//Will return place tile add to list if everything is valid
            if (coordsInbounds(t1.getCoords(), t2.getCoords()) && isValidPlace(t1.getType(), t2.getType(), t1.getCoords(), t2.getCoords()))
            {
                updateBounds(t1.getCoords());
                tiles.Add(t1);
                updateBounds(t2.getCoords());
                tiles.Add(t2);
                return true;
            }
            return false;
        }

        public bool isValidPlace(tileType anchorTileType, tileType secondaryTileType, (int, int) anchCoord, (int, int) secondCoor)
        {
            Dictionary<(int, int), tileType> occupiedSpaces = getOccSpaces();

            if (occupiedSpaces.ContainsKey(anchCoord) || occupiedSpaces.ContainsKey(secondCoor))
            {
                return false;         //False forr Overlaping
            }

            //Check if the anchCoord or secondCoord will be out of bounds


            if (!matchingNeighbors(occupiedSpaces, anchCoord, anchorTileType))
            {
                return false;         //There were no neighbors for anchor
            }

            if (!matchingNeighbors(occupiedSpaces, secondCoor, secondaryTileType))
            {
                return false;         //There were no neighbors for secondary
            }

            return true;
            //Add then and update bounds

        }

        public bool matchingNeighbors(Dictionary<(int, int), tileType> occupiedSpaces, (int, int) target, tileType tt)
        {
            (int, int) tempTarget = (target.Item1 + 1, target.Item2);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target] == tt)
            {
                return true;
            }
            tempTarget = (target.Item1 - 1, target.Item2);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target] == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 + 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target] == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 - 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target] == tt)
            {
                return true;
            }
            return false;
        }

        private Dictionary<(int, int), tileType> getOccSpaces()
        {                       //helper for getPossible to just return all coordinate occupied by current tiles on board
            Dictionary<(int, int), tileType> occupSpace = new Dictionary<(int, int), tileType>();
            foreach (Tile t in tiles)
            {
                occupSpace.Add(t.getCoords(), t.getType());
            }
            return occupSpace;
        }

        private void updateBounds((int, int) xy)
        {                     //updates the bounds of the board every time tile is added and will flag if x or y's limit is reached
            if (xy.Item1 < xValues.Item1)
            {
                xValues.Item1 = xy.Item1;
            }
            else if (xy.Item1 > xValues.Item2)
            {
                xValues.Item2 = xy.Item1;
            }
            if (xy.Item2 < yValues.Item1)
            {
                yValues.Item1 = xy.Item2;
            }
            else if (xy.Item2 > yValues.Item2)
            {
                yValues.Item2 = xy.Item2;
            }
            if (!xCap && xValues.Item2 - xValues.Item1 >= boardSize)
            {
                // Debug.Log("Flag Worked for x");
                xCap = true;
            }
            if (!yCap && yValues.Item2 - yValues.Item1 >= boardSize)
            {
                yCap = true;
                // Debug.Log("Flag Worked for y");
            }
        }

        private bool coordsInbounds((int, int) xy1, (int, int) xy2)
        {
            if (xValues.Item2 - xy1.Item1 > boardSize || xValues.Item2 - xy2.Item1 > boardSize)
            {
                return false;
            }
            else if (xy1.Item1 - xValues.Item1 > boardSize || xy2.Item1 - xValues.Item1 > boardSize)
            {
                return false;
            }
            else if (yValues.Item2 - xy1.Item2 > boardSize || yValues.Item2 - xy2.Item2 > boardSize)
            {
                return false;
            }
            else if (xy1.Item2 - yValues.Item1 > boardSize || xy2.Item2 - yValues.Item1 > boardSize)
            {
                return false;
            }
            return true;
        }

    }
}
