﻿using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL
{

    public interface ISqlPropEvalDAO
    {
        Task<Result> loadProfileAsync(string username);
        Task<Result> saveProfileAsync(string username, PropertyProfile propertyProfile);
        Task<Result> updatePropEval(string username, int evalPrice);
    }
}