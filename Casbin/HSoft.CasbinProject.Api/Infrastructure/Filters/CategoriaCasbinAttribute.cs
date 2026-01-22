using Casbin;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HSoft.CasbinProject.Api.Infrastructure.Filters
{
    public class CategoriaCasbinAttribute : TypeFilterAttribute
    {
        public CategoriaCasbinAttribute(string action, string categoria) : base(typeof(CategoriaCasbinFilter))
        {
            Arguments = new object[] { action, categoria };
        }
    }

    public class CategoriaCasbinFilter : IAsyncAuthorizationFilter
    {
        private readonly string _action;
        private readonly string _categoria;
        private readonly Enforcer _enforcer;

        public CategoriaCasbinFilter(string action, string categoria, Enforcer enforcer)
        {
            _action = action;
            _categoria = categoria;
            _enforcer = enforcer;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User.Identity?.Name;
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var resource = context.HttpContext.Request.Path;
            var categoria = _categoria;

            // Verificar permiso con Casbin
            var allowed = await _enforcer.EnforceAsync(user, "productos", _action, categoria);

            if (!allowed)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
