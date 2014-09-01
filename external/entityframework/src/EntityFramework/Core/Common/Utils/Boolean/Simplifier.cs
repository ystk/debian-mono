// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
    using System.Collections.Generic;
    using System.Diagnostics;

    // Simplifier visitor for Boolean expressions. Performs the following
    // simplifications bottom-up:
    // - Eliminate True and False (A Or False iff. A, A And True iff. A)
    // - Resolve tautology (A Or !A iff. True, True Or A iff. True) and 
    // contradiction (A And !A iff. False, False And A iff. False)
    // - Flatten nested negations (!!A iff. A)
    // - Evaluate bound literals (!True iff. False, etc.)
    // - Flatten unary/empty And/Or expressions
    internal class Simplifier<T_Identifier> : BasicVisitor<T_Identifier>
    {
        internal static readonly Simplifier<T_Identifier> Instance = new Simplifier<T_Identifier>();

        protected Simplifier()
        {
        }

        internal override BoolExpr<T_Identifier> VisitNot(NotExpr<T_Identifier> expression)
        {
            var child = expression.Child.Accept(this);
            switch (child.ExprType)
            {
                case ExprType.Not:
                    return ((NotExpr<T_Identifier>)child).Child;
                case ExprType.True:
                    return FalseExpr<T_Identifier>.Value;
                case ExprType.False:
                    return TrueExpr<T_Identifier>.Value;
                default:
                    return base.VisitNot(expression);
            }
        }

        internal override BoolExpr<T_Identifier> VisitAnd(AndExpr<T_Identifier> expression)
        {
            return SimplifyTree(expression);
        }

        internal override BoolExpr<T_Identifier> VisitOr(OrExpr<T_Identifier> expression)
        {
            return SimplifyTree(expression);
        }

        private BoolExpr<T_Identifier> SimplifyTree(TreeExpr<T_Identifier> tree)
        {
            var isAnd = ExprType.And == tree.ExprType;
            Debug.Assert(isAnd || ExprType.Or == tree.ExprType);

            // Get list of simplified children, flattening nested And/Or expressions
            var simplifiedChildren = new List<BoolExpr<T_Identifier>>(tree.Children.Count);
            foreach (var child in tree.Children)
            {
                var simplifiedChild = child.Accept(this);
                // And(And(A, B), C) iff. And(A, B, C)
                // Or(Or(A, B), C) iff. Or(A, B, C)
                if (simplifiedChild.ExprType
                    == tree.ExprType)
                {
                    simplifiedChildren.AddRange(((TreeExpr<T_Identifier>)simplifiedChild).Children);
                }
                else
                {
                    simplifiedChildren.Add(simplifiedChild);
                }
            }

            // Track negated children separately to identify tautologies and contradictions
            var negatedChildren = new Dictionary<BoolExpr<T_Identifier>, bool>(tree.Children.Count);
            var otherChildren = new List<BoolExpr<T_Identifier>>(tree.Children.Count);
            foreach (var simplifiedChild in simplifiedChildren)
            {
                switch (simplifiedChild.ExprType)
                {
                    case ExprType.Not:
                        negatedChildren[((NotExpr<T_Identifier>)simplifiedChild).Child] = true;
                        break;
                    case ExprType.False:
                        // False And A --> False
                        if (isAnd)
                        {
                            return FalseExpr<T_Identifier>.Value;
                        }
                        // False || A --> A (omit False from child collections)
                        break;
                    case ExprType.True:
                        // True Or A --> True
                        if (!isAnd)
                        {
                            return TrueExpr<T_Identifier>.Value;
                        }
                        // True And A --> A (omit True from child collections)
                        break;
                    default:
                        otherChildren.Add(simplifiedChild);
                        break;
                }
            }
            var children = new List<BoolExpr<T_Identifier>>();
            foreach (var child in otherChildren)
            {
                if (negatedChildren.ContainsKey(child))
                {
                    // A && !A --> False, A || !A --> True
                    if (isAnd)
                    {
                        return FalseExpr<T_Identifier>.Value;
                    }
                    else
                    {
                        return TrueExpr<T_Identifier>.Value;
                    }
                }
                children.Add(child);
            }
            foreach (var child in negatedChildren.Keys)
            {
                children.Add(child.MakeNegated());
            }
            if (0 == children.Count)
            {
                // And() iff. True
                if (isAnd)
                {
                    return TrueExpr<T_Identifier>.Value;
                }
                    // Or() iff. False
                else
                {
                    return FalseExpr<T_Identifier>.Value;
                }
            }
            else if (1 == children.Count)
            {
                // Or(A) iff. A, And(A) iff. A
                return children[0];
            }
            else
            {
                // Construct simplified And/Or expression
                TreeExpr<T_Identifier> result;
                if (isAnd)
                {
                    result = new AndExpr<T_Identifier>(children);
                }
                else
                {
                    result = new OrExpr<T_Identifier>(children);
                }
                return result;
            }
        }
    }
}
