# SharpEval

SharpEval is a library that allows you to parse & evaluate math expressions at runtime

## Features

- Parses and evaluates expressions extremely fast
- Supports custom variables and functions
- Compatible with Unity 3.4.0 and newer
- Supports Unity IL2CPP
- No reflection
- No external dependencies

## Usage

### Parse & evaluate an expression:

```cs
using SharpEval;
using SharpEval.Tokens;
using SharpEval.Expressions;

internal static class Program
{
  internal static void Main()
  {
    Token[] tokens = new Tokenizer(" ( ( ( 2 + 2 ) ^ 2 + 16 ) - 2 ^ ( 4 + 4 ) ) / 2").ToArray();
  
    Expression expression = new Parser(tokens).ParseExpression();
  
    Ineterpreter.DictionaryContext context = new Ineterpreter.DictionaryContext();
  
    double result = Interpreter.Evaluate(expression);
    
    Console.WriteLine(result); // -112
  }
}
```

### Define custom values and functions:

```cs
using SharpEval;
using SharpEval.Tokens;
using SharpEval.Expressions;

internal static class Program
{
  internal static void Main()
  {
    Interpreter.DictionaryContext context = Ineterpreter.DictionaryContext
    {
        Values =
        {
            ["x"] = 10.0d,
            ["y"] = 20.0d,
        },
        
        Functions =
        {
            ["sum"] = args => args.Sum(),
        },
    };
  
    Token[] tokens = new Tokenizer(" sum ( x , y ) ").ToArray();
    
    Expression expression = new Parser(tokens).ParseExpression();
    
    double result = Interpreter.Evaluate(expression, context);
    
    Console.WriteLine(result); // 30
  }
}
```

## Contributing

Pull requests are welcome for the SharpEval project. If you would like to suggest a major change, please open an issue to discuss it before submitting the pull request.

## Donation

If you like SharpEval, please consider supporting me by donating. Your generosity will be greatly appreciated. [Click here to donate](https://ko-fi.com/winterboltgames). Thank you for your support!
