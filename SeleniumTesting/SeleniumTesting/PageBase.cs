﻿using System;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

using SKBKontur.SeleniumTesting.Internals;

namespace SKBKontur.SeleniumTesting
{
    public class PageBase : ISearchContainer
    {
        public PageBase(RemoteWebDriver webDriver)
        {
            this.webDriver = webDriver;
            ExecuteInitAction();
        }

        private void ExecuteInitAction()
        {
            foreach(var pageActionAttribute in GetType().GetCurrentTypeAttributes<IPageActionAttribute>())
                pageActionAttribute.OnInit(this);
        }

        public IWebElement Search(ISelector selector)
        {
            return webDriver.FindElement(selector.SeleniumBy);
        }

        public IWebElement SearchGlobal(ISelector selector)
        {
            return Search(selector);
        }

        public object ExecuteScript(string script, params object[] arguments)
        {
            if(string.IsNullOrWhiteSpace(script))
                throw new ArgumentException("script");
            try
            {
                webDriver.ExecuteScript("window.callArgs = arguments", arguments);
                return webDriver.ExecuteScript(script, arguments);
            }
            catch(StaleElementReferenceException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Can't execute script \"{0}\"", script), ex);
            }
        }

        public virtual void WaitLoaded()
        {
            WaitControlsMarkedWithAttribute();
        }

        private void WaitControlsMarkedWithAttribute()
        {
            var propertyInfos = GetType().GetProperties().Where(prop => prop.IsDefined(typeof(LoadingCompleteAttribute), false));
            var properties = propertyInfos.Select(x => x.GetValue(this)).OfType<ControlBase>().ToArray();
            Waiter.Wait(() => properties.All(x => x.IsPresentObsolete), "Загрузка страницы");
        }

        public string GetAbsolutePathBySelectors()
        {
            return null;
        }

        public ISearchContainer GetRootContainer()
        {
            return this;
        }

        public Actions CreateWebDriverActions()
        {
            return new Actions(webDriver);
        }

        public RemoteWebDriver DangerousGetWebDriverInstance()
        {
            return webDriver;
        }

        public virtual TPage GoTo<TPage>() where TPage : PageBase
        {
            return InitializePage<TPage>(webDriver);
        }

        public static TPage InitializePage<TPage>(RemoteWebDriver webDriver) where TPage : PageBase
        {
            TPage page;
            if(typeof(ReactPage).IsAssignableFrom(typeof(TPage)))
            {
                page = (TPage)Activator.CreateInstance(typeof(TPage));
                if(page == null)
                    throw new InvalidOperationException("Page cannot be null");
                // ReSharper disable once PossibleNullReferenceException
                (page as ReactPage).SetRemoteWebDriver(webDriver);
            }
            else
            {
                page = (TPage)Activator.CreateInstance(typeof(TPage), webDriver);
                if(page == null)
                    throw new InvalidOperationException("Page cannot be null");
            }
            page.WaitLoaded();
            return page;
        }

        protected internal RemoteWebDriver webDriver;
    }
}