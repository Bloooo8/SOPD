using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace SOPD.Infrastructure
{
    public class AdminAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isAdmin = context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(),"Administrator");

                if (!isAdmin)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }

    public class DeanAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isDean=context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(), "Dziekan"); 

                if (!isDean)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }

    public class PromoterAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isPromoter = context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(), "Promotor");

                if (!isPromoter)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }

    public class ReviewerAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isReviewer= context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(), "Recenzent");

                if (!isReviewer)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }

    public class EmployeeAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isEmployee = context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(), "Pracownik");

                if (!isEmployee)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }

    public class StudentAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var isStudent = context.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(context.HttpContext.User.Identity.GetUserId(), "Dyplomant");

                if (!isStudent)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
            }
        }
    }
}