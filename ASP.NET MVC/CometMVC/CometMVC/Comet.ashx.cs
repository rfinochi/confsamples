using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Security.Principal;
using System.Timers;

namespace CometMVC
{
    public class CometResult : IAsyncResult
    {
        private HttpContext context;
        private AsyncCallback callback;

        private object objLock = new object();
        private object data;
        private ManualResetEvent completeEvent = new ManualResetEvent(false);
        private bool isComplete = false;

        public CometResult(HttpContext context, AsyncCallback cb, object extraData)
        {
            this.context = context;
            this.callback = cb;
            this.data = extraData;
        }

        public HttpContext Context
        {
            get { return this.context; }
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.isComplete = true;

            this.completeEvent.Set();

            if (callback != null)
            {
                callback(this);
            }
        }

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return data; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return completeEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get;
            set;
        }

        public bool IsCompleted
        {
            get { return this.isComplete; }
        }

        #endregion
    }

    public class CometHandler : IHttpAsyncHandler, IReadOnlySessionState
    {
        System.Timers.Timer timer = null;

        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var result = new CometResult(context, cb, extraData);

            timer = new System.Timers.Timer(5000);
            timer.AutoReset = false; 
            timer.Elapsed += result.timer_Elapsed;
            timer.Enabled = true;

            return result;
        }


        public void EndProcessRequest(IAsyncResult result)
        {
            CometResult res = result as CometResult;

            res.Context.Response.Write( String.Format( "<h1>Test: {0}</h1>", DateTime.Now.Ticks ));

            res.Context.Response.Flush();
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get{ return true; }
        }

        public void ProcessRequest(HttpContext context)
        { }

        #endregion
    }
}
