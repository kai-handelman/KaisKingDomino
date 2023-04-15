using System.Collections.Generic;
using System.Linq;
using tileNamespace;

namespace boardNameSpace
{
    public class BoardManager
    {
        private int playerNum;
        private (int, int) xValues;
        private (int, int) yValues;
        private bool xCap;
        private bool yCap;
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
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[tempTarget].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1 - 1, target.Item2);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[tempTarget].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 + 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[tempTarget].getType() == tt)
            {
                return true;
            }
            tempTarget = (target.Item1, target.Item2 - 1);
            if (occupiedSpaces.ContainsKey(tempTarget) && occupiedSpaces[tempTarget].getType() == tt)
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
            ((int, int), Dictionary<(int, int), Tile>) helperResults;

            while (tempSpaceDic.Count != 0)
            {
                if (currentTile.Item2.getType() != tileType.King)
                {
                    helperResults = pointHelper(currentTile.Item2.getType(), currentTile, tempSpaceDic); //Calcs the currentTile Plus ConnectTile points, then adds to Total Points
                    totalPoints += helperResults.Item1.Item1 * helperResults.Item1.Item2; //Calculates the total points of that streak
                    tempSpaceDic = helperResults.Item2;
                }
                if (tempSpaceDic.Count != 0)                                            //Grab next avaible tile if thers still uncalculated Tiles on the board
                {
                    currentTile = (tempSpaceDic.ElementAt(0).Key, tempSpaceDic.ElementAt(0).Value);
                    tempSpaceDic.Remove(currentTile.Item1);
                }
            }
            return totalPoints;
        }

        // Returns a the amount of crowns and tiles in a section of the board
        private ((int, int), Dictionary<(int, int), Tile>) pointHelper(tileType tileType, ((int, int), Tile) currentTile, Dictionary<(int, int), Tile> tempDictionary)
        {
            List<((int, int), Tile)> tempList = new List<((int, int), Tile)>();
            int tempCrownCount = 0;
            int tempTileCount = 0;
            ((int, int), Dictionary<(int, int), Tile>) recursiveResult;

            (int, int) tempTarget = (currentTile.Item1.Item1 + 1, currentTile.Item1.Item2);
            if (tempDictionary.ContainsKey(tempTarget) && tempDictionary[tempTarget].getType() == tileType)
            {
                tempList.Add((tempTarget, tempDictionary[tempTarget]));
                tempCrownCount += tempDictionary[tempTarget].getValue();
                tempTileCount += 1;
                tempDictionary.Remove(tempTarget);
            }
            tempTarget = (currentTile.Item1.Item1 - 1, currentTile.Item1.Item2);
            if (tempDictionary.ContainsKey(tempTarget) && tempDictionary[tempTarget].getType() == tileType)
            {
                tempCrownCount += tempDictionary[tempTarget].getValue();
                tempTileCount += 1;
                tempList.Add((tempTarget, tempDictionary[tempTarget]));
                tempDictionary.Remove(tempTarget);
            }
            tempTarget = (currentTile.Item1.Item1, currentTile.Item1.Item2 + 1);
            if (tempDictionary.ContainsKey(tempTarget) && tempDictionary[tempTarget].getType() == tileType)
            {
                tempCrownCount += tempDictionary[tempTarget].getValue();
                tempTileCount += 1;
                tempList.Add((tempTarget, tempDictionary[tempTarget]));
                tempDictionary.Remove(tempTarget);
            }
            tempTarget = (currentTile.Item1.Item1, currentTile.Item1.Item2 + 1);
            if (tempDictionary.ContainsKey(tempTarget) && tempDictionary[tempTarget].getType() == tileType)
            {
                tempCrownCount += tempDictionary[tempTarget].getValue();
                tempTileCount += 1;
                tempList.Add((tempTarget, tempDictionary[tempTarget]));
                tempDictionary.Remove(tempTarget);
            }
            if (tempTileCount > 0)
            {
                foreach (((int, int), Tile) item in tempList)
                {
                    recursiveResult = pointHelper(tileType, item, tempDictionary);
                    tempCrownCount += recursiveResult.Item1.Item1;
                    tempTileCount += recursiveResult.Item1.Item2;
                    tempDictionary = recursiveResult.Item2;
                }
            }
            return ((tempCrownCount, tempTileCount), tempDictionary);
        }
    }
}
