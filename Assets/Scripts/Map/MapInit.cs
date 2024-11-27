using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawerWithMesh : MonoBehaviour
{
    public int rows = 10;      // ����
    public int columns = 10;   // ����
    public float cellSize = 1f; // ÿ�����ӵĴ�С��1x1��

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] indices;

    void Start()
    {
        // ����һ���µ� Mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        DrawGrid();
    }

    void DrawGrid()
    {
        // ˮƽ�ߵĶ�����: (rows + 1) ��ˮƽ�ߣ�ÿ��ˮƽ���� 2 ������
        // ��ֱ�ߵĶ�����: (columns + 1) ����ֱ�ߣ�ÿ����ֱ���� 2 ������
        int vertexCount = (rows + 1) * 2 + (columns + 1) * 2;
        vertices = new Vector3[vertexCount];

        // ÿ�������������㹹�ɣ������������Ĵ�СΪ������
        indices = new int[vertexCount];

        int vIndex = 0;

        // ����ˮƽ��
        for (int i = -rows / 2; i <= rows / 2; i++)
        {
            // ����
            vertices[vIndex++] = new Vector3(-columns / 2 * cellSize, i * cellSize, 0);
            vertices[vIndex++] = new Vector3(columns / 2 * cellSize, i * cellSize, 0);
        }

        // ���ƴ�ֱ��
        for (int i = -columns / 2; i <= columns / 2; i++)
        {
            // �ϵ���
            vertices[vIndex++] = new Vector3(i * cellSize, -rows / 2 * cellSize, 0);
            vertices[vIndex++] = new Vector3(i * cellSize, rows / 2 * cellSize, 0);
        }

        // �������񶥵�
        mesh.vertices = vertices;

        // Ϊÿ���㴴��һ���ߣ�ÿ�������ڵĶ����ʾһ���ߣ�
        for (int i = 0; i < vertices.Length; i++)
        {
            indices[i] = i;
        }

        // ���������������ʹ�� MeshTopology.Lines �������߶�
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }
}

