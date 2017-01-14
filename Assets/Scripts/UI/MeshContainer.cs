using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * MeshContainer component stores a list of meshes included in this object 
 */
public class MeshContainer : MonoBehaviour {
    public MeshRenderer[] subMeshes;
    public MeshContainer[] subContainers;
    public MeshRenderer[] allMeshes;

    public void Start()
    {
        //inefficient, recursive call for each member of subContainers as well
        updateAllMeshes();
    }

    /**
     * Updates the array of all meshes from the submeshes of this meshcontainer and its subcontainers;
     */
    public MeshRenderer[] updateAllMeshes()
    {
        int meshCount = subMeshes.Length;
        MeshRenderer[][] meshArrays = new MeshRenderer[subContainers.Length + 1][];
        meshArrays[0] = subMeshes;
        for(int i = 1; i <= subContainers.Length; i++)
        {
            meshArrays[i] = subContainers[i].updateAllMeshes();
            meshCount += meshArrays[i].Length;
        }
        MeshRenderer[] totalSubMeshes = new MeshRenderer[meshCount];
        int kernel = 0;
        for(int i=0; i< meshArrays.Length; i++)
        {
            for(int j = 0; j< meshArrays[i].Length; j++)
            {
                totalSubMeshes[kernel] = meshArrays[i][j];
                kernel++;
            }
        }
        return allMeshes = totalSubMeshes;
    }
}
