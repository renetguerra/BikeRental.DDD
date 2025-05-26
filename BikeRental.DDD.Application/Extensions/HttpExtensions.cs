using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace BikeRental.DDD.Application.Extensions
{
    /// <summary>
    /// Clase auxiliar para la comunicación HTTP con paginado
    /// </summary>
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
