using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Student
{
    public class BasicAuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Extract credentials and decode them
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                byte[] data = Convert.FromBase64String(encodedUsernamePassword);
                string decodedCredentials = System.Text.Encoding.UTF8.GetString(data);
                string[] credentials = decodedCredentials.Split(':');
                string username = credentials[0];
                string password = credentials[1];

                // Check if the credentials are valid (you should replace this with your own authentication logic)
                if (IsValidUser(username, password))
                {
                    // Authentication succeeded
                    return;
                }
            }

            // Authentication failed
            context.Result = new UnauthorizedResult();
        }

        private bool IsValidUser(string username, string password)
        {
            // Replace this with your own logic to validate the username and password
            // For simplicity, we are hardcoding a sample username and password
            return username == "user" && password == "pass";
        }
    }

}