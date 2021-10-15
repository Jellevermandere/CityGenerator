using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JelleVer.CityGenerator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class BuildingGenerator : MonoBehaviour
    {
        [Tooltip("The size of the building in X,Y,Z")]
        public Vector3 buildingSize = Vector3.one;
        [SerializeField]
        [Tooltip("The size of tiling of the building texture")]
        private Vector2 textureUnitSize = new Vector2(3, 3);
        [Tooltip("Update the building @ runtime or not")]
        [SerializeField]
        bool updateRuntime = false;
        [SerializeField]
        bool updateEditor = false;

        MeshFilter meshFilter;
        MeshCollider meshCollider;

        // Start is called before the first frame update
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = gameObject.GetComponent<MeshCollider>();

            CreateBuilding();
        }

        private void Update()
        {
            if (updateRuntime)
            {
                UpdateSize(buildingSize);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying) return;

            if (updateEditor)
            {
                UpdateSize(buildingSize);
            }
        }

        //update methods for specifice axis
        public void UpdateX(float X)
        {
            buildingSize.x = X;
            UpdateFlatMeshSize();

            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        public void UpdateY(float Y)
        {
            buildingSize.y = Y;
            UpdateFlatMeshSize();

            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        public void UpdateZ(float Z)
        {
            buildingSize.z = Z;
            UpdateFlatMeshSize();

            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        public void UpdateSize(Vector3 size)
        {
            buildingSize = size;

            UpdateFlatMeshSize();

            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        public void SetMaterial(Material mat)
        {
            GetComponent<MeshRenderer>().sharedMaterial = mat;
        }

        // the initial building generation
        public void CreateBuilding()
        {
            if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
            if (!meshCollider) meshCollider = gameObject.GetComponent<MeshCollider>();

            meshFilter.sharedMesh = CreateFlatBuildingMesh();
            meshFilter.sharedMesh.RecalculateBounds();
            meshCollider.sharedMesh = meshFilter.sharedMesh;

            //int textureRepeat = Mathf.RoundToInt(tiling * pathLength * 0.05f);
            //GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
        }

        // the initial mesh generation
        public Mesh CreateFlatBuildingMesh()
        {
            Vector3[] verts = new Vector3[4 * 6];
            Vector2[] uvs = new Vector2[verts.Length];
            int[] tris = new int[6 * 2 * 3]; //6 sides per cube, 2 tris per side, 3 points per tri

            //vertices
            verts[0] = -transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f;
            verts[1] = transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f;
            verts[2] = transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f;
            verts[3] = -transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f;

            verts[4] = -transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[5] = transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[6] = transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[7] = -transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;

            verts[8] = verts[0];
            verts[9] = verts[1];
            verts[10] = verts[2];
            verts[11] = verts[3];

            verts[12] = verts[4];
            verts[13] = verts[5];
            verts[14] = verts[6];
            verts[15] = verts[7];

            verts[16] = verts[0];
            verts[17] = verts[1];
            verts[18] = verts[2];
            verts[19] = verts[3];

            verts[20] = verts[4];
            verts[21] = verts[5];
            verts[22] = verts[6];
            verts[23] = verts[7];

            //uv's
            float topUV = Mathf.CeilToInt(buildingSize.y / textureUnitSize.y);
            float rightUV = Mathf.CeilToInt(buildingSize.x / textureUnitSize.x);
            float forwardUV = Mathf.CeilToInt(buildingSize.z / textureUnitSize.x);

            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(rightUV, 0);
            uvs[2] = new Vector2(rightUV + forwardUV, 0);
            uvs[3] = new Vector2(forwardUV, 0);

            uvs[4] = new Vector2(0, topUV);
            uvs[5] = new Vector2(rightUV, topUV);
            uvs[6] = new Vector2(rightUV + forwardUV, topUV);
            uvs[7] = new Vector2(forwardUV, topUV);

            uvs[8] = uvs[0];
            uvs[9] = uvs[1];
            uvs[10] = uvs[2];
            uvs[11] = uvs[3];

            uvs[12] = uvs[4];
            uvs[13] = uvs[5];
            uvs[14] = uvs[6];
            uvs[15] = uvs[7];

            uvs[16] = uvs[0];
            uvs[17] = uvs[1];
            uvs[18] = uvs[2];
            uvs[19] = uvs[3];

            uvs[20] = uvs[4];
            uvs[21] = uvs[5];
            uvs[22] = uvs[6];
            uvs[23] = uvs[7];

            //tri's
            //bottom side
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 3;

            tris[3] = 1;
            tris[4] = 2;
            tris[5] = 3;

            //top side
            tris[6] = 4;
            tris[7] = 7;
            tris[8] = 5;

            tris[9] = 5;
            tris[10] = 7;
            tris[11] = 6;

            //front side
            tris[12] = 8;
            tris[13] = 12;
            tris[14] = 9;

            tris[15] = 9;
            tris[16] = 12;
            tris[17] = 13;

            //right side
            tris[18] = 17;
            tris[19] = 21;
            tris[20] = 18;

            tris[21] = 18;
            tris[22] = 21;
            tris[23] = 22;

            //back side
            tris[24] = 10;
            tris[25] = 14;
            tris[26] = 11;

            tris[27] = 11;
            tris[28] = 14;
            tris[29] = 15;

            //left side
            tris[30] = 19;
            tris[31] = 23;
            tris[32] = 16;

            tris[33] = 16;
            tris[34] = 23;
            tris[35] = 20;

            //mesh
            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.name = "Building";
            mesh.RecalculateNormals();
            return mesh;

        }


        // update the vertex positions
        private void UpdateFlatMeshSize()
        {

            if (!CheckMesh())
            {
                CreateBuilding();
                return;
            }

            Vector3[] verts = meshFilter.sharedMesh.vertices;
            Vector2[] uvs = meshFilter.sharedMesh.uv;

            //vertices
            verts[0] = -transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f;
            verts[1] = transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f;
            verts[2] = transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f;
            verts[3] = -transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f;

            verts[4] = -transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[5] = transform.right * buildingSize.x / 2f - transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[6] = transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;
            verts[7] = -transform.right * buildingSize.x / 2f + transform.forward * buildingSize.z / 2f + transform.up * buildingSize.y;

            verts[8] = verts[0];
            verts[9] = verts[1];
            verts[10] = verts[2];
            verts[11] = verts[3];

            verts[12] = verts[4];
            verts[13] = verts[5];
            verts[14] = verts[6];
            verts[15] = verts[7];

            verts[16] = verts[0];
            verts[17] = verts[1];
            verts[18] = verts[2];
            verts[19] = verts[3];

            verts[20] = verts[4];
            verts[21] = verts[5];
            verts[22] = verts[6];
            verts[23] = verts[7];

            //uv's
            float topUV = Mathf.CeilToInt(buildingSize.y / textureUnitSize.y);
            float rightUV = Mathf.CeilToInt(buildingSize.x / textureUnitSize.x);
            float forwardUV = Mathf.CeilToInt(buildingSize.z / textureUnitSize.x);

            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(rightUV, 0);
            uvs[2] = new Vector2(rightUV + forwardUV, 0);
            uvs[3] = new Vector2(forwardUV, 0);

            uvs[4] = new Vector2(0, topUV);
            uvs[5] = new Vector2(rightUV, topUV);
            uvs[6] = new Vector2(rightUV + forwardUV, topUV);
            uvs[7] = new Vector2(forwardUV, topUV);

            uvs[8] = uvs[0];
            uvs[9] = uvs[1];
            uvs[10] = uvs[2];
            uvs[11] = uvs[3];

            uvs[12] = uvs[4];
            uvs[13] = uvs[5];
            uvs[14] = uvs[6];
            uvs[15] = uvs[7];

            uvs[16] = uvs[0];
            uvs[17] = uvs[1];
            uvs[18] = uvs[2];
            uvs[19] = uvs[3];

            uvs[20] = uvs[4];
            uvs[21] = uvs[5];
            uvs[22] = uvs[6];
            uvs[23] = uvs[7];

            meshFilter.sharedMesh.vertices = verts;
            meshFilter.sharedMesh.uv = uvs;
            meshFilter.sharedMesh.RecalculateNormals();
            meshFilter.sharedMesh.RecalculateBounds();

        }

        bool CheckMesh()
        {
            if (meshFilter == null || meshCollider == null)
            {
                meshFilter = GetComponent<MeshFilter>();
                meshCollider = gameObject.GetComponent<MeshCollider>();
            }

            if(meshFilter.sharedMesh != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the current meshfilter
        /// </summary>
        public void ResetMesh()
        {
            meshFilter = null;
            meshCollider = null;
        }
    }
}