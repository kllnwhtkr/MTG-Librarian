﻿using System;

namespace MTG_Librarian
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> left, Predicate<T> right) => x => left(x) && right(x);
        public static Predicate<T> Or<T>(this Predicate<T> left, Predicate<T> right) => x => left(x) || right(x);
    }
}
