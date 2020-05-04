using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Repository.Cache
{
    public interface IRepositoryCache
    {
        void Clear();
        bool IsLoaded();
    }
}
