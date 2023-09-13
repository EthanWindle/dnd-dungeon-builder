using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;

/*
 * Class which randomly places prefabs in the dungeon
 */
public class DungeonGenerator
{

    private int numberOfRooms = 30;

    public GameObject[] generateDungeon(GameObject[] initialRooms, out int[] xOffsets, out int[] yOffsets, int gridWidth, int gridHeight)
    {
        GameObject[] finalRooms = new GameObject[numberOfRooms];
        xOffsets = new int[numberOfRooms];
        yOffsets = new int[numberOfRooms];

        bool[,] grid = new bool[gridWidth, gridHeight];

        for (int roomIndex = 0; roomIndex < numberOfRooms; roomIndex++)
        {
            GameObject roomToPlace = initialRooms[UnityEngine.Random.Range(0, initialRooms.Length)];
            int roomToPlaceWidth = roomToPlace.GetComponent<RoomController>().width;
            int roomToPlaceHeight = roomToPlace.GetComponent<RoomController>().height;

            int maxAttempts = 100;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Generate random position within grid boundaries
                int randomX = UnityEngine.Random.Range(0, gridWidth - roomToPlaceWidth + 1);
                int randomY = UnityEngine.Random.Range(0, gridHeight - roomToPlaceHeight + 1);

                // Check if the room would overlap with any existing rooms
                bool overlap = false;
                for (int x = randomX; x < randomX + roomToPlaceWidth; x++)
                {
                    for (int y = randomY; y < randomY + roomToPlaceHeight; y++)
                    {
                        if (grid[x, y])
                        {
                            overlap = true;
                            break;
                        }
                    }
                    if (overlap) break;
                }

                // If no overlap, place the room and mark the grid cells as occupied
                if (!overlap)
                {
                    finalRooms[roomIndex] = roomToPlace;
                    xOffsets[roomIndex] = randomX;
                    yOffsets[roomIndex] = randomY;
                    var minRoomGap = 1;
                    for (int x = randomX; x < randomX + roomToPlaceWidth + minRoomGap; x++)
                    {
                        for (int y = randomY; y < randomY + roomToPlaceHeight + minRoomGap; y++)
                        {
                            if(x >= 0  && y >= 0 && x < gridWidth && y < gridHeight)
                            grid[x, y] = true;
                        }
                    }
                    break;
                }

            }
        }

        return finalRooms;
    }


}
