using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] // Необходимые компоненты
public class GridGeneration : MonoBehaviour
{
    [SerializeField]
    private int _xSize, _ySize; // Размер сетки

    private Vector3[] _vertices; // Массив векторных значений, зависящий от размера сетки
    private Mesh _mesh;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "Grid";

        _vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
        Vector2[] uvs = new Vector2[_vertices.Length]; //массив для uv координат
        Vector4[] tangents = new Vector4[_vertices.Length]; // Для Улучшения отображения карты нормалей
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        // Циклы для позиционирования
        for (int i = 0, y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, y);
                uvs[i] = new Vector2((float)x / _xSize, (float)y / _ySize); //Приводим к float для точности
                tangents[i] = tangent;
            }
        }
        _mesh.vertices = _vertices; // Массив вершин
        _mesh.uv = uvs;
        _mesh.tangents = tangents;
        
        
        //Вершины для треугольников
        int[] triangles = new int[_xSize * _ySize * 6]; // Шесть вершин для однорй ячейки
        for (int ti = 0, vi = 0, y = 0; y < _ySize; y++, vi++) // vi - индекс вершины, ti - индекс треугольника
        {
            for (int x = 0; x < _xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + _xSize + 1;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = vi + _xSize + 2;
            }
        }
        
        

        _mesh.triangles = triangles; // массив треугольников
        _mesh.RecalculateNormals(); // Вычисление нормали каждой вершины - какие треугоники соеденены с этой вершиной(нужно для отрисовки рисунка)

    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.2f);
        }
    }
}
