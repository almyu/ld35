using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public class SheepModel : MonoBehaviour {

        public MeshFilter body;
        public Mesh ballMesh;
        public Vector2 ballScaleRange = new Vector2(0.9f, 1.1f);

        private void Awake() {
            var vertices = body.sharedMesh.vertices;
            var normals = body.sharedMesh.normals;

            var submeshes = new CombineInstance[vertices.Length];

            for (int i = 0; i < submeshes.Length; ++i) {
                submeshes[i] = new CombineInstance {
                    mesh = ballMesh,
                    transform = Matrix4x4.TRS(
                        vertices[i],
                        Quaternion.LookRotation(normals[i]),
                        Random.Range(ballScaleRange.x, ballScaleRange.y) * Vector3.one)
                };
            }

            var combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(submeshes, true, true);
            body.sharedMesh = combinedMesh;
        }

        public void PaintSheep(Material material) {
            body.GetComponent<Renderer>().sharedMaterial = material;
        }
    }
}
