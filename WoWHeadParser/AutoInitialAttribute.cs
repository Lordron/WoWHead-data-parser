﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWHeadParser
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoInitialAttribute : Attribute
    {
        public int Order;
    }
}
