using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickClassroom
{
    internal class Teacher
    {
        private const int teacherSpeed = 1; 
        public Point Position { get; set; }

        public Rectangle teacherRect
        {
            get => new(Position, new Point(Globals.StudentSize + 5, Globals.StudentSize + 5));
            set
            {
                Position = new Point(value.X, value.Y);
            }
        }

        private Texture2D texture;
        private float rotationAngle; // Angle the teacher is facing in radians

        // Vision cone properties
        public float VisionConeAngle { get; set; } = MathHelper.ToRadians(45); // Half-angle of the vision cone in radians
        public float VisionConeRadius { get; set; } = 200f; // Radius of the vision cone (distance the teacher can "see")

        public int nextDX;
        public int nextDY;

        public Teacher(int x, int y, Texture2D texture)
        {
            this.texture = texture;
            this.Position = new Point(x, y);
            this.rotationAngle = 0f; // Default to facing right (0 radians)
        }

        public void Update()
        {
            if (Position.X < 500)
            {
                Position = new Point(Position.X + teacherSpeed, Position.Y);
            }


            rotationAngle += 0.01f; // Rotate slowly (for testing, adjust as needed)
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, teacherRect, Color.Green);

            // Draw vision cone for visual debugging 
            DrawVisionCone(spriteBatch);
        }

        // Draw the vision cone using lines (for debugging)
        private void DrawVisionCone(SpriteBatch spriteBatch)
        {
            Texture2D lineTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            lineTexture.SetData(new[] { Color.Red });

            // Calculate the center of the teacher rectangle
            Vector2 teacherCenter = new Vector2(Position.X, Position.Y);

            // Calculate the origin point of the FOV at the edge of the teacher rectangle based on rotation angle
            Vector2 fovOrigin = CalculateFOVOrigin();

            // Calculate the direction vectors for the left and right edges of the vision cone
            Vector2 leftEdge = new Vector2((float)Math.Cos(rotationAngle - VisionConeAngle), (float)Math.Sin(rotationAngle - VisionConeAngle));
            Vector2 rightEdge = new Vector2((float)Math.Cos(rotationAngle + VisionConeAngle), (float)Math.Sin(rotationAngle + VisionConeAngle));

            // Scale the direction vectors by the cone radius to get the endpoints
            Vector2 leftEndpoint = fovOrigin + leftEdge * VisionConeRadius;
            Vector2 rightEndpoint = fovOrigin + rightEdge * VisionConeRadius;

            // Draw the lines representing the vision cone
            DrawLine(spriteBatch, fovOrigin, leftEndpoint, Color.Red, lineTexture);
            DrawLine(spriteBatch, fovOrigin, rightEndpoint, Color.Red, lineTexture);
        }

        // Helper method to draw a line between two points using a 1x1 texture
        private void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, Texture2D texture)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            spriteBatch.Draw(texture, point1, null, color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 0);
        }

        // Calculate the origin point of the FOV based on the rotation and teacherRect's edge
        private Vector2 CalculateFOVOrigin()
        {
            // Center of the teacher rectangle
            Vector2 teacherCenter = new Vector2(Position.X + teacherRect.Width / 2, Position.Y + teacherRect.Height / 2);

            // Calculate the direction vector based on the rotation angle
            Vector2 directionVector = new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle));

            // Calculate the origin point at the edge of the rectangle in the direction of the rotation angle
            float halfWidth = teacherRect.Width / 2;
            float halfHeight = teacherRect.Height / 2;

            Vector2 fovOrigin = teacherCenter + directionVector * halfWidth; // Use halfWidth to move to the edge of the teacherRect

            return fovOrigin;
        }

        // Check if a given point (e.g., player's position) is within the teacher's vision cone
        public bool IsPointInVisionCone(Point point)
        {
            Vector2 teacherPosition = new Vector2(Position.X, Position.Y);
            Vector2 pointPosition = new Vector2(point.X, point.Y);

            // Calculate the vector from the teacher to the point
            Vector2 directionToPoint = pointPosition - teacherPosition;
            float distanceToPoint = directionToPoint.Length();

            // Normalize the direction vector
            directionToPoint.Normalize();

            // Calculate the angle between the teacher's facing direction and the direction to the point
            Vector2 facingDirection = new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle));
            float dotProduct = Vector2.Dot(facingDirection, directionToPoint);
            float angleToPoint = (float)Math.Acos(dotProduct);

            // Check if the point is within the vision cone angle and radius
            return angleToPoint <= VisionConeAngle && distanceToPoint <= VisionConeRadius;
        }
    }
}
