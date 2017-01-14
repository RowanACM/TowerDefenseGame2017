using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClosestBugPriority : PriorityMode {

    /**
     * Get the closest Bug from the given options!
     */
    public override Transform GetBest(Transform[] options)
    {
        Transform best = null;
        float shortestDistance = 0;
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetComponent<Bug>())
            {
                float distance = (options[i].position - transform.position).sqrMagnitude;
                if (best)
                {
                    if (distance < shortestDistance)
                    {
                        best = options[i];
                        shortestDistance = distance;
                    }
                }
                else
                {
                    best = options[i];
                    shortestDistance = distance;
                }
            }
        }
        return best;
    }

    /**
     * A costly operation for large quantity values, the worst case is O(nq) time, where n is the size of options and q is quantity. A better algorithm than repeated min/max evaluation, as complexity is always tightly bound by O(nq)
     * With low values of q, efficiency approaches O(n) time. I'm also using this algorithm because it only uses 3 arrays and no other function calls!
     */
    public override Transform[] GetBest(Transform[] options, int quantity)
    {
        if (quantity == 0)
        {
            return new Transform[0];
        }
        else if (quantity == 1)
        {
            return new Transform[] { GetBest(options) };
        }
        else
        {
            //selected will be the array we write transforms to
            Transform[] selected = new Transform[quantity];
            //keep track of the distances for each index in selected, we don't want to have to 
            float[] distances = new float[quantity];
            //establish a linked structure. We know upfront the max (and most likely) size of the linked structure when we build it sop we establish links in an array
            int[] lesserIndexes = new int[quantity];
            //initialize lesser indexes to -1
            for (int i = 0; i < lesserIndexes.Length; i++)
            {
                lesserIndexes[i] = -1;
            }

            //keep track of the current greatest distance and index
            int greatestIndex = 0;

            //the count of how many valid entries are actually in the selected array currently
            int count = 0;

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].transform.GetComponent<Bug>())
                {
                    //calculate square distance from this object to the bug
                    float distance = (options[i].position - transform.position).sqrMagnitude;
                    //if we haven't filled the slected array yet
                    if (count < selected.Length)
                    {
                        selected[count] = options[i];
                        if (distance >= distances[greatestIndex])
                        {
                            lesserIndexes[count] = greatestIndex;
                            greatestIndex = count;
                        }
                        else
                        {
                            //case where it is less than max (delve into the array!)
                            int lesserIndex = lesserIndexes[greatestIndex];
                            int greaterIndex = greatestIndex;
                            while (lesserIndex > -1 && distance < distances[lesserIndex])
                            {
                                greaterIndex = lesserIndex;
                                lesserIndex = lesserIndexes[lesserIndex];
                            }
                            lesserIndexes[count] = lesserIndex;
                            lesserIndexes[greaterIndex] = count;
                        }
                        distances[count] = distance;
                        count++;
                    }
                    else
                    {
                        //if the array is filled, we'll use the built data structures to find the appropriate slot to replace and replace it.
                        //only worry about adding items that have a distance less than the set!
                        if (distance < distances[greatestIndex])
                        {
                            //insert this into the linked structure, then update greatest index to reflect the greatest index
                            int nextGreatestIndex;
                            int lesserIndex = nextGreatestIndex = lesserIndexes[greatestIndex];
                            int greaterIndex = greatestIndex;
                            while (lesserIndex > -1 && distance < distances[lesserIndex])
                            {
                                greaterIndex = lesserIndex;
                                lesserIndex = lesserIndexes[lesserIndex];
                            }
                            //now we found the appropriate lesser and greater indexes in the link structure, we'll reassign links corresponding to the former greatest index (where we are replacing)
                            lesserIndexes[greatestIndex] = lesserIndex;
                            lesserIndexes[greaterIndex] = greatestIndex;

                            //perform the replacement
                            distances[greatestIndex] = distance;
                            selected[greatestIndex] = options[i];

                            //set greatest
                            greatestIndex = nextGreatestIndex;
                        }
                    }
                }
            }
            return selected;
        }
    }
}
