using System;
using System.Collections.Generic;
using DemonstrationEngine.Physics;
using Microsoft.Xna.Framework;

namespace DemonstrationEngine
{
    public class SAT_CLass
    {
        public bool Intersect;
        public Vector2 MTV;
        public Vector2 ClosingVelo;
        public Vector2 CNormal;

        /// <summary>
        /// Test for collisions between Convex Polygons
        /// </summary>
        /// <param name="_ent1"></param>
        /// <param name="_ent2"></param>
        /// <param name="velocity"></param>
        public void PolygonVsPolygon(IAsset _ent1, IAsset _ent2)
        {

            //Initialise booleans
            Intersect = true;


            //Iniitialise edges lists
            int ent1Edges = _ent1.Edges().Count;
            int ent2Edges = _ent2.Edges().Count;

            //Variabls for MTV
            float minInterDis = float.PositiveInfinity;
            Vector2 edgeNormal = new Vector2();
            Vector2 edgeNumber;

            //Get the edges we are testing against
            for (int i = 0; i < ent1Edges + ent2Edges; i++)
            {

                //============================================================  Generate Edges ================================================================\\

                if (i < ent1Edges)
                {
                    edgeNumber = _ent1.Edges()[i];
                }
                else
                {
                    edgeNumber = _ent2.Edges()[i - ent1Edges];
                }

                edgeNumber.Normalize();   //Convert Axies to a unit Vector 


                //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\


                float Ent1Min = 0, Ent2Min = 0, Ent1Max = 0, Ent2Max = 0;       //Initialise min/Max variables for each obj

                ProjectPolygon(edgeNumber, _ent1, ref Ent1Min, ref Ent1Max);    //Get the distance of object 1's min and max points on the axies
                ProjectPolygon(edgeNumber, _ent2, ref Ent2Min, ref Ent2Max);    //Get the distance of object 2's min and max points on the axies

                //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\

                //Determine if there is an overlap
                float interdis = PolyIntervalDistance(Ent1Min, Ent1Max, Ent2Min, Ent2Max);
                if (interdis > 0)
                {
                    //if the value is greater than zero, there is no collision
                    Intersect = false;
                    break;
                }
                else if (interdis < 0)
                {
                    Intersect = true;
                }

                interdis = Math.Abs(interdis);
                if (interdis < minInterDis)
                {
                    minInterDis = interdis;
                    edgeNormal = edgeNumber;

                    Vector2 centerDistance = _ent1.Center() - _ent2.Center();
                    if (Vector2.Dot(centerDistance, edgeNormal) < 0)
                    {
                        edgeNormal = -edgeNormal;
                    }
                }
                //Set the MTV variable if collision             

                MTV = edgeNormal * minInterDis;
                //CNormal = edgeNormal;
                // ClosingVelocity(edgeNormal, _ent1.Velocity(), _ent2.Velocity());



            }


        }

        //public void PolygonVsPlane(IPlane plane, IAsset entity)
        //{

        //    //Initialise booleans
        //    Intersect = true;


        //    //Iniitialise edges lists
        //    int entEdges = entity.Edges().Count;

        //    //Variabls for MTV
        //    float minInterDis = float.PositiveInfinity;
        //    Vector2 Normal = plane.Normal;
        //    Vector2 edgeNumber = new Vector2();

        //    //Get the edges we are testing against
        //    for (int i = 0; i < entEdges; i++)
        //    {

        //        //============================================================  Generate Edges of Polygon ================================================================\\

        //        if (i < entEdges)
        //        {
        //            edgeNumber = entity.Edges()[i];
        //        }

        //        //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\


        //        float EntMin = 0, EntMax = 0;       //Initialise min/Max variables for each obj

        //        ProjectPolygon(Normal, entity, ref EntMin, ref EntMax);    //Get the distance of object 2's min and max points on the axies
        //        float planePoint = (Vector2.Dot(plane.Normal, plane.Position))

        //        //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\

        //        //Determine if there is an overlap
        //        float interdis = PolyVsPlaneIntervalDistance(EntMin, EntMax, plane);
        //        if (interdis > 0)
        //        {
        //            //if the value is greater than zero, there is no collision
        //            Intersect = false;
        //            break;
        //        }
        //        else if (interdis < 0)
        //        {
        //            Intersect = true;
        //        }

        //        interdis = Math.Abs(interdis);
        //        if (interdis < minInterDis)
        //        {
        //            minInterDis = interdis;
        //            edgeNormal = edgeNumber;

        //            Vector2 centerDistance = _ent1.Center() - enti.Center();
        //            if (Vector2.Dot(centerDistance, edgeNormal) < 0)
        //            {
        //                edgeNormal = -edgeNormal;
        //            }
        //        }
        //        //Set the MTV variable if collision             

        //        MTV = edgeNormal * minInterDis;

        //    }


        //}

        public float PolyIntervalDistance(float Ent1Min, float Ent1Max, float Ent2Min, float Ent2Max)
        {
            //IF 
            if (Ent1Min < Ent2Min)
            {
                return Ent2Min - Ent1Max;
            }
            else
            {
                return Ent1Min - Ent2Max;
            }
        }

        //public float PolyVsPlaneIntervalDistance(float entMin, float entMax, IPlane plane)
        //{
            
        //}

        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        public void ProjectPolygon(Vector2 axis, IAsset Entity, ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            List<Vector2> points = Entity.Point();


            float projection = Vector2.Dot(axis, points[0]);
            min = projection;
            max = projection;
            for (int i = 0; i < points.Count; i++)
            {
                projection = Vector2.Dot(axis, points[i]);
                if (projection < min)
                {
                    min = projection;
                }
                else if (projection > max)
                {
                    max = projection;
                }
            }
        }

        public Vector2 ClosingVelocity(Vector2 CNormal, Vector2 Velocity1, Vector2 Velocity2)
        {


            float dotProduct = Vector2.Dot(CNormal, (Velocity1 - Velocity2));
            ClosingVelo = dotProduct * CNormal;

            return ClosingVelo;
        }
    }


}

