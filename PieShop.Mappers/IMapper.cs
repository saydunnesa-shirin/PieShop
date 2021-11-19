using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Mappers
{
    public interface IMapper<TFrom, TTo>
    {
        TTo Map(TFrom @from);
    }

    public interface IInclude<T>
    {
        IQueryable<T> Inlcude(IQueryable<T> query);
    }
}
