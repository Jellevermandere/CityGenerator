using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JelleVer.CityGenerator
{
    [CustomEditor(typeof(BuildingGenerator))]
    public class BuildingGeneratorEditor : UnityEditor.Editor
    {
        BuildingGenerator buildingGenerator;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(GUILayout.Button("Reset Mesh"))
            {
                buildingGenerator.ResetMesh();
            }
        }

        private void OnEnable()
        {
            buildingGenerator = (BuildingGenerator)target;
        }
    }
}

