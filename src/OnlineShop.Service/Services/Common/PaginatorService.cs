﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShop.Service.Common.Utils;
using OnlineShop.Service.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShop.Service.Services.Common
{
    public class PaginatorService : IPaginatorService
    {
        private readonly IHttpContextAccessor _accessor;
        public PaginatorService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<IList<T>> ToPagedAsync<T>(IList<T> items, int pageNumber, int pageSize)
        {
            int totalItems = items.Count();
            PaginationMetaData paginationMetaData = new PaginationMetaData()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                HasPrevious = pageNumber > 1
            };
            paginationMetaData.HasNext = paginationMetaData.CurrentPage < paginationMetaData.TotalPages;

            string json = JsonConvert.SerializeObject(paginationMetaData);
            _accessor.HttpContext!.Response.Headers.Add("X-Pagination", json);

            return items.Skip(pageNumber * pageSize - pageSize)
                              .Take(pageSize).ToList();
        }
    }
}
