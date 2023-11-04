using UnityEngine;

public class WindBlowGenerator : MonoBehaviour
{
    private float smallRadius = 1f;
    private float cylinderLengthPerSecond = 0.2f;
    private int numVertices = 30;
    private float totalDuration = 5f;
    private float colliderInterval = 1f; // 콜라이더 생성 간격
    private GameObject windCylinderPrefab;
    private float largeRadius = 2f;

    private GameObject windCylinder;
    private MeshFilter meshFilter;
    private float currentHeight = 0f;
    private float elapsedTime = 0f;
    private float lastColliderSpawnTime = 0f;

    public void Init(float _smallRadius, float _largeRadius, float _totalDuration, float _colliderInterval, float _cylinderLengthPerSecond, int _numVertices, GameObject _windCylinderPrefab)
    {
        smallRadius = _smallRadius;
        largeRadius = _largeRadius;
        totalDuration = _totalDuration;
        colliderInterval = _colliderInterval;
        cylinderLengthPerSecond = _cylinderLengthPerSecond;
        numVertices = _numVertices;
        windCylinderPrefab = _windCylinderPrefab;

        windCylinder = Instantiate(windCylinderPrefab, transform.position, transform.rotation);
        windCylinder.tag = "WindBlow";

        meshFilter = windCylinder.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // 초기 메쉬 생성
        UpdateWindCylinder(smallRadius, currentHeight);
    }

    public void UpdateWindBlow()
    {
        float radiusLarge = Mathf.Lerp(smallRadius, largeRadius, Mathf.Clamp01(elapsedTime / totalDuration));

        if (currentHeight < totalDuration * cylinderLengthPerSecond)
        {
            currentHeight += Time.deltaTime * cylinderLengthPerSecond;
            elapsedTime += Time.deltaTime;

            // 콜라이더 생성 인터벌 체크
            if(Time.time - lastColliderSpawnTime >= colliderInterval)
            {
                CreateCollider(currentHeight, radiusLarge);
            }
        }

        // 메쉬 업데이트
        UpdateWindCylinder(radiusLarge, currentHeight);
    }

    public void FinishGenerate()
    {
        Destroy(windCylinder);

        windCylinder = null;
        meshFilter = null;
        currentHeight = 0f;
        elapsedTime = 0f;
        lastColliderSpawnTime = 0f;
    }

    private void CreateCollider(float height, float radiusLarge)
    {
        lastColliderSpawnTime = Time.time;
        GameObject colliderObject = new GameObject("WindBlowCollider");
        colliderObject.transform.position = transform.position + transform.up * height;
        colliderObject.transform.parent = windCylinder.transform;
        colliderObject.transform.tag = "WindBlow";
        colliderObject.layer = LayerMask.NameToLayer("BossProjectile");

        BoxCollider collider = colliderObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(2f * radiusLarge, cylinderLengthPerSecond * colliderInterval, 2f * radiusLarge);
        collider.center = new Vector3(0f, collider.center.y, 0f);
    }

    private void UpdateWindCylinder(float radiusLarge, float height)
    {
        Vector3[] vertices = new Vector3[numVertices * 2];
        int[] triangles = new int[numVertices * 6];

        for (int i = 0; i < numVertices; i++)
        {
            float radian = Mathf.Deg2Rad * (i * 360f / numVertices);
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);

            vertices[i] = new Vector3(x * smallRadius, 0f, z * smallRadius);
            vertices[i + numVertices] = new Vector3(x * radiusLarge, height, z * radiusLarge);

            int nextIndex = (i + 1) % numVertices;
            triangles[i * 6] = i;
            triangles[i * 6 + 1] = nextIndex;
            triangles[i * 6 + 2] = i + numVertices;

            triangles[i * 6 + 3] = i + numVertices;
            triangles[i * 6 + 4] = nextIndex;
            triangles[i * 6 + 5] = nextIndex + numVertices;
        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
    }

}
