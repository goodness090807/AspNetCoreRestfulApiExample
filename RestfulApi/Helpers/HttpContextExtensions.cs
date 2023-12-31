﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Helpers
{
    public static class HttpContextExtensions
    {
        // 製作一個分頁的
        public async static Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext
            , IQueryable<T> queryable, int recordsPerPage)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double count = await queryable.CountAsync();
            double totalAmountPages = Math.Ceiling(count / recordsPerPage);

            httpContext.Response.Headers.Add("totalAmountPages", totalAmountPages.ToString());

        }
    }
}
