﻿using System.Collections.Generic;

namespace Refracto.Services
{
    public interface IStore
    {
        bool Create(Timeline timeline);

        IEnumerable<Timeline> ReadAll();

        void ReadData(Timeline timeline);

        void Update(Timeline timeline);

        void Delete(string id);
    }
}
