﻿using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces;

public interface IPropEvalManager
{
    Task<Result> evaluateAsync(string username, PropertyProfile propertyProfile);
    Task<Result> loadProfileAsync(string username);
    Task<Result> saveProfileAsync(string username, PropertyProfile propertyProfile);
}