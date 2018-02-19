using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class NewSAT
    {
        public bool WillIntersect;
        public bool Intersect;
        public Vector2 Ent1Point;
        public Vector2 Ent2Point;
        public Vector2 MTV;


        /// <summary>
        /// Test for collisions between Convex Polygons
        /// </summary>
        /// <param name="_ent1"></param>
        /// <param name="_ent2"></param>
        /// <param name="velocity"></param>
        public void PolygonVsPolygon(IAsset _ent1, IAsset _ent2, Vector2 velocity)
        {

            //Initialise booleans
            Intersect = true;
            WillIntersect = true;

            //Iniitialise edges lists
            int ent1Edges = _ent1.Edges().Count;
            int ent2Edges = _ent2.Edges().Count;

            //Variabls for MTV
            float minInterDis = float.PositiveInfinity;
            Vector2 transAxis = new Vector2();
            Vector2 edge;

            //Get the edges we are testing against
            for (int i = 0; i < ent1Edges + ent2Edges; i++)
            {

                //============================================================  Generate Edges ================================================================\\

                if (i < ent1Edges)
                {
                    edge = _ent1.Edges()[i];
                }
                else
                {
                    edge = _ent2.Edges()[i - ent1Edges];
                }


                //attach the axis into a new vector2
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                //Normalize the axis
                axis.Normalize();


                //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\

                //Initialise min/Max variables for each obj
                float minA = 0, minB = 0, maxA = 0, maxB = 0;

                ProjectPoly(axis, _ent1, ref minA, ref maxA);
                ProjectPoly(axis, _ent2, ref minB, ref maxB);

                //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\

                //Determine if there is an overlap
                if (IntervalDistance(minA, maxA, minB, maxB) > 0)
                {
                    //if the value is greater than zero, there is no collision
                    Intersect = false;

                }
                //================================================= Collision Prediction ================================================\\

                float velocityProj = Vector2.Dot(axis, velocity);
                if (velocityProj < 0)
                {
                    minA += velocityProj;
                }
                else
                {
                    maxA += velocityProj;
                }

                //Determine intersect distance
                float interdis = IntervalDistance(minA, maxA, minB, maxB);
                if (interdis > 0)
                {
                    //if its greater than 0, there is no possible collision
                    WillIntersect = false;
                }
                if (!Intersect && !WillIntersect)
                {
                    break;
                }

                interdis = Math.Abs(interdis);
                if (interdis < minInterDis)
                {
                    minInterDis = interdis;
                    transAxis = axis;

                    //========================================= Set Collision Side ==================================================== \\

                    //sET THE CENTER POINTS 
                    Vector2 d = _ent1.Center() - _ent2.Center();

                    //IF the dot product of the centerpoint is less thank 0, invert the trans axies
                    if (Vector2.Dot(d, transAxis) < 0)
                    {
                        transAxis = -transAxis;
                    }
                }

                //Set the MTV variable if entities have a chance of colliding
                if (WillIntersect)
                {

                    MTV = transAxis * minInterDis;
                }


            }

        }

        /// <summary>
        /// Determines the interesct range between two entities - Requires min & Max points 
        /// </summary>
        /// <param name="minA"></param>
        /// <param name="maxA"></param>
        /// <param name="minB"></param>
        /// <param name="maxB"></param>
        /// <returns></returns>
        public float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        /// <summary>
        /// Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="Entity"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void ProjectPoly(Vector2 axis, IAsset Entity, ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            List<Vector2> points = Entity.Point();


            float d = Vector2.Dot(axis, points[0]);
            min = d;
            max = d;
            for (int i = 0; i < points.Count; i++)
            {
                d = Vector2.Dot(axis, points[i]);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }


    }
}
