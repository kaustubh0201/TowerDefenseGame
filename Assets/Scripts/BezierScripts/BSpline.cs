using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline {
    
    private int m_npoints;
    private Vector3[] m_controlPoints; 
    private Vector3[] m_terminalTangents; 

    public BSpline(){  }

    public BSpline(Vector3[] controlPoints, Vector3 d0, Vector3 dn){
        m_npoints = controlPoints.Length;
        m_controlPoints = controlPoints;

        m_terminalTangents = new Vector3[m_npoints]; 
        m_terminalTangents[0] = d0;
        m_terminalTangents[m_npoints - 1] = dn;
    }


    public BSpline(Vector3[] controlPoints){
        m_npoints = controlPoints.Length;
        m_controlPoints = controlPoints;

        m_terminalTangents = new Vector3[m_npoints]; 
        m_terminalTangents[0] = (controlPoints[1] - controlPoints[0])/3.0f;
        m_terminalTangents[m_npoints - 1] = (controlPoints[m_npoints - 1] - controlPoints[m_npoints - 2])/3.0f; 
    }


    public void fit(){
        if(m_npoints <= 2) return;

        Vector3[] a = new Vector3[m_npoints - 1];  
        float[] b = new float[m_npoints - 1]; 

        a[0] = (m_controlPoints[2] - m_controlPoints[0] - m_terminalTangents [0]) / 4.0f;
        b[0] = -0.25f;


        for(int i = 1; i < m_npoints - 1; i++){
            b[i] = -1.0f/(4.0f + b[i -1]);
            a[i] = (m_controlPoints[i + 1] - m_controlPoints[i - 1] - a[i - 1])/(b[i - 1] + 4.0f);
        }


        for(int i = m_npoints - 2; i >= 0; i--){
            m_terminalTangents[i] = a[i] + b[i] * m_terminalTangents[i + 1];
        }
    }

    public void fit(Vector3[] controlPoints){
        m_npoints = controlPoints.Length;
        m_controlPoints = controlPoints;

        m_terminalTangents = new Vector3[m_npoints]; 
        m_terminalTangents[0] = (controlPoints[1] - controlPoints[0])/3.0f;
        m_terminalTangents[m_npoints - 1] = (controlPoints[m_npoints - 1] - controlPoints[m_npoints - 2])/3.0f;

        fit();
    }

    public void fit(Vector3[] controlPoints, Vector3 d0, Vector3 dn){
        m_npoints = controlPoints.Length;
        m_controlPoints = controlPoints;

        m_terminalTangents = new Vector3[m_npoints]; 
        m_terminalTangents[0] = d0;
        m_terminalTangents[m_npoints - 1] = dn;

        fit();
    }

    
    public Vector3[] evenlySpacedPoints(float spacing, float resolution){
        
        List<Vector3> points = new List<Vector3>();
        points.Add(m_controlPoints[0]);
        Vector3 prev = m_controlPoints[0];
        float distSinceLast = 0.0f;
        


        for (int i = 0; i < (m_npoints) - 1; i++){
            var cp = returnControlPoints(i);
            
            float estimatedCurveLength = Vector3.Distance(cp.p0, cp.p3) 
                                    + ( Vector3.Distance(cp.p0, cp.p1) +
                                        Vector3.Distance(cp.p1, cp.p2) + 
                                        Vector3.Distance(cp.p2, cp.p3)) / 2.0f;

            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10.0f);
            float t = 0.0f;

            while(t <= 1.0f){
                t += 1.0f/divisions;

                Vector3 p = evalBCurve(cp.p0, cp.p1, cp.p2, cp.p3, t);
                distSinceLast += Vector3.Distance(prev, p);

                while(distSinceLast >= spacing){
                    float overshootDist = distSinceLast - spacing;
                    Vector3 newPoint = p + (prev - p).normalized * overshootDist;
                    points.Add(newPoint);
                    prev = newPoint;
                    distSinceLast = overshootDist;
                }

                prev = p;
            }
        }

        points.Add(m_controlPoints[m_npoints - 1]);

        return points.ToArray();
    }


    private 
        (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) 
        returnControlPoints(int j){
       
            Vector3 p0 = m_controlPoints[j];
            Vector3 p1 = m_controlPoints[j] + m_terminalTangents[j];
            Vector3 p2 = m_controlPoints[j + 1] - m_terminalTangents[j + 1]; 
            Vector3 p3 = m_controlPoints[j + 1];

            return (p0, p1, p2, p3);
    }


    private Vector3 eval(float t, int j){

        var controlPoints = returnControlPoints(j);
        
        return evalBCurve(controlPoints.p0, 
                controlPoints.p1, controlPoints.p2, controlPoints.p3, t);
    }

    private Vector3 evalBCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
        return  
            (1 - t) * (1 - t) * (1 - t) * p0 
            + 3 * t * (1 - t) * (1 - t) * p1 
            + 3 * t * t * (1 - t) * p2 
            + t * t * t * p3;
    }
}
