using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawerWithMesh : MonoBehaviour
{
    public int rows = 10;      // 行数
    public int columns = 10;   // 列数
    public float cellSize = 1f; // 每个格子的大小（1x1）

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] indices;

    void Start()
    {
        // 创建一个新的 Mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        DrawGrid();
    }

    void DrawGrid()
    {
        // 水平线的顶点数: (rows + 1) 个水平线，每条水平线有 2 个顶点
        // 垂直线的顶点数: (columns + 1) 个垂直线，每条垂直线有 2 个顶点
        int vertexCount = (rows + 1) * 2 + (columns + 1) * 2;
        vertices = new Vector3[vertexCount];

        // 每条线由两个顶点构成，因此索引数组的大小为顶点数
        indices = new int[vertexCount];

        int vIndex = 0;

        // 绘制水平线
        for (int i = -rows / 2; i <= rows / 2; i++)
        {
            // 左到右
            vertices[vIndex++] = new Vector3(-columns / 2 * cellSize, i * cellSize, 0);
            vertices[vIndex++] = new Vector3(columns / 2 * cellSize, i * cellSize, 0);
        }

        // 绘制垂直线
        for (int i = -columns / 2; i <= columns / 2; i++)
        {
            // 上到下
            vertices[vIndex++] = new Vector3(i * cellSize, -rows / 2 * cellSize, 0);
            vertices[vIndex++] = new Vector3(i * cellSize, rows / 2 * cellSize, 0);
        }

        // 设置网格顶点
        mesh.vertices = vertices;

        // 为每个点创建一条线（每两个相邻的顶点表示一条线）
        for (int i = 0; i < vertices.Length; i++)
        {
            indices[i] = i;
        }

        // 设置网格的索引，使用 MeshTopology.Lines 来创建线段
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }
}

