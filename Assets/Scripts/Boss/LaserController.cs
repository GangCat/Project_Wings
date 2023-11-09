using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public delegate void DestroyBombDelegate(GameObject _bombGo);
    public void Init(float _launchDuration, float _lengthPerSec, DestroyBombDelegate _destroyBombCallback, float _initWidth, float _initHeight)
    {
        destroyBombCallback = _destroyBombCallback;
        launchDuration = _launchDuration;
        lengthPerSec = _lengthPerSec;
        initWidth = _initWidth;
        initHeight = _initHeight;

        waitFixedTime = new WaitForFixedUpdate();

        mf = GetComponentInChildren<MeshFilter>();

        StartCoroutine(LaunchLaserCoroutine(Time.time));
    }

    private IEnumerator LaunchLaserCoroutine(float _startTime)
    {
        while(Time.time - _startTime < launchDuration)
        {
            //사운드
            // 연출

            curHeight += lengthPerSec * Time.fixedDeltaTime;
            ChangeForm();

            yield return waitFixedTime;
        }
    }

    private void ChangeForm()
    {
        //mf.mesh = mesh;

        // 버텍스 버퍼
        Vector3[] verticesArr = new Vector3[]
            {
                new Vector3(-initWidth * 0.5f, initHeight * 0.5f, 0f),
                new Vector3(initWidth * 0.5f, initHeight * 0.5f, 0f),
                new Vector3(-initWidth * 0.5f, -initHeight * 0.5f, 0f),
                new Vector3(initWidth * 0.5f, -initHeight * 0.5f, 0f),
                new Vector3(-initWidth * (0.5f + curHeight * 0.005f), initHeight * (0.5f + curHeight * 0.005f), curHeight),
                new Vector3(initWidth * (0.5f + curHeight * 0.005f), initHeight * (0.5f + curHeight * 0.005f), curHeight),
                new Vector3(-initWidth * (0.5f + curHeight * 0.005f), -initHeight * (0.5f + curHeight * 0.005f), curHeight),
                new Vector3(initWidth * (0.5f + curHeight * 0.005f), -initHeight * (0.5f + curHeight * 0.005f), curHeight)
                //,
                //new Vector3(-0.5f, 0.5f,-0.5f),
                //new Vector3(0.5f,0.5f,-0.5f),
                //new Vector3(-0.5f,-0.5f,-0.5f),
                //new Vector3(0.5f,-0.5f,-0.5f),
                //new Vector3(-0.5f,0.5f,0.5f),
                //new Vector3(0.5f,0.5f,0.5f),
                //new Vector3(-0.5f,-0.5f,0.5f),
                //new Vector3(0.5f,-0.5f,0.5f)
            };
        Vector3[] vertices = SetVertices(verticesArr);

        // 인덱스 버퍼
        int[] indices = SetIndices();

        // 노멀값
        Vector3[] normals = CalcNormal(vertices, indices);


        mf.mesh.Clear();
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = indices;
        mf.mesh.normals = normals;
    }

    private Vector3[] SetVertices(Vector3[] _verticesArr)
    {
        Vector3[] vertices = new Vector3[]
            {
            //forward
            _verticesArr[0],
            _verticesArr[1],
            _verticesArr[2],
            _verticesArr[3],
            // upper
            _verticesArr[4],
            _verticesArr[5],
            _verticesArr[0],
            _verticesArr[1],
            // right
            _verticesArr[1],
            _verticesArr[5],
            _verticesArr[3],
            _verticesArr[7],
            // left
            _verticesArr[4],
            _verticesArr[0],
            _verticesArr[6],
            _verticesArr[2],
            // lower
            _verticesArr[2],
            _verticesArr[3],
            _verticesArr[6],
            _verticesArr[7],
            // backward
            _verticesArr[6],
            _verticesArr[7],
            _verticesArr[4],
            _verticesArr[5]
            };

        return vertices;
    }

    private int[] SetIndices()
    {
        int[] indices = new int[]
            {
                // foward
                0, 1, 2,
                1, 3 ,2,
                // upper
                4, 5, 6,
                5, 7, 6,
                // right
                8, 9, 10,
                9, 11, 10,
                // left
                12, 13, 14,
                13, 15, 14,
                // lower
                16, 17, 18,
                17, 19, 18,
                // backward
                20, 21, 22,
                21, 23, 22
            };

        return indices;
    }


    private Vector3[] CalcNormal(Vector3[] _vertices, int[] _indices)
    {
        // 바깥 방향으로 노멀값이 나가야 함.
        Vector3[] normals = new Vector3[_vertices.Length];
        Vector3 normal = Vector3.zero;
        for (int i = 0; i < _indices.Length; i += 3)
        {
            normal = Vector3.Cross(
                _vertices[_indices[i + 2]] - _vertices[_indices[i + 1]],
                _vertices[_indices[i]] - _vertices[_indices[i + 1]]);

            normals[_indices[i]] = normal;
            normals[_indices[i + 1]] = normal;
            normals[_indices[i + 2]] = normal;
        }

        return normals;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TimeBomb"))
            destroyBombCallback?.Invoke(other.gameObject);
    }

    private DestroyBombDelegate destroyBombCallback = null;
    private float launchDuration = 0f;
    private float lengthPerSec = 0f;
    private WaitForFixedUpdate waitFixedTime = null;

    private MeshFilter mf = null;
    private float initWidth = 0f;
    private float initHeight = 0f;
    private float curHeight = 0f;
}
