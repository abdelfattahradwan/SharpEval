# SharpEval

SharpEval is a library that allows you to parse & evaluate math expressions at runtime

## Features

- Parses and evaluates expressions extremely fast.
- Supports custom variables and functions.
- Compatible with Unity 3.4.0 and newer.
- Supports Unity IL2CPP.
- No reflection.
- No external dependencies.

## Usage

- Parse & evaluate a simple expression:

```cs
using SharpEval;

internal static class Program
{
  internal static void Main()
  {
    Console.WriteLine(Interpreter.ParseAndEvaluate(" 2 * ( 2 + 2 ) / 2 ")); // 4
  }
}
```

- Parse & evaluate a more complex expression:

```cs
using SharpEval;

internal static class Program
{
  internal static void Main()
  {
    Console.WriteLine(Interpreter.ParseAndEvaluate(" ( ( ( 2 + 2 ) ^ 2 + 16 ) - 2 ^ ( 4 + 4 ) ) / 2")); // -112
  }
}
```

- Define custom read-only variables:

```cs
using SharpEval;

internal static class Program
{
  internal static void Main()
  {
    Interpreter.Variables["x"] = new ReadOnlyVariable(10.0d);
    Interpreter.Variables["y"] = new ReadOnlyVariable(15.0d);
    
    Console.WriteLine(Interpreter.ParseAndEvaluate(" x + y ")); // 25
  }
}
```

- Define custom computed variables:

```cs
using SharpEval;
using System;

internal static class Program
{
  internal static void Main()
  {
    var random = new Random();
  
    Interpreter.Variables["rnd"] = new ComputedVariable(() => random.NextDouble());
    
    Interpreter.ParseAndEvaluate("rnd * 100.0d");
  }
}
```

- Define custom functions:

```cs
using SharpEval;
using System.Linq;

internal static class Program
{
  internal static void Main()
  {
    Interpreter.Functions["sum"] = args => args.Sum();
    
    Console.WriteLine(Interpreter.ParseAndEvaluate(" sum ( 2 , 2 , 2 , 2 , 2 ) ")); // 10
  }
}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Donation

If you like SharpEval consider [buying me a coffee](https://ko-fi.com/winterboltgames).
