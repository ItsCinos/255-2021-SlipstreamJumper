using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins
{
    public class AABB : MonoBehaviour
    {
        public Vector3 boxSize;
        public Vector3 min;
        public Vector3 max;

        void Start()
        {
            RecalcAABB();
        }

        /// <summary>
        /// This function should be called whenever the position 
        /// or size of the collider changes
        /// </summary>
        public void RecalcAABB()
        {            
            min = transform.position - boxSize / 2;
            max = transform.position + boxSize / 2;
        }

        /// <summary>
        /// What happens when there is collision
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool OverlapCheck(AABB other)
        {
            if (other.min.x > this.max.x) return false; //gap to right == no collision
            if (other.max.x < this.min.x) return false; //gap to left == no collision

            if (other.min.y > this.max.y) return false; //gap above == no collision
            if (other.max.y < this.min.y) return false; //gap below == no collision

            if (other.min.z > this.max.z) return false; //gap 'forward' == no collision
            if (other.max.z < this.min.z) return false; //gap 'behind' == no collision

            return true;
        }

        public Vector3 FindFix(AABB other)
        {
            float moveRight = other.max.x - this.min.x; //positive number
            float moveLeft  = other.min.x - this.max.x; //negative number
            float moveUp    = other.max.y - this.min.y; //positive number
            float moveDown  = other.min.y - this.max.y; //negative number

            Vector3 fix = Vector3.zero;

            if (Mathf.Abs(moveLeft) < Mathf.Abs(moveRight))
            {
                fix.x = moveLeft;
            } else {
                fix.x = moveRight;
            }

            if(Mathf.Abs(moveUp) < Mathf.Abs(moveDown))
            {
                fix.y = moveUp;
            } else {
                fix.y = moveDown;
            }

            if(Mathf.Abs(fix.x) < Mathf.Abs(fix.y))
            {
                fix.y = 0; //going with horizontal solution; clearing vertical...
            } else {
                fix.x = 0; //going with vertical solution; clearing horizontal...
            }

            return fix;
        }

        private void OnDrawGizmos()
        {
            // draws AABB cube in scene view.
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}