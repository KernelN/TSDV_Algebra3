using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Quat : IEquatable<Quat>
    {
        //Variables
        public float x;
        public float y;
        public float z;
        public float w;

        //Static Properties
        public static readonly Quat identity = new Quat(0.0f, 0.0f, 0.0f, 1f);

        //Properties
        public Vec3 eulerAngles { get { return Vec3.Zero; } } //NOT IMPLEMENTED
        public Quat normalized { get { return identity; } } //NOT IMPLEMENTED

        //Constructors
        public Quat(float x, float y, float z, float w)
        {
            //https://docs.unity3d.com/ScriptReference/Quaternion.html
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            
            //PUBLIC METHODS
            Vector3 v = new Vec3(x,y,z);
            Quaternion q = new Quaternion(x,y,z,w);
            q.Set(x,y,z,w); //https://docs.unity3d.com/ScriptReference/Quaternion.Set.html
            q.SetFromToRotation(Vec3.Zero, Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.SetFromToRotation.html
            q.SetLookRotation(Vec3.Zero, Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.SetLookRotation.html
            q.ToAngleAxis(out x, out v); //https://docs.unity3d.com/ScriptReference/Quaternion.ToAngleAxis.html
            q.ToString(); //https://docs.unity3d.com/ScriptReference/Quaternion.ToString.html
            
            //STATIC METHODS
            Quaternion.Angle(Quaternion.identity, Quaternion.identity); //https://docs.unity3d.com/ScriptReference/Quaternion.Angle.html
            Quaternion.AngleAxis(0, Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.AngleAxis.html
            Quaternion.Dot(Quaternion.identity, Quaternion.identity); //https://docs.unity3d.com/ScriptReference/Quaternion.Dot.html
            Quaternion.Euler(Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html
            Quaternion.FromToRotation(Vec3.Zero, Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.FromToRotation.html
            Quaternion.Inverse(Quaternion.identity); //https://docs.unity3d.com/ScriptReference/Quaternion.Inverse.html
            Quaternion.Lerp(Quaternion.identity, Quaternion.identity, 0); //https://docs.unity3d.com/ScriptReference/Quaternion.Lerp.html
            Quaternion.LerpUnclamped(Quaternion.identity, Quaternion.identity, 0); //https://docs.unity3d.com/ScriptReference/Quaternion.LerpUnclamped.html
            Quaternion.LookRotation(Vec3.Zero); //https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
            Quaternion.RotateTowards(Quaternion.identity, Quaternion.identity, 0); //https://docs.unity3d.com/ScriptReference/Quaternion.RotateTowards.html
            Quaternion.Slerp(Quaternion.identity, Quaternion.identity, 0); //https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
            Quaternion.SlerpUnclamped(Quaternion.identity, Quaternion.identity, 0); //https://docs.unity3d.com/ScriptReference/Quaternion.SlerpUnclamped.html
        }

        #region Operators

        public static Quat operator*(Quat q1, Quat q2)
        {
            throw new NotImplementedException();
        }
        public static bool operator==(Quat q1, Quat q2)
        {
            throw new NotImplementedException();
        }
        public static bool operator!=(Quat q1, Quat q2)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region IEquatable

        public bool Equals(Quat other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Quat other && Equals(other);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods

        

        #endregion

        #region Static Methods

        

        #endregion
    }
}