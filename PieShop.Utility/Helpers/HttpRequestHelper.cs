using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionCare.Utility.Helpers
{
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpRequestHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public bool SetDbNameToHttpHeader(string organization)
        {
            var db = LocalStoreMapper.Instance.GetDbInstance(organization);
            if (db != null)
            {
                _contextAccessor.HttpContext.Request.Headers.Add("Db", new Microsoft.Extensions.Primitives.StringValues(db));
                return true;
            }
            return false;
        }
    }

    public interface IHttpRequestHelper
    {
        bool SetDbNameToHttpHeader(string organization);
    }
}
