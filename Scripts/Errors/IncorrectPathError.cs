using System;

namespace JellyBrain.Scripts.Errors;

public class IncorrectPathError: Exception
{
    public IncorrectPathError(string path) : base(path)
    {
        
    }
}