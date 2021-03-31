using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The super class that checks for overlapping aabb objects.
/// </summary>
namespace Kortge
{
    public class OverlapObject : MonoBehaviour
    {
        /// <summary>
        /// The aabb collider attached to this object.
        /// </summary>
        AABB aabb;

        void Start() // Get aabb.
        {
            aabb = GetComponent<AABB>();
            Zone.main.powerups.Add(aabb);
        }

        private void OnDestroy()
        {
            if (Zone.main == null) return; // do nothing
            Zone.main.powerups.Remove(aabb);
        }

        /// <summary>
        /// This function should be overridden by child classes (hence 'virtual')
        /// </summary>
        /// <param name="pm"></param>
        virtual public void OnOverlap(PlayerMovement pm)
        {
            print("Overlap");
        }
    }
}