using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private MeshCollider _collider;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _collider = gameObject.AddComponent<MeshCollider>();
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells)
    {
        _mesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();
        _collider.sharedMesh = _mesh;
    }

    private void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                center,
                center + HexMetrics.corners[i],
                center + HexMetrics.corners[i + 1]
            );
            cell.GetCurrentColor();
            cell.SetPrice();
        }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }
}