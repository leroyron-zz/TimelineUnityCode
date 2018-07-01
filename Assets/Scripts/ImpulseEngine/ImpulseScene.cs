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
using System.Collections.Generic;
using UnityEngine;

public partial class ImpulseEngine
{
    public class ImpulseScene
    {
        public float dt;
        public int iterations;
        public List<Body> bodies = new List<Body>();
        public List<Manifold> contacts = new List<Manifold>();

        public ImpulseScene(float dt, int iterations)
        {
            this.dt = ImpulseMath.DT = dt;
            ImpulseMath.RESTING = ImpulseMath.GRAVITY.Mul(ImpulseMath.DT).LengthSq() + ImpulseMath.EPSILON;
            this.iterations = iterations;
        }

        public void Step()
        {
            // Generate new collision info
            contacts.Clear();
            for (int i = 0; i < bodies.Count; ++i)
            {
                Body A = bodies[i];

                for (int j = i + 1; j < bodies.Count; ++j)
                {
                    Body B = bodies[j];

                    if (A.invMass == 0 && B.invMass == 0)
                    {
                        continue;
                    }

                    Manifold m = new Manifold(A, B);
                    m.Solve();

                    if (m.contactCount > 0)
                    {
                        contacts.Add(m);
                    }
                }
            }

            // Integrate forces
            for (int i = 0; i < bodies.Count; ++i)
            {
                IntegrateForces(bodies[i], dt);
            }

            // Initialize collision
            for (int i = 0; i < contacts.Count; ++i)
            {
                contacts[i].Initialize();
            }

            // Solve collisions
            for (int j = 0; j < iterations; ++j)
            {
                for (int i = 0; i < contacts.Count; ++i)
                {
                    contacts[i].ApplyImpulse();
                }
            }

            // Integrate velocities
            for (int i = 0; i < bodies.Count; ++i)
            {
                IntegrateVelocity(bodies[i], dt);
            }

            // Correct positions
            for (int i = 0; i < contacts.Count; ++i)
            {
                contacts[i].PositionalCorrection();
            }

            // Clear all forces
            for (int i = 0; i < bodies.Count; ++i)
            {
                Body b = bodies[i];
                b.force.Set(0, 0);
                b.torque = 0;
            }
        }

        public Body Add(Shape shape, float x, float y)
        {
            Body b = new Body(shape, x, y);
            bodies.Add(b);
            return b;
        }

        public void Clear()
        {
            contacts.Clear();
            bodies.Clear();
        }

        // Acceleration
        // F = mA
        // => A = F * 1/m

        // Explicit Euler
        // x += v * dt
        // v += (1/m * F) * dt

        // Semi-Implicit (Symplectic) Euler
        // v += (1/m * F) * dt
        // x += v * dt

        // see http://www.niksula.hut.fi/~hkankaan/Homepages/gravity.html
        public void IntegrateForces(Body b, float dt)
        {
            //		if(b->im == 0.0f)
            //			return;
            //		b->velocity += (b->force * b->im + gravity) * (dt / 2.0f);
            //		b->angularVelocity += b->torque * b->iI * (dt / 2.0f);

            if (b.invMass == 0.0f)
            {
                return;
            }

            float dts = dt * 0.5f;

            b.velocity.Addsi(b.force, b.invMass * dts);
            b.velocity.Addsi(ImpulseMath.GRAVITY, dts);
            b.angularVelocity += b.torque * b.invInertia * dts;
        }

        public void IntegrateVelocity(Body b, float dt)
        {
            //		if(b->im == 0.0f)
            //			return;
            //		b->position += b->velocity * dt;
            //		b->orient += b->angularVelocity * dt;
            //		b->SetOrient( b->orient );
            //		IntegrateForces( b, dt );

            if (b.invMass == 0.0f)
            {
                return;
            }

            b.position.Addsi(b.velocity, dt);
            b.orient += b.angularVelocity * dt;
            b.SetOrient(b.orient);

            IntegrateForces(b, dt);
        }
    }
}