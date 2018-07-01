/*
    Copyright (c) 2013 Randy Gaul http://RandyGaul.net

    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.

    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
      
    Port to Java by Philip Diffenderfer http://magnos.org - Port to C# Unity by Leroy Thompson http://leroy.ron@gmail.com
*/
public partial class ImpulseEngine
{
    public class Circle : Shape
    {
        public Circle(float r)
        {
            radius = r;
        }

        public override Shape Clone()
        {
            return new Circle(radius);
        }

        public override void Initialize()
        {
            ComputeMass(1.0f);
        }

        public override void ComputeMass(float density)
        {
            body.mass = ImpulseMath.PI * radius * radius * density;
            body.invMass = (body.mass != 0.0f) ? 1.0f / body.mass : 0.0f;
            body.inertia = body.mass * radius * radius;
            body.invInertia = (body.inertia != 0.0f) ? 1.0f / body.inertia : 0.0f;
        }

        public override void SetOrient(float radians)
        {
        }

        public override Type getType()
        {
            return Type.Circle;
        }
    }
}