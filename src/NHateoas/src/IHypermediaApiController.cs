﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NHateoas
{
    public interface IHypermediaApiControllerConfigurator
    {
        void ConfigureHypermedia(HttpConfiguration httpConfiguration);
    }
}
