# SharpEval

SharpEval is a library that allows you to parse & evaluate math expressions at runtime

## Features

- Parses and evaluates expressions extremely fast.
- Supports custom variables and functions.
- Works with Unity 2018.1 and upwards.
- Supports Unity IL2CPP.
- No reflection.
- No external dependencies.

## Usage

- Evaluate & parse a simple expression:

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


- Evaluate & parse a more complex expression:

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

- Define custom variables:

```cs
using SharpEval;

internal static class Program
{
  internal static void Main()
  {
    Interpreter.Variables["x"] = () => 10.0d;
    Interpreter.Variables["y"] = () => 15.0d;
    
    Console.WriteLine(Interpreter.ParseAndEvaluate(" x + y ")); // 25
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

## Notes

The library targets .NET Standard 2.0 (https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Donation

If you like SharpEval consider [buying me a coffee](https://ko-fi.com/winterboltgames).
