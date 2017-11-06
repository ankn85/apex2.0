using Apex.Data.Entities.Logs;
using Apex.Services.Constants;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Apex.Websites.Controllers
{
    public abstract class BaseAdminController : Controller
    {
        protected string GetSystemKeyword()
        {
            var actionDescriptor = ControllerContext.ActionDescriptor;

            return SystemKeyword.GetSystemKeyword(actionDescriptor.ControllerName, actionDescriptor.ActionName);
        }

        protected ActivityLog GetActivityLog(Type objectType, object oldValue = null, object newValue = null)
        {
            var activityLog = new ActivityLog
            {
                CreatedOn = DateTime.UtcNow,
                ObjectFullName = objectType.FullName,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                ApplicationUserId = 1
            };

            if (oldValue != null)
            {
                activityLog.OldValue = ObjectToJson(oldValue);
            }

            if (newValue != null)
            {
                activityLog.NewValue = ObjectToJson(newValue);
            }

            return activityLog;
        }

        protected string ObjectToJson(object value)
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}