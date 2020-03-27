using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreation : MonoBehaviour
{
    public float firstInset = .1f; 
    public float secondsInset = .1f; 
    public float thirdInset = .1f;

    [ContextMenu("Create Mesh")]
    public void CreateMesh() {
        var scale = transform.localScale;
        transform.localScale = Vector3.one;

        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        Quaternion[] q = new Quaternion[] {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(180, 0, 0),
            Quaternion.Euler(270, 0, 0),
            Quaternion.Euler(0, 0, 90),
            Quaternion.Euler(0, 0, 270)
        };
        for (int i = 0; i < 6; i++) {
            var vertexSide = new List<Vector3>();
            vertexSide.AddRange(GetPointRing(0, 0));
            vertexSide.AddRange(GetPointRing(firstInset, 0));
            vertexSide.AddRange(GetPointRing(firstInset, 0));
            vertexSide.AddRange(GetPointRing(secondsInset, -.03f));
            vertexSide.AddRange(GetPointRing(secondsInset, -.03f));
            vertexSide.AddRange(GetPointRing(thirdInset, 0));

            print(q[i].eulerAngles);
            
            for (int j = 0; j < vertexSide.Count; j++) {
                vertexSide[j] += new Vector3(-.5f, .5f, -.5f);
                var v = vertexSide[j];
                v = q[i] * v;
                vertexSide[j] = v;
                //vertexSide[j] = q * vertexSide[j];

                Debug.DrawRay(vertexSide[j], (q[i] * Vector3.up) * .1f, Color.red, 5);
            }
            vertices.AddRange(vertexSide);
            vertexSide.Clear();
        }

        mesh.SetVertices(vertices);
        var tris = new List<int>();

        for (int h = 0; h < 6 * 6; h+=2) {
            for (int i = 0; i < 4; i++) {

                var x = h * 8;

                tris.AddRange(GetQuad(
                    x + 0 + i * 2,
                    x + (1 + i * 2) % 8,
                    x + 8 + (2 * i + 1) % 8,
                    x + 8 + i * 2));
            }
        }



        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        
        //        mesh.SetTriangles(new int[] { 0, 1, 4 , 4, 1, 5},0);

        GetComponent<MeshFilter>().mesh = mesh;
    }

    List<Vector3> GetPointRing(float insetAmount, float extrude) {
        var vertices = new List<Vector3>();

        var ia = insetAmount;
        vertices.Add(new Vector3(ia, extrude, ia));
        vertices.Add(new Vector3(ia, extrude, 1 - ia));
        vertices.Add(new Vector3(ia, extrude, 1 - ia));
        vertices.Add(new Vector3(1 - ia, extrude, 1 - ia));
        vertices.Add(new Vector3(1 - ia, extrude, 1 - ia));
        vertices.Add(new Vector3(1 - ia, extrude, ia));
        vertices.Add(new Vector3(1 - ia, extrude, ia));
        vertices.Add(new Vector3(ia, extrude, ia));
        return vertices;
    }

    int[] GetQuad(int a, int b, int c, int d) {
        return new int[] {
                    a, b,d,
                    d, b, c};
    }

}
