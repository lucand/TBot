using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace TBot.Helpers
{
    public static class ConfigHelper
    {
        public static NameValueCollection webTemplate = (NameValueCollection)ConfigurationManager.GetSection("webTemplate");
    }
}
