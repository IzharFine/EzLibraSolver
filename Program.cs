using System;
using System.Collections.Generic;
using System.Linq;

namespace HackRank
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> weightStack = new Queue<int>();
            Console.WriteLine("Enter your 'RL' string:");
            string rightLeftString = Console.ReadLine();
            Dictionary<LibraEnum, Stack<int>> libra = new Dictionary<LibraEnum, Stack<int>>();
            libra = InitLibra(libra);
            for (int i = 1 ; i <= rightLeftString.Length; i++)
            {
                weightStack.Enqueue(i);
            }
            SolveLibra(weightStack, libra, rightLeftString, 0);
        }

        private static Dictionary<LibraEnum, Stack<int>> InitLibra(Dictionary<LibraEnum, Stack<int>> libra)
        {
            libra[LibraEnum.LEFT_SIDE] = new Stack<int>();
            libra[LibraEnum.RIGHT_SIDE] = new Stack<int>();
            libra[LibraEnum.LEFT_INDEX] = new Stack<int>();
            libra[LibraEnum.RIGHT_INDEX] = new Stack<int>();
            libra[LibraEnum.IS_FINISHED] = new Stack<int>();
            return libra;
        }

        private static void SolveLibra(Queue<int> weightStack, Dictionary<LibraEnum, Stack<int>> libra, string rightLeftString, int counter)
        {
            for(int i=0;i< weightStack.Count && libra[LibraEnum.IS_FINISHED].Count == 0; i++)
            {
                int weight = weightStack.Dequeue();
                LibraEnum sideNeedToBeHeavy = CharToLibraSideEnum(rightLeftString[0]);
                if (IsValidMove(libra, sideNeedToBeHeavy, LibraEnum.LEFT_SIDE, weight))
                {
                    HandleLibraMove(LibraEnum.LEFT_SIDE, weight, weightStack, libra, rightLeftString, counter);
                }
                if (IsValidMove(libra, sideNeedToBeHeavy, LibraEnum.RIGHT_SIDE, weight))
                {
                    HandleLibraMove(LibraEnum.RIGHT_SIDE, weight, weightStack, libra, rightLeftString, counter);
                }
                weightStack.Enqueue(weight);
            }
        }

        private static void HandleLibraMove(LibraEnum side,
            int weight, Queue<int> weightStack,
            Dictionary<LibraEnum, Stack<int>> libra,
            string rightLeftString,
            int counter)
        {
            libra[side].Push(weight);
            libra[GetIndexBySide(side)].Push(counter);
            if (!weightStack.Any())
            {
                libra[LibraEnum.IS_FINISHED].Push(1);
                return;
            }
            SolveLibra(weightStack, libra, rightLeftString.Substring(1, rightLeftString.Length - 1), ++counter);
            if (libra[LibraEnum.IS_FINISHED].Count == 0)
            {
                libra[side].Pop();
                libra[GetIndexBySide(side)].Pop();
            }
        }

        private static LibraEnum CharToLibraSideEnum(char chr)
        {
            switch (chr)
            {
                case 'R':
                    return LibraEnum.RIGHT_SIDE;
                case 'L':
                    return LibraEnum.LEFT_SIDE;
            }
            return LibraEnum.ERROR;
        }

        private static bool IsValidMove(Dictionary<LibraEnum, Stack<int>> libra, LibraEnum sideNeedToBeHeavy, LibraEnum addToSide, int weight)
        {
            return sideNeedToBeHeavy == GetHeavySide(libra, addToSide, weight);
        }

        private static LibraEnum GetHeavySide(Dictionary<LibraEnum, Stack<int>> libra, LibraEnum addToSide, int weight)
        {
            int addToSideSum = libra[addToSide].Sum() + weight;
            int oppositeSideSum = libra[GetOppositeEnum(addToSide)].Sum();

            if (addToSideSum > oppositeSideSum)
            {
                return addToSide;
            }
            else if (addToSideSum < oppositeSideSum)
                return GetOppositeEnum(addToSide);
            return LibraEnum.EQUAL;
        }

        private static LibraEnum GetOppositeEnum(LibraEnum side)
        {
            return (side == LibraEnum.RIGHT_SIDE) ? LibraEnum.LEFT_SIDE : LibraEnum.RIGHT_SIDE;
        }

        private static LibraEnum GetIndexBySide(LibraEnum side)
        {
            return (side == LibraEnum.RIGHT_SIDE) ? LibraEnum.RIGHT_INDEX : LibraEnum.LEFT_INDEX;
        }

        private enum LibraEnum
        {
            RIGHT_SIDE,
            LEFT_SIDE,
            EQUAL,
            RIGHT_INDEX,
            LEFT_INDEX,
            IS_FINISHED,
            ERROR
        }
    }
}
