using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.Filters
{
    public static class FiltersHelper
    {
        // Properties - List of the logical operators, depending on the field type (numbers, characters)

        public static readonly IEnumerable<string> LogicalOperatorsForCharacters = new List<string>
        {
            "length <", "length <=", "length =", "length >=", "length >", "length <>",
            "string starts with", "string contains", "string <>", "string equals"
        };
        public static readonly IEnumerable<string> LogicalOperatorsForNumbers = new List<string>
        {
            "<", "<=", "=", ">=", ">", "<>"
        };

        public static readonly IEnumerable<string> AllLogicalOperators = LogicalOperatorsForCharacters
            .Concat(LogicalOperatorsForNumbers).Distinct();


        // Evaluation methods

        public static bool Evaluate(string valueToTest, string filterValue, string logicalOperator)
        {
            switch (logicalOperator)
            {
                case "length <":
                    return LengthLess(valueToTest, filterValue);
                case "length <=":
                    return LengthLessOrEqual(valueToTest, filterValue);
                case "length =":
                    return LengthEqual(valueToTest, filterValue);
                case "length >=":
                    return LengthGreaterOrEqual(valueToTest, filterValue);
                case "length >":
                    return LengthGreater(valueToTest, filterValue);
                case "length <>":
                    return LengthDifferent(valueToTest, filterValue);
                case "string starts with":
                    return StartsWith(valueToTest, filterValue);
                case "string contains":
                    return Contains(valueToTest, filterValue);
                case "string <>":
                    return Different(valueToTest, filterValue);
                case "string equals":
                    return StringEquals(valueToTest, filterValue);
                default:
                    return false;
            }
        }

        public static bool Evaluate(double valueToTest, double filterValue, string logicalOperator)
        {
            switch (logicalOperator)
            {
                case "<":
                    return Less(valueToTest, filterValue);
                case "<=":
                    return LessOrEqual(valueToTest, filterValue);
                case "=":
                    return Equal(valueToTest, filterValue);
                case ">=":
                    return GreaterOrEqual(valueToTest, filterValue);
                case ">":
                    return Greater(valueToTest, filterValue);
                case "<>":
                    return Different(valueToTest, filterValue);
                default:
                    return false;
            }
        }

        // Characters

        private static bool LengthLess(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length < Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool LengthLessOrEqual(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length <= Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool LengthEqual(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length == Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool LengthGreaterOrEqual(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length >= Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool LengthGreater(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length > Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool LengthDifferent(string valueToTest, string filterValue)
        {
            try { return valueToTest.Length != Convert.ToDouble(filterValue); }
            catch (FormatException) { return false; }
        }

        private static bool StartsWith(string valueToTest, string filterValue)
        {
            return valueToTest.StartsWith(filterValue, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool Contains(string valueToTest, string filterValue)
        {
            return valueToTest.ToLower().Contains(filterValue.ToLower());
        }

        private static bool Different(string valueToTest, string filterValue)
        {
            return !valueToTest.Equals(filterValue, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool StringEquals(string valueToTest, string filterValue)
        {
            return valueToTest.Equals(filterValue, StringComparison.InvariantCultureIgnoreCase);
        }

        // Numbers

        private static bool Less(double valueToTest, double filterValue)
        {
            return valueToTest < filterValue;
        }

        private static bool LessOrEqual(double valueToTest, double filterValue)
        {
            return valueToTest <= filterValue;
        }

        private static bool Equal(double valueToTest, double filterValue)
        {
            return valueToTest == filterValue;
        }

        private static bool GreaterOrEqual(double valueToTest, double filterValue)
        {
            return valueToTest >= filterValue;
        }

        private static bool Greater(double valueToTest, double filterValue)
        {
            return valueToTest > filterValue;
        }

        private static bool Different(double valueToTest, double filterValue)
        {
            return valueToTest != filterValue;
        }
    }
}