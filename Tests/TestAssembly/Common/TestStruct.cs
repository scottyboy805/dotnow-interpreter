using System;

namespace TestAssembly
{
    // Test struct with various field types
    public struct TestStruct
    {
        public int IntField;
        public float FloatField;
        public bool BoolField;
        public string StringField; // reference type in struct
        public DateTime DateTimeField; // another struct

        public TestStruct(int intVal, float floatVal, bool boolVal, string stringVal)
        {
            IntField = intVal;
            FloatField = floatVal;
            BoolField = boolVal;
            StringField = stringVal;
            DateTimeField = DateTime.Now;
        }

        public int GetSum()
        {
            return IntField + (int)FloatField;
        }

        public void ModifyFields(int newInt, float newFloat)
        {
            IntField = newInt;
            FloatField = newFloat;
        }
    }

    // Nested struct
    public struct Point2D
    {
        public float X;
        public float Y;

        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Distance => (float)Math.Sqrt(X * X + Y * Y);
    }

    public struct Rectangle
    {
        public Point2D TopLeft;
        public Point2D BottomRight;

        public Rectangle(Point2D topLeft, Point2D bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public float Area => Math.Abs((BottomRight.X - TopLeft.X) * (BottomRight.Y - TopLeft.Y));
    }

    // Struct with overridden methods
    public struct CustomStruct
    {
        public int Value;

        public CustomStruct(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"CustomStruct({Value})";
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomStruct other)
                return Value == other.Value;
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public static class TestStructMethods
    {
        // Test struct creation and field access
        public static object[] TestStructCreation()
        {
            var s = new TestStruct(42, 3.14f, true, "test");
            return new object[] { s.IntField, s.FloatField, s.BoolField, s.StringField };
        }

        // Test struct field modification
        public static object[] TestStructFieldModification()
        {
            var s = new TestStruct(10, 2.5f, false, "initial");
            s.IntField = 20;
            s.FloatField = 5.0f;
            s.BoolField = true;
            s.StringField = "modified";
            return new object[] { s.IntField, s.FloatField, s.BoolField, s.StringField };
        }

        // Test struct method calls
        public static int TestStructMethodCall()
        {
            var s = new TestStruct(15, 7.8f, true, "test");
            return s.GetSum();
        }

        // Test struct method that modifies fields
        public static object[] TestStructMethodModification()
        {
            var s = new TestStruct(5, 1.0f, true, "test");
            s.ModifyFields(100, 200.5f);
            return new object[] { s.IntField, s.FloatField };
        }

        // Test pass by value (struct copy)
        public static object[] TestStructPassByValue()
        {
            var original = new TestStruct(10, 5.0f, true, "original");
            var copy = ModifyStructByValue(original);

            // Original should be unchanged, copy should be modified
            return new object[] {
   original.IntField, original.FloatField, original.StringField,
                copy.IntField, copy.FloatField, copy.StringField
  };
        }

        private static TestStruct ModifyStructByValue(TestStruct s)
        {
            s.IntField = 999;
            s.FloatField = 888.0f;
            s.StringField = "modified";
            return s;
        }

        // Test pass by reference
        public static object[] TestStructPassByRef()
        {
            var original = new TestStruct(10, 5.0f, true, "original");
            ModifyStructByRef(ref original);

            // Original should be modified
            return new object[] { original.IntField, original.FloatField, original.StringField };
        }

        private static void ModifyStructByRef(ref TestStruct s)
        {
            s.IntField = 777;
            s.FloatField = 666.0f;
            s.StringField = "ref modified";
        }

        // Test struct out parameter
        public static object[] TestStructOutParameter()
        {
            TestStruct result;
            CreateStructViaOut(out result);
            return new object[] { result.IntField, result.FloatField, result.BoolField, result.StringField };
        }

        private static void CreateStructViaOut(out TestStruct s)
        {
            s = new TestStruct(123, 456.7f, false, "out param");
        }

        // Test struct in parameter
        public static int TestStructInParameter()
        {
            var s = new TestStruct(50, 25.0f, true, "test");
            return GetSumViaIn(in s);
        }

        private static int GetSumViaIn(in TestStruct s)
        {
            // Should not be able to modify s here
            return s.IntField + (int)s.FloatField;
        }

        // Test struct boxing/unboxing
        public static object[] TestStructBoxing()
        {
            var s = new TestStruct(42, 3.14f, true, "boxed");
            object boxed = s; // Boxing
            var unboxed = (TestStruct)boxed; // Unboxing

            return new object[] {
  unboxed.IntField, unboxed.FloatField, unboxed.BoolField, unboxed.StringField
  };
        }

        // Test struct in arrays
        public static object[] TestStructInArrays()
        {
            var structs = new TestStruct[3];
            structs[0] = new TestStruct(1, 1.1f, true, "first");
            structs[1] = new TestStruct(2, 2.2f, false, "second");
            structs[2] = new TestStruct(3, 3.3f, true, "third");

            // Modify one element
            structs[1].IntField = 999;

            return new object[] {
       structs[0].IntField, structs[1].IntField, structs[2].IntField,
       structs[0].StringField, structs[1].StringField, structs[2].StringField
            };
        }

        // Test nested structs
        public static object[] TestNestedStructs()
        {
            var rect = new Rectangle(
            new Point2D(10.0f, 20.0f),
     new Point2D(30.0f, 40.0f)
                 );

            return new object[] {
     rect.TopLeft.X, rect.TopLeft.Y,
         rect.BottomRight.X, rect.BottomRight.Y,
          rect.Area
     };
        }

        // Test struct property access
        public static float TestStructProperties()
        {
            var point = new Point2D(3.0f, 4.0f);
            return point.Distance; // Should be 5.0
        }

        // Test struct default constructor
        public static object[] TestStructDefaultConstructor()
        {
            var defaultStruct = new TestStruct();
            return new object[] {
      defaultStruct.IntField,
      defaultStruct.FloatField,
          defaultStruct.BoolField,
         defaultStruct.StringField
     };
        }

        // Test struct with overridden methods
        public static object[] TestStructOverrides()
        {
            var s1 = new CustomStruct(42);
            var s2 = new CustomStruct(42);
            var s3 = new CustomStruct(99);

            return new object[] {
     s1.ToString(),
         s1.Equals(s2),
           s1.Equals(s3),
      s1.GetHashCode() == s2.GetHashCode()
     };
        }

        // Test struct mutation in methods
        public static object[] TestStructMutation()
        {
            var s = new TestStruct(10, 5.0f, true, "test");
            var original = s;

            // This should NOT modify s (pass by value)
            MutateStruct(s);

            // This SHOULD modify s (pass by reference)
            MutateStructRef(ref s);

            return new object[] {
         original.IntField, original.StringField,  // Original values
              s.IntField, s.StringField    // Modified values
            };
        }

        private static void MutateStruct(TestStruct s)
        {
            s.IntField = 888;
            s.StringField = "should not change";
        }

        private static void MutateStructRef(ref TestStruct s)
        {
            s.IntField = 999;
            s.StringField = "changed via ref";
        }

        // Test struct assignment (should copy)
        public static object[] TestStructAssignment()
        {
            var s1 = new TestStruct(100, 200.0f, true, "s1");
            var s2 = s1; // Should create a copy

            s2.IntField = 300;
            s2.StringField = "s2";

            return new object[] {
          s1.IntField, s1.StringField,  // Original should be unchanged
            s2.IntField, s2.StringField   // Copy should be modified
 };
        }

        // Test struct comparison (value equality)
        public static bool[] TestStructEquality()
        {
            var s1 = new CustomStruct(42);
            var s2 = new CustomStruct(42);
            var s3 = new CustomStruct(99);

            return new bool[] {
       s1.Equals(s2),  // Should be true (same value)
                s1.Equals(s3),  // Should be false (different value)
     object.ReferenceEquals(s1, s2)  // Should be false (value types, no reference equality)
            };
        }

        // Test struct as interface (boxing scenario)
        public static string TestStructAsInterface()
        {
            var s = new CustomStruct(42);
            object obj = s; // Boxing
            return obj.ToString(); // Should call overridden ToString
        }

        // Test struct with readonly fields behavior
        public static object[] TestStructWithReadonlyBehavior()
        {
            // Even though we don't have readonly fields in our test struct,
            // we can test that struct fields behave as expected
            var s = new TestStruct(1, 2.0f, true, "test");
            var copy = s;

            // Modify copy
            copy.IntField = 999;

            // Original should be unchanged
            return new object[] { s.IntField, copy.IntField };
        }

        // Test large struct to verify memory handling
        public static bool TestLargeStructHandling()
        {
            var rects = new Rectangle[1000];
            for (int i = 0; i < 1000; i++)
            {
                rects[i] = new Rectangle(
                        new Point2D(i, i * 2),
                 new Point2D(i + 10, i * 2 + 20)
                           );
            }

            // Verify first and last elements
            return rects[0].TopLeft.X == 0 &&
          rects[999].TopLeft.X == 999 &&
     rects[999].BottomRight.Y == (999 * 2 + 20);
        }
    }
}