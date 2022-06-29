using UnityEngine;

public class CubicSplineObj : MonoBehaviour {

    private BSpline m_spline = new BSpline();
    
    [SerializeField] private float m_width;


    public void fitCubicSpline(float[] points, Vector3 start, Vector3 end, Vector3 up){
        m_spline = new BSpline();


        int n = points.Length;    

        Vector3 dir = (end - start).normalized;
        Vector3 dir2 = Vector3.Cross(up, dir);
        
        float d = Vector3.Distance(start, end) / (n - 1);
        
        Vector3[] controlPoints = new Vector3[n];
        for(int i = 0; i < n; i++){
            controlPoints[i] = start + dir * i * d + dir2 * points[i] * m_width;

        }


        m_spline.fit(controlPoints);

    }


    public Vector3[] samplePoints(float spacing, float resolution, Vector3 start, Vector3 end){ 
        Vector3[] points = m_spline.evenlySpacedPoints(spacing, resolution);        
        return points; 
    }
}
