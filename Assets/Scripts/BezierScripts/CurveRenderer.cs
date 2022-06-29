using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(CubicSplineObj))]
public class CurveRenderer : MonoBehaviour {
    

    [SerializeField] private Transform m_start;
    [SerializeField] private Transform m_end;
    
    [SerializeField] private uint samples = 10;

    [SerializeField] private float thickness = 0.3f;
    [SerializeField] private float scale = 0.3f;
    // [SerializeField] private int smoothness  = 10; 
    [SerializeField] private float spacing = 0.2f;
    [SerializeField] private float resolution = 1.0f;
    
    
    [Range(0.0f, Mathf.PI / 4.0f)]
    [SerializeField] private float wallInclination = 0.2f;
    [SerializeField] private float wallHeight = 0.2f;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    
    [SerializeField] private float planeThickness = 5.0f;
    [SerializeField] private GameObject leftPlane;
    [SerializeField] private GameObject rightPlane;


    [field: SerializeField] public bool autoUpdate {get; set;}

    void Start() { }

    public void display(){
        Vector3 start = m_start.position;        
        Vector3 end = m_end.position;

        float[] temp = NoiseGenerator.generatePerlinNoise1D((int)samples, scale);
        
        float[] noise = new float[samples + 2]; 
        noise[0] = 0.0f;
        noise[samples + 1] = 0.0f;

        for(int i = 1; i <= samples; i++){
            noise[i] = 2.0f * (temp[i - 1] - 0.5f);
        }

        CubicSplineObj cso = GetComponent<CubicSplineObj>();
        cso.fitCubicSpline(noise, start, end, Vector3.up);
        Vector3[] sampledPoints = cso.samplePoints(spacing, resolution, start, end); 
        
        createPath(this.gameObject, sampledPoints);

        var verts = createSideVerts(sampledPoints, thickness);

        foreach(Vector3 v in verts.leftVerts){
            //GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //g.transform.position = v;
            //g.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        createWall(leftWall, verts.leftVerts, true);
        createWall(rightWall, verts.rightVerts, false);

        //createTopPlane(leftPlane, verts.leftVerts, true);
        //createTopPlane(rightPlane, verts.rightVerts, false);

    }

    private void createPath(GameObject obj, Vector3[] points){    
        var verts = createSideVerts(points, thickness);
        var meshdata = genMeshBtwTwoCurves(verts.leftVerts, verts.rightVerts);

        Mesh mesh = new Mesh();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        
        mesh.Clear();
        mesh.vertices = meshdata.meshverts;
        mesh.triangles = meshdata.triangles;
        mesh.uv = meshdata.uvs;
        mesh.RecalculateNormals();
    }


    private void createTopPlane(GameObject obj, Vector3[] points, bool isLeft){
        var verts = createSideVerts(points, planeThickness);
        Vector3[] vert2 = isLeft ? verts.leftVerts : verts.rightVerts;    
       
        if(isLeft){
            Vector3[] temp = points;
            points = vert2;
            vert2 = temp;
        }

        var meshdata = genMeshBtwTwoCurves(points, vert2);

        Mesh mesh = new Mesh();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        
        mesh.Clear();
        mesh.vertices = meshdata.meshverts;
        mesh.triangles = meshdata.triangles;
        mesh.uv = meshdata.uvs;
        mesh.RecalculateNormals();
 
    }


    private void createWall(GameObject obj, Vector3[] verts, bool isLeft){
        int n = verts.Length;
        Vector3[] verts2 = new Vector3[n];

        float sign = isLeft ? -1.0f : 1.0f;

        for(int i = 0; i < n; i++){
            Vector3 dir = Vector3.zero;
            if(i < n - 1){
                dir += verts[i + 1] - verts[i]; 
            }

            if(i > 0){
                dir += verts[i] - verts[i - 1];
            }

            dir.Normalize();
            Vector3 left = sign * Vector3.Cross(Vector3.up, dir); 

            verts2[i] = verts[i] + Vector3.up * wallHeight + left * (wallHeight / Mathf.Tan(Mathf.PI/2.0f - wallInclination));
        }

         
        createTopPlane(isLeft ? leftPlane : rightPlane, verts2, isLeft);
        
        if(isLeft){
            Vector3[] temp = verts;
            verts = verts2;
            verts2 = temp;
        }

        var meshData = genMeshBtwTwoCurves(verts, verts2);
        Mesh mesh = new Mesh();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        
        mesh.Clear();
        mesh.vertices = meshData.meshverts;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uvs;
        mesh.RecalculateNormals();
    }

    private static (Vector3[] meshverts, int[] triangles, Vector2[] uvs)
    genMeshTwoSidedBtwTwoCurves(Vector3[] curveOneVerts, Vector3[] curveTwoVerts){
    
        if(curveOneVerts.Length != curveTwoVerts.Length){
            return (null, null, null);
        }

        int n = curveOneVerts.Length;

        Vector3[] meshverts = new Vector3[2 * n];
        int[] triangles = new int[3 * 2 * (n - 1) * 2];
        Vector2[] uvs = new Vector2[2 * n];


        int meshVertexIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < n; i++){
            // create and add points 
            meshverts[meshVertexIndex] = curveOneVerts[i];
            meshverts[meshVertexIndex + 1] = curveTwoVerts[i];
            
            // uv calculation
            float completionPercent = i/(n - 1.0f);
            uvs[meshVertexIndex] = new Vector2(0.0f, completionPercent);
            uvs[meshVertexIndex + 1] = new Vector2(1.0f, completionPercent);

            // create triangles 
            if(i < n - 1){
                triangles[triangleIndex] = meshVertexIndex;
                triangles[triangleIndex + 1] = meshVertexIndex + 2;
                triangles[triangleIndex + 2] = meshVertexIndex + 1;

                triangles[triangleIndex + 3] = meshVertexIndex + 1;
                triangles[triangleIndex + 4] = meshVertexIndex + 2;
                triangles[triangleIndex + 5] = meshVertexIndex + 3;

                triangles[triangleIndex + 6] = meshVertexIndex;
                triangles[triangleIndex + 7] = meshVertexIndex + 1;
                triangles[triangleIndex + 8] = meshVertexIndex + 2;

                triangles[triangleIndex + 9] = meshVertexIndex + 1;
                triangles[triangleIndex + 10] = meshVertexIndex + 3;
                triangles[triangleIndex + 11] = meshVertexIndex + 2;


                triangleIndex += 12;
            }


            meshVertexIndex += 2; 
        }

        return (meshverts, triangles, uvs);
    } 

    private static (Vector3[] meshverts, int[] triangles, Vector2[] uvs)
    genMeshBtwTwoCurves(Vector3[] curveOneVerts, Vector3[] curveTwoVerts){
    
        if(curveOneVerts.Length != curveTwoVerts.Length){
            return (null, null, null);
        }

        int n = curveOneVerts.Length;

        Vector3[] meshverts = new Vector3[2 * n];
        int[] triangles = new int[3 * 2 * (n - 1)];
        Vector2[] uvs = new Vector2[2 * n];


        int meshVertexIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < n; i++){
            // create and add points 
            meshverts[meshVertexIndex] = curveOneVerts[i];
            meshverts[meshVertexIndex + 1] = curveTwoVerts[i];
            
            // uv calculation
            float completionPercent = i/(n - 1.0f);
            uvs[meshVertexIndex] = new Vector2(0.0f, completionPercent);
            uvs[meshVertexIndex + 1] = new Vector2(1.0f, completionPercent);

            // create triangles 
            if(i < n - 1){
                triangles[triangleIndex] = meshVertexIndex;
                triangles[triangleIndex + 1] = meshVertexIndex + 2;
                triangles[triangleIndex + 2] = meshVertexIndex + 1;

                triangles[triangleIndex + 3] = meshVertexIndex + 1;
                triangles[triangleIndex + 4] = meshVertexIndex + 2;
                triangles[triangleIndex + 5] = meshVertexIndex + 3;

                triangleIndex += 6;
            }


            meshVertexIndex += 2; 
        }

        return (meshverts, triangles, uvs);
    } 


    private static (Vector3[] leftVerts, Vector3[] rightVerts) 
        createSideVerts (Vector3[] points, float thickness) {
        uint numPoints = (uint)points.Length;

        Vector3[] rightVerts = new Vector3[numPoints];
        Vector3[] leftVerts = new Vector3[numPoints];
        
        int meshVertexIndex = 0;
        Vector3 dir;
        for (int i = 0; i != numPoints; i++){ 
            /// find left direction
            dir = Vector3.zero;
            if(i < numPoints - 1){
                dir += points[i + 1] - points[i]; 
            }

            if(i > 0){
                dir += points[i] - points[i - 1];
            }

            dir.Normalize();
            Vector3 left = -1f * Vector3.Cross(Vector3.up, dir);

            /// create and add points 
            leftVerts[meshVertexIndex] = points[i] + (thickness/2.0f) * left;
            rightVerts[meshVertexIndex] = points[i] - (thickness/2.0f) * left;

       
            meshVertexIndex += 1; 
        }

        return (leftVerts, rightVerts); 

    }

}
