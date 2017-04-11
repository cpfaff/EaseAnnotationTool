
using System.Collections.Generic;

namespace CAFE.Core.Configuration
{
    public interface IEmailTemplatesRenderer
    {
        string Render(string template, Dictionary<string, string> values);
    }
}
