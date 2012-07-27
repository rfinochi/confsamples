using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace CometMVC.Controllers
{
    public class CometController : AsyncController
    {
        static object monitor = new object();
        static Dictionary<string, List<string>> pending = new Dictionary<string, List<string>>();
        static ManualResetEvent mre = new ManualResetEvent(false);
        
        //
        // GET: /Comet/

        //public ActionResult Index() 
        //{
        //    return View();
        //} 

        [HttpPost]
        public void IndexAsync(string user) 
        {
            AsyncManager.OutstandingOperations.Increment();
            if (!pending.ContainsKey(user)) 
            { 
                lock (monitor)
                {
                    pending[user] = new List<string>();
                }
                AsyncManager.Parameters["items"] = new List<string> { string.Format("Bienvenido {0}!", user) };
                AsyncManager.OutstandingOperations.Decrement();
            }
            else if (pending[user].Count != 0)
            {
                lock (monitor)
                {
                    AsyncManager.Parameters["items"] = pending[user];
                    pending[user].Clear();
                    AsyncManager.OutstandingOperations.Decrement();
                }
            }
            else
            {
                ThreadPool.RegisterWaitForSingleObject(
                    mre,
                    (state, timedOut) =>
                    {
                        if (pending.ContainsKey(user))
                        {
                            if (pending[user].Count != 0)
                            {
                                lock (monitor) 
                                {
                                    var controller = state as CometController;
                                    controller.AsyncManager.Parameters["items"] = new List<string>(pending[user]);
                                    pending[user].Clear();
                                    controller.AsyncManager.OutstandingOperations.Decrement();
                                }
                            }
                        }

                    },
                    this, -1, true);
            }
        }
        
        public JsonResult IndexCompleted(List<string> items)
        {
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Say(string message, string user)
        {
            if (!pending.ContainsKey(user))
            {
                lock (monitor)
                {
                    pending[user] = new List<string> { message };
                }
                mre.Set(); 
                mre = new ManualResetEvent(false);
            }
            else
            {
                pending[user].Add(message);
                mre.Set();
                mre = new ManualResetEvent(false);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
