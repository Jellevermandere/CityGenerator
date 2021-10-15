using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JelleVer.CityGenerator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class BorderGenerator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 innerSize;
        [SerializeField]
        [Tooltip("x = borderwidth, y = endDepth")]
        private Vector2 outerSize;

        MeshFilter meshFilter;
        MeshCollider meshCollider;


        private bool isMade;
        [SerializeField]
        private bool update;

        // Start is called before the first frame update
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = gameObject.GetComponent<MeshCollider>();

            CreateWall();
        }

        // Update is called once per frame
        void Update()
        {
            if (update)
            {
                UpdateFlatMeshSize();
                meshCollider.sharedMesh = meshFilter.mesh;
            }
        }

        public void UpdateSize(Vector3 inner, Vector2 outer)
        {
            innerSize = inner;
            outerSize = outer;

            UpdateFlatMeshSize();

            meshCollider.sharedMesh = meshFilter.mesh;
        }

        // the initial building generation
        public void CreateWall()
        {
            if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
            if (!meshCollider) meshCollider = gameObject.GetComponent<MeshCollider>();

            meshFilter.mesh = CreateFlatWallMesh();
            meshFilter.mesh.RecalculateBounds();
            meshCollider.sharedMesh = meshFilter.mesh;

            //int textureRepeat = Mathf.RoundToInt(tiling * pathLength * 0.05f);
            //GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
        }

        // the initial mesh generation
        public Mesh CreateFlatWallMesh()
        {
            Vector3[] verts = new Vector3[40];
            Vector2[] uvs = new Vector2[verts.Length];
            int[] tris = new int[4 * 3 * 2 * 3]; //4 sides per wall, 3 quads per side, 2 tris per quad, 3 points per tri

            verts = UpdateVerts(verts);
            uvs = UpdateUvs(uvs);

            Debug.Log("vertices: " + verts.Length + "Uvs: " + uvs.Length);

            //tri's
            //inner side front
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 5;

            tris[3] = 0;
            tris[4] = 5;
            tris[5] = 4;

            //inner side right
            tris[6] = 17;
            tris[7] = 18;
            tris[8] = 22;

            tris[9] = 17;
            tris[10] = 22;
            tris[11] = 21;

            //inner side back
            tris[12] = 2;
            tris[13] = 3;
            tris[14] = 7;

            tris[15] = 2;
            tris[16] = 7;
            tris[17] = 6;

            //inner side left
            tris[18] = 19;
            tris[19] = 16;
            tris[20] = 20;

            tris[21] = 19;
            tris[22] = 20;
            tris[23] = 23;

            //top side front
            tris[24] = 32;
            tris[25] = 33;
            tris[26] = 37;

            tris[27] = 32;
            tris[28] = 37;
            tris[29] = 36;

            //top side right
            tris[30] = 33;
            tris[31] = 34;
            tris[32] = 38;

            tris[33] = 33;
            tris[34] = 38;
            tris[35] = 37;

            //top side back
            tris[36] = 34;
            tris[37] = 35;
            tris[38] = 39;

            tris[39] = 34;
            tris[40] = 39;
            tris[41] = 38;

            //top side left
            tris[42] = 35;
            tris[43] = 32;
            tris[44] = 36;

            tris[45] = 35;
            tris[46] = 36;
            tris[47] = 39;

            //outer side front
            tris[48] = 8;
            tris[49] = 9;
            tris[50] = 13;

            tris[51] = 8;
            tris[52] = 13;
            tris[53] = 12;

            //outer side right
            tris[54] = 25;
            tris[55] = 26;
            tris[56] = 30;

            tris[57] = 25;
            tris[58] = 30;
            tris[59] = 29;

            //outer side back
            tris[60] = 10;
            tris[61] = 11;
            tris[62] = 15;

            tris[63] = 10;
            tris[64] = 15;
            tris[65] = 14;

            //outer side left
            tris[66] = 27;
            tris[67] = 24;
            tris[68] = 28;

            tris[69] = 27;
            tris[70] = 28;
            tris[71] = 31;


            //mesh
            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;
            mesh.RecalculateNormals();

            isMade = true;

            return mesh;

        }


        // update the vertex positions
        private void UpdateFlatMeshSize()
        {

            if (!isMade)
            {
                CreateWall();
                return;
            }

            meshFilter.mesh.vertices = UpdateVerts(meshFilter.mesh.vertices);
            meshFilter.mesh.uv = UpdateUvs(meshFilter.mesh.uv);

            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();

        }

        Vector3[] UpdateVerts(Vector3[] verts)
        {
            //vertices
            verts[0] = -transform.right * innerSize.x / 2f - transform.forward * innerSize.z / 2f;
            verts[1] = transform.right * innerSize.x / 2f - transform.forward * innerSize.z / 2f;
            verts[2] = transform.right * innerSize.x / 2f + transform.forward * innerSize.z / 2f;
            verts[3] = -transform.right * innerSize.x / 2f + transform.forward * innerSize.z / 2f;

            verts[4] = -transform.right * innerSize.x / 2f - transform.forward * innerSize.z / 2f + transform.up * innerSize.y;
            verts[5] = transform.right * innerSize.x / 2f - transform.forward * innerSize.z / 2f + transform.up * innerSize.y;
            verts[6] = transform.right * innerSize.x / 2f + transform.forward * innerSize.z / 2f + transform.up * innerSize.y;
            verts[7] = -transform.right * innerSize.x / 2f + transform.forward * innerSize.z / 2f + transform.up * innerSize.y;

            verts[8] = -transform.right * (innerSize.x / 2f + outerSize.x) - transform.forward * (innerSize.z / 2f + outerSize.x) + transform.up * innerSize.y;
            verts[9] = transform.right * (innerSize.x / 2f + outerSize.x) - transform.forward * (innerSize.z / 2f + outerSize.x) + transform.up * innerSize.y;
            verts[10] = transform.right * (innerSize.x / 2f + outerSize.x) + transform.forward * (innerSize.z / 2f + outerSize.x) + transform.up * innerSize.y;
            verts[11] = -transform.right * (innerSize.x / 2f + outerSize.x) + transform.forward * (innerSize.z / 2f + outerSize.x) + transform.up * innerSize.y;

            verts[12] = -transform.right * (innerSize.x / 2f + outerSize.x) - transform.forward * (innerSize.z / 2f + outerSize.x) - transform.up * outerSize.y;
            verts[13] = transform.right * (innerSize.x / 2f + outerSize.x) - transform.forward * (innerSize.z / 2f + outerSize.x) - transform.up * outerSize.y;
            verts[14] = transform.right * (innerSize.x / 2f + outerSize.x) + transform.forward * (innerSize.z / 2f + outerSize.x) - transform.up * outerSize.y;
            verts[15] = -transform.right * (innerSize.x / 2f + outerSize.x) + transform.forward * (innerSize.z / 2f + outerSize.x) - transform.up * outerSize.y;

            verts[16] = verts[0];
            verts[17] = verts[1];
            verts[18] = verts[2];
            verts[19] = verts[3];

            verts[20] = verts[4];
            verts[21] = verts[5];
            verts[22] = verts[6];
            verts[23] = verts[7];

            verts[24] = verts[8];
            verts[25] = verts[9];
            verts[26] = verts[10];
            verts[27] = verts[11];

            verts[28] = verts[12];
            verts[29] = verts[13];
            verts[30] = verts[14];
            verts[31] = verts[15];

            verts[32] = verts[4];
            verts[33] = verts[5];
            verts[34] = verts[6];
            verts[35] = verts[7];

            verts[36] = verts[8];
            verts[37] = verts[9];
            verts[38] = verts[10];
            verts[39] = verts[11];

            return verts;
        }

        Vector2[] UpdateUvs(Vector2[] uvs)
        {
            //uv's

            //uv devide -> inner:0.4, top:0.2, outer:0.4

            uvs[0] = new Vector2(0, 1);
            uvs[1] = new Vector2(1, 1);
            uvs[4] = new Vector2(0, 0.6f);
            uvs[5] = new Vector2(1, 0.6f);
            uvs[8] = new Vector2(0, 0.4f);
            uvs[9] = new Vector2(1, 0.4f);
            uvs[12] = new Vector2(0, 0);
            uvs[13] = new Vector2(1, 0);

            uvs[2] = new Vector2(1, 1);
            uvs[3] = new Vector2(0, 1);
            uvs[6] = new Vector2(1, 0.6f);
            uvs[7] = new Vector2(0, 0.6f);
            uvs[10] = new Vector2(1, 0.4f);
            uvs[11] = new Vector2(0, 0.4f);
            uvs[14] = new Vector2(1, 0);
            uvs[15] = new Vector2(0, 0);

            uvs[16] = uvs[0];
            uvs[17] = uvs[1];
            uvs[18] = uvs[2];
            uvs[19] = uvs[3];

            uvs[20] = uvs[4];
            uvs[21] = uvs[5];
            uvs[22] = uvs[6];
            uvs[23] = uvs[7];

            uvs[24] = uvs[8];
            uvs[25] = uvs[9];
            uvs[26] = uvs[10];
            uvs[27] = uvs[11];

            uvs[28] = uvs[12];
            uvs[29] = uvs[13];
            uvs[30] = uvs[14];
            uvs[31] = uvs[15];

            uvs[32] = uvs[4];
            uvs[33] = uvs[5];
            uvs[34] = uvs[6];
            uvs[35] = uvs[7];

            uvs[36] = uvs[8];
            uvs[37] = uvs[9];
            uvs[38] = uvs[10];
            uvs[39] = uvs[11];

            return uvs;
        }
    }
}