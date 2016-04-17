using System;

public interface IUnitWithHealth
{
    float NormalizedHealth { get; }

    event EventHandler<EventArgs> OnHealthChanged;
}