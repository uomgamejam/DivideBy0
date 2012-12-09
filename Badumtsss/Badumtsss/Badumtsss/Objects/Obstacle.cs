using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Badumtsss.Objects;

namespace Badumtsss
{
    class Obstacle : RotatedRectangle
    {
        private Vector2 a, b, c, d;
        private ObjectType type;
        private Texture2D texture;
        
        public enum ObjectType
        {
            MIRROR,
            WALL
        };

        #region Properties

        public Enum Type
        {
            get { return type; }
            set { type = (ObjectType)value; }
        }

        public Vector2 A
        {
            get { return a; }
            set { a = value; }
        }

        public Vector2 B
        {
            get { return b; }
            set { b = value; }
        }

        public Vector2 C
        {
            get { return c; }
            set { c = value; }
        }

        public Vector2 D
        {
            get { return d; }
            set { d = value; }
        }
        #endregion

        /* Old constructor
        /// <summary>
        /// Creates a new obstacle, as a rectangle within the given points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Obstacle(ObjectType type, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
            : base(new Rectangle(a.X, a.Y, Vector2.Distance(b, a), Vector2.Distance(c, b)), Vector2.Distance(
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.type = type;
            setTexture(type);
        }*/

        /// <summary>
        /// Creates a new Obstacle as a rectangle
        /// </summary>
        /// <param name="location">Location of the upper left corner</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">length of the rectangle</param>
        /// <param name="initialRotation">The initial rotation of the object</param>
        public Obstacle(ObjectType type, Vector2 location, float width, float height, float initialRotation)
            : base(new Rectangle((int)location.X, (int)location.Y, (int)width, (int)height), initialRotation)
        {
            a = UpperLeftCorner();
            b = UpperRightCorner();
            c = LowerRightCorner();
            d = LowerLeftCorner();

            //setTexture(type);
        }

        #region Methods

        private void setTexture(ObjectType type)
        {
            Game1 game = new Game1();
            if (type == ObjectType.MIRROR)
                texture = game.Content.Load<Texture2D>("mirror_tex");
        }

        private void Rotate(float rotation)
        {
            this.rotation += rotation;
            a = UpperLeftCorner();
            b = UpperRightCorner();
            c = LowerRightCorner();
            d = LowerLeftCorner();
        }

        #endregion

    }
}
