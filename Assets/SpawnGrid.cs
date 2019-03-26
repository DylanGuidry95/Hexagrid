﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public int perlinSeed;
    public List<GameObject> outerWall, lowLand, medLand, highLand, Special;
    public int rows, cols;
    public TraversableNode[,] gridNodes;

    IEnumerator Start()
    {
        gridNodes = new TraversableNode[cols, rows];

        for(int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                float perlinScale = 8f;
                float xCord = (float)i/(float)cols;
                float yCord = (float)j/(float)rows;

                float perlinHeight = GetPerlinNoiseValue(xCord, yCord, perlinScale, perlinSeed);
                
                GameObject gridCell = null;

                bool isWall = (i == 0 || j == 0 || i == cols-1 || j == rows-1);

                float biomeScale = 8f;
                int perlinFort = (int)GetPerlinNoiseValue(xCord, yCord, 32f, perlinSeed, 10);

                if(isWall)
                    gridCell = Instantiate(outerWall[(int)GetPerlinNoiseValue(xCord, yCord, biomeScale, perlinSeed/2, outerWall.Count)]) as GameObject;

                else if(perlinFort == 9)
                    gridCell = Instantiate(Special[(int)GetPerlinNoiseValue(xCord, yCord, biomeScale, perlinSeed/2, Special.Count)]) as GameObject;

                else if(perlinHeight >= 0.8f)
                    gridCell = Instantiate(highLand[(int)GetPerlinNoiseValue(xCord, yCord, biomeScale, perlinSeed/2, highLand.Count)]) as GameObject;

                else  if(perlinHeight >= 0.4f)
                    gridCell = Instantiate(medLand[(int)GetPerlinNoiseValue(xCord, yCord, biomeScale, perlinSeed/2, medLand.Count)]) as GameObject;

                else
                    gridCell = Instantiate(lowLand[(int)GetPerlinNoiseValue(xCord, yCord, biomeScale, perlinSeed/2, lowLand.Count)]) as GameObject;


                gridCell.name = $"[{i}, {j}]";
                gridCell.transform.parent = transform;
                gridCell.transform.localPosition = new Vector3(i * 0.85f, 0, j + (i%2*.5f));
                gridCell.transform.Rotate(Vector3.up, 30);

                float height = (int)(perlinHeight * 8f) + 1;
                Vector3 scale = isWall ? new Vector3(1f, 16f, 1f) : new Vector3(1f, height,  1f);
                gridCell.transform.localScale = scale;

                for(int k = 0; k < gridCell.transform.childCount; k++)
                {
                    Transform child = gridCell.transform.GetChild(k);
                    child.gameObject.SetActive(false);

                    Vector3 thisCellGlobalScale = gridCell.transform.lossyScale;
                    thisCellGlobalScale.x = 1f / thisCellGlobalScale.x;
                    thisCellGlobalScale.y = 1f / thisCellGlobalScale.y;
                    thisCellGlobalScale.z = 1f / thisCellGlobalScale.z;

                    child.localScale = thisCellGlobalScale;
                    child.localPosition += Vector3.up * 0.16f;
                }

                gridNodes[i, j] = (gridCell.AddComponent<TraversableNode>());
                gridNodes[i, j]._xCoord = i;
                gridNodes[i, j]._yCoord = j;
                gridNodes[i, j]._travelCost = height;
                
                if(j > 0)
                    gridNodes[i, j].AddNeighbor(gridNodes[i, j-1]);

                if(i > 0)
                {
                    gridNodes[i, j].AddNeighbor(gridNodes[i-1, j]);

                    int nextJ = j+(i%2==0?-1:1);
                    if(nextJ >= 0 && nextJ < rows)
                        gridNodes[i, j].AddNeighbor(gridNodes[i-1, nextJ]);
                }
            }
        }
        yield return null;
    }

    bool ClearBoard()
    {
        if(gridNodes == null || gridNodes[rows-1, cols-1] == null)
            return false;

        for(int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Destroy(gridNodes[i, j].gameObject);
            }
        }

        return true;
    }

    [ContextMenu("NewMap")]
    public void NewMap()
    {
        if(ClearBoard())
            StartCoroutine(Start());
    }


    float GetPerlinNoiseValue(float xCord, float yCord, float scale, long seed = 0, float valueMod = 1f)
    {   
        xCord += seed;
        yCord += seed;

        xCord *= scale;
        yCord *= scale;

        return Mathf.Clamp01(Mathf.PerlinNoise(xCord, yCord)) * valueMod;
    }
}