using System;
using System.Collections.Generic;

namespace Gma.CodeCloud.Base.FileIO
{
    internal static class PatternMatcher
    {
        public static bool MatchesAny(IEnumerable<string> expressions, string input)
        {
            foreach (string expression in expressions)
            {
                if (Matches(expression, input))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MatchesAll(IEnumerable<string> expressions, string input)
        {
            foreach (string expression in expressions)
            {
                if (!Matches(expression, input))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The original implementation used RegEx.
        /// Was much inneficient then this unreadable on :D
        /// Feel free to readabilize it. There is an appropriate testset for that.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// * - Zero or more characters.
        /// ? - Exactly one character.
        /// </remarks>
        public static bool Matches(string expression, string input)
        {
            if (ExprOrInputIsEmpty(input, expression))
            {
                return false;
            }

            if (MatchesAnyInput(expression))
            {
                return true;
            }

            if (IsEndsWithExpr(expression))
            {
                string restExpr = expression.Substring(1);
                return input.EndsWith(restExpr, StringComparison.OrdinalIgnoreCase);
            }

            if (IsStartsWithExpr(expression))
            {
                string restExpr = expression.Substring(0, expression.Length - 1);
                return input.StartsWith(restExpr, StringComparison.OrdinalIgnoreCase);
            }

            expression = expression.ToLowerInvariant();
            input = input.ToLowerInvariant();

            if (IsContainsExpr(expression))
            {
                string restExpr = expression.Substring(1, expression.Length - 1);
                return input.Contains(restExpr);
            }


            const int matchesArraySize = 16;
            const char ansiDosStar = '>';
            const char ansiDosQm = '<';
            const char dosDot = '"';

            char nameChar = '\0';

            int[] previousMatches = new int[matchesArraySize];
            int[] currentMatches = new int[matchesArraySize];

            int currentState;

            bool inputFinished = false;

            //
            //  The idea behind the algorithm is pretty simple.  We keep track of
            //  all possible locations in the regular expression that are matching
            //  the input.  If when the name has been exhausted one of the locations
            //  in the expression is also just exhausted, the name is in the language
            //  defined by the regular expression.
            //

            //
            //  Walk through the name string, picking off characters.  We go one
            //  character beyond the end because some wild cards are able to match
            //  zero characters beyond the end of the string.
            //
            //  With each new name character we determine a new set of states that
            //  match the name so far.  We use two arrays that we swap back and forth
            //  for this purpose.  One array lists the possible expression states for
            //  all name characters up to but not including the current one, and other
            //  array is used to build up the list of states considering the current
            //  name character as well.  The arrays are then switched and the process
            //  repeated.
            //
            //  There is not a one-to-one correspondence between state number and
            //  offset into the expression.  This is evident from the NFAs in the
            //  initial comment to this function.  State numbering is not continuous.
            //  This allows a simple conversion between state number and expression
            //  offset.  Each character in the expression can represent one or two
            //  states.  * and DOS_STAR generate two states: ExprOffset*2 and
            //  ExprOffset*2 + 1.  All other expreesion characters can produce only
            //  a single state.  Thus ExprOffset = State/2.
            //
            //
            //  Here is a short description of the variables involved:
            //
            //  NameOffset  - The offset of the current name char being processed.
            //
            //  ExprOffset  - The offset of the current expression char being processed.
            //
            //  SrcCount    - Prior match being investigated with current name char
            //
            //  DestCount   - Next location to put a matching assuming current name char
            //
            //  NameFinished - Allows one more itteration through the Matches array
            //                 after the name is exhusted (to come *s for example)
            //
            //  PreviousDestCount - This is used to prevent entry duplication, see coment
            //
            //  PreviousMatches   - Holds the previous set of matches (the Src array)
            //
            //  CurrentMatches    - Holds the current set of matches (the Dest array)
            //
            //  AuxBuffer, LocalBuffer - the storage for the Matches arrays
            //

            //
            //  Set up the initial variables
            //

            previousMatches[0] = 0;
            int matchesCount = 1;

            int currentCharOffset = 0;
            int maxState = expression.Length * 2;

            while (!inputFinished)
            {
                if (currentCharOffset < input.Length)
                {
                    nameChar = input[currentCharOffset];
                    currentCharOffset++;
                }
                else
                {
                    inputFinished = true;

                    //
                    //  if we have already exhasted the expression, C#.  Don't
                    //  continue.
                    //
                    if (previousMatches[matchesCount - 1] == maxState)
                    {
                        break;
                    }
                }

                //
                //  Now, for each of the previous stored expression matches, see what
                //  we can do with this name character.
                //
                int srcCount = 0;
                int destCount = 0;
                int previousDestCount = 0;

                while (srcCount < matchesCount)
                {

                    //
                    //  We have to carry on our expression analysis as far as possible
                    //  for each character of name, so we loop here until the
                    //  expression stops matching.  A clue here is that expression
                    //  cases that can match zero or more characters end with a
                    //  continue, while those that can accept only a single character
                    //  end with a break.
                    //
                    int exprOffset = ((previousMatches[srcCount++] + 1) / 2);
                    int length = 0;

                    while (true)
                    {
                        if (exprOffset == expression.Length)
                        {
                            break;
                        }

                        //
                        //  The first time through the loop we don't want
                        //  to increment ExprOffset.
                        //

                        exprOffset += length;

                        currentState = exprOffset * 2;

                        if (exprOffset == expression.Length)
                        {
                            currentMatches[destCount++] = maxState;
                            break;
                        }

                        char exprChar = expression[exprOffset];
                        length = 1;

                        //
                        //  Before we get started, we have to check for something
                        //  really gross.  We may be about to exhaust the local
                        //  space for ExpressionMatches[][], so we have to allocate
                        //  some pool if this is the case.  Yuk!
                        //

                        if (destCount >= matchesArraySize - 2)
                        {
                            int newSize = currentMatches.Length * 2;
                            int[] tmp = new int[newSize];
                            Array.Copy(currentMatches, tmp, currentMatches.Length);
                            currentMatches = tmp;

                            tmp = new int[newSize];
                            Array.Copy(previousMatches, tmp, previousMatches.Length);
                            previousMatches = tmp;
                        }

                        //
                        //  * matches any character zero or more times.
                        //

                        if (exprChar == '*')
                        {
                            currentMatches[destCount++] = currentState;
                            currentMatches[destCount++] = (currentState + 1);
                            continue;
                        }

                        //
                        //  DOS_STAR matches any character except . zero or more times.
                        //

                        if (exprChar == ansiDosStar)
                        {
                            bool iCanEatADot = false;

                            //
                            //  If we are at a period, determine if we are allowed to
                            //  consume it, ie. make sure it is not the last one.
                            //
                            if (!inputFinished && (nameChar == '.'))
                            {
                                for (int offset = currentCharOffset; offset < input.Length; offset++)
                                {
                                    char tmpChar = input[offset];
                                    length = 1;

                                    if (tmpChar != '.')
                                    {
                                        continue;
                                    }
                                    iCanEatADot = true;
                                    break;
                                }
                            }

                            if (inputFinished || (nameChar != '.') || iCanEatADot)
                            {
                                currentMatches[destCount++] = currentState;
                                currentMatches[destCount++] = (currentState + 1);
                                continue;
                            }

                            //
                            //  We are at a period.  We can only match zero
                            //  characters (ie. the epsilon transition).
                            //
                            currentMatches[destCount++] = (currentState + 1);
                            continue;
                        }

                        //
                        //  The following expreesion characters all match by consuming
                        //  a character, thus force the expression, and thus state
                        //  forward.
                        //
                        currentState += length * 2;

                        //
                        //  DOS_QM is the most complicated.  If the name is finished,
                        //  we can match zero characters.  If this name is a '.', we
                        //  don't match, but look at the next expression.  Otherwise
                        //  we match a single character.
                        //
                        if (exprChar == ansiDosQm)
                        {

                            if (inputFinished || (nameChar == '.'))
                            {
                                continue;
                            }

                            currentMatches[destCount++] = currentState;
                            break;
                        }

                        //
                        //  A DOS_DOT can match either a period, or zero characters
                        //  beyond the end of input.
                        //
                        if (exprChar == dosDot)
                        {

                            if (inputFinished)
                            {
                                continue;
                            }

                            if (nameChar == '.')
                            {
                                currentMatches[destCount++] = currentState;
                                break;
                            }
                        }

                        //
                        //  From this point on a name character is required to even
                        //  continue, let alone make a match.
                        //
                        if (inputFinished)
                        {
                            break;
                        }

                        //
                        //  If this expression was a '?' we can match it once.
                        //
                        if (exprChar == '?')
                        {
                            currentMatches[destCount++] = currentState;
                            break;
                        }

                        //
                        //  Finally, check if the expression char matches the name char
                        //
                        if (exprChar == nameChar)
                        {
                            currentMatches[destCount++] = currentState;
                            break;
                        }

                        //
                        //  The expression didn't match so go look at the next
                        //  previous match.
                        //

                        break;
                    }


                    //
                    //  Prevent duplication in the destination array.
                    //
                    //  Each of the arrays is montonically increasing and non-
                    //  duplicating, thus we skip over any source element in the src
                    //  array if we just added the same element to the destination
                    //  array.  This guarentees non-duplication in the dest. array.
                    //

                    if ((srcCount >= matchesCount) || (previousDestCount >= destCount))
                    {
                        continue;
                    }

                    while (previousDestCount < destCount)
                    {
                        int previousLength = previousMatches.Length;
                        while ((srcCount < previousLength) && (previousMatches[srcCount] < currentMatches[previousDestCount]))
                        {
                            srcCount += 1;
                        }
                        previousDestCount += 1;
                    }
                }

                //
                //  If we found no matches in the just finished itteration, it's time
                //  to bail.
                //

                if (destCount == 0)
                {
                    return false;
                }

                //
                //  Swap the meaning the two arrays
                //

                {
                    int[] tmp = previousMatches;
                    previousMatches = currentMatches;
                    currentMatches = tmp;
                }

                matchesCount = destCount;
            }

            currentState = previousMatches[matchesCount - 1];

            return currentState == maxState;
        }

        private static bool IsContainsExpr(string expression)
        {
            return
                (expression[0] == '*')
                && (expression[expression.Length - 1] == '*')
                && (expression.IndexOf('*', 0, expression.Length - 1) == -1);
        }

        private static bool IsEndsWithExpr(string expression)
        {
            return (expression[0] == '*')
                   && (expression.IndexOf('*', 1) == -1);
        }


        private static bool IsStartsWithExpr(string expression)
        {
            return (expression[expression.Length - 1] == '*')
                   && (expression.IndexOf('*', 0, expression.Length - 1) == -1);
        }

        private static bool MatchesAnyInput(string expression)
        {
            return expression.Equals("*") || expression.Equals("*.*");
        }

        private static bool ExprOrInputIsEmpty(string input, string expression)
        {
            return string.IsNullOrEmpty(input) || (string.IsNullOrEmpty(expression));
        }
    }
}
