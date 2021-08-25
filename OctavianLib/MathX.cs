using System.Linq;

namespace System
{
    public static class MathX
    {
        /// <summary>
        /// Calculates the Greatest Common Factor of two integers.
        /// </summary>
        public static int GCF(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calculates the Greatest Common Factor of integer array.
        /// </summary>
        public static int GCF(int[] numbers)
        {
            if(numbers.Length > 2)
            {
                var _newNums = new int[numbers.Length - 1];
                Array.Copy(numbers, 2, _newNums, 1, _newNums.Length - 1);

                int a = numbers[0].Copy();
                int b = numbers[1].Copy();

                _newNums[0] = MathX.GCF(a, b);

                return MathX.GCF(_newNums);
            }

            else if(numbers.Length == 2)
            {

                int a = numbers[0].Copy();
                int b = numbers[1].Copy();

                return MathX.GCF(a, b);
            }

            else if(numbers.Length == 1)
            {
                return numbers[0];
            }
            
            return 0;
        }

        /// <summary>
        /// Calculates the Least Common Denominator of two integers.
        /// </summary>
        public static int LCD(int a, int b)
        {
            return (a / MathX.GCF(new int[]{a, b})) * b;
        }

        /// <summary>
        /// Calculates the Least Common Denominator of integer array.
        /// </summary>
        public static int LCD(int[] numbers)
        {
            if(numbers.Length > 2)
            {
                var _newNums = new int[numbers.Length - 1];
                Array.Copy(numbers, 2, _newNums, 1, _newNums.Length - 1);

                int a = numbers[0].Copy();
                int b = numbers[1].Copy();

                _newNums[0] = MathX.LCD(a, b);

                return MathX.LCD(_newNums);
            }
            else if(numbers.Length == 2)
            {
                int a = numbers[0].Copy();
                int b = numbers[1].Copy();

                return MathX.LCD(a, b);
            }

            else if(numbers.Length == 1)
            {
                return numbers[0];
            }

            return 0;
        }
    }
}