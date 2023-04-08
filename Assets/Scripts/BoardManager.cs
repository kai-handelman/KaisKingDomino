using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tileNamespace;
using UnityEngine;

namespace boardNameSpace
{
    public class BoardManager
    {
        private int playerNum;
        private (int, int) xValues;
        private (int, int) yValues;
        private bool xCap;
        private bool yCap;
        // private List<Tile> tiles = new List<Tile>();
        // private GameObject objToSpawn;
        private int boardSize;
        private Dictionary<(int, int), Tile> occupiedSpaces;

        public BoardManager()
        {
            Tile kingTile = new Tile(tileType.King, 0, 0, 5);
            xValues = (0, 0);
            yValues = (0, 0);
            xCap = false;
            yCap = false;
            occupiedSpaces.Add(kingTile.getCoords(), kingTile);
            boardSize = 5;
        }

        public bool addTile(Tile t1, Tile t2)
        {//Will return place tile add to list if everything is valid
            if (isValidPlace(t1.getType(), t2.getType(), t1.getCoords(), t2.getCoords()))
            {
                updateBounds(t1.getCoords());
                occupiedSpaces.Add(t1.getCoords(), t1);
                updateBounds(t2.getCoords());
                occupiedSpaces.Add(t2.getCoords(), t2);
                return true;
            }
            return false;
        }

        public bool isValidPlace(tileType anchorTileType, tileType secondaryTileType, (int, int) anchCoord, (int, int) secondCoor)
        {
            //Checks if the anchCoord or secondCoord will be out of bounds
            if (coordsInbounds(anchCoord, secondCoor))
            {
                //Coords weren't in bound
                return false;
            }


            if (occupiedSpaces.ContainsKey(anchCoord) || occupiedSpaces.ContainsKey(secondCoor))
            {
                return false;         //There is no possible match
            }

            if (!matchingNeighbors(anchCoord, anchorTileType))
            {
                return false;         //There were no neighbors for anchor
            }

            if (!matchingNeighbors(secondCoor, secondaryTileType))
            {
                return false;         //There were no neighbors for secondary
            }


            //Update bounds
            updateBounds(anchCoord);
            updateBounds(secondCoor);
            //Add Tile
            return true;
        }

        public bool matchingNeighbors((int, int) target, tileType tt)
        {
            //Checks if any of the surrounding squares of the target of the tile is occupied and has the same type
            (int, int) tempTarget = (target.Item1 + 1, target.Item2);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1 - 1, target.Item2);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 + 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 - 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[target].getType() == tt)
            {
                return true;
            }
            return false;
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
                xCap = true;
            }
            if (!yCap && yValues.Item2 - yValues.Item1 >= boardSize)
            {
                yCap = true;
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

        public int points()
        {
            Dictionary<(int, int), Tile> tempSpaceDic = occupiedSpaces;
            ((int, int), Tile) currentTile = (tempSpaceDic.ElementAt(0).Key, tempSpaceDic.ElementAt(0).Value);
            tempSpaceDic.Remove(currentTile.Item1);

            int totalPoints = 0;
            (int, List<(int, int)>) helperResults;

            while (tempSpaceDic.Count != 0)
            {
                if (currentTile.Item2.getType() != tileType.King)
                {
                    helperResults = pointHelper(currentTile.Item2.getType(), ((0, 0), null), tempSpaceDic); //Calcs the currentTile Plus ConnectTile points, then adds to Total Points
                    totalPoints += ((helperResults.Item1 + currentTile.Item2.getValue()) * helperResults.Item2.Count); //Calculates the total points of that streak

                    // foreach ((int, int) coords in helperResults.Item2)                 //Removes Tiles that's been calc'd
                    // {
                    //     tempSpaceDic.Remove(coords);
                    // }
                }
                if (tempSpaceDic.Count != 0)                                            //Grab next avaible tile if thers still uncalculated Tiles on the board
                {
                    currentTile = (tempSpaceDic.ElementAt(0).Key, tempSpaceDic.ElementAt(0).Value);
                    tempSpaceDic.Remove(currentTile.Item1);
                }
            }

            return totalPoints;
        }
        // Returns a list of Coords of tiles that are the same type and are neighbors and the corresponding total crowns(value) of that list
        private (int, List<(int, int)>) pointHelper(tileType Type, ((int, int), Tile) currentTile, Dictionary<(int, int), Tile> tiles)
        {

            return (0, null);
        }


    }
}
