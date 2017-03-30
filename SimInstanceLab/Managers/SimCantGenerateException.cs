using System;

namespace SimInstanceLab.Managers
{
    public class SimCantGenerateException : Exception { public SimCantGenerateException(string message) : base(message) { } }
}