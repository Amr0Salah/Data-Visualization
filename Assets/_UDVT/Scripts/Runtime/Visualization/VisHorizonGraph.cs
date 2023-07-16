using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Data;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class VisHorizonGraph : Vis
{
    //NewCode_12

    public double[,] KDEresult = null;
    private Material material01;
    private Material material02;
    public VisHorizonGraph()
    {
        title = "HorizonGraph";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
        material01 = (Material)Resources.Load("VisHorizMaterials/VisHorizontalMaterial_01");
        material02 = (Material)Resources.Load("VisHorizMaterials/VisHorizontalMaterial_02");
    }

    // TODO: It will change
    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);
        KDEresult = KernelDensityEstimation.KDE(dataSets[0].ElementAt(0).Value, CurrentParams.kdeSigmaValue, CurrentParams.kdeStepsValue);

        ChangeDataMarks();
        //## 01:  Create Axes and Grids
        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);
        // Y Axis
        visContainer.CreateAxis("HorizonGraph", dataSets[0].ElementAt(1).Value, Direction.Y);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YPos, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(1).Value);

        //## 03: Draw all Data Points with the provided Channels
        
        visContainer.CreateHorizonGraphDataMarks(dataMarkPrefab);
        //visContainer.CreateBaseLineDataMarks(dataMarkPrefab , 0.25f);
        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        List<DataMark> datamarks = visContainer.dataMarkList;
        CreateBaseLine(datamarks, 3, 0);

        ConnectDataMarks(datamarks);

       


        return visContainerObject;
    }
    public override void ChangeDataMarks()
    {
        var xName = dataSets[0].ElementAt(0).Key;
        var yName = dataSets[0].ElementAt(1).Key;

        dataSets[0][xName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 0]).ToArray();
        dataSets[0][yName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 1]).ToArray();
    }

    /// <summary>
    /// It connects the dots with a line.
    /// </summary>
    /// <param name="datamarks"></param>


    public void CreateBaseLine(List<DataMark> datamarks, int bins,int split)
    {

        int minPosY = 0, maxPosY = 0, maxPosX = 0, minPosX = 0;
        double minValueY = 0, maxValueY = 0, maxValueX = 0, minValueX = 0;

        var StartLine = datamarks[split].GetDataMarkInstance();

        var startTransform = datamarks[0].GetDataMarkInstance().transform;
        for (var i = 0; i < datamarks.Count; i++)
        {
            startTransform = datamarks[i].GetDataMarkInstance().transform;
            if (i == 0)
            {
                minValueY = startTransform.position.y;
                minPosY = i;
                maxValueY = startTransform.position.y;
                maxPosY = i;
                minValueX = startTransform.position.x;
                minPosX = i;
                maxValueX = startTransform.position.x;
                maxPosX = i;
            }
            if (minValueY > startTransform.position.y)
            {
                minValueY = startTransform.position.y;
                minPosY = i;
            }
            if (maxValueY < startTransform.position.y)
            {
                maxValueY = startTransform.position.y;
                maxPosY = i;
            }
            if (minValueX > startTransform.position.x)
            {
                minValueX = startTransform.position.x;
                minPosX = i;
            }
            if (maxValueX < startTransform.position.x)
            {
                maxValueX = startTransform.position.x;
                maxPosX = i;
            }
        }

        startTransform = datamarks[minPosX].GetDataMarkInstance().transform;
        var endTransform = datamarks[maxPosX].GetDataMarkInstance().transform;
        var firstLineEnd = new Vector3(0, StartLine.transform.position.y, startTransform.position.z);
        var firstLineStart = new Vector3(endTransform.position.x + 0.025f, StartLine.transform.position.y, endTransform.position.z);
        float PosY = startTransform.position.y;
        List<GameObject> breakpoints = new List<GameObject>()
        {
            StartLine
        };
        var refBreakpoint = StartLine.transform;
        List<Mesh> meshes = new List<Mesh>()
        {
            new Mesh()
        };
        int currentMesh = 0;
        bool increasing = false;
        List<Vector3> points = new List<Vector3>()
        {
            StartLine.transform.position
        };
        for (var i = 1; i < datamarks.Count; i++)
        {
            endTransform = datamarks[i].GetDataMarkInstance().transform;
            points.Add(datamarks[i].GetDataMarkInstance().transform.position);

            if (refBreakpoint.position.y < endTransform.position.y && !increasing)
            {
                refBreakpoint = endTransform;
                breakpoints.Add(datamarks[i].GetDataMarkInstance());
                increasing = true;
                meshes[currentMesh++].vertices = points.ToArray();
                points.Clear();
                points.Add(datamarks[i].GetDataMarkInstance().transform.position);

                meshes.Add(new Mesh());

            }
            else if (refBreakpoint.position.y > endTransform.position.y && increasing)
            {
                refBreakpoint = endTransform;
                breakpoints.Add(datamarks[i].GetDataMarkInstance());
                increasing = false;
                meshes[currentMesh++].vertices = points.ToArray();
                points.Clear();
                points.Add(datamarks[i].GetDataMarkInstance().transform.position);

                meshes.Add(new Mesh());

            }
        }

        meshes[currentMesh].vertices = points.ToArray();
        int index = 0;
        foreach (var mesh in meshes)
        {
            
            var go = new GameObject("mesh test" + UnityEngine.Random.seed);
            //  var gored = new GameObject("mesh test" + UnityEngine.Random.seed);
            List<int> triangles = new List<int>();
            List<int> trianglesRed = new List<int>();
            List<Vector3> meshVertices = mesh.vertices.ToList();
            // List<Vector3> meshVertices2 = mesh.vertices.ToList();
            for (int i = 0; i < mesh.vertices.Length - 1; i++)
            {
                if (mesh.vertices[i].y < mesh.vertices[split].y)
                {
                    meshVertices[i] = new Vector3(meshVertices[i].x, Math.Abs(meshVertices[i].y - meshVertices[split].y) + meshVertices[split].y, meshVertices[i].z);

                }
                if (mesh.vertices[i+1].y < mesh.vertices[split].y)
                {
                    meshVertices[i + 1] = new Vector3(meshVertices[i + 1].x, Math.Abs(meshVertices[i + 1].y - meshVertices[split].y) + meshVertices[split].y, meshVertices[i + 1].z);

                }

                meshVertices.Add(new Vector3(meshVertices[i].x, meshVertices[0].y, meshVertices[i].z));

                triangles.AddRange(new int[] { i, i + 1, meshVertices.Count() - 1 });
                if (triangles.Count > 3)
                {
                    triangles.AddRange(new int[] { i, meshVertices.Count() - 1, meshVertices.Count() - 2 });
                }
            }

            meshVertices.Add(new Vector3(meshVertices[mesh.vertices.Length - 1].x, meshVertices[mesh.vertices.Length - 1].y, meshVertices[mesh.vertices.Length - 1].z));
            triangles.AddRange(new int[] { mesh.vertices.Length - 1, mesh.vertices.Length - 2, meshVertices.Count() - 1 });

           // triangles.AddRange(new int[] { mesh.vertices.Length - 1, meshVertices.Count() - 1, meshVertices.Count() - 2 });

            mesh.vertices = meshVertices.ToArray();
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.RecalculateNormals();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshFilter>();
            go.GetComponent<MeshFilter>().mesh = mesh;
            if(index==0)
            {
            go.GetComponent<MeshRenderer>().material = material02;
                index = 1;
            }
            else if(index==1)
            {
                go.GetComponent<MeshRenderer>().material = material01;
                index = 0;

            }
            
        }
        var refTransform = datamarks[split].GetDataMarkInstance().transform;
        for (int i = 0; i < datamarks.Count() ; i++)
        {
            var label = datamarks[i].GetDataMarkInstance().transform;
            if (label.position.y < refTransform.position.y)
            {
                label.position = new Vector3(label.position.x, Math.Abs(label.position.y - refTransform.position.y) + refTransform.position.y, label.position.z);

            }
        }
        var start = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, startTransform.localPosition.z);
        GameObject line = new GameObject();
        line.name = "BaseLine";
        line.transform.localPosition = start;
        line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        line.AddComponent<LineRenderer>();
        LineRenderer renderer = line.GetComponent<LineRenderer>();
        renderer.useWorldSpace = true;
        renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
        renderer.material.SetColor("_Color", Color.white);
        renderer.startWidth = 0.003f;
        renderer.endWidth = 0.003f;

        renderer.SetPosition(0, firstLineStart);
        renderer.SetPosition(1, firstLineEnd);
    }

    public void ConnectDataMarks(List<DataMark> datamarks)
    {
        for (int i = 0; i < datamarks.Count - 1; i++)
        {
            var startTransform = datamarks[i].GetDataMarkInstance().transform;
            var endTransform = datamarks[i + 1].GetDataMarkInstance().transform;
            var start = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, startTransform.localPosition.z);

            GameObject line = new GameObject();
            line.name = i + "";
            line.transform.localPosition = start;
            line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            line.AddComponent<LineRenderer>();

            LineRenderer renderer = line.GetComponent<LineRenderer>();
            renderer.useWorldSpace = true;
            renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
            renderer.material.SetColor("_Color", Color.white);
            renderer.startWidth = 0.003f;
            renderer.endWidth = 0.003f;
            renderer.SetPosition(0, startTransform.position);
            renderer.SetPosition(1, endTransform.position);
        }
    }

}
