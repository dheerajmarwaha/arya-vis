using Arya.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Repository
{
    public abstract class BaseRepository
    {
        protected readonly ISqlProvider SqlProvider;
        protected BaseRepository(ISqlProvider sqlProvider)
        {
            SqlProvider = sqlProvider;            
        }
    }
}
