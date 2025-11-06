namespace TestAssembly
{
    public static class TestBitwise
    {
        // Bitwise AND tests
        public static object[] TestBitwiseAnd()
        {
            int a = 0xFF; // 255
            int b = 0xF0; // 240
            uint ua = 0xFFU;
            uint ub = 0xF0U;
            byte ba = 0xFF;
            byte bb = 0xF0;
            long la = 0xFFL;
            long lb = 0xF0L;

            return new object[]
      {
     a & b,      // int & int
                ua & ub,    // uint & uint
        ba & bb,    // byte & byte (promoted to int)
         la & lb,    // long & long
      a & 0x0F,   // int & literal
  ua & 0xFFU, // uint & literal
            };
        }

        // Bitwise OR tests
        public static object[] TestBitwiseOr()
        {
            int a = 0x0F; // 15
            int b = 0xF0; // 240
            uint ua = 0x0FU;
            uint ub = 0xF0U;
            byte ba = 0x0F;
            byte bb = 0xF0;
            long la = 0x0FL;
            long lb = 0xF0L;

            return new object[]
                      {
    a | b,      // int | int
     ua | ub,    // uint | uint
ba | bb,    // byte | byte (promoted to int)
         la | lb,    // long | long
        a | 0x00,   // int | literal
         ua | 0x00U, // uint | literal
                      };
        }

        // Bitwise XOR tests
        public static object[] TestBitwiseXor()
        {
            int a = 0xFF; // 255
            int b = 0xF0; // 240
            uint ua = 0xFFU;
            uint ub = 0xF0U;
            byte ba = 0xFF;
            byte bb = 0xF0;
            long la = 0xFFL;
            long lb = 0xF0L;

            return new object[]
     {
           a ^ b,      // int ^ int
                ua ^ ub,    // uint ^ uint
      ba ^ bb,    // byte ^ byte (promoted to int)
       la ^ lb,    // long ^ long
       a ^ 0xFF,   // int ^ literal
           ua ^ 0xFFU, // uint ^ literal
};
        }

        // Bitwise NOT tests
        public static object[] TestBitwiseNot()
        {
            int a = 0x0F;
            uint ua = 0x0FU;
            byte ba = 0x0F;
            long la = 0x0FL;

            return new object[]
                {
     ~a,     // ~int
 ~ua,    // ~uint
      ~ba,    // ~byte (promoted to int)
      ~la,    // ~long
                   };
        }

        // Left shift tests
        public static object[] TestLeftShift()
        {
            int a = 1;
            uint ua = 1U;
            long la = 1L;
            byte ba = 1;

            return new object[]
{
  a << 1,     // int << int
            a << 4,     // int << int
       ua << 1,    // uint << int
        ua << 4,    // uint << int
  la << 1,    // long << int
           la << 4,    // long << int
     ba << 1,    // byte << int (promoted)
       ba << 4,    // byte << int (promoted)
            };
        }

        // Right shift tests
        public static object[] TestRightShift()
        {
            int a = 16;
            uint ua = 16U;
            long la = 16L;
            int negative = -16;

            return new object[]
            {
        a >> 1,         // int >> int
            a >> 4,         // int >> int
                ua >> 1,        // uint >> int
                ua >> 4,      // uint >> int
  la >> 1,        // long >> int
     la >> 4,        // long >> int
    negative >> 1,  // negative int >> int (sign extension)
      negative >> 4,  // negative int >> int (sign extension)
       };
        }
    }
}