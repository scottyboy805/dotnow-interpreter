namespace TestAssembly
{
    public static class TestControlFlow
    {
        // Simple if-else tests
        public static object[] TestIfElse()
        {
            int a = 10;
            int b = 20;
            object result1 = null;
            object result2 = null;
            object result3 = null;

            if (a > b)
                result1 = "a is greater";
            else
                result1 = "b is greater or equal";

            if (a == 10)
                result2 = "a is ten";

            if (a > 0)
                result3 = "positive";
            else if (a < 0)
                result3 = "negative";
            else
                result3 = "zero";

            return new object[] { result1, result2, result3 };
        }

        // Switch statement tests
        public static object[] TestSwitch()
        {
            object result1 = null;
            object result2 = null;
            object result3 = null;

            int value1 = 1;
            int value2 = 2;
            int value3 = 99;

            switch (value1)
            {
                case 1:
                    result1 = "one";
                    break;
                case 2:
                    result1 = "two";
                    break;
                default:
                    result1 = "other";
                    break;
            }

            switch (value2)
            {
                case 1:
                    result2 = "one";
                    break;
                case 2:
                    result2 = "two";
                    break;
                default:
                    result2 = "other";
                    break;
            }

            switch (value3)
            {
                case 1:
                    result3 = "one";
                    break;
                case 2:
                    result3 = "two";
                    break;
                default:
                    result3 = "other";
                    break;
            }

            return new object[] { result1, result2, result3 };
        }

        // For loop tests
        public static object[] TestForLoop()
        {
            int sum = 0;
            int count = 0;

            for (int i = 0; i < 5; i++)
            {
                sum += i;
                count++;
            }

            int sum2 = 0;
            for (int i = 1; i <= 3; i++)
            {
                sum2 += i * 2;
            }

            return new object[] { sum, count, sum2 };
        }

        // While loop tests
        public static object[] TestWhileLoop()
        {
            int sum = 0;
            int i = 0;

            while (i < 5)
            {
                sum += i;
                i++;
            }

            int factorial = 1;
            int n = 4;
            while (n > 0)
            {
                factorial *= n;
                n--;
            }

            return new object[] { sum, factorial };
        }

        // Do-while loop tests
        public static object[] TestDoWhileLoop()
        {
            int sum = 0;
            int i = 0;

            do
            {
                sum += i;
                i++;
            } while (i < 3);

            int count = 0;
            int j = 5;
            do
            {
                count++;
                j--;
            } while (j > 0);

            return new object[] { sum, count };
        }

        // Break and continue tests
        public static object[] TestBreakContinue()
        {
            int sum1 = 0;
            int sum2 = 0;

            // Test break
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                    break;
                sum1 += i;
            }

            // Test continue
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    continue;
                sum2 += i;
            }

            return new object[] { sum1, sum2 };
        }

        // Nested loops test
        public static object[] TestNestedLoops()
        {
            int sum = 0;

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    sum += i * j;
                }
            }

            return new object[] { sum };
        }

        // Conditional branches - equality tests (brtrue/brfalse with ceq)
        public static object[] TestBranchEquality()
        {
            object[] results = new object[8];
            int index = 0;

            int a = 10, b = 10, c = 20;
            float f1 = 10.5f, f2 = 10.5f, f3 = 20.5f;

            // Test brtrue with ceq (int)
            if (a == b) results[index++] = "equal_int_true";
            else results[index++] = "equal_int_false";

            if (a == c) results[index++] = "not_equal_int_true";
            else results[index++] = "not_equal_int_false";

            // Test brtrue with ceq (float)
            if (f1 == f2) results[index++] = "equal_float_true";
            else results[index++] = "equal_float_false";

            if (f1 == f3) results[index++] = "not_equal_float_true";
            else results[index++] = "not_equal_float_false";

            // Test brfalse with ceq (negated conditions)
            if (!(a == b)) results[index++] = "negated_equal_true";
            else results[index++] = "negated_equal_false";

            if (!(a == c)) results[index++] = "negated_not_equal_true";
            else results[index++] = "negated_not_equal_false";

            if (!(f1 == f2)) results[index++] = "negated_equal_float_true";
            else results[index++] = "negated_equal_float_false";

            if (!(f1 == f3)) results[index++] = "negated_not_equal_float_true";
            else results[index++] = "negated_not_equal_float_false";

            return results;
        }

        // Conditional branches - greater than tests (brtrue/brfalse with cgt)
        public static object[] TestBranchGreaterThan()
        {
            object[] results = new object[12];
            int index = 0;

            int a = 10, b = 5, c = 15;
            uint ua = 10U, ub = 5U, uc = 15U;
            float f1 = 10.5f, f2 = 5.5f, f3 = 15.5f;

            // Test brtrue with cgt (int)
            if (a > b) results[index++] = "greater_int_true";
            else results[index++] = "greater_int_false";

            if (b > a) results[index++] = "not_greater_int_true";
            else results[index++] = "not_greater_int_false";

            // Test brtrue with cgt.un (unsigned)
            if (ua > ub) results[index++] = "greater_uint_true";
            else results[index++] = "greater_uint_false";

            if (ub > ua) results[index++] = "not_greater_uint_true";
            else results[index++] = "not_greater_uint_false";

            // Test brtrue with cgt (float)
            if (f1 > f2) results[index++] = "greater_float_true";
            else results[index++] = "greater_float_false";

            if (f2 > f1) results[index++] = "not_greater_float_true";
            else results[index++] = "not_greater_float_false";

            // Test brfalse with cgt (negated conditions)
            if (!(a > b)) results[index++] = "negated_greater_true";
            else results[index++] = "negated_greater_false";

            if (!(b > a)) results[index++] = "negated_not_greater_true";
            else results[index++] = "negated_not_greater_false";

            if (!(f1 > f2)) results[index++] = "negated_greater_float_true";
            else results[index++] = "negated_greater_float_false";

            if (!(f2 > f1)) results[index++] = "negated_not_greater_float_true";
            else results[index++] = "negated_not_greater_float_false";

            if (!(ua > ub)) results[index++] = "negated_greater_uint_true";
            else results[index++] = "negated_greater_uint_false";

            if (!(ub > ua)) results[index++] = "negated_not_greater_uint_true";
            else results[index++] = "negated_not_greater_uint_false";

            return results;
        }

        // Conditional branches - less than tests (brtrue/brfalse with clt)
        public static object[] TestBranchLessThan()
        {
            object[] results = new object[12];
            int index = 0;

            int a = 10, b = 5, c = 15;
            uint ua = 10U, ub = 5U, uc = 15U;
            float f1 = 10.5f, f2 = 5.5f, f3 = 15.5f;

            // Test brtrue with clt (int)
            if (b < a) results[index++] = "less_int_true";
            else results[index++] = "less_int_false";

            if (a < b) results[index++] = "not_less_int_true";
            else results[index++] = "not_less_int_false";

            // Test brtrue with clt.un (unsigned)
            if (ub < ua) results[index++] = "less_uint_true";
            else results[index++] = "less_uint_false";

            if (ua < ub) results[index++] = "not_less_uint_true";
            else results[index++] = "not_less_uint_false";

            // Test brtrue with clt (float)
            if (f2 < f1) results[index++] = "less_float_true";
            else results[index++] = "less_float_false";

            if (f1 < f2) results[index++] = "not_less_float_true";
            else results[index++] = "not_less_float_false";

            // Test brfalse with clt (negated conditions)
            if (!(b < a)) results[index++] = "negated_less_true";
            else results[index++] = "negated_less_false";

            if (!(a < b)) results[index++] = "negated_not_less_true";
            else results[index++] = "negated_not_less_false";

            if (!(f2 < f1)) results[index++] = "negated_less_float_true";
            else results[index++] = "negated_less_float_false";

            if (!(f1 < f2)) results[index++] = "negated_not_less_float_true";
            else results[index++] = "negated_not_less_float_false";

            if (!(ub < ua)) results[index++] = "negated_less_uint_true";
            else results[index++] = "negated_less_uint_false";

            if (!(ua < ub)) results[index++] = "negated_not_less_uint_true";
            else results[index++] = "negated_not_less_uint_false";

            return results;
        }

        // Direct comparison branch instructions (beq, bne, bgt, bge, ble, blt)
        public static object[] TestDirectBranchComparisons()
        {
            object[] results = new object[24];
            int index = 0;

            int a = 10, b = 10, c = 5, d = 15;

            // beq (branch on equal) tests
            if (a == b) results[index++] = "beq_true";
            else results[index++] = "beq_false";

            if (a == c) results[index++] = "beq_false_case";
            else results[index++] = "beq_false_else";

            // bne (branch on not equal) tests  
            if (a != c) results[index++] = "bne_true";
            else results[index++] = "bne_false";

            if (a != b) results[index++] = "bne_false_case";
            else results[index++] = "bne_false_else";

            // bgt (branch on greater than) tests
            if (a > c) results[index++] = "bgt_true";
            else results[index++] = "bgt_false";

            if (c > a) results[index++] = "bgt_false_case";
            else results[index++] = "bgt_false_else";

            // bge (branch on greater than or equal) tests
            if (a >= b) results[index++] = "bge_equal_true";
            else results[index++] = "bge_equal_false";

            if (a >= c) results[index++] = "bge_greater_true";
            else results[index++] = "bge_greater_false";

            if (c >= a) results[index++] = "bge_false_case";
            else results[index++] = "bge_false_else";

            // ble (branch on less than or equal) tests
            if (a <= b) results[index++] = "ble_equal_true";
            else results[index++] = "ble_equal_false";

            if (c <= a) results[index++] = "ble_less_true";
            else results[index++] = "ble_less_false";

            if (a <= c) results[index++] = "ble_false_case";
            else results[index++] = "ble_false_else";

            // blt (branch on less than) tests
            if (c < a) results[index++] = "blt_true";
            else results[index++] = "blt_false";

            if (a < c) results[index++] = "blt_false_case";
            else results[index++] = "blt_false_else";

            // Test with zero
            if (a == 10) results[index++] = "beq_zero_like";
            else results[index++] = "beq_zero_like_false";

            if (c != 0) results[index++] = "bne_zero_true";
            else results[index++] = "bne_zero_false";

            return results;
        }

        // Unsigned comparison branch instructions
        public static object[] TestUnsignedBranchComparisons()
        {
            object[] results = new object[12];
            int index = 0;

            uint a = 10U, b = 5U, c = 15U;
            uint large = uint.MaxValue;

            // bgt.un (branch on greater than unsigned) tests
            if (a > b) results[index++] = "bgt_un_true";
            else results[index++] = "bgt_un_false";

            if (large > a) results[index++] = "bgt_un_large_true";
            else results[index++] = "bgt_un_large_false";

            // bge.un (branch on greater than or equal unsigned) tests
            if (a >= a) results[index++] = "bge_un_equal_true";
            else results[index++] = "bge_un_equal_false";

            if (large >= a) results[index++] = "bge_un_large_true";
            else results[index++] = "bge_un_large_false";

            // ble.un (branch on less than or equal unsigned) tests
            if (b <= a) results[index++] = "ble_un_true";
            else results[index++] = "ble_un_false";

            if (a <= large) results[index++] = "ble_un_large_true";
            else results[index++] = "ble_un_large_false";

            // blt.un (branch on less than unsigned) tests
            if (b < a) results[index++] = "blt_un_true";
            else results[index++] = "blt_un_false";

            if (a < large) results[index++] = "blt_un_large_true";
            else results[index++] = "blt_un_large_false";

            return results;
        }

        // Null reference branch instructions (brtrue/brfalse with reference types)
        public static object[] TestNullReferenceBranches()
        {
            object[] results = new object[8];
            int index = 0;

            string nullStr = null;
            string nonNullStr = "test";
            object nullObj = null;
            object nonNullObj = new object();

            // brtrue tests (branching when reference is not null)
            if (nonNullStr != null) results[index++] = "brtrue_string_true";
            else results[index++] = "brtrue_string_false";

            if (nullStr != null) results[index++] = "brtrue_null_string_true";
            else results[index++] = "brtrue_null_string_false";

            if (nonNullObj != null) results[index++] = "brtrue_object_true";
            else results[index++] = "brtrue_object_false";

            if (nullObj != null) results[index++] = "brtrue_null_object_true";
            else results[index++] = "brtrue_null_object_false";

            // brfalse tests (branching when reference is null)
            if (nullStr == null) results[index++] = "brfalse_null_true";
            else results[index++] = "brfalse_null_false";

            if (nonNullStr == null) results[index++] = "brfalse_nonnull_true";
            else results[index++] = "brfalse_nonnull_false";

            if (nullObj == null) results[index++] = "brfalse_null_obj_true";
            else results[index++] = "brfalse_null_obj_false";

            if (nonNullObj == null) results[index++] = "brfalse_nonnull_obj_true";
            else results[index++] = "brfalse_nonnull_obj_false";

            return results;
        }

        // Boolean branch instructions (brtrue/brfalse with bool values)
        public static object[] TestBooleanBranches()
        {
            object[] results = new object[8];
            int index = 0;

            bool trueVal = true;
            bool falseVal = false;

            // brtrue tests
            if (trueVal) results[index++] = "brtrue_true";
            else results[index++] = "brtrue_false";

            if (falseVal) results[index++] = "brtrue_false_val";
            else results[index++] = "brtrue_false_val_else";

            // brfalse tests
            if (!falseVal) results[index++] = "brfalse_false_true";
            else results[index++] = "brfalse_false_false";

            if (!trueVal) results[index++] = "brfalse_true_val";
            else results[index++] = "brfalse_true_val_else";

            // Compound boolean expressions
            bool result1 = trueVal && !falseVal;
            if (result1) results[index++] = "compound_true";
            else results[index++] = "compound_false";

            bool result2 = falseVal || trueVal;
            if (result2) results[index++] = "compound_or_true";
            else results[index++] = "compound_or_false";

            bool result3 = trueVal && falseVal;
            if (result3) results[index++] = "compound_and_false_true";
            else results[index++] = "compound_and_false_false";

            bool result4 = falseVal || falseVal;
            if (result4) results[index++] = "compound_or_false_true";
            else results[index++] = "compound_or_false_false";

            return results;
        }

        // Exception handling branch instructions (leave, endfinally, etc.)
        public static object[] TestExceptionHandlingBranches()
        {
            object[] results = new object[6];
            int index = 0;

            // Try-catch blocks
            try
            {
                results[index++] = "try_block_executed";
                int zero = 0;
                // This will throw - testing leave instruction
                int divide = 10 / zero;
                results[index++] = "try_block_after_exception"; // Should not execute
            }
            catch (System.DivideByZeroException)
            {
                results[index++] = "catch_block_executed";
            }

            // Try-finally blocks (testing endfinally instruction)
            try
            {
                results[index++] = "try_finally_executed";
            }
            finally
            {
                results[index++] = "finally_block_executed";
            }

            // Try-catch-finally blocks
            try
            {
                results[index++] = "try_catch_finally_executed";
                throw new System.InvalidOperationException("Test exception");
            }
            catch (System.InvalidOperationException)
            {
                // This catch block should execute
            }
            finally
            {
                // Finally block should always execute
                // Note: we can't add to results here as index might be out of bounds
                // This tests the endfinally instruction path
            }

            return results;
        }

        // Complex nested branching scenarios
        public static object[] TestNestedBranching()
        {
            object[] results = new object[10];
            int index = 0;

            int a = 10, b = 5, c = 15;

            // Nested if-else with multiple conditions
            if (a > b)
            {
                if (a < c)
                {
                    if (b < c)
                    {
                        results[index++] = "nested_all_true";
                    }
                    else
                    {
                        results[index++] = "nested_inner_false";
                    }
                }
                else
                {
                    results[index++] = "nested_middle_false";
                }
            }
            else
            {
                results[index++] = "nested_outer_false";
            }

            // Complex loop with nested conditions
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (j == 0)
                        {
                            results[index++] = $"nested_loop_{i}_{j}";
                        }
                    }
                }
            }

            // Switch with nested conditions
            int switchVal = 2;
            switch (switchVal)
            {
                case 1:
                    if (a > b) results[index++] = "switch_case1_true";
                    else results[index++] = "switch_case1_false";
                    break;
                case 2:
                    if (b < c) results[index++] = "switch_case2_true";
                    else results[index++] = "switch_case2_false";
                    break;
                default:
                    results[index++] = "switch_default";
                    break;
            }

            // Multiple return paths (testing ret instruction with different paths)
            string returnTest = TestMultipleReturnPaths(true);
            results[index++] = returnTest;

            returnTest = TestMultipleReturnPaths(false);
            results[index++] = returnTest;

            // Fill remaining slots to avoid null entries
            while (index < results.Length)
            {
                results[index++] = "filled_slot";
            }

            return results;
        }

        private static string TestMultipleReturnPaths(bool condition)
        {
            if (condition)
            {
                if (condition && true)
                {
                    return "early_return_path1";
                }
                return "early_return_path2";
            }
            else
            {
                return "late_return_path";
            }
        }

        // Unconditional branch instructions (br)
        public static object[] TestUnconditionalBranches()
        {
            object[] results = new object[6];
            int index = 0;

            // Goto statements (br instruction)
            bool gotoExecuted = false;
            bool afterGoto = false;

            goto GotoLabel;
            afterGoto = true; // Should not execute

        GotoLabel:
            gotoExecuted = true;

            results[index++] = gotoExecuted ? "goto_executed" : "goto_not_executed";
            results[index++] = afterGoto ? "after_goto_executed" : "after_goto_not_executed";

            // Loop with break (br instruction)
            int loopCount = 0;
            while (true)
            {
                loopCount++;
                if (loopCount >= 3)
                    break; // br instruction to exit loop
            }
            results[index++] = $"loop_break_count_{loopCount}";

            // Loop with continue (br instruction)
            int continueCount = 0;
            int continueSum = 0;
            for (int i = 0; i < 5; i++)
            {
                if (i == 2)
                {
                    continueCount++;
                    continue; // br instruction to start of loop
                }
                continueSum += i;
            }
            results[index++] = $"continue_count_{continueCount}";
            results[index++] = $"continue_sum_{continueSum}";

            // Return statement (ret instruction)
            string returnResult = TestUnconditionalReturn();
            results[index++] = returnResult;

            return results;
        }

        private static string TestUnconditionalReturn()
        {
            // Unreachable code after return
            return "return_executed";
            // This line should never execute (generates unreachable code)
        }

        // Switch statement with jump table (switch instruction)
        public static object[] TestSwitchJumpTable()
        {
            object[] results = new object[10];
            int index = 0;

            // Test various switch patterns
            for (int testCase = 0; testCase < 6; testCase++)
            {
                string result = "";
                switch (testCase)
                {
                    case 0:
                        result = "case_0";
                        break;
                    case 1:
                        result = "case_1";
                        break;
                    case 2:
                        result = "case_2";
                        break;
                    case 3:
                        result = "case_3";
                        break;
                    case 4:
                        result = "case_4";
                        break;
                    default:
                        result = "default_case";
                        break;
                }
                results[index++] = result;
            }

            // Test switch with fall-through cases
            int fallThroughTest = 1;
            string fallThroughResult = "";
            switch (fallThroughTest)
            {
                case 0:
                case 1:
                case 2:
                    fallThroughResult = "cases_0_1_2";
                    break;
                case 3:
                    fallThroughResult = "case_3";
                    break;
                default:
                    fallThroughResult = "default";
                    break;
            }
            results[index++] = fallThroughResult;

            // Test switch with non-contiguous cases
            int sparseTest = 10;
            string sparseResult = "";
            switch (sparseTest)
            {
                case 1:
                    sparseResult = "sparse_1";
                    break;
                case 5:
                    sparseResult = "sparse_5";
                    break;
                case 10:
                    sparseResult = "sparse_10";
                    break;
                case 20:
                    sparseResult = "sparse_20";
                    break;
                default:
                    sparseResult = "sparse_default";
                    break;
            }
            results[index++] = sparseResult;

            // Test string switch (different IL pattern)
            string stringTest = "test";
            string stringResult = "";
            switch (stringTest)
            {
                case "hello":
                    stringResult = "string_hello";
                    break;
                case "test":
                    stringResult = "string_test";
                    break;
                case "world":
                    stringResult = "string_world";
                    break;
                default:
                    stringResult = "string_default";
                    break;
            }
            results[index++] = stringResult;

            // Fill remaining slots
            while (index < results.Length)
            {
                results[index++] = "switch_filled";
            }

            return results;
        }
    }
}