using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Path))]
public class PathEditor : Editor {

    public class PointData
    {
        public Transform transform;
        public int next;
        public int prev;

        public PointData(Transform transform, int next, int prev)
        {
            this.transform = transform;
            this.next = next;
            this.prev = prev;
        }
    }
    [SerializeField]
    public List<PointData> points;

    public List<bool> filled;

    public float editorPointSize = 2.0f;

    private void OnSceneGUI()
    {
        UpdatePathData();
        DrawPath();
    }

    private void UpdatePathData()
    {
        Path p = target as Path;
        if (points == null)
        {
            //Debug.Log("The points field is null! Resetting");
            points = new List<PointData>(p.points.Length);
            filled = new List<bool>(p.points.Length - 1);
            for(int i = 0; i < p.points.Length; i++)
            {
                
                points.Add(new PointData(p.points[i], i + 1, i - 1));
                if (p.points[i] != null)
                {
                    p.points[i].transform.gameObject.name = "Point";
                    filled.Add(true);
                }
                else
                {
                    filled.Add(false);
                }
            }
            points[points.Count - 1].next = -1;
            if (points[points.Count - 1].transform != null)
            {
                points[points.Count - 1].transform.gameObject.name = "EndPoint";
            }
        }
            bool modified = false;
            //if no elements in points, it hasn't been initialized yet or must be reset (in either case we'll reset)
            if (points.Count == 0)
            {
                //Debug.Log("the points field has no elements");
                modified = true;
                if (p.transform.childCount > 0)
                {
                    points = new List<PointData>(p.transform.childCount + 1);
                    points.Add(new PointData(p.transform, 1, -1)); //INVARIANT - the first slot in points is always reserved for the path object's transform
                    filled = new List<bool>(p.transform.childCount);
                    for (int i = 1; i < p.transform.childCount + 1; i++)
                    {
                        points.Add(new PointData(p.transform.GetChild(i - 1), i + 1, i - 1)); //default configuration establishes an ordered path of points
                        filled.Add(true);
                    }
                    points[points.Count - 1].next = -1; //set end node to point to -1 to mark end of linked structure
                    points[points.Count - 1].transform.gameObject.name = "EndPoint";
                } else
                {
                    points = new List<PointData>(2);
                    points.Add(new PointData(p.transform, 1, -1)); //INVARIANT - the first slot in points is always reserved for the path object's transform
                    GameObject endPoint = new GameObject("EndPoint");
                    Vector3 position = p.transform.position + new Vector3(5.0f,0.0f,0.0f);
                    endPoint.transform.position = position;
                    endPoint.transform.parent = p.transform;
                    points.Add(new PointData(endPoint.transform, -1, 0));
                    filled = new List<bool>(1);
                    filled.Add(true);
                }
            }
            //if there are currently elements in points, it has been initialized, so let's iterate through the linked structure and initialize new objects at the end
            else
            {
                int currIndex = 0; //start at the path origin
                bool[] found = new bool[p.transform.childCount]; //keep track of which points we found
                filled = new List<bool>(new bool[points.Count - 1]); //keep track of which points don't exist anymore or aren't a child of the path
                PointData curr = points[currIndex]; //keep track of the current index object, will end first loopo at last points element
                int iterationCounter = 0;
                while (curr.next < points.Count && curr.next > -1)
                {
                    iterationCounter++;
                    if(iterationCounter > 1000)
                    {
                        throw new Exception("Above 1000 iterations of path linked structure, cyclic loop detected. Exiting UpdatePathData method.");
                    }
                    currIndex = curr.next;
                    curr = points[currIndex];
                    Transform currTransform = points[currIndex].transform;
                    if (currTransform != null && currTransform.IsChildOf(p.transform))
                    {
                        found[points[currIndex].transform.GetSiblingIndex()] = true;
                        filled[currIndex-1] = true;
                    }
                    else
                    {
                        modified = true;
                    }
                }

                //handle case where endPoint is deleted
                if (p.transform.Find("EndPoint") == null)
                {
                //Debug.Log("Reassigning EndPoint");
                    modified = true;
                    int index = 0;
                    int lastFilledIndex = 0;
                    int secondLastFilledIndex = 0;
                    while(points[index].next < points.Count && points[index].next > -1)
                    {
                        index = points[index].next;
                        if (filled[index - 1])
                        {
                            secondLastFilledIndex = lastFilledIndex;
                            lastFilledIndex = index;
                        }
                    }
                    if (lastFilledIndex != 0)
                    {
                        points[lastFilledIndex].transform.gameObject.name = "EndPoint";
                        points[lastFilledIndex].next = -1;
                        points[lastFilledIndex].prev = secondLastFilledIndex;
                        points[secondLastFilledIndex].next = lastFilledIndex;
                    }
                    else
                    {
                        GameObject endPoint = new GameObject("EndPoint");
                        Vector3 position = p.transform.position + new Vector3(5.0f, 0.0f, 0.0f);
                        endPoint.transform.position = position;
                        endPoint.transform.parent = p.transform;
                        int addedIndex = addPathPoint(new PointData(endPoint.transform, -1, 0));
                        points[0].next = addedIndex;
                    }
                }

                //bridging of gaps in linked structure in case of item removal
                int bridgeIndex = 0;
                int counter = 0;
                while (bridgeIndex < points.Count && bridgeIndex > -1)
                {
                    if(counter++ > 1000)
                    {
                        throw new Exception("Above 1000 iterations of path linked structure loop, cyclic structure detected. Exiting UpdatePathData method.");
                    }
                    if (bridgeIndex == 0 || filled[bridgeIndex - 1])
                    {
                        currIndex = bridgeIndex; //guaranteed to be the latest valid index by the last iteration
                        int kernel = points[bridgeIndex].next;
                        int kernelCounter = 0;
                        while (kernel < points.Count && kernel > -1)
                        {
                            if(kernelCounter++ > 1000)
                            {
                                throw new Exception("Above 1000 iterations of path linked structure loop, cyclic structure detected. Exiting UpdatePathData method.");
                            }
                            if (filled[kernel - 1])
                            {
                                points[bridgeIndex].next = kernel;
                                points[kernel].prev = bridgeIndex;
                                break;
                            }
                            kernel = points[kernel].next;
                        }

                        bridgeIndex = kernel;   //is set to kernel both so that kernel's next can be repaired on the next iteration and currIndex can be assigned to kernel
                    }
                    else
                    {
                        bridgeIndex = points[bridgeIndex].next;
                    }
                }
               

                //addition of new found points loop
                for (int i = 0; i < found.Length; i++)
                {
                    //if the child wasn't found in the points array, add it to the end, linked to the current last node
                    if (found[i] == false)
                    {
                    //Debug.Log("Adding not found item");
                        modified = true;
                        points[currIndex].transform.gameObject.name = "Point";
                        int foundIndex = addPathPoint(new PointData(p.transform.GetChild(i), -1, currIndex));
                        currIndex = points[currIndex].next = foundIndex;
                    }
                }
                points[currIndex].transform.gameObject.name = "EndPoint";
            }

            //if the editor stored path is modified, then we will recompile the transform array contained in the actual path object
            if (modified)
            {
            compilePath();
            }
        
    }

    private void compilePath()
    {
        Path p = target as Path;
        int numPoints = 1; //assumes always a point for 
        for (int i = 0; i < filled.Count; i++)
        {
            if (filled[i])
                numPoints++;
        }
        p.points = new Transform[numPoints];
        int kernel = 0;
        int index = 0;
        while (index < points.Count && index > -1)
        {
            p.points[kernel++] = points[index].transform;
            index = points[index].next;
        }
    }

    /**
     * Inserts a given path point (with next and previous indexes already assigned) into a free space in the path's points array
     * @return  The index the point was inserted at.
     */
    private int addPathPoint(PointData point)
    {
        for(int i = 1; i < points.Count; i++)
        {
            if (!filled[i - 1])
            {
                points[i] = point;
                filled[i - 1] = true;
                return i;
            }
        }
        points.Add(point);
        filled.Add(true);
        return points.Count - 1;
    }

    private void insertPathPoint(int index)
    {
        
        Path p = target as Path;
        if (index < points.Count && index > 0)
        {
                Vector3 position = (points[index].transform.position + points[points[index].prev].transform.position) / 2;
                GameObject newPoint = new GameObject("Point");
                newPoint.transform.position = position;
                int addedIndex = addPathPoint(new PointData(newPoint.transform, index, points[index].prev));
                points[points[index].prev].next = addedIndex;
                points[index].prev = addedIndex;
                newPoint.transform.SetParent(p.transform);
                newPoint.transform.SetAsLastSibling();
        }
    }

    private void DrawPath()
    {
        Path p = target as Path;
        Handles.color = Color.green;
        if (p.points.Length > 0)
        {
            for (int i = 1; i < p.points.Length; i++)
            {

                EditorGUI.BeginChangeCheck();
                Vector3 pointPosition = Handles.PositionHandle(p.points[i].transform.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p.points[i].transform, "Changed Look Target");
                    p.points[i].transform.position = pointPosition;
                }
            }


            //display insert buttons now
            for (int i = 1; i < points.Count; i++)
            {
                if (filled[i - 1])
                {
                    //draw an insert button to the previous
                    bool pressed = Handles.Button((points[i].transform.position + points[points[i].prev].transform.position) / 2, Quaternion.identity, 1, 1, Handles.CubeCap);
                    if (pressed)
                    {
                        insertPathPoint(i);
                        compilePath();
                    }
                }
            }
        }
    }

}
