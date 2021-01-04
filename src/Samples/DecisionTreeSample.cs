#region Copyright and license
// <copyright file="DecisionTreeSample.cs">
//     Copyright (c) 2020-2021 Oliver Zick. All rights reserved.
// </copyright>
// <author>Oliver Zick</author>
// <license>
//     MIT License
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
// 
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>
#endregion

namespace Delizious
{
    using System;

    internal sealed class DecisionTreeSample
    {
        enum CarType
        {
            CityCar,
            Subcompact,
            Compact,
            MidSize,
            FullSize
        }

        class Customer
        {
            public int Age { get; set; }

            public int Budget { get; set; }

            public override string ToString()
                => $"Age: {this.Age}, Budget: {this.Budget}";
        }

        internal static void Run()
        {
            static Match<Customer> MatchProperty<T>(Func<Customer, T> value, Match<T> match)
                => Match.Transform(value, match);

            var decisionTree = DecisionTree.Composite(DecisionTree.Decision(MatchProperty(s => s.Budget,
                                                                                          Match.LessThan(8000)),
                                                                            DecisionTree.Result<Customer, CarType>(CarType.CityCar)),

                                                      DecisionTree.Decision(Match.Any(MatchProperty(s => s.Age,
                                                                                                    Match.LessThanOrEqualTo(21)),
                                                                                      MatchProperty(s => s.Budget,
                                                                                                    Match.LessThan(10000))),
                                                                            DecisionTree.Result<Customer, CarType>(CarType.Subcompact)),

                                                      DecisionTree.Decision(Match.Always<Customer>(),
                                                                            DecisionTree.Result<Customer, CarType>(CarType.Compact)),

                                                      DecisionTree.Decision(Match.Any(
                                                                                      MatchProperty(s => s.Age,
                                                                                                    Match.GreaterThanOrEqualTo(25)),
                                                                                      MatchProperty(s => s.Budget,
                                                                                                    Match.GreaterThanOrEqualTo(25000))),
                                                                            DecisionTree.Result<Customer, CarType>(CarType.MidSize)),

                                                      DecisionTree.Decision(MatchProperty(s => s.Budget,
                                                                                          Match.GreaterThanOrEqualTo(50000)),
                                                                            DecisionTree.Result<Customer, CarType>(CarType.FullSize))
                                                     );

            var customers = new[]
                            {
                                new Customer { Age = 18, Budget = 7999 },
                                new Customer { Age = 18, Budget = 12000 },
                                new Customer { Age = 21, Budget = 25000 },
                                new Customer { Age = 22, Budget = 9999 },
                                new Customer { Age = 22, Budget = 10000 },
                                new Customer { Age = 24, Budget = 24999 },
                                new Customer { Age = 24, Budget = 25000 },
                                new Customer { Age = 25, Budget = 24999 },
                                new Customer { Age = 50, Budget = 9999 },
                                new Customer { Age = 50, Budget = 50000 },
                            };

            foreach (var customer in customers)
            {
                var recommendedCarTypes = decisionTree.All(customer);

                var recommendations = string.Join(", ", recommendedCarTypes);

                Console.WriteLine($"{customer} = {recommendations}");
            }
        }
    }
}
