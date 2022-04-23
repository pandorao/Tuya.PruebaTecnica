using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Tuya.PruebaTecnica.SDK.Models
{
    public class ServiceResult : AbstractServiceResult
    {
        public ServiceResult()
        {
            Errors = new Dictionary<string, string[]>();
        }
    }

    public class ServiceResult<T> : AbstractServiceResult
    {
        public ServiceResult()
        {
            Errors = new Dictionary<string, string[] >();
        }

        /// <summary>
        /// Objeto de respuesta
        /// </summary>
        public T ResponseObject { get; set; }
    }

    public abstract class AbstractServiceResult
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NoContent;

        public Dictionary<string, string[]> Errors { get; set; }

        public bool Succeeded()
        {
            return Errors.Count() == 0 && StatusCode == HttpStatusCode.OK;
        }
    }
}
