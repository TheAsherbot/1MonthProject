using System.Collections;
using System.Collections.Generic;
using System.IO;

using TheAshBot.Grid;

using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{


    private bool updateMesh;

    private IntGrid grid;
    private MeshFilter meshFilter;
    private Mesh mesh;


    private void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>(); 

        meshFilter.mesh = mesh;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    public void SetGrid(IntGrid grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, IntGrid.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void UpdateHeatMapVisual()
    {
        MeshHelper.CreateEmptyMeshArray(grid.GetHeight() * grid.GetWidth(), out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = Vector2.one * grid.GetCellSize();
                Vector2 offsetSize = quadSize / 2;

                int gridValue = grid.GetValue(x, y);
                float gridValeuNormalized = gridValue / 100f;

                Vector2 gridValueUV = new Vector2(gridValeuNormalized, 0f);
                MeshHelper.AddToMeshArrays(vertices, uvs, triangles, index, grid.GetWorldPosition(x, y) + offsetSize, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }


}
