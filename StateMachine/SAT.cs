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
        public bool Intersect;
        public Vector2 MTV;


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
            Vector2 transAxis = new Vector2();
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


                //attach the axis into a new vector2
                Vector2 axis = new Vector2(-edgeNumber.Y, edgeNumber.X); //Rotate the axies edge by 90 degrees
                //Normalize the axis
                axis.Normalize();   //Convert Axies to a unit Vector 


                //=============================================== PROJECT EVERY POINT ON EVERY AXIES FOR BOTH OBJECTS ===========================================\\


                float Ent1Min = 0, Ent2Min = 0, Ent1Max = 0, Ent2Max = 0;       //Initialise min/Max variables for each obj

                ProjectPolygon(axis, _ent1, ref Ent1Min, ref Ent1Max);    //Get the distance of object 1's min and max points on the axies
                ProjectPolygon(axis, _ent2, ref Ent2Min, ref Ent2Max);    //Get the distance of object 2's min and max points on the axies

                //====================================================== DETERMINE IF COLLISIONS ARE OCCURING ====================================================\\

                //Determine if there is an overlap
                float interdis = IntervalDistance(Ent1Min, Ent1Max, Ent2Min, Ent2Max);
                if (interdis > 0)
                {
                    //if the value is greater than zero, there is no collision
                    Intersect = false;
                    break;
                }

                interdis = Math.Abs(interdis);
                if (interdis < minInterDis)
                {
                    minInterDis = interdis;
                    transAxis = axis;

                    Vector2 CN = _ent1.Center() - _ent2.Center();
                    if (Vector2.Dot(CN, transAxis) < 0)
                    {
                        transAxis = -transAxis;
                    }
                }
                //Set the MTV variable if collision             

                MTV = 0.5*  transAxis * minInterDis;
                


            }

        }

        public float IntervalDistance(float Ent1Min, float Ent1Max, float Ent2Min, float Ent2Max)
        {
            if (Ent1Min < Ent2Min)
            {
                return Ent2Min - Ent1Max;
            }
            else
            {
                return Ent1Min - Ent2Max;
            }
        }

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
    }


}

