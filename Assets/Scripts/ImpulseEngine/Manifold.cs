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
using UnityEngine;

public partial class ImpulseEngine
{
    public class Manifold
    {
        public Body A;
        public Body B;
        public float penetration;
        public readonly Vec2 normal = new Vec2();
        public readonly Vec2[] contacts = { new Vec2(), new Vec2() };
        public int contactCount;
        public float e;
        public float df;
        public float sf;

        public Manifold(Body a, Body b)
        {
            A = a;
            B = b;
        }

        public void Solve()
        {

            int ia = A.shape is Circle ? 0 : 1;//.ordinal;//System.Array.IndexOf(Shape.Type.GetValues( A.shape.GetType()),  A.shape);
            int ib = B.shape is Circle ? 0 : 1;//.ordinal;//System.Array.IndexOf(Shape.Type.GetValues( B.shape.GetType()),  B.shape);

            Collisions.dispatch[ia][ib].HandleCollision(this, A, B);
        }

        public void Initialize()
        {
            // Calculate average restitution
            // e = std::Min( A->restitution, B->restitution );
            e = Mathf.Min(A.restitution, B.restitution);

            // Calculate static and dynamic friction
            // sf = std::sqrt( A->staticFriction * A->staticFriction );
            // df = std::sqrt( A->dynamicFriction * A->dynamicFriction );
            sf = (float)Mathf.Sqrt(A.staticFriction * A.staticFriction + B.staticFriction * B.staticFriction);
            df = (float)Mathf.Sqrt(A.dynamicFriction * A.dynamicFriction + B.dynamicFriction * B.dynamicFriction);

            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                // Vec2 ra = contacts[i] - A->position;
                // Vec2 rb = contacts[i] - B->position;
                Vec2 ra = contacts[i].Sub(A.position);
                Vec2 rb = contacts[i].Sub(B.position);

                // Vec2 rv = B->velocity + Cross( B->angularVelocity, rb ) -
                // A->velocity - Cross( A->angularVelocity, ra );
                Vec2 rv = B.velocity.Add(Vec2.Cross(B.angularVelocity, rb, new Vec2())).Subi(A.velocity).Subi(Vec2.Cross(A.angularVelocity, ra, new Vec2()));

                // Determine if we should perform a resting collision or not
                // The idea is if the only thing moving this object is gravity,
                // then the collision should be performed without any restitution
                // if(rv.LenSqr( ) < (dt * gravity).LenSqr( ) + EPSILON)
                if (rv.LengthSq() < ImpulseMath.RESTING)
                {
                    e = 0.0f;
                }
            }
        }

        public void ApplyImpulse()
        {
            // Early out and positional correct if both objects have infinite mass
            // if(Equal( A->im + B->im, 0 ))
            if (ImpulseMath.Equal(A.invMass + B.invMass, 0))
            {
                InfiniteMassCorrection();
                return;
            }

            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                // Vec2 ra = contacts[i] - A->position;
                // Vec2 rb = contacts[i] - B->position;
                Vec2 ra = contacts[i].Sub(A.position);
                Vec2 rb = contacts[i].Sub(B.position);

                // Relative velocity
                // Vec2 rv = B->velocity + Cross( B->angularVelocity, rb ) -
                // A->velocity - Cross( A->angularVelocity, ra );
                Vec2 rv = B.velocity.Add(Vec2.Cross(B.angularVelocity, rb, new Vec2())).Subi(A.velocity).Subi(Vec2.Cross(A.angularVelocity, ra, new Vec2()));

                // Relative velocity along the normal
                // real contactVel = Dot( rv, normal );
                float contactVel = Vec2.Dot(rv, normal);

                // Do not resolve if velocities are separating
                if (contactVel > 0)
                {
                    return;
                }

                // real raCrossN = Cross( ra, normal );
                // real rbCrossN = Cross( rb, normal );
                // real invMassSum = A->im + B->im + Sqr( raCrossN ) * A->iI + Sqr(
                // rbCrossN ) * B->iI;
                float raCrossN = Vec2.Cross(ra, normal);
                float rbCrossN = Vec2.Cross(rb, normal);
                float invMassSum = A.invMass + B.invMass + (raCrossN * raCrossN) * A.invInertia + (rbCrossN * rbCrossN) * B.invInertia;

                // Calculate impulse scalar
                float j = -(1.0f + e) * contactVel;
                j /= invMassSum;
                j /= contactCount;

                // Apply impulse
                Vec2 impulse = normal.Mul(j);
                A.ApplyImpulse(impulse.Neg(), ra);
                B.ApplyImpulse(impulse, rb);

                // Friction impulse
                // rv = B->velocity + Cross( B->angularVelocity, rb ) -
                // A->velocity - Cross( A->angularVelocity, ra );
                rv = B.velocity.Add(Vec2.Cross(B.angularVelocity, rb, new Vec2())).Subi(A.velocity).Subi(Vec2.Cross(A.angularVelocity, ra, new Vec2()));

                // Vec2 t = rv - (normal * Dot( rv, normal ));
                // t.Normalize( );
                Vec2 t = new Vec2(rv);
                t.Addsi(normal, -Vec2.Dot(rv, normal));
                t.Normalize();

                // j tangent magnitude
                float jt = -Vec2.Dot(rv, t);
                jt /= invMassSum;
                jt /= contactCount;

                // Don't apply tiny friction impulses
                if (ImpulseMath.Equal(jt, 0.0f))
                {
                    return;
                }

                // Coulumb's law
                Vec2 tangentImpulse;
                // if(std::abs( jt ) < j * sf)
                if (Mathf.Abs(jt) < j * sf)
                {
                    // tangentImpulse = t * jt;
                    tangentImpulse = t.Mul(jt);
                }
                else
                {
                    // tangentImpulse = t * -j * df;
                    tangentImpulse = t.Mul(j).Muli(-df);
                }

                // Apply friction impulse
                // A->ApplyImpulse( -tangentImpulse, ra );
                // B->ApplyImpulse( tangentImpulse, rb );
                A.ApplyImpulse(tangentImpulse.Neg(), ra);
                B.ApplyImpulse(tangentImpulse, rb);
            }
        }

        public void PositionalCorrection()
        {
            // const real k_slop = 0.05f; // Penetration allowance
            // const real percent = 0.4f; // Penetration percentage to correct
            // Vec2 correction = (std::Max( penetration - k_slop, 0.0f ) / (A->im +
            // B->im)) * normal * percent;
            // A->position -= correction * A->im;
            // B->position += correction * B->im;

            float correction = Mathf.Max(penetration - ImpulseMath.PENETRATION_ALLOWANCE, 0.0f) / (A.invMass + B.invMass) * ImpulseMath.PENETRATION_CORRETION;

            A.position.Addsi(normal, -A.invMass * correction);
            B.position.Addsi(normal, B.invMass * correction);
        }

        public void InfiniteMassCorrection()
        {
            A.velocity.Set(0, 0);
            B.velocity.Set(0, 0);
        }
    }
}