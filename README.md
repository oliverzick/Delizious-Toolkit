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
* [Matching of values](#matching-of-values) that was adopted from [Delizious Filtering](https://github.com/oliverzick/Delizious-Filtering) library

Upcoming features:
* Decision trees

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
