using System;
using UnityEngine;
using TLExtensions;

public partial class Timeline
{
    public Core code = new Core();

    public partial class Core
    {
        Timeline _timeline;
        public void Init(Timeline timeline)
        {
            this._timeline = timeline;
            TimelineCode.Log("Code Started");
        }
        
        public bool IsVector(object value) {
            return value.MemberExists("x") && value.MemberExists("y") || value.MemberExists("u") && value.MemberExists("v");
        }
        public bool SmartCheckIndexType(object[] options, int i) {
            if (IsVector(options[i]) && i < options.Length) {
                TLVectors tempVect = new TLVectors(options[i]);
                if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y", "z"}, true)) 
                {
                    options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y"}, true)) 
                {
                    options[i] = new TLVector2(tempVect.x, tempVect.y, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y", "z", "w"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.w, tempVect.isRadian ? "translate" : "radian");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"u", "v"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.u, tempVect.v);
                    return true;
                };
            }
            return false;
        }
        public object[] BindInstructionSet(object[] options)
        {
            int optionsLen = options.Length;

            // if xyz combo for optimal
            int[] xyz = new int[3];
            // prepare rows
            int[] rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (options[o] == null) break;

                if (CheckIndexType(options, o, typeof(bool))) break;

                // o = 0 Timeline
                if (CheckIndexType(options, o, typeof(Timeline)))
                    rows[0]++;

                if (CheckIndexType(options, o, typeof(Timeline[])))
                    rows[0] += ((Timeline[])options[o]).Length;

                // o = 1 bind/element/transform, o = 2 int
                if (CheckIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || CheckIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"})
                 || SmartCheckIndexType(options, o))
                    rows[1]++;

                // o = 3 string, o = 4 int, o = 5 int
                if (CheckIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "a", "r", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || CheckIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'a', 'r', 'w', 'u', 'v' })) {
                    rows[2]++;
                    if (options[o].ToString() == "x") xyz[0] = 1;
                    if (options[o].ToString() == "y") xyz[1] = 1;
                    if (options[o].ToString() == "z") xyz[2] = 1;
                }

                // o = 6 int x 4
                if (CheckIndexType(options, o, typeof(int)))
                    rows[3]++;
            }

            bool isXYZ = xyz[0] + xyz[1] + xyz[2] == 3;

            object[][] phrase = new object[rows.Length][];
            for (int r = 0; r < rows.Length; r++)
            {
                phrase[r] = new object[rows[r]];
            }
            
            // add data to rows
            rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (options[o] == null) break;

                if (CheckIndexType(options, o, typeof(bool))) break;

                // o = 0 Timeline
                if (CheckIndexType(options, o, typeof(Timeline)))
                {
                    phrase[0][rows[0]] = options[o];
                    rows[0]++;
                }

                if (CheckIndexType(options, o, typeof(Timeline[]))) {
                    for (int t = 0; t < ((object[])options[o]).Length; t++) {
                        phrase[0][rows[0]] = ((object[])options[o])[t];
                        rows[0]++;
                    }
                }

                // o = 1 bind/element/tranform, o = 2 int
                if (CheckIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || CheckIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"}))
                {
                    options[o] = options[o] is TLType ? options[o] as TLType : options[o].CastToElement(options);

                    phrase[1][rows[1]] = new object[]
                    {
                            options[o],
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            null,
                            null
                    };
                    rows[1]++;
                }

                // o = 3 string, o = 4 int, o = 5 int
                if (CheckIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "a", "r", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || CheckIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'a', 'r', 'w', 'u', 'v' }))
                {
                    
                    phrase[2][rows[2]] = new object[]
                    {
                            options[o].ToString() == "a" ? "alpha" : options[o].ToString() == "r" ? "rotation" : options[o].ToString(),
                            CheckIndexType(options, o+1, typeof(float)) || CheckIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(float)) || CheckIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            isXYZ
                    };
                    rows[2]++;
                }
                
                // o = 6 int x 4
                if (CheckIndexType(options, o, typeof(int)))
                {
                    phrase[3] = new object[]
                    {
                            options[o],
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null
                    };
                }
            }

            return new object[]
            {
                    phrase[0],
                    phrase[1],
                    phrase[2],
                    phrase[3],
                    CheckIndexType(options, options.Length-2, typeof(bool)) ? options[options.Length-2] : false,
                    CheckIndexType(options, options.Length-1, typeof(float)) ? options[options.Length-1] : 1F
            };
        }
        public class InstructionSet {
            object[][] instructions;
            public InstructionSet (object[] options) {
                instructions = instructions.Concat(options);
            }
        }

        public static bool CheckIndexType(object[] options, int i, System.Type type)
        {
            if (i < options.Length && options[i].GetType() == type)
                return true;
            else
                return false;
        }
        public static T checkTypeCast<T>(object obj, T type)
        {
            return (T)obj;
        }
        public static bool CheckIndexFieldTypeByString(object[] options, int i, string[] fieldTypes, bool exact = false)
        {
            if (i < options.Length) {
                int e = 0;
                for (int f = 0; f < fieldTypes.Length; f++) {
                    if (options[i].GetType().GetProperty(fieldTypes[f]) != null || options[i].GetType().GetField(fieldTypes[f]) != null) {
                        e++;
                        if (exact == false) return true;
                    }
                }
                if (e == fieldTypes.Length) return true;
            }
            return false;
        }
        public static bool CheckIndexListTypes(object[] options, int i, System.Type[] list)
        {
            if (i < options.Length)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].GetType() == list[l]) 
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public static System.Type CheckIndexListTypesGet(object[] options, int i, System.Type Base,  System.Type[] list)
        {
            if (i < options.Length)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].GetType() == list[l]) 
                    return list[l];
                }
                return Base;
            }
            else
            {
                return Base;
            }
        }
        public static bool CheckIndexTypeList(object[] options, int i, System.Type type, object[] list)
        {
            if (i < options.Length && options[i].GetType() == type)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].Equals(list[l])) return true; // compare Chars *.Equals
                    // if (options[i] == list[l]) return true;
                }
                return false;
            }
            else
                return false;
        }
        public static bool CheckList(object option, object[] list)
        {
            for (int l = 0; l < list.Length; l++)
            {
                if (option.Equals(list[l])) return true; // compare Chars *.Equals
                // if (option == list[l]) return true;
            }
            return false;
        }
        public static object CheckListGet(string option, string[] list)
        {
            foreach (var pair in list)
            {
                string[] itemArr = pair.Split('=');
                if (option == itemArr[0]) return itemArr[1];
            }
            return "uniform";
        }
    }
}