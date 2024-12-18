using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CA_Final_Persons_Reg_Sys.Swagger
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody?.Content.ContainsKey("multipart/form-data") == true)
            {
                operation.RequestBody.Content["multipart/form-data"].Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["File"] = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    }
                };
            }
        }
    }
    
}
