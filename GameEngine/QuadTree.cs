using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class to manage QuadTrees, objects held, number of levels and the type of collision objs it can store
/// </summary>
namespace GameEngine
{
    class QuadTree
    {
        //Number of objects to be held in a branch before a new node will be created
        private int _MaxObjs = 10;
        private int _MaxLevels;

        //level of the node 
        private int _Level;

        private List<IAsset> _Objects;
        private Rectangle _Bounds;
        private List<QuadTree> _Nodes;


        public QuadTree(int level, Rectangle area)
        {
            _Level = level;
            _Objects = new List<IAsset>();
            _Bounds = area;
            _Nodes = new List<QuadTree>();

        }


        /// <summary>
        /// Clears the QuadTree - Clearing removing each node of their collidable entities
        /// </summary>
        public void Clear()
        {
            //clear the _objects list
            _Objects.Clear();

            //clear each of the nodes lists
            for (int i = 0; i < _Nodes.Count; i++)
            {
                if (_Nodes[i] != null)
                {
                    _Nodes[i].Clear();
                    _Nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Used to devide the note into 4 subnodes
        /// </summary>
        public void Split()
        {
            int _SubWidth = (_Bounds.Width / 2);
            int _SubHeight = (_Bounds.Height / 2);
            int x = _Bounds.X;
            int y = _Bounds.Y;

            _Nodes[0] = new QuadTree(_Level + 1, new Rectangle(x + _SubWidth, y, _SubWidth, _SubHeight));
            _Nodes[1] = new QuadTree(_Level + 1, new Rectangle(x, y, _SubWidth, _SubHeight));
            _Nodes[2] = new QuadTree(_Level + 1, new Rectangle(x, y + _SubHeight, _SubWidth, _SubHeight));
            _Nodes[3] = new QuadTree(_Level + 1, new Rectangle(x + _SubWidth, y + _SubHeight, _SubWidth, _SubHeight));
        }

        /// <summary>
        /// Determines Which node an object belongs to.
        /// -1 means an object doesnt fit and is in the parent node.
        /// </summary>
        /// <param name="_Rect"></param>
        /// <returns></returns>
        private int GetIndex(IAsset _Rect)
        {
            int _Index = 1;
            //the central points of the bounding area
            double _VerticalMidPoint = _Bounds.X + (_Bounds.Width / 2);
            double _HorizontalMidPoint = _Bounds.Y + (_Bounds.Height / 2);

            //Object fits in top quads
            bool _TopQuad = (_Rect.getPos.Y < _HorizontalMidPoint && _Rect.getPos.X + _Rect.getTex.Width < _VerticalMidPoint);
            //object fits into LowerQuads
            bool _LowQuad = (_Rect.getPos.Y > _HorizontalMidPoint);

            //Tests if object fits in left Quads
            if (_Rect.getPos.X < _VerticalMidPoint && _Rect.getPos.X + _Rect.getTex.Width < _VerticalMidPoint)
            {
                if (_TopQuad)
                    _Index = 1;
                else
                    _Index = 2;
            }
            //Tests if Object fits in the right Quads
            else if (_LowQuad)
            {
                if (_TopQuad)
                    _Index = 0;
                else if (_LowQuad)
                    _Index = 3;
            }

            return _Index;
        }

        /// <summary>
        /// Place objects into the quad tree.
        /// If the node exceeds _maxObjs, split node and place objects into correct nodes
        /// </summary>
        /// <param name="_Rect"></param>
        public void InsertObj(IAsset _Rect)
        {
            //If there is a node
            if (_Nodes[0] != null)
            {
                //Get the Asset
                int _Index = GetIndex(_Rect);

                // If the Asset isn't -1 
                if (_Index != -1)
                {
                    _Nodes[_Index].InsertObj(_Rect);
                }
                return;
            }

            //add the assett to the _objects list
            _Objects.Add(_Rect);

            //if the list of Assets is larger than the 
            if (_Objects.Count() > _MaxObjs && _Level < _MaxLevels)
            {
                if (_Nodes[0] == null)
                { Split(); }

                int i = 0;
                //while the count is greater that i, place the objects into the node list
                while (i < _Objects.Count())
                {
                    int _Index = GetIndex(_Objects[i]);

                    if (_Index != -1)
                    {
                        _Nodes[_Index].InsertObj(_Objects[i]); 
                    }
                    else i++; 
                }
            }
        }

        /// <summary>
        /// Return the objects that could collide with each other in a node
        /// </summary>
        /// <param name="returnObjs"></param>
        /// <param name="_rect"></param>
        /// <returns></returns>
        public List<IAsset> getList(List<IAsset> returnObjs, IAsset _Rect)
        {
            int _Index = GetIndex(_Rect);

            //if the index is -1 and there is nodes, call the getList method 
            if (_Index != -1 && _Nodes[0] != null)
            { _Nodes[_Index].getList(returnObjs, _Rect); }

            //else, return the _Objects list
            returnObjs = _Objects;

            return returnObjs;
        }

        public void Draw(SpriteBatch sprite)
        {

        }
    }
}


