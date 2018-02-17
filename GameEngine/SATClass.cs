using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

/// <summary>
/// Finds out when two passed objs are colliding through the use of SAT theorem
/// Author: Natha Robertson
/// Version: 0.1
/// </summary>
namespace GameEngine
{
    class SATClass
    {
        #region Variables

        //Lists for the Object points to be obtained from
        public List<Vector2> PointList1;
        public List<Vector2> PointList2;
        //Bool to alert to collision
        public static bool _ColBool = false;
        #endregion

        /// <summary>
        /// Structure that contains the results of the collision function
        /// </summary>
        public struct CollisionResult
        {
            //Will objects collide
            public bool WillIntersect;

            // Are objects colliding
            public bool Intersect;

            //The range of intersection, used to seperate the two objects
            public Vector2 TransVector;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SATClass()
        {

        }

        /// <summary>
        /// Detects collisions between object with 4 points
        /// 
        /// Project the object points onto the Axis
        /// Rotate Axis and repeat projection
        /// Return True if there is an interesection
        /// 
        /// </summary>
        public void SquareVsSquare(IAsset _ent1, IAsset _ent2)
        {
            //Get Points for each list
            PointList1 = _ent1.getPoints(); // sq
            PointList2 = _ent2.getPoints(); //r playe

            //Get the axies to be tested on
            List<Vector2> Axies1 = _ent1.getAxies(); //sq
            List<Vector2> Axies2 = _ent2.getAxies(); //player


            ////Run a loop going through each of the axies of the square
            for (int i = 0; i < Axies1.Count; i++)
            {

                //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\
                //Call Project Method
                Tuple<float, float> Ent1 = projectMethod(Axies1[i], PointList1); // axies of square, point list of square
                Tuple<float, float> Ent2 = projectMethod(Axies1[i], PointList2); // axies of square, point list of player

                //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\
                //ITEM 1 = MIN --------- ITEM2 = MAX
                float _col1 = Ent2.Item1 - Ent1.Item2;
                float _col2 = Ent1.Item1 - Ent2.Item2;

                // Console.WriteLine("col1 = " + _col1);
                // Console.WriteLine("Col2 = " + _col2);
                if (_col1 < 0)
                {
                    _ColBool = true;
                    IntersectDistance(Ent1, Ent2);
                }
                else
                {
                    _ColBool = false;
                    break;
                }
            }

            //////Run a loop going through each of the axies
            for (int i = 0; i < Axies2.Count; i++)
            {

                //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\
                //Call Project Method
                Tuple<float, float> Ent1 = projectMethod(Axies2[i], PointList1);
                Tuple<float, float> Ent2 = projectMethod(Axies2[i], PointList2);

                //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\
                //ITEM 1 = MIN --------- ITEM2 = MAX
                float _col1 = Ent2.Item1 - Ent1.Item2;
                float _col2 = Ent1.Item1 - Ent2.Item2;

                if (_col1 < 0)
                {
                    _ColBool = true;
                    IntersectDistance(Ent1, Ent2);
                }
                else
                {
                    _ColBool = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Utilises Dot Product function to determine the max and the minimum points on an object
        /// </summary>
        /// <param name="testAxies"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public Tuple<float, float> projectMethod(Vector2 testAxies, List<Vector2> points)
        {

            float _min = Vector2.Dot(testAxies, points[0]);
            float _max = _min;

            for (int i = 0; i < points.Count; i++)
            {
                float _dotPoint = Vector2.Dot(testAxies, points[i]);

                if (_dotPoint >= _max)
                    _max = _dotPoint;
                if (_dotPoint <= _min)
                    _min = _dotPoint;
            }

            return Tuple.Create(_min, _max);
        }

        /// <summary>
        /// Calculate the overlap between the two objects
        /// </summary>
        /// <param name="min1"></param>
        /// <param name="min2"></param>
        /// <param name="max1"></param>
        /// <param name="max2"></param>
        /// <returns></returns>
        public float IntersectDistance(Tuple<float, float> ent1, Tuple<float, float> ent2)
        {

            //ITEM 1 = MIN  ITEM 2 = MAX 
            if (ent1.Item1 < ent2.Item2)
            {
                return ent2.Item1 - ent1.Item2;
            }
            else if (ent1.Item1 > ent2.Item2)
            {
                return ent1.Item1 - ent2.Item2;
            }

            else
                return 0f;
        }

        /// <summary>
        /// Used to determine the range of collision between two objects
        /// </summary>
        /// <param name="CollisionRange"></param>
        /// <returns></returns>
        //public Tuple<float, float> MinimumTranslationVector(Vector2 CollisionRange)
        //{

        //    float _mtvX;
        //    float _mtvY;

        //    return Tuple.Create(_mtvX, _mtvY);

        //}

        /// <summary>
        /// Look to see whether a collision is possible
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="play"></param>
        /// <param name="Speed"></param>4
        /// <returns></returns>
        public CollisionResult CollisionResultTest(Square sq, TestPlayer play, Vector2 velocity)
        {
            CollisionResult result = new CollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;



            //FIND IF OBJECTS WILL INTERSECT
            //Project Velocity onto the current axis
            //object pos + velocity
            //return will collide

            //Push out objects which collide
            //use Belocity 

            return result;

        }

        //IF COLLIDE PUSH OBECTS BY INTERSECT
    }
}
