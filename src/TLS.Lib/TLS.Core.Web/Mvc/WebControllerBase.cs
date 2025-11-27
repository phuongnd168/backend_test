using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Web.Mvc
{
    //public abstract class WebControllerBase<T> : ControllerBase where T : ControllerBase
    //{
    //    private ILogger<T> _logger;
    //    private IMediator _mediator;
    //    protected ILogger<T> Logger
    //    {
    //        get
    //        {
    //            return _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    //        }
    //    }
    //    protected IMediator Mediator
    //    {
    //        get
    //        {
    //            return _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    //        }
    //    }
    //    /// <summary>
    //    /// Trace identifier of current request
    //    /// </summary>
    //    protected string RequestID
    //    {
    //        get
    //        {
    //            return HttpContext.TraceIdentifier;
    //        }
    //    }

    //    /// <summary>
    //    /// Last 8 characters of RequestID
    //    /// </summary>
    //    protected string RequestIDShort
    //    {
    //        get
    //        {
    //            return RequestID.Substring(RequestID.Length - 8);
    //        }
    //    }
    //}
}
