using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurveRenderer))]
public class CurveRendererEditor : Editor {
    public override void OnInspectorGUI() {
        CurveRenderer crvRen = (CurveRenderer) target;

        if(DrawDefaultInspector() && crvRen.autoUpdate){
            crvRen.display();
        }


        if(GUILayout.Button("Generate")){
            crvRen.display();
        }
    }

}
