
using System;

namespace TestAssembly
{
    public enum TestEnumValue
    {
        Value1,
        Value2,
        Value3,
    }

    public class TestEnum
    {
        public static TestEnumValue TestReturnEnum()
        {
            return TestEnumValue.Value2;
        }

        public static string TestReturnEnumName()
        {
            return Enum.GetName(typeof(TestEnumValue), TestEnumValue.Value3);
        }

        public static string TestGetEnumNames()
        {
            return Enum.GetNames(typeof(TestEnumValue))[0];
        }

        public static object TestParseEnum()
        {
            return Enum.Parse(typeof(TestEnumValue), "Value2");
        }

        public static object TestParseGenericEnum()
        {
            return Enum.Parse<TestEnumValue>("Value2");
        }

        public static object TestTryParseEnum()
        {
            Enum.TryParse(typeof(TestEnumValue), "Value2", out object result);
            return result;
        }

        public static object TestTryParseGenericEnum()
        {
            Enum.TryParse<TestEnumValue>("Value2", out TestEnumValue result);
            return result;
        }

        public static object TestGetUnderlyingEnumType()
        {
            return Enum.GetUnderlyingType(typeof(TestEnumValue));
        }

        public static object TestGetEnumValues()
        {
            return Enum.GetValues(typeof(TestEnumValue));
        }

        public static object TestFormatEnum()
        {
            return Enum.Format(typeof(TestEnumValue), TestEnumValue.Value2, "g");
        }
    }
}
