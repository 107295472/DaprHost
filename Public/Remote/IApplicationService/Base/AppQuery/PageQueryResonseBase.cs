using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AppQuery
{
    public class PageQueryResonseBase<T>
    {
        public PageQueryResonseBase(List<T> data, long total)
        {
            PageData = data;
            PageTotal = total;
        }
        public List<T> PageData { get; set; }
        public long PageTotal { get; set; }
    }
}
