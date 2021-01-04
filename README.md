# Delizious Toolkit
## What?
Delizious Toolkit is an easy to use .NET library entirely written in C# that contains solutions to requirements I have faced in my (professional) life and provides ready-to-use components. I always strive for well-crafted and clean code with a solid API design (e.g. prefer composition over inheritance and favor factory methods over using constructors) and focus on object-functional programming paradigm. 

Further things to mention:
* Implementation is mostly based on [immutability](https://blogs.msdn.microsoft.com/ericlippert/2007/11/13/immutability-in-c-part-one-kinds-of-immutability/) and value semantics where applicable and useful
* Separation of object graph construction and application logic as discussed [here](http://googletesting.blogspot.de/2008/08/by-miko-hevery-so-you-decided-to.html) (by the way a very interesting article about writing testable code!)

If you like or use my work and you are interested in this kind of software development let's get in touch. :)

# Features
## Overview
Delizious Toolkit provides the following features:
* [Matching of values](#matching-of-values) that is adopted from [Delizious Filtering](https://github.com/oliverzick/Delizious-Filtering) library
* [Decision trees](#decision-trees)

Upcoming features:
* Undo-Redo engine that is adopted from [ImmutableUndoRedo](https://github.com/oliverzick/ImmutableUndoRedo) library

## Matching of values
If you want to get rid of procedural and complex `if-the-else` and `switch` statements to evaluate and match values, the `Match` type is what you are looking for. It lets you easily define value matches in a declarative, configurable and extensible way and comes with the following strategies:

Match | What
----- | --------
`Always` | Matches always no matter what value is matched
`Never` | Matches never no matter what value is matched
`Null` | Matches when a value is a `null` reference
`NotNull` | Matches when a value is not a `null` reference
`Same` | Matches when a value is the same instance the match was given
`NotSame` | Matches when a value is not the same instance the match was given
`Equal` | Matches when a value equals an expected value the match was given (uses equality comparison)
`NotEqual` | Matches when a value does not equal an expected value the match was given (uses equality comparison)
`EqualTo` | Matches when a value is equal to an expected value the match was given (uses comparison)
`NotEqualTo` | Matches when a value is not equal to an expected value the match was given (uses comparison)
`GreaterThan` | Matches when a value is greater than an expected value the match was given (uses comparison)
`GreaterThanOrEqualTo` | Matches when a value is greater than or equal to an expected value the match was given (uses comparison)
`LessThan` | Matches when a value is less than an expected value the match was given (uses comparison)
`LessThanOrEqualTo` | Matches when a value is less than or equal to an expected value the match was given (uses comparison)
`All` | Matches when a value matches all of the given matches
`Any` | Matches when a value matches any of the given matches
`None` | Matches when a value matches none of the given matches
`Except` | (**obsolete**) Matches when a value matches none of the given matches
`Custom` | Matches when a value matches the given custom match
`Transform` | Transforms a current value into a transformed value first and then matches the transformed value with a given match

### Sample
The following sample shows the use of composition to combine different matches:

```csharp
// Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10
var match = Match.Any(Match.LessThanOrEqualTo(-1),
                      Match.EqualTo(2),
                      Match.All(Match.GreaterThanOrEqualTo(4),
                                Match.LessThan(7)),
                      Match.GreaterThan(10));

var values = Enumerable.Range(-5, 20);

Console.WriteLine("Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10");

foreach (var value in values)
{
    var result = match.Matches(value);

    Console.WriteLine($"Match value {value}: {result}");
}

/* Output:

Match when value <= -1 or value == 2 or(value => 4 and value < 7) or value > 10
Match value -5: True
Match value -4: True
Match value -3: True
Match value -2: True
Match value -1: True
Match value 0: False
Match value 1: False
Match value 2: True
Match value 3: False
Match value 4: True
Match value 5: True
Match value 6: True
Match value 7: False
Match value 8: False
Match value 9: False
Match value 10: False
Match value 11: True
Match value 12: True
Match value 13: True
Match value 14: True

*/
```

## Decision trees
Making decisions that goes beyond a boolean result provided by [matching of values](#matching-of-values) is where [decision trees](https://en.wikipedia.org/wiki/Decision_tree) come into the game. This library provides an implementation of a generic `DecisionTree` that supports `Composite`, `Decision` and `Result` nodes. You can specify both the decision trees context that is used to make decisions and the result it provides. Decisions are specified by matches using a given context.

### Sample
```csharp
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

/* Output:

Age: 18, Budget: 12000 = Subcompact, Compact
Age: 21, Budget: 25000 = Subcompact, Compact, MidSize
Age: 22, Budget: 9999 = Subcompact, Compact
Age: 22, Budget: 10000 = Compact
Age: 24, Budget: 24999 = Compact
Age: 24, Budget: 25000 = Compact, MidSize
Age: 25, Budget: 24999 = Compact, MidSize
Age: 50, Budget: 9999 = Subcompact, Compact, MidSize
Age: 50, Budget: 50000 = Compact, MidSize, FullSize

*/
```

## License information
```
MIT License

Copyright (c) 2020-2021 Oliver Zick

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
