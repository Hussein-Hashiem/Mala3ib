using Microsoft.AspNetCore.Hosting;

namespace Mala3ib.BLL.Helpers
{
    public class EmailBodyBuilder
    {
        private readonly IWebHostEnvironment _env;

        public EmailBodyBuilder(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
        {
            var path = Path.Combine(_env.ContentRootPath,"Templates",template);

            var body = File.ReadAllText(path);

            foreach (var item in templateModel)
                body = body.Replace(item.Key, item.Value);

            return body;
        }
    }
}
