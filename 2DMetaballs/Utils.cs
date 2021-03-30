using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaballs
{
    public static class Utils
    {
        public static Vector2 Normalized(this Vector2 vector)
        {
            vector.Normalize();
            return vector;
        }

        /// <summary>
        /// Returns a copy of the vector rotated by the specified angle
        /// </summary>
        /// <param name="vector">The vector to rotate</param>
        /// <param name="angle">Angle to rotate the vector by</param>
        /// <returns>Rotated vector copy</returns>
        public static Vector2 RotatedBy(this Vector2 vector, float angle) => Vector2.Transform(vector, Matrix.CreateRotationZ(angle));
    }
}
