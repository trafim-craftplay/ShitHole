using System.Collections.Generic;
using UnityEngine;

public class TestHole : MonoBehaviour
{
    [SerializeField] private GameObject _holeCenter = null;
    [Space(30)]
    [Range(0f, 100f)]
    [SerializeField] private int _quarter—ircleVerticesCount = 0;
    [SerializeField] private float _width = 0;
    [SerializeField] private float _length = 0;
    [SerializeField] private float _radius = 0;

    void Start()
    {
        CreateMesh();
    }

    private void OnValidate()
    {
        CreateMesh();
    }

    private void Update()
    {
        if (_holeCenter.transform.hasChanged)
        {
            CreateMesh();
        }
    }

    private void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = Vertices().ToArray();
        mesh.triangles = Triangles().ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private List<Vector3> Vertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(new Vector3(-_width, -_length));
        vertices.Add(new Vector3(_width, -_length));
        vertices.Add(new Vector3(_width, _length));
        vertices.Add(new Vector3(-_width, _length));

        int temp = _quarter—ircleVerticesCount * 4 + 4;

        float angleStep = 360f / temp;
        float angle = 270f;

        for (int i = 0; i < temp; i++)
        {
            Vector3 pos = new Vector3(-_holeCenter.transform.position.x + (Mathf.Cos(Mathf.Deg2Rad * angle) * _radius), _holeCenter.transform.position.z + (Mathf.Sin(Mathf.Deg2Rad * angle) * _radius), 0);

            angle += angleStep;
            vertices.Add(pos);
        }

        return vertices;
    }
  
    private List<int> Triangles()
    {
        List<int> triangles = new List<int>();

        for (int i = 0; i <= 3; i++)
        {
            triangles.Add(i);

            triangles.Add(i == 3 ? 0 : i + 1);

            triangles.Add(4 + i * (_quarter—ircleVerticesCount + 1));

            if (i != 0)
            {
                for (int a = 0; a <= _quarter—ircleVerticesCount; a++)
                {
                    int tempValue = 4 + i * (_quarter—ircleVerticesCount + 1) - a;

                    triangles.Add(tempValue);
                    triangles.Add(tempValue - 1);
                    triangles.Add(i);
                }
            }
            else
            {
                for (int a = 0; a <= _quarter—ircleVerticesCount; a++)
                {
                    int tempValue = 4 + 3 * (_quarter—ircleVerticesCount + 1) + a;

                    triangles.Add(tempValue + 1 > 7 + _quarter—ircleVerticesCount * 4 ? 4 : tempValue + 1);
                    triangles.Add(tempValue);
                    triangles.Add(i);
                }
            }
        }

        return triangles;
    }
}