using System;
using System.Collections;
using System.Collections.Generic;

using TheAshBot.Grid;

using UnityEngine;

public class GridVisual : MonoBehaviour
{


    [Serializable]
    public struct TilemapSpriteUV
    {
        public GridObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoordinates
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }


    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVArray;


    private bool updateMesh;

    private Grid grid;
    private MeshFilter meshFilter;
    private Mesh mesh;

    private Dictionary<GridObject.TilemapSprite, UVCoordinates> uvCoordinatesDictionary;


    private void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;


        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;


        uvCoordinatesDictionary = new Dictionary<GridObject.TilemapSprite, UVCoordinates>();

        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray)
        {
            uvCoordinatesDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoordinates
            {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
        UpdateTilemapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
        grid.OnLoaded += Tilemap_OnLoaded;
    }

    private void Tilemap_OnLoaded(object sender, EventArgs e)
    {
        updateMesh = true;
    }

    private void Grid_OnGridValueChanged(object sender, Grid.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void UpdateTilemapVisual()
    {
        int numberOfQuads = 0;
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                GridObject gridObject = grid.GetGridObject(x, y);
                GridObject.TilemapSprite tilemapSprite = gridObject.tilemapSprite;
                if (tilemapSprite != GridObject.TilemapSprite.None)
                {
                    numberOfQuads++;
                }
            }
        }

        MeshHelper.CreateEmptyMeshArray(numberOfQuads * 2, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        if (numberOfQuads != 0)
        {
            int index = 0;
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    Vector3 quadSize = Vector2.one * grid.GetCellSize();
                    Vector2 offsetSize = quadSize / 2;

                    GridObject gridObject = grid.GetGridObject(x, y);
                    GridObject.TilemapSprite tilemapSprite = gridObject.tilemapSprite;

                    Vector2 gridUV00, gridUV11;
                    if (tilemapSprite != GridObject.TilemapSprite.None)
                    {
                        UVCoordinates uVCoordinates = uvCoordinatesDictionary[tilemapSprite];
                        gridUV00 = uVCoordinates.uv00;
                        gridUV11 = uVCoordinates.uv11;

                        AddToMeshArrays();
                    }

                    void AddToMeshArrays()
                    {
                        index++;
                        MeshHelper.AddToMeshArrays(vertices, uvs, triangles, index, grid.GetWorldPosition(x, y) + offsetSize, 0f, quadSize, gridUV00, gridUV11);
                    }
                }
            }

            if (vertices.Length > 3 && uvs.Length > 3)
            {
                mesh.vertices = vertices;
                mesh.uv = uvs;
                mesh.triangles = triangles;
                meshFilter.mesh = mesh;
            }
            return;
        }
        else
        {
            mesh = new Mesh();
            meshFilter.mesh = new Mesh();
        }
    }


}
